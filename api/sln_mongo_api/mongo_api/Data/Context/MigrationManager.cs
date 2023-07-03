using Microsoft.EntityFrameworkCore;

namespace mongo_api.Data.Context
{
    public static class MigrationManager
    {

        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<AplicationContext>())
                {
                    try
                    {
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return webApp;
        }
    }
}
