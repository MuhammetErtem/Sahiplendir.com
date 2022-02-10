using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public enum Genders
    {
        [Display(Name = "Bay")]
        Male,
        [Display(Name = "Bayan")]
        Female
    }

    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Genders? Gender { get; set; }

    }
}
