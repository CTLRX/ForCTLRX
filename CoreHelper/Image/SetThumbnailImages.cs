using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;

namespace Omipay.Core
{
    /// <summary>
    /// 按图片比例给图片自动生成缩略图，生成该类调用时，注意首先创建对象，以便初始化应用构建器
    /// </summary>
    public class SetThumbnailImages
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SetThumbnailImages()
        {
            #region  构建器(初始化)
            htmimes[".jpe"] = "image/jpeg";
            htmimes[".jpeg"] = "image/jpeg";
            htmimes[".jpg"] = "image/jpeg";
            htmimes[".png"] = "image/png";
            htmimes[".tif"] = "image/tiff";
            htmimes[".tiff"] = "image/tiff";
            htmimes[".bmp"] = "image/bmp";
            #endregion
        }

        /// <summary>
        /// MIME信息列表
        /// </summary>
        public Hashtable htmimes = new Hashtable();

        /// <summary>
        /// 允许的文件后缀名
        /// </summary>
        public string AllowExt = ".jpe|.jpeg|.jpg|.png|.tif|.tiff|.bmp";


        /// <summary>
        /// 获取图像编码解码器的所有相关信息 
        /// 包含编码解码器的多用途网际邮件扩充协议 (MIME) 类型的字符串 
        /// 返回图像编码解码器的所有相关信息 
        /// </summary>
        /// <param name="mimeType">MIME类别</param>
        /// <returns>编码器对象</returns>
        public ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /// <summary>
        /// 检测扩展名的有效性 
        /// 文件名扩展名 
        /// 如果扩展名有效,返回true,否则返回false. 
        /// </summary>
        /// <param name="ext">文件后缀</param>
        /// <returns>如果扩展名有效,返回true,否则返回false. </returns>
        public bool CheckValidExt(string ext)
        {
            bool flag = false;
            string[] aExt = AllowExt.Split('|');
            foreach (string filetype in aExt)
            {
                if (filetype.ToLower() == ext)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// 保存图片 
        /// Image 对象 
        /// 保存路径 
        /// 指定格式的编解码参数 
        /// </summary>
        /// <param name="image">Image对象</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="ici">图像编码信息</param>
        public void SaveImage(System.Drawing.Image image, string savePath, ImageCodecInfo ici)
        {
            //设置 原图片 对象的 EncoderParameters 对象 
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ((long)90));
            image.Save(savePath, ici, parameters);
            parameters.Dispose();
        }

        /// <summary>
        /// 生成缩略图 
        /// 原图片路径(绝对路径) 
        /// 生成的缩略图路径,如果为空则保存为原图片路径(绝对路径) 
        /// 缩略图的宽度（高度与按源图片比例自动生成） 
        /// </summary>
        /// <param name="SourceImagePath">源文件路径</param>
        /// <param name="ThumbnailImagePath">缩放图片保存路径</param>
        /// <param name="ThumbnailImageWidth">缩略图宽度</param>
        public void ToThumbnailImages(string SourceImagePath, string ThumbnailImagePath, int ThumbnailImageWidth)
        {
            //取得扩展名
            string sExt = SourceImagePath.Substring(SourceImagePath.LastIndexOf(".")).ToLower();
            if (SourceImagePath.ToString() == System.String.Empty) throw new NullReferenceException("SourceImagePath is null!");
            if (!CheckValidExt(sExt))
            {
                throw new ArgumentException("原图片文件格式不正确,支持的格式有[ " + AllowExt + " ]", SourceImagePath);
            }
            //从原图片创建Image 对象 
            System.Drawing.Image image = System.Drawing.Image.FromFile(SourceImagePath);
            int num = ((ThumbnailImageWidth / 4) * 3);
            int width = image.Width;
            int height = image.Height;
            //计算图片的比例 
            if ((((double)width) / ((double)height)) >= 1.3333333333333333f)
            {
                num = ((height * ThumbnailImageWidth) / width);
            }
            else
            {
                ThumbnailImageWidth = ((width * num) / height);
            }
            if ((ThumbnailImageWidth < 1) || (num < 1))
            {
                return;
            }
            //用指定的大小和格式初始化 Bitmap 类的新实例 
            Bitmap bitmap = new Bitmap(ThumbnailImageWidth, num, PixelFormat.Format32bppArgb);
            //从指定的 Image 对象创建新 Graphics 对象 
            Graphics graphics = Graphics.FromImage(bitmap);
            //清除整个绘图面并以透明背景色填充 
            graphics.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制 原图片 对象 
            graphics.DrawImage(image, new Rectangle(0, 0, ThumbnailImageWidth, num));
            image.Dispose();
            try
            {
                //将此原图片以指定格式并用指定的编解码参数保存到指定文件 
                string savepath = (ThumbnailImagePath == null ? SourceImagePath : ThumbnailImagePath);
                SaveImage(bitmap, savepath, GetCodecInfo((string)htmimes[sExt]));

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                bitmap.Dispose();
                graphics.Dispose();
            }
        }
    }
}
