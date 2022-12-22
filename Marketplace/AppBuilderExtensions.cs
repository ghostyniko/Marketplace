using Marketplace.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Marketplace
{
   
    public static class AppBuilderDatabaseExtensions
    {
        public static void EnsureDatabase(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ClassifiedAdDbContext>();
                if (!context.Database.EnsureCreated())
                    context.Database.Migrate();
            }
            
        }
    }
    
}
