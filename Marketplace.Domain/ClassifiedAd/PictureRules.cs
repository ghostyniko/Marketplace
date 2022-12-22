using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public static class PictureRules
    {
        public static bool HasCorrectSize(this Picture picture)
            => picture != null
            && picture.Size.Width >= 800
            && picture.Size.Height >= 600;
    }
}
