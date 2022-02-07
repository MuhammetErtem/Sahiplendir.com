using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public abstract class BaseEntity
    {

        public int Id { get; set; }
        [Display(Name = "Etkin")]
        public bool Enabled { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
