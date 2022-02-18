using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



namespace Sahiplendir.Models
{
    public class Animal : NamedBaseEntity, IHasImage
    {
        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int CategoryId { get; set; }

        [Display(Name = "Kullanıcı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int BrandId { get; set; }


        [Display(Name = "Hayvan Ana Görsel")]
        public string Image { get; set; }

        [Display(Name = "Açıklamalar")]
        public string Descriptions { get; set; }

        [NotMapped]
        public string SafeImage => Image ?? "/content/images/no-image.png";

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public IFormFile[] ImageFiles { get; set; }
        [NotMapped]
        public int[] ImagesToDeleted { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }

        public ICollection<OrderItem> OrderItem { get; set; } = new HashSet<OrderItem>();
        [Display(Name = "Hayvan Yan Görselleri")]
        public ICollection<AnimalImage> AnimalImages { get; set; } = new HashSet<AnimalImage>();


    }

}