using Glammor.Models;
using Microsoft.EntityFrameworkCore;

namespace Glammor.Db
{
	public class EarringsDbContext : DbContext 
	{
		private readonly IConfiguration _configuration;

        public EarringsDbContext(IConfiguration configuration)
        {
				_configuration = configuration;
        }

        public DbSet<Earrings> _Earrings { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			string connectionString = _configuration.GetConnectionString("DefaultConnection");
			options.UseSqlServer(connectionString);
		}
	}
}
