using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VetenarienLibrary
{
    partial class petOwner
    {
        public override string ToString()
        {
            return name;
        }
    }

    partial class pet
    {
        public override string ToString()
        {
            string gender = (sex == true) ? "Hankøn" :  "Hunkøn";
            return "Navn: " + name + " Fødselsdato: " + birthday + " Køn: " + gender;
        }
    }

    partial class consultation
    {
        public override string ToString()
        {
            return "Start: " + startDate.TimeOfDay + " Slut: " + endDate.TimeOfDay;
        }
    }
}
