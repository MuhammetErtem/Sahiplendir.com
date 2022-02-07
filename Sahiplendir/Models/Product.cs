using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;



namespace Sahiplendir.Models
{
    public class Product : NamedBaseEntity
    {
        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int CategoryId { get; set; }

        [Display(Name = "Marka")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int BrandId { get; set; }

        [Display(Name = "Fiyat")]
        public decimal Price { get; set; }

        [Display(Name = "İndirim Oranı (%)")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [Range(0, 99, ErrorMessage = "{0} alanı en az {1} en fazla {2} olmalıdır!")]
        public int DiscountRate { get; set; }

        public decimal DiscountedPrice => Price - (Price * DiscountRate / 100.0m);
        [Display(Name = "Ürün Ana Görsel")]
        public string Image { get; set; }

        [Display(Name = "Açıklamalar")]
        public string Descriptions { get; set; }

        [NotMapped]
        [Display(Name = "Liste Fiyatı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        [RegularExpression("^[+-]?[0-9]{1,3}(?:.?[0-9]{3})*(,[0-9]{1,2})?$", ErrorMessage = "Lütfen geçerli bir {0} yazınız!")]
        public string PriceText { get; set; }

        [NotMapped]
        public string SafeImage => Image ?? "/content/images/no-image.png";

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public IFormFile[] ImageFiles { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }

        public ICollection<OrderItem> OrderItem { get; set; } = new HashSet<OrderItem>();
        [Display(Name = "Ürün Yan Görselleri")]
        public ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();


    }

}