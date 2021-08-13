using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebP_Image_Converter.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        public IActionResult UploadImg(IFormFile file, string ImageUrl)
        {


            if (file != null)
            {
                ImageUrl = file.FileName;
                var deletepath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                if (System.IO.File.Exists(deletepath1))
                {
                    System.IO.File.Delete(deletepath1);
                }
                var filename1 = Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyymmddMMss") + ".webp";
                ImageUrl = filename1;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", filename1);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                    {
                        imageFactory.Load(file.OpenReadStream())
                                    .Format(new WebPFormat())
                                    .Quality(80)
                                    .Save(stream);
                    }
                }
            }

            TempData["BlogSuccess"] = "Upload File Success";
            return RedirectToAction("Index");
        }

    }
}
