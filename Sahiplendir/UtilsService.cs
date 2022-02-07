using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Sahiplendir.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sahiplendir.Sahiplendir
{

    public class ResizeImageOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Watermark { get; set; } = false;
    }

    public interface IUtilsService
    {
        Task<string> PrepareImage(Stream stream, int w, int h, bool waterMark = false);
    }

    public class UtilsService : IUtilsService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public UtilsService(
            IWebHostEnvironment webHostEnvironment
            )
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> PrepareImage(Stream stream, int w, int h, bool waterMark = false)
        {
            if (stream is null)
                return null;

            try
            {
                using var image = await Image.LoadAsync(stream);
                image.Mutate(p => p.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(w, h)
                }));

                if (waterMark)
                {
                    using var waterMarkImage = await Image.LoadAsync(
                                   new FileStream(
                                       Path.Combine(webHostEnvironment.WebRootPath, "content", "images", "logo.png"),
                                       FileMode.Open, FileAccess.Read)
                                   );
                    waterMarkImage.Mutate(p => p.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Max,
                        Size = new Size(w / 10, h / 10)
                    }));

                    image.Mutate(p => p.DrawImage(
                        waterMarkImage,
                        new Point(w - w / 10, h - h / 10),
                        .4f));

                }

                return image.ToBase64String(PngFormat.Instance);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<string> PrepareImage(Stream stream, ResizeImageOptions options)
        {
            return await PrepareImage(stream, options.Width, options.Height, options.Watermark);
        }

        public async void AddImage(IHasImage model, ResizeImageOptions options)
        {
            if (model.ImageFile is not null)
                model.Image = await PrepareImage(model.ImageFile?.OpenReadStream(), options);

        }
    }
}