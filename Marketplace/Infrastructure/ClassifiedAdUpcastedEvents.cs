namespace Marketplace.Infrastructure
{
    public static class ClassifiedAdUpcastedEvents
    {
        public static class V1
        {
            public class ClassifiedAdPublished
            {
                public Guid Id { get; set; }
                public Guid OwnerId { get; set; }
                public string SellersPhotoUrl { get; set; }
                public Guid ApprovedBy { get; set; }
            }
        }
    }

}
