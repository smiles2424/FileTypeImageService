using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace FileTypeImageService.Controllers
{
    public class ImageController : Controller
    {
        public static readonly List<string> Types = new List<string>()
        {
            "eps",
            "png",
            "psd",
            "svg"
        };

        private const string DefaultFileType = "png";
        // GET: Image
        public ActionResult Index(string extension, string imageType)
        {
            if (imageType.IsNullOrWhiteSpace())
            {
                imageType = DefaultFileType;
            }

            if (extension.IsNullOrWhiteSpace())
            {
                return View();
            }

            if (!Types.Contains(imageType.ToLowerInvariant()))
            {
                ViewBag.Message = "Filetype not supported";
                return View("Error");
            }

            var dir = Path.Combine(Server.MapPath("~/Content"), imageType);
            var files = Directory.GetFiles(dir).Select(Path.GetFileNameWithoutExtension).ToArray();
            if (!files.Contains(extension))
            {
                //We don't have an icon for this file type
                return null;
            }
            var file = extension + "." + imageType; 
            var fileLocation = Path.Combine(dir, file);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = file,

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(fileLocation, "image/" + imageType);
        }
    }
}