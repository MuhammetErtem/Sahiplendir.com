using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public enum Genders
    {
        Male,
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
