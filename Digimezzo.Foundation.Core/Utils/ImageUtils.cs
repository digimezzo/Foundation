using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Digimezzo.Foundation.Core.Utils
{
    public static class ImageUtils
    {
        private static readonly ImageCodecInfo JpegCodec;

        static ImageUtils()
        {
            JpegCodec = ImageCodecInfo.GetImageEncoders().First(encoder => encoder.MimeType == "image/jpeg");
        }

        /// <summary>
        /// Converts an image file to a gray scale Byte array
        /// </summary>
        /// <param name="filename">The image file</param>
        /// <returns>A gray scale Byte array</returns>
        public static byte[] Image2GrayScaleByteArray(string filename)
        {
            byte[] byteArray = null;

            try
            {
                if (string.IsNullOrEmpty(filename)) return null;

                Bitmap bmp = default(Bitmap);

                using (Bitmap tempBmp = new Bitmap(filename))
                {

                    bmp = MakeGrayscale(new Bitmap(tempBmp));
                }

                ImageConverter converter = new ImageConverter();

                byteArray = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            }
            catch (Exception)
            {
                throw;
            }

            return byteArray;
        }

        /// <summary>
        /// Converts a System.Drawing.Bitmap to a gray scale System.Drawing.Bitmap
        /// </summary>
        /// <param name="original">The original System.Drawing.Bitmap</param>
        /// <returns>A gray scale System.Drawing.Bitmap</returns>
        public static Bitmap MakeGrayscale(Bitmap original)
        {
            // Create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            // Get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            // Create the gray scale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {new float[] {0.3f,0.3f,0.3f,0,0},
                                                                     new float[] {0.59f,0.59f,0.59f,0,0},
                                                                     new float[] {0.11f,0.11f,0.11f,0,0},
                                                                     new float[] {0,0,0,1,0},
                                                                     new float[] {0,0,0,0,1}
                                                                    });

            // Create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            // Set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            // Draw the original image on the new image using the gray scale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            // Dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        /// <summary>
        /// Converts an image file to a Byte array
        /// </summary>
        /// <param name="filename">The image file</param>
        /// <returns>A Byte array</returns>
        public static byte[] Image2ByteArray(string filename)
        {
            byte[] byteArray = null;

            try
            {
                if (string.IsNullOrEmpty(filename))
                    return null;

                Bitmap bmp = default(Bitmap);

                using (Bitmap tempBmp = new Bitmap(filename))
                {
                    bmp = new Bitmap(tempBmp);
                }

                ImageConverter converter = new ImageConverter();

                byteArray = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
            }
            catch (Exception)
            {
                throw;
            }

            return byteArray;
        }

        /// <summary>
        /// Converts a System.Drawing.Image to a file on disk
        /// </summary>
        /// <param name="img">The System.Drawing.Image</param>
        /// <param name="codec">The codec</param>
        /// <param name="filename">The file name</param>
        /// <param name="width">The width to which the image should be rescaled (no rescale when 0)</param>
        /// <param name="height">The height to which the image should be rescaled (no rescale when 0)</param>
        /// <param name="qualityPercent">The compression quality</param>
        public static void Image2File(Image img, ImageCodecInfo codec, string filename,int width, int height, long qualityPercent)
        {
            var encoderParams = new EncoderParameters
            {
                Param = { [0] = new EncoderParameter(Encoder.Quality, qualityPercent) }
            };

            Image scaledImg = null;
            try
            {
                if (width > 0 && height > 0)
                {
                    scaledImg = img.GetThumbnailImage(width, height, null, IntPtr.Zero);
                    img = scaledImg;
                }

                if (File.Exists(filename))
                    File.Delete(filename);
                img.Save(filename, codec, encoderParams);
            }
            finally
            {
                scaledImg?.Dispose();
            }
        }

        /// <summary>
        /// Converts a Byte array to a image file on disk
        /// </summary>
        /// <param name="imageData">The Byte array containing the image data</param>
        /// <param name="codec">The codec</param>
        /// <param name="filename">The file name</param>
        /// <param name="width">The width to which the image should be rescaled (no rescale when 0)</param>
        /// <param name="height">The height to which the image should be rescaled (no rescale when 0)</param>
        /// <param name="qualityPercent">The compression quality</param>
        public static void Byte2ImageFile(byte[] imageData, ImageCodecInfo codec, string filename, int width, int height,
            long qualityPercent)
        {
            using (var ms = new MemoryStream(imageData))
            {
                using (var img = Image.FromStream(ms))
                {
                    Image2File(img, codec, filename, width, height, qualityPercent);
                }
            }
        }

        /// <summary>
        /// Converts a Byte array to a JPG file on disk
        /// </summary>
        /// <param name="imageData">The Byte array containing the image data</param>
        /// <param name="filename">The file name</param>
        /// <param name="width">The width to which the image should be rescaled (no rescale when 0)</param>
        /// <param name="height">The height to which the image should be rescaled (no rescale when 0)</param>
        /// <param name="qualityPercent">The compression quality</param>
        public static void Byte2Jpg(byte[] imageData, string filename, int width, int height, long qualityPercent)
        {
            Byte2ImageFile(imageData, JpegCodec, filename, width, height, qualityPercent);
        }

        /// <summary>
        /// Gets the size (width and height) of an image contained in a Byte array
        /// </summary>
        /// <param name="imageData">The Byte array containing the image data</param>
        /// <returns>The size (width and height) of the image</returns>
        public static Size GetImageDataSize(byte[] imageData)
        {
            Size size = new Size(0,0);

            try
            {
                using (Image img = Image.FromStream(new MemoryStream(imageData)))
                {
                    size = new Size(img.Width, img.Height);
                }

            }
            catch (Exception)
            {
            }

            return size;
        }

        /// <summary>
        /// Converts a file to a System.Windows.Media.Imaging.BitmapImage
        /// </summary>
        /// <param name="filename">The file name</param>
        /// <param name="width">The width to which the image should be rescaled (no rescale when 0)</param>
        /// <param name="height">The height to which the image should be rescaled (no rescale when 0)</param>
        /// <returns></returns>
        public static BitmapImage PathToBitmapImage(string filename, int width, int height)
        {
            if (File.Exists(filename))
            {
                BitmapImage bi = new BitmapImage();

                bi.BeginInit();

                if (width > 0 && height > 0)
                {
                    bi.DecodePixelWidth = width;
                    bi.DecodePixelHeight = height;
                }

                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.UriSource = new Uri(filename);
                bi.EndInit();
                bi.Freeze();

                return bi;
            }

            return null;
        }

        /// <summary>
        /// Converts a file to a System.Windows.Media.Imaging.BitmapImage
        /// </summary>
        /// <param name="byteData">The Byte array containing the image data</param>
        /// <param name="width">The width to which the image should be rescaled (no rescale when 0)</param>
        /// <param name="height">The height to which the image should be rescaled (no rescale when 0)</param>
        /// <param name="maxLength">The maximum allowed length for width and height</param>
        /// <returns></returns>
        public static BitmapImage ByteToBitmapImage(byte[] byteData, int width, int height, int maxLength)
        {
            if (byteData != null && byteData.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(byteData))
                {
                    BitmapImage bi = new BitmapImage();

                    bi.BeginInit();

                    if (width > 0 && height > 0)
                    {
                        var size = new Size(width, height);
                        if (maxLength > 0) size = GetScaledSize(new Size(width, height), maxLength);

                        bi.DecodePixelWidth = size.Width;
                        bi.DecodePixelHeight = size.Height;
                    }

                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = ms;
                    bi.EndInit();
                    bi.Freeze();

                    return bi;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the scaled size of an image, when rescaled using a maximum length for width and height
        /// </summary>
        /// <param name="originalSize">The original size of the image</param>
        /// <param name="maxLength">The maximum allowed length for width and height</param>
        /// <returns></returns>
        public static Size GetScaledSize(Size originalSize, int maxLength)
        {
            var scaledSize = new Size();

            if (originalSize.Height > originalSize.Width)
            {
                scaledSize.Height = maxLength;
                scaledSize.Width = Convert.ToInt32(((double)originalSize.Width / maxLength) * 100);
            }
            else
            {
                scaledSize.Width = maxLength;
                scaledSize.Height = Convert.ToInt32(((double)originalSize.Height / maxLength) * 100);
            }

            return scaledSize;
        }

        /// <summary>
        /// Gets the dominant color in an image file
        /// </summary>
        /// <param name="filename">The image file</param>
        /// <returns>The dominant System.Windows.Media.Color</returns>
        public static System.Windows.Media.Color GetDominantColor(string filename)
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(filename);

            return GetDominantColor(bitmap);
        }

        /// <summary>
        /// Gets the dominant color in an image Byte array
        /// </summary>
        /// <param name="imageData">The Byte array containing the image data</param>
        /// <returns>The dominant System.Windows.Media.Color</returns>
        public static System.Windows.Media.Color GetDominantColor(byte[] imageData)
        {
            Bitmap bitmap;

            using (var ms = new MemoryStream(imageData))
            {
                bitmap = new Bitmap(ms);
            }

            return GetDominantColor(bitmap);
        }

        /// <summary>
        /// Gets the dominant color in a System.Drawing.Bitmap
        /// </summary>
        /// <param name="bitmap">The Byte array containing the image data</param>
        /// <returns>The dominant System.Windows.Media.Color</returns>
        private static System.Windows.Media.Color GetDominantColor(Bitmap bitmap)
        {
            var newBitmap = new Bitmap(1, 1);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                // Interpolation mode needs to be HighQualityBilinear or HighQualityBicubic
                // or this method doesn't work. With either setting, the averaging result is
                // slightly different.
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, new Rectangle(0, 0, 1, 1));
            }

            Color color = newBitmap.GetPixel(0, 0);

            return System.Windows.Media.Color.FromRgb(color.R, color.G, color.B); ;
        }

        /// <summary>
        /// Rescales an image in a byte[] to another byte[]
        /// </summary>
        /// <param name="inputImage">The image to rescale</param>
        /// <param name="width">The width to which the image should to be rescaled</param>
        /// <param name="height">The height to which the image should to be rescaled</param>
        /// <returns>The rescaled image</returns>
        public static byte[] ResizeImageInByteArray(byte[] inputImage, int width, int height)
        {
            byte[] outputImage = null;

            if(inputImage != null)
            {
                MemoryStream inputMemoryStream = new MemoryStream(inputImage);
                Image fullsizeImage = Image.FromStream(inputMemoryStream);

                Bitmap fullSizeBitmap = new Bitmap(fullsizeImage, new Size(width, height));
                MemoryStream resultStream = new MemoryStream();

                fullSizeBitmap.Save(resultStream, fullsizeImage.RawFormat);

                outputImage = resultStream.ToArray();
                resultStream.Dispose();
                resultStream.Close();
            }

            return outputImage;
        }
    }
}
