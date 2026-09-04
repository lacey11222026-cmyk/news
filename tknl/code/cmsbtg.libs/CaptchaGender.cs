using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Drawing;

namespace cms.libs  
{
    public class CaptchaGender
    {
        Random rand = new Random();
        private string GetRandomText(HttpContext context)
        {
            StringBuilder randomText = new StringBuilder();
            string alphabets = "ABCDFEGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random r = new Random();
            for (int j = 0; j <= 4; j++)
            {
                randomText.Append(alphabets[r.Next(alphabets.Length)]);
            }
            return randomText.ToString();
            //context.Session["CaptCha_Code"] = randomText.ToString();
            //return context.Session["CaptCha_Code"] as String;
        }

        private void DrawRandomLines(Graphics g)
        {
            SolidBrush green = new SolidBrush(Color.Green);
            for (int i = 0; i < 20; i++)
            {
                g.DrawLines(new Pen(green, 2), GetRandomPoints());
            }
        }

        private Point[] GetRandomPoints()
        {
            Point[] points = { new Point(rand.Next(10, 100), rand.Next(10, 100)), new Point(rand.Next(10, 100), rand.Next(10, 100)) };
            return points;
        }

        public Bitmap GenCaptcha(HttpContext context, out string CaptChaCode)
        {
            string code = GetRandomText(context);
            CaptChaCode = code;
            Bitmap bitmap = new Bitmap(90, 20,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.Yellow);
            Rectangle rect = new Rectangle(0, 0, 90, 20);

            SolidBrush b = new SolidBrush(Color.AliceBlue);
            SolidBrush blue = new SolidBrush(Color.Maroon);
            int counter = 0;
            g.DrawRectangle(pen, rect);
            g.FillRectangle(b, rect);
            for (int i = 0; i < code.Length; i++)
            {
                g.DrawString(code[i].ToString(),
                new Font("Arial", 10),
                blue, new PointF(1 + counter, 0));
                counter += 20;
            }
            return bitmap;
        }

        public Bitmap GenCaptcha(HttpContext context, int w, int h, out string captchaCode)
        {
            string code = GetRandomText(context);
            captchaCode = code;
            Bitmap bitmap = new Bitmap(w, h,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.Yellow);
            Rectangle rect = new Rectangle(0, 0, w, h);

            SolidBrush b = new SolidBrush(Color.Cornsilk);
            SolidBrush blue = new SolidBrush(Color.Maroon);
            int counter = 0;
            g.DrawRectangle(pen, rect);
            g.FillRectangle(b, rect);
            for (int i = 0; i < code.Length; i++)
            {
                g.DrawString(code[i].ToString(),
                //new Font("Verdena", 10 + rand.Next(14, 18)),
                new Font("Arial", 10),
                blue, new PointF(2 + counter, 0));
                counter += 20;
            }
            return bitmap;
        }
    }
}
