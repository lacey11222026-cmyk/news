
namespace WebMVC4
{
    /// <summary>
    /// Summary description for srv_thumb
    /// </summary>
    using System;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Web;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Web.Caching;
    using System.Configuration;
    using PhotoLibs;
    using UTILS;

    public class srv_thumb : IHttpHandler
    {

        //  ----- Copyright Philipos Sakellaropoulos 2002 -- -----
        Image _img;
        ImageResize _ImageResize = new ImageResize();
        String _path;
        String _pathnoimage;
        String _noimageFile;
        String _CurrentDir;
        bool _bStretch, _bBevel, _bUseCOMobject;


        public void ProcessRequest(HttpContext context)
        {
            bool bFoundImages = false;
            string sSaveThumbnails = "";
            bool bSaveThumbnails = !(ConfigurationSettings.AppSettings["DefaultSaveThumbnails"] == "false");
            bool bFoundSaveThumbnails = false;
            int _width = 0;
            int _height = 0;
            float nPercent = 1;
            //int source = 0;

            String sCacheKey;
            bool bFoundInCache = true; // by default
            // create our COM thumbnail generator
            // get width and height
            if (context.Request["w"] != null) Int32.TryParse(context.Request["w"], out _width);
            if (context.Request["h"] != null) Int32.TryParse(context.Request["h"], out _height);
            //if (context.Request["source"] != null) Int32.TryParse(context.Request["source"],out source);
            if (_width < 0 || _width > 1280) _width = 150;
            if (_height < 0 || _height > 1280) _height = 230;

            // get path of 'no thumbnail' image
            _noimageFile = "images\\upload\\no_image.jpg";
            //if (source<1)
            //{
            //  _noimageFile = "upload\\images\\no_image.jpg";   
            //}
            //else
            //{
            //    _noimageFile = "upload\\images\\source_logo\\" + source + ".png";
            //}

            // map requested path
            if (HttpContext.Current.Request.PhysicalApplicationPath != null)
            {
                _path = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, HttpUtility.UrlDecode(context.Request["f"].Trim()));
                var _pathOut = _path.Replace(".jpg", "_out.jpg");
                try
                {
                    if (!File.Exists(_path) && File.Exists(_pathOut))
                    {
                        File.Copy(_pathOut, _path);
                    }
                }
                catch { }
                _pathnoimage = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, _noimageFile);
                if (!File.Exists(_pathnoimage))
                {
                    _pathnoimage = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "images\\upload\\no_image.jpg");
                }
            }
            // allow stretch of thumbnails
            _bStretch = (context.Request["AllowStretch"] == "true");
            // bevel thumbnails
            _bBevel = (context.Request["Bevel"] == "true");
            _bUseCOMobject = (context.Request["UseCOMobj"] == "true");
            // put parameters for thumbnail requested

            // get a reference to the cache object
            Cache MyCache = context.Cache;
            sCacheKey = _ImageResize.GetUniqueThumbName(_path, _width, _height);


            //File cache cua anh
            sSaveThumbnails = _path + "." + _width.ToString() + "." + _height.ToString() + "." + "jpg";

            // --- remove from cache when we want to refresh
            bool bRefresh = (context.Request["Refresh"] == "true");
            if (bRefresh)
            {
                MyCache.Remove(sCacheKey);

                //Xoa disk cache  
                string[] aPath = _path.Split("\\".ToCharArray());
                _CurrentDir = _path.Replace(aPath[aPath.Length - 1], "");
                string[] files = Directory.GetFiles(_CurrentDir, aPath[aPath.Length - 1] + ".*.jpg");
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        NLogLogger.PublishException(ex);
                    }
                }


            }

            //Kiem tra File cache cua anh
            bFoundSaveThumbnails = File.Exists(sSaveThumbnails);

            if (MyCache[sCacheKey] == null)
            {
                // the thumbnail does not exist in cache, create it...
                // Create a bitmap of the thumbnail and show it
                // bitmap = _oGenerator.ExtractThumbnail();    //chỗ này tự khóa



                if (!bRefresh && bFoundSaveThumbnails && bSaveThumbnails)
                {

                    try
                    {
                        bFoundImages = true;
                        _img = Image.FromFile(sSaveThumbnails);
                    }
                    catch (Exception ex)
                    {
                        NLogLogger.PublishException(ex);
                        bFoundImages = false;
                        //Edit by CuongNV
                        //Insert no_image to cache
                        string stringCacheKeyNoImage = "string_cache_key_no_image_" + _pathnoimage;
                        if (MyCache["string_cache_key_no_image"] == null)
                        {
                            _img = Image.FromFile(_pathnoimage);
                            MyCache[stringCacheKeyNoImage] = _img;
                        }
                        else
                        {
                            _img = (Image)MyCache[stringCacheKeyNoImage];
                        }
                    }
                }
                else
                {
                    Image orgImage = null;

                    Bitmap bmpLogo = new Bitmap(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "images\\upload\\logo.png"));
                    // Get the color of a background pixel.

                    //commented by Nghia:  avoid transparent logo.
                    //Color backColor = bmpLogo.GetPixel(5, 5);
                    // Make backColor transparent for myBitmap.
                    //bmpLogo.MakeTransparent(backColor);



                    try
                    {
                        bFoundImages = true;
                        orgImage = Image.FromFile(_path);
                    }
                    catch (Exception ex)
                    {
                        NLogLogger.PublishException(ex);
                        bFoundImages = false;
                        //Edit by CuongNV
                        //Insert no_image to cache
                        string stringCacheKeyNoImage2 = "string_cache_key_no_image_" + _pathnoimage;
                        if (MyCache["string_cache_key_no_image"] == null)
                        {
                            orgImage = Image.FromFile(_pathnoimage);
                            MyCache[stringCacheKeyNoImage2] = orgImage;
                        }
                        else
                        {
                            orgImage = (Image)MyCache[stringCacheKeyNoImage2];
                        }

                    }

                    //if (_height > 0)
                    //{
                    //    if (_width > 0)
                    //    {
                    //        var hPercent = ((float)_height / (float)orgImage.Height);
                    //        var wPercent = ((float)_width / (float)orgImage.Width);
                    //        nPercent = (hPercent < wPercent) ? hPercent : wPercent;

                    //        _img = ScaleByPercent1(orgImage, nPercent);


                    //    }
                    //    else
                    //    {
                    //        nPercent = ((float)_height / (float)orgImage.Height);
                    //        if (nPercent > 1) nPercent = 1;

                    //        _img = ScaleByPercent1(orgImage, nPercent);
                    //    }
                    //}
                    //else
                    //{
                    //    nPercent = ((float)_width / (float)orgImage.Width);
                    //    if (nPercent > 1) nPercent = 1;

                    //    _img = ScaleByPercent1(orgImage, nPercent);
                    //}
                    if (_height.Equals(0))
                    {
                        nPercent = (_width / (float)orgImage.Width);
                        if (nPercent > 1) nPercent = 1;
                        _img = _ImageResize.ScaleByPercent(orgImage, nPercent);
                    }
                    else if (_width.Equals(0))
                    {
                        nPercent = (_height / (float)orgImage.Height);
                        if (nPercent > 1) nPercent = 1;
                        _img = _ImageResize.ScaleByPercent(orgImage, nPercent);
                    }
                    else
                    {
                        _img = _ImageResize.Crop(orgImage, _width, _height, ImageResize.AnchorPosition.Center);
                    }
                    bmpLogo.Dispose();
                    orgImage.Dispose();
                }

                bFoundInCache = false;

            }
            else
            { // bitmap is in cache
                _img = (Image)MyCache[sCacheKey];
            }
            ////crop border of image
            //_img = _ImageResize.Crop(_img, _img.Size.Width - 2, _img.Size.Height - 2, ImageResize.AnchorPosition.Center);

            // let's cache this for 1 Year
            //context.Response.AddHeader("Pragma", "no-cache");
            context.Response.ContentType = "image/jpeg";
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetLastModified(DateTime.Now);
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(30));
            context.Response.Cache.SetValidUntilExpires(true);

            if (!bRefresh)
            {

                context.Response.Cache.SetExpires(DateTime.Now.AddYears(-1));

            }
            else
            {
                context.Response.Cache.SetExpires(DateTime.Now.AddYears(-1));
            }




            System.Drawing.Imaging.Encoder Enc = System.Drawing.Imaging.Encoder.Transformation;
            EncoderParameters EncParms = new EncoderParameters(1);

            EncParms.Param = new EncoderParameter[]
                {
                    new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L)
                };

            ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

            _img.Save(
                context.Response.OutputStream,
                ici,
                EncParms
                );
            if (bFoundImages && !bFoundSaveThumbnails && bSaveThumbnails && !bFoundInCache)
            {
                //Save thumbnail
                try
                {
                    _img.Save(
                    sSaveThumbnails,
                    ici,
                    EncParms
                    );
                }
                catch { }
            }

            //if(bFoundInCache) LogMessage("Found in cache");
            // else LogMessage("NOT Found in cache");
            //cache thumbnail, make it dependent upon the file and thumbnail size

            bool bUseCache = !(ConfigurationSettings.AppSettings["UseCache"] == "false");
            if (!bFoundInCache && bUseCache)
            {
                CacheDependency dependency = new CacheDependency(_path);

                int mins; try
                {
                    mins = int.Parse(ConfigurationSettings.AppSettings["SlidingExpireMinutes"]);
                }
                catch (ArgumentNullException ex)
                {
                    NLogLogger.PublishException(ex);
                    mins = 20;
                }
                MyCache.Insert(sCacheKey, _img, dependency,
                   Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(mins),
                   CacheItemPriority.Default, new CacheItemRemovedCallback(RemovedCallback));
                dependency.Dispose();
            }
            // bitmap in cache, dont dispose yet
            //img.Dispose ();



        }
        private ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            ImageCodecInfo[] encoders;

            encoders = ImageCodecInfo.GetImageEncoders();

            for (int j = 0; j < encoders.Length; ++j)
            {

                if (encoders[j].MimeType == mimeType)

                    return encoders[j];

            }

            return null;

        }

        static public void RemovedCallback(String k, Object item, CacheItemRemovedReason r)
        {
            ((Bitmap)item).Dispose();
            //LogMessage("Callback");
        }

        // for custom tracing, normal tracing does not work with WebHandlers
        static void LogMessage(String mess)
        {
            StreamWriter sw = new StreamWriter("c:\\ASP.NET_log.txt", true);
            sw.WriteLine(mess);
            sw.Close();
        }

        public bool IsReusable
        {
            get { return true; }
        }
        public Image ScaleByPercent1(Image img, float percentage)
        {
            //get the height and width of the image
            int originalW = img.Width;
            int originalH = img.Height;

            //get the new size based on the percentage change
            int resizedW = (int)(originalW * percentage);
            int resizedH = (int)(originalH * percentage);

            //create a new Bitmap the size of the new image
            Bitmap bmp = new Bitmap(resizedW, resizedH);
            bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            //create a new graphic from the Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.None;
            graphic.CompositingQuality = CompositingQuality.HighQuality;
            //draw the newly resized image
            graphic.DrawImage(img, 0, 0, resizedW, resizedH);
            //dispose and free up the resources
            graphic.Dispose();
            //return the image
            return (Image)bmp;
        }
    }
}