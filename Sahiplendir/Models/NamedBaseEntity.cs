using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public abstract class NamedBaseEntity : BaseEntity
    {
        [Display(Name = "Ad")]
        [Required(ErrorMessage ="Bu alan boş bırakılamaz!")]
        public string Name { get; set; }

    }
}
