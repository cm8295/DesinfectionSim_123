using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.IO;


namespace Disinfection_UI
{
    class BlankCheck
    {
        public double bc(string path, Canvas canvas)
        {
            exporttojpg(path, canvas);
            return picprocess(path);
        }
        void exporttojpg(string path, Canvas c)
        {
            if (path == null) return;
            Size size = new Size((int)c.Width, (int)c.Height);
            RenderTargetBitmap renderbitmap = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Pbgra32);
            renderbitmap.Render(c);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream outsream = new FileStream(path+@"\TMP.jpg", FileMode.Create))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderbitmap));
                encoder.Save(outsream);
            }
        }
        private double picprocess(string path)
        {
            Int32 whites = 0;
            Bitmap bm = new Bitmap(path + @"\TMP.jpg");
            System.Drawing.Color color; int wide = (int)bm.Width; int height = (int)bm.Height;
            for (int i = 0; i < wide; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    color = bm.GetPixel(i, j);
                    if (color.R == 255 & color.B == 255 & color.G == 255)
                    {
                        whites++;
                    }
                }
            }
            double a = (double)whites / (bm.Height * bm.Width);
            return (a);
        }
    }
}
