using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public class Category : NamedBaseEntity
    {

        [Display(Name = "Reyon")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int RayonId { get; set; }

        //NavigationProperty
        public Rayon Rayon { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public ICollection<Animal> Animals { get; set; } = new HashSet<Animal>();

    }
}