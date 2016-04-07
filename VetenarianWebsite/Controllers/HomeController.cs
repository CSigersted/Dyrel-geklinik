using System;
using System.Activities.Expressions;
using System.Activities.Validation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VetenarianWebsite.Models;
using VetenarienLibrary;

namespace VetenarianWebsite.Controllers
{

    public class HomeController : Controller
    {
        Service service = Service.Instance;

        // GET: Home
        [HttpGet]
        public ActionResult Index()
        {
            Session["User"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Index(petOwner guestResponse)
        {
            Session["User"] = service.FindOwner(guestResponse.name, guestResponse.phonenumber);

            if ((Session["User"] as petOwner) != null)
            {
                petOwner user = (Session["User"] as petOwner);
                Session["model"] = new BookingView
                {
                    PetOwnersPets = service.GetOwnersPetsEnumerable(user.petOwner_ID),
                    JobItems = service.GetJobsEnumerable(),
                    date = DateTime.Today,
                    AvailableTimes = new SelectList(new[] { "" })
                };
                
                var model = (Session["model"] as BookingView);
                ViewBag.Username = user.name;
                return View("Booking", model);
            }
            else return View();
        }

        [HttpGet]
        public ActionResult Booking()
        {
            petOwner user = (Session["User"] as petOwner);
            if (user != null)
            {
                var model = (Session["model"] as BookingView);
                ViewBag.Username = user.name;
                return View(model);
            }
            else return View("Index");
        }

        [HttpPost]
        public ActionResult BookingFase1(BookingView bookingView)
        {
            petOwner user = (Session["User"] as petOwner);
            var model = (Session["model"] as BookingView);
            if (bookingView.date >= DateTime.Today)
            {
                model.date = bookingView.date;
                model.AvailableTimes = service.GetAvailableConsultations(model.date.Year, model.date.Month, model.date.Day);
                ViewBag.Username = user.name;
                return View("Booking", model);                
            }
            ViewBag.Name = "Datoen er for gammel!";
            return View("Booking", model);
        }

        [HttpPost]
        public ActionResult BookingFase2(BookingView bookingView)
        {
            petOwner user = (Session["User"] as petOwner);
            var model = (Session["model"] as BookingView);
            if (bookingView.selectedPetID > 0  &&
                bookingView.Time_ID != null && 
                bookingView.date < DateTime.Today && 
                bookingView.Jo_ID != null && 
                bookingView.Time_ID != null)
            {
                DateTime startDate = model.date;
                string[] jobsStrings = bookingView.Jo_ID.Select(j => j).ToArray();
                List<int> jobIDs = new List<int>();
                Array.ForEach(jobsStrings, s => jobIDs.Add(Convert.ToInt32(s)));
                string[] times = bookingView.Time_ID.Select(t => t).ToArray();
                List<DateTime> dates = service.CheckTimes(times, startDate);
                ViewBag.Username = user.name;
                if (dates == null ||
                    !service.CreateConsultation(dates.First(), dates.Max(), bookingView.Description, bookingView.selectedPetID,
                        jobIDs))
                {
                    ViewBag.Name = "Booking lykkedes ikke prøv igen";
                    return View("Booking", model);
                }
                else
                {
                    ViewBag.Name = "Booking lykkedes!";
                    return View("Booking", model);
                }
            }
            ViewBag.Name = "Du mangler at udfylde tid, ydelse, beskrivelse eller vælge et kæledyr!";
            return View("Booking", model);
        }

        [HttpGet]
        public ActionResult FutureBookings()
        {
            petOwner owner = (Session["User"] as petOwner);
            if (owner != null)
            {
                var model = (Session["model"] as BookingView);
                model.ConsltationItems = service.GetPetOwnersPetsConsultations(owner.petOwner_ID);
                if (model.ConsltationItems != null)
                {
                    return View(model);                                    
                }
                model.ConsltationItems = new SelectList(new[] { "" });
                return View();                

            }
            return View("Index");
        }
    }
}