using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public class Picture : Entity<PictureId>
    {
        public Picture(Action<object> applier) : base(applier)
        {
        }

        public Guid PictureId { get; private set; }
        public ClassifiedAddId ParentId { get; private set; }
        public PictureSize Size { get; internal set; }
        public Uri Location { get; internal set; }
        public int Order { get; internal set; }

        internal Picture()
        {

        }
        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddedToAClassifiedAd e:
                    Id = new PictureId(e.PictureId);
                    Size = new PictureSize
                    { Height = e.Height, Width = e.Width };
                    Location = new Uri(e.Url);
                    Order = e.Order;
                    PictureId = e.PictureId;
                    ParentId = new ClassifiedAddId(e.ClassifiedAdId);
                    break;
                case Events.ClassifiedAdPictureResized e:
                    Size = new PictureSize
                    { Height = e.Height, Width = e.Width };
                    break;
            }
        }
        internal void Resize(PictureSize newSize)
        {
            Apply(new Events.ClassifiedAdPictureResized
            {
                PictureId = Id,
                Height = newSize.Height,
                Width = newSize.Width
            });

        }
    }

    public class PictureId : Value<PictureId>
    {
        public PictureId(Guid value) => Value = value;
        public Guid Value { get; }

        internal PictureId() { }
        public static implicit operator Guid(PictureId self) => self.Value;

    }

    public class PictureSize : Value<PictureSize>
    {
        public int Height { get; internal set; }
        public int Width { get; internal set; }
        public PictureSize(int height, int width)
        {
            if (Width <= 0)
                throw new ArgumentOutOfRangeException(
                nameof(width),
                "Picture width must be a positive number");
            if (Height <= 0)
                throw new ArgumentOutOfRangeException(
                nameof(height),
                "Picture height must be a positive number");
            Width = width;
            Height = height;
        }

        internal PictureSize() { }



    }

}
