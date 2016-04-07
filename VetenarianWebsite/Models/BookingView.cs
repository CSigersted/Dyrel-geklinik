using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VetenarienLibrary;

namespace VetenarianWebsite.Models
{
    public class BookingView
    {
        [Display(Name = "VetenarienLibrary")]
        public int selectedPetID { get; set; }
        public IEnumerable<SelectListItem> PetOwnersPets { get; set; }
        public DateTime date { get; set; }
        public IEnumerable<string> Time_ID { get; set; }
        public IEnumerable<SelectListItem> AvailableTimes { get; set; }
        public IEnumerable<string> Jo_ID { get; set; }
        public IEnumerable<SelectListItem> JobItems { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> ConsultationID { get; set; }
        public IEnumerable<SelectListItem> ConsltationItems { get; set; }
    }
}