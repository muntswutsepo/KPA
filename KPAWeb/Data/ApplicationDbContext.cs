using KPAWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace KPAWeb.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        } 
        public DbSet<KPA> KPAs { get; set; }
        public DbSet<KPI> KPIs { get; set; }
        public DbSet<KPIEvidence> KPIEvidences { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<KPI>()
                        .HasOne(e => e.KPA)
                        .WithMany(e => e.KPIs)
                        .HasForeignKey(e => e.KPA_Ref_No)
                        .IsRequired();

            modelBuilder.Entity<KPIEvidence>()
                        .HasOne(e => e.KPI)
                        .WithMany(e => e.KPIEvidences)
                        .HasForeignKey(e => e.KPI_Ref_No)
                        .IsRequired();

            modelBuilder.Entity<KPIEvidence>().Property(t => t.Final_Score).HasComputedColumnSql("(cast([Weighting] as float) / 100) * cast([Line_Manager_Score] as Float)");
            modelBuilder.Entity<KPIEvidence>().Property(t => t.No_Of_Days).HasComputedColumnSql("DateDiff(dd, [Start_Date], [End_Date])");

        }
    }
}
