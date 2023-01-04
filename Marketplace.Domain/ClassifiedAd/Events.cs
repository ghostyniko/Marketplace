using Marketplace.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Domain.ClassifiedAd
{
    public static class Events
    {
        public class ClassifiedAddCreated
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
        }
        public class ClassifiedAddTitleChanged
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
        }
        public class ClassifiedAddPriceChanged
        {
            public Guid Id { get; set; }
            public decimal Price { get; set; }
            public string CurrencyCode { get; set; }

        }
        public class ClassifiedAddTextChanged
        {
            public Guid Id { get; set; }
            public string Text { get; set; }

        }
        public class ClassifiedAddSentForReview
        {
            public Guid Id { get; set; }
        }
        public class ClassifiedAddApproved
        {
            public Guid Id { get; set; }
            public Guid ApprovedBy { get; set; }
            public Guid OwnerId { get; set; }
        }

        public class PictureAddedToAClassifiedAd
        {
            public Guid ClassifiedAdId { get; set; }
            public Guid PictureId { get; set; }
            public string Url { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
            public int Order { get; set; }
        }
        public class ClassifiedAdPictureResized
        {
            public Guid PictureId { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }
    }
}
