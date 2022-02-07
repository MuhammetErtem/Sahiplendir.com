using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Models
{
    public class Banner : BaseEntity, IHasImage
    {
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public string SafeImage => Image ?? "/content/images/no-image.png";
    }
}
