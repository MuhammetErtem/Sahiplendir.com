using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Areas.Admin.Models
{
    public class ErrorDescriber
    {
        public static string ConstraintError(string name) => $"{name} isimli bir kayıt zaten mevcut olduğu için, ekleme işlemi tamamlanamıyor!";

        public static string ConcurrencyError(string name) => $"{name} isimli kayıt bir ya da daha fazla kayıt ile ilişkili olduğundan silme işlemi tamamlanamıyor!";

        public static string NoImageError() => $"Lütfen bir görsel dosyası yükleyiniz!";
    }
}
