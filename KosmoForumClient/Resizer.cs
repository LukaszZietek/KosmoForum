using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace KosmoForumClient
{
    public static class Resizer
    {
        public static byte[] Resize(Stream img, int width, int height)
        {
            using (MemoryStream m1 = new MemoryStream())
            {
                using (Image image = Image.Load(img))
                {
                    image.Mutate(x => x.Resize(width, height));
                    image.SaveAsJpeg(m1);
                    return m1.ToArray();
                }
            }
        }
    }
}
