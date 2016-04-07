using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace VetenarienLibrary
{
    public class Service
    {
        private static Service instance;

        private Service() { }

        public static Service Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Service();
                }
                return instance;
            }
        }

        public List<petOwner> GetPetOwners()
        {
            using (var db = new VetenarianDBEntities1())
            {
                return db.petOwners.ToList();
            }
        }

        public petOwner createPetOwner(string name, string address, string phonenumber)
        {
            using (var db = new VetenarianDBEntities1())
            {
                petOwner newPetOwner = new petOwner();
                newPetOwner.name = name;
                newPetOwner.addres = address;
                newPetOwner.phonenumber = phonenumber;
                db.petOwners.Add(newPetOwner);
                db.SaveChanges();
                return newPetOwner;
            }            
        }

        public void updatePetOwner(int id, string name, string address, string phonenumber)
        {
            using (var db = new VetenarianDBEntities1())
            {
                petOwner po = db.petOwners.Find(id);
                po.name = name;
                po.addres = address;
                po.phonenumber = phonenumber;
                db.SaveChanges();
            }
        }

        public void deletePetOwner(int id)
        {
            using (var db = new VetenarianDBEntities1())
            {
                //Finder alle dyr der tilhører ejeren
                List<pet> petsToDelete = (from p in db.pets where p.petOwner1.petOwner_ID == id select p).ToList();
                //Sletter alle dyr tilhørende ejeren
                petsToDelete.ForEach(p => db.pets.Remove(p));
                //Sletter ejeren
                db.petOwners.Remove(db.petOwners.Find(id));
                //gemmer ændringerne
                db.SaveChanges();
            }
        }

        public bool CreateConsultation(DateTime startTime, DateTime endTime, string description, int pet, List<int> jobIDs)
        {
            using (var db = new VetenarianDBEntities1())
            using(var trans = new TransactionScope(TransactionScopeOption.Required, 
                new TransactionOptions{IsolationLevel = IsolationLevel.RepeatableRead}))
            {
                if (CheckIfTimeAvailable(startTime))
                {                    
                    consultation newConsultation = new consultation();
                    newConsultation.startDate = startTime;
                    newConsultation.endDate = endTime.AddMinutes(15);
                    newConsultation.descriptions = description;
                    newConsultation.vetDescriptions = "";
                    newConsultation.pet = db.pets.Find(pet);
                    List<job> js = new List<job>();
                    foreach (int jobID in jobIDs)
                    {
                        js.Add(db.jobs.Find(jobID));
                    }
                    newConsultation.jobs = js;
                    db.consultations.Add(newConsultation);
                
                    db.SaveChanges();
                    trans.Complete();
                    return true;
                }
                trans.Dispose();
                return false;
            }
        }

        public bool CheckIfTimeAvailable(DateTime date)
        {
            using (var db = new VetenarianDBEntities1())
            {
                //Tjekker på om der allrede eksistere en consultation med den dato
                var time = (from t in db.consultations 
                            where t.startDate.Year == date.Year
                            where t.startDate.Month == date.Month
                            where t.startDate.Day == date.Day
                            where t.startDate.Hour == date.Hour
                            where t.startDate.Minute == date.Minute select t).FirstOrDefault();
                try
                {
                    //Hvis der gør returneres false da den er optaget
                    if (time.startDate == date)
                    {
                        return false;
                    }
                }
                catch (NullReferenceException)
                {
                    //Ellers returneres true
                    return true;                    
                }
            }
            return false;
        }

        public List<pet> GetOwnersPets(int id)
        {
            using (var db = new VetenarianDBEntities1())
            {
                return (from p in db.pets where p.petOwner1.petOwner_ID == id select p).ToList();
            }
        }

        public IEnumerable<SelectListItem> GetOwnersPetsEnumerable(int id)
        {
            var db = new VetenarianDBEntities1();
            var pets = (from p in db.pets
                        where p.petOwner1.petOwner_ID == id
                        select new SelectListItem { Value = p.pet_ID.ToString(), Text = p.name });

            return new SelectList(pets, "Value", "Text");
        }
        public IEnumerable<SelectListItem> GetJobsEnumerable()
        {
            var db = new VetenarianDBEntities1();
            var jobs = (from j in db.jobs 
                        select new SelectListItem { Value = j.job_ID.ToString(), Text = j.name});

            return new SelectList(jobs, "Value", "Text");
        }

        public List<consultation> GetConsultations(DateTime date)
        {
            using (var db = new VetenarianDBEntities1())
            {
                return (from c in db.consultations
                        where c.startDate.Year == date.Year
                        where c.startDate.Month == date.Month
                        where c.startDate.Day == date.Day
                        select c).ToList();
            }
        }

        public IEnumerable<SelectListItem> GetAvailableConsultations(int year, int month, int day)
        {
            var db = new VetenarianDBEntities1();
                var bookingsOfTheDay = (from c in db.consultations
                        where c.startDate.Year == year
                        where c.startDate.Month == month
                        where c.startDate.Day == day
                        select c).ToList();

            DateTime date = new DateTime(year,month,day);
            List<TimeSpan> availableTimes = new List<TimeSpan>();
            date = date.AddHours(8);
            DateTime breakTime = date.AddHours(4);
            DateTime stop = date.AddHours(7);
            stop = stop.AddMinutes(30);
            while (date <= stop)
            {
                foreach (var bookedTimes in bookingsOfTheDay)
                {
                    if (date == bookedTimes.startDate)
                    {
                        date = bookedTimes.endDate;
                    }
                }
                if (date == breakTime || date >= stop)
                {
                    date = date.AddHours(1);
                }
                else
                {
                    availableTimes.Add(date.TimeOfDay);
                    date = date.AddMinutes(15);                    
                }
            }
            return new SelectList(availableTimes);
        }

        public List<consultation> GetConsultationsForAPet(consultation consultation)
        {
            using (var db = new VetenarianDBEntities1())
            {
                return (from c in db.consultations
                    where c.pet_ID == consultation.pet_ID
                    select c).ToList();
            }
        }

        public void SetVetDescription(int id, string description)
        {
            using (var db = new VetenarianDBEntities1())
            {
                consultation co = db.consultations.Find(id);
                co.vetDescriptions = description;
                db.SaveChanges();
            }
        }

        public petOwner FindOwner(string name, string phonenumber)
        {
            using (var db = new VetenarianDBEntities1())
            {
                petOwner findOwner = null;
                try
                {
                findOwner =
                    (from po in db.petOwners where po.name == name where po.phonenumber == phonenumber select po).FirstOrDefault();
                }
                catch (InvalidOperationException)
                {
                    return findOwner;
                }
                return findOwner;
            }
        }

        public List<DateTime> CheckTimes(string[] times, DateTime day)
        {
            List<DateTime> dates = new List<DateTime>(); 
            TimeSpan checkSpan = TimeSpan.Parse(times[0]);
            foreach (string value in times)
            {
                try
                {
                    if (checkSpan != TimeSpan.Parse(value))
                    {
                        // Send en advarsel om at bookingerne ikke er sat efter hinanden
                        return null;
                    }
                    else
                    {
                        dates.Add(day + TimeSpan.Parse(value));
                        checkSpan = checkSpan.Add(new TimeSpan(0, 15, 0));
                    }
                }
                catch (FormatException)
                {
                    return null;
                }
                catch (OverflowException)
                {
                    return null;
                }
            }
            return dates;
        }

        public IEnumerable<SelectListItem> GetPetOwnersPetsConsultations(int id)
        {
            var db = new VetenarianDBEntities1();
            DateTime currentDate = DateTime.Today;
            var consultations = (from c in db.consultations
                from p in db.pets
                where p.petOwner1.petOwner_ID == id
                where c.pet_ID == p.pet_ID
                where c.startDate.Year >= currentDate.Year
                where c.startDate.Month >= currentDate.Month
                where c.startDate.Day >= currentDate.Day
                select
                    new SelectListItem
                    {
                        Value = c.consultation_ID.ToString(),
                        Text = c.startDate + " - " + c.endDate + " Beskrivelse: " + c.descriptions
                    });
            return new SelectList(consultations, "Value", "Text");
        }
    }
}
