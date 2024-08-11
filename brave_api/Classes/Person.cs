using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace brave_api.Classes
{
    public class Person
    {
        [Range(0, int.MaxValue)]
        public int AgeOfDeath { get; set; }

         [Range(0, int.MaxValue)]
        public int YearOfDeath { get; set; }
    }
}