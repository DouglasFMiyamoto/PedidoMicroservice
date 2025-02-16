using Microsoft.EntityFrameworkCore;
using PedidoMicroservice.Adapters.Database.PostgreSQL;

namespace PedidoMicroservice.Adapters.Database.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using PedidoContext dbContext =
                scope.ServiceProvider.GetRequiredService<PedidoContext>();

            dbContext.Database.Migrate();
        }
    }
}
