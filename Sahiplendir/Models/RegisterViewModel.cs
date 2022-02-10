using System;
using System.ComponentModel.DataAnnotations;

namespace Sahiplendir.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "E-Posta")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Email { get; set; }

        [Display(Name = "Ad Soyad")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Name { get; set; }

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "{0} alanı ile {1} alanı aynı olmalıdır!")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Display(Name = "Cinsiyet")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public Genders Gender { get; set; }

    }
}
