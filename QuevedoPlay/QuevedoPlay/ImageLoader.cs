using System;
using System.Drawing;
using System.IO;

namespace QuevedoPlay
{
    public static class ImageLoader
    {
        public static Image Load(string fileName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDir, "Resources", "Imagenes", fileName);

            if (!File.Exists(path)) return null;

            using (var temp = Image.FromFile(path))
            {
                return new Bitmap(temp);
            }
        }
    }
}
