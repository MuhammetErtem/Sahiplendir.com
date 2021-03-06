using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public class AnimalImage
    {
        public int Id { get; set; }

        public int AnimalId { get; set; }

        public string Image { get; set; }
        
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public Animal Animal { get; set; }
    }
}