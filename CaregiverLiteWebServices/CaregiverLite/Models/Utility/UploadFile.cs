using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace CaregiverLite.Models.Utility
{
    public class UploadFile
    {
        public static string getUploadFile_and_getFileURL(HttpPostedFileBase Myfile,string FilePath, string FileName) 
        {
            
            string url = "";
            string path = "";
            string fileName = "";
            if (Myfile != null && Myfile.ContentLength > 0)
                try
                {
                    //DirectoryInfo di = Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(directoryName));
                    //path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(directoryName),Path.GetFileName(Myfile.FileName));
                    path = Path.Combine(FilePath, FileName);
                    //path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Image/CareGiverProfileImages"), FileName);
                    //path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(FilePath), FileName);

                    string tempfileName = "";
                    if (System.IO.File.Exists(path))
                    {
                        int counter = 2;
                        while (System.IO.File.Exists(path))
                        {
                            tempfileName = FileName;
                            path = Path.Combine(FilePath, tempfileName);
                            counter++;
                        }
                        fileName = tempfileName;
                    }
                    else
                    {

                        fileName = FileName;
                    }

                    Myfile.SaveAs(path);
                }
                catch (Exception ex)
                {
                    //ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                //ViewBag.Message = "You have not specified a file.";
            }
            //file Upload End
            url =  fileName;

            return url;
        }

        public static void UploadProductImageThumbnail(HttpPostedFileBase file, string directoryName, string filename)
        {
            string url = "";
            string path = "";
            string fileName = "";

            using (var image = Image.FromStream(file.InputStream, true, true))
            {
                var thumbWidth = 200;
                var thumbHeight = 200;

                using (var thumb = image.GetThumbnailImage(
                    thumbWidth,
                    thumbHeight,
                    () => false,
                    IntPtr.Zero))
                {
                    var jpgInfo = ImageCodecInfo.GetImageEncoders()
                        .Where(codecInfo => codecInfo.MimeType == "image/jpeg").First();

                    using (var encParams = new EncoderParameters(1))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(directoryName));
                        path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(directoryName), Path.GetFileName(filename));

                        //string thumbPath = "~/Images/Thumbnails";
                        //var thumbPathFull = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(path), file.FileName);

                        long quality = 100;
                        encParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                        thumb.Save(path, jpgInfo, encParams);
                    }
                }
            }
        }


    }
}