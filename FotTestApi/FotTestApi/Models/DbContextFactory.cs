using FotTestApi.Data;

namespace FotTestApi.Models
{
	public class DbContextFactory
	{
		public static ApplicationBDContext CreateDbContext(IServiceProvider serviceProvider)
		{
			var scope = serviceProvider.CreateScope();
			return scope.ServiceProvider.GetRequiredService<ApplicationBDContext>();
		}
	}
}
