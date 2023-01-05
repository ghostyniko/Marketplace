namespace Marketplace.Infrastructure.Profanity
{
    public class ProfanityClient
    {
        public async Task<bool> CheckForProfanity(string text)
        {
            return await Task.FromResult(true);
        }
    }
}
