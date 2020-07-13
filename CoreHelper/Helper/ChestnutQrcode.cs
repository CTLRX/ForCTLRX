using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.Helper
{
    /// <summary>
    /// 二维码生成
    /// </summary>
   public class ChestnutQrcode
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="version">版本 1 ~ 40</param>
        /// <param name="pixel">像素点大小</param>
        /// <param name="icon_path">图标路径</param>
        /// <param name="icon_size">图标尺寸</param>
        /// <param name="icon_border">图标边框厚度</param>
        /// <param name="white_edge">二维码白边</param>
        /// <returns>位图</returns>
        public static Bitmap code(string msg, int pixel, string icon_path, int icon_size, int icon_border, bool white_edge)
        {

            QRCoder.QRCodeGenerator code_generator = new QRCoder.QRCodeGenerator();

            QRCoder.QRCodeData code_data = code_generator.CreateQrCode(msg, QRCoder.QRCodeGenerator.ECCLevel.M/* 这里设置容错率的一个级别 */, true, true, QRCoder.QRCodeGenerator.EciMode.Utf8);

            QRCoder.QRCode code = new QRCoder.QRCode(code_data);

            Bitmap icon = new Bitmap(icon_path);

            Bitmap bmp = code.GetGraphic(pixel, Color.Black, Color.White, icon, icon_size, icon_border, white_edge);

            return bmp;

        }
        /// <summary>
        /// 图片合成
        /// </summary>
        /// <param name="imgBack"></param>
        /// <param name="img"></param>
        /// <param name="xDeviation"></param>
        /// <param name="yDeviation"></param>
        /// <returns></returns>
        public static Bitmap CombinImage(Image imgBack, Image img, int xDeviation = 0, int yDeviation = 0)
        {

            Bitmap bmp = new Bitmap(imgBack.Width, imgBack.Height + img.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2 + xDeviation, imgBack.Height + yDeviation, img.Width, img.Height);
            GC.Collect();
            return bmp;
        }
        /// <summary>
        /// 给二维码添加文字
        /// </summary>
        /// <param name="bitmapFile"></param>
        public static void Texts(string bitmapFile = @"D:\C#\WestUnion测试\二维码生成\13.png")
        {
            Bitmap bitmap = new Bitmap(bitmapFile);
            Graphics gp = Graphics.FromImage(bitmap);
            String label = "SUSHI MOTTO";
            Font font = new Font("SimHei", 30, FontStyle.Bold);
            SolidBrush sbrush = new SolidBrush(Color.Black);
            int i = System.Text.Encoding.Default.GetBytes(label).Length;
            int x = ((bitmap.Width / 2) - (i * 23)) / 2;
            int y = 240;
            gp.DrawString(label, font, sbrush, x, y);
            bitmap.Save(@"D:\C#\WestUnion测试\二维码生成\14.png");
        }
    }
}
