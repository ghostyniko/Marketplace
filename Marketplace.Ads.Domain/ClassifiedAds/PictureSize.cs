using Marketplace.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Ads.Domain.ClassifiedAds
{
    public class PictureSize : Value<PictureSize>
    {
        public PictureSize(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(width),
                    "Picture width must be a positive number"
                );

            if (height <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(height),
                    "Picture height must be a positive number"
                );

            Width = width;
            Height = height;
        }

        internal PictureSize() { }
        public int Width { get; internal set; }
        public int Height { get; internal set; }
    }
}
