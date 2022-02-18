using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public class Brand : NamedBaseEntity
    {
        [Display(Name = "Logo")]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public ICollection<Animal> Animals { get; set; } = new HashSet<Animal>();


        public string SafeImage => Image ?? "/content/images/no-image.png";
    }
}
