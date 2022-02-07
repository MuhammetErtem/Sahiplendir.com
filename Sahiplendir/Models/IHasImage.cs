using Microsoft.AspNetCore.Http;

namespace Sahiplendir.Models
{
    public interface IHasImage
    {
        string Image { get; set; }

        string SafeImage { get; }

        IFormFile ImageFile { get; set; }
    }
}