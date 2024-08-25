using FotTestApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FotTestApi.Data
{
	public class ApplicationBDContext : DbContext
	{

		public ApplicationBDContext(DbContextOptions<ApplicationBDContext> options)
			: base(options)


		{

		}
		public DbSet<MissionModel> Missions { get; set; }
		public DbSet<AgentModel> Agents { get; set; }
		public DbSet<TargetModel> Targets { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MissionModel>()
				.HasOne(m => m.Agent)
				.WithMany(x => x.Missions)
				.HasForeignKey(m => m.IdAgent);
			modelBuilder.Entity<MissionModel>()
				.HasOne(m => m.Target)
				.WithMany()
				.HasForeignKey(m => m.IdTarget);
			
		}
	}
}

