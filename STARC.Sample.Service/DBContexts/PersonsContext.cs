using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using STARC.Sample.Service.Configs;
using STARC.Sample.Service.EntityModels;

namespace STARC.Sample.Service.DBContexts
{
    public class PersonsContext : DbContext
    {
        private IOptions<ConfigSettings> _config;

        public PersonsContext(IOptions<ConfigSettings> configOptions, DbContextOptions<PersonsContext> options) : base(options)
        {
            _config = configOptions;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config.Value.ConnectionString);
        }

        public DbSet<Person> Persons { get; set; }
    }
}
