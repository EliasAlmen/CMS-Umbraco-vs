using EC07_CMS_Umbraco_vs.Models;
using Microsoft.EntityFrameworkCore;

namespace EC07_CMS_Umbraco_vs.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<SubscribersEntity> Subscribers { get; set;}
    }
}
