using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Capbreak.Areas.Wx.Models;

namespace Capbreak.Areas.Wx.Controllers
{
    public class PlacefileController : Controller
    {
        private static readonly ConcurrentDictionary<DateTime, CachedImage> ImageCache = new ConcurrentDictionary<DateTime, CachedImage>();
        private const int CacheExpirationMinutes = 1;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SpcMesoanalysis(string sector, string parameter)
        {
            if (string.IsNullOrEmpty(sector) || string.IsNullOrEmpty(parameter))
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "The page you requested could not be found.";

                return null;
            }

            var sectorName = string.Empty;
            string parameterName;
            var data = string.Empty;

            switch (sector)
            {
                case "s13":
                    sectorName = "Northern Plains";
                    data = CoordinateModel.NorthernPlains;
                    break;
                case "s14":
                    sectorName = "Central Plains";
                    data = CoordinateModel.CentralPlains;
                    break;
            }

            switch (parameter)
            {
                case "ttd":
                    parameterName = "Temp/Dewpoint/Wind";
                    break;
                case "eshr":
                    parameterName = "Effective Shear";
                    break;
                default:
                    parameterName = parameter;
                    break;
            }

            if (!string.IsNullOrEmpty(sector) && !string.IsNullOrEmpty(parameter))
            {
                // Create placefile
                // TODO handling for dev/prod endpoints
                //var imageUrl = String.Format("http://capbreak.com/wx/placefile/mesoanalysisimage/{0}/{1}", sector, parameter);
                var imageUrl = String.Format("http://localhost:20685/wx/placefile/mesoanalysisimage?sector={0}&parameter={1}", sector, parameter);
                var placefile = String.Format("; Capbreak SPC Mesoanalysis Placefile\r\nTitle: SPC {0} {1}\r\n" +
                    "RefreshSeconds: 300\r\n" +
                    "Threshold: 999\r\n" +
                    "Image: {2}\r\n" +
                    "{3}" +
                    "End:\r\n", sectorName, parameterName, imageUrl, data);

                Response.ContentType = "text/plain";
                Response.Write(placefile);

                return null;
            }

            Response.StatusCode = 404;
            Response.StatusDescription = "The page you requested could not be found.";

            return null;
        }

        public async Task<ActionResult> MesoanalysisImage(string sector, string parameter)
        {
            Byte[] cachedImage = null;
            var imageName = string.Format("spc_{0}_{1}", sector, parameter);
            if (ImageCache.IsEmpty ||
                ImageCache.Values.FirstOrDefault(x => x.Name == imageName) == null ||
                ImageCache.First(x => x.Value.Name.Equals(imageName)).Key < DateTime.UtcNow)
            {
                try
                {
                    ImageCache.Clear();

                    byte[] spcImageBytes;
                    var spcUrl = new Uri(string.Format("http://www.spc.noaa.gov/exper/mesoanalysis/{0}/{1}/{1}.gif?{2}", sector, parameter, DateTime.Now.Ticks));

                    using (var client = new HttpClient())
                    {
                        spcImageBytes = await client.GetByteArrayAsync(spcUrl);
                    }

                    if (spcImageBytes != null)
                    {
                        var imageStream = new MemoryStream(spcImageBytes);
                        var spcImage = Image.FromStream(imageStream);
                        spcImage.Save(imageStream, ImageFormat.Png);
                        var bitmapImage = Image.FromStream(imageStream) as Bitmap;
                        var pngImageStream = new MemoryStream();

                        // White to transparent - should possibly do this with unsafe code to be faster
                        if (bitmapImage != null)
                        {
                            var color = bitmapImage.GetPixel(0, 0);
                            bitmapImage.MakeTransparent(color);
                            bitmapImage.Save(pngImageStream, ImageFormat.Png);
                        }

                        cachedImage = pngImageStream.ToArray();
                        ImageCache.GetOrAdd(DateTime.UtcNow.AddMinutes(CacheExpirationMinutes), new CachedImage
                        {
                            ImageBytes = cachedImage,
                            Name = imageName
                        });
                    }
                }
                catch (Exception ex)
                { }
            }
            else
            {
                cachedImage = ImageCache.First(x => x.Value.Name.Equals(imageName)).Value.ImageBytes;
            }

            if (cachedImage != null)
            {
                Response.ContentType = "image/png";
                Response.BinaryWrite(cachedImage);
            }
            else
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "The page you requested could not be found.";
            }

            return null;
        }
	}
}