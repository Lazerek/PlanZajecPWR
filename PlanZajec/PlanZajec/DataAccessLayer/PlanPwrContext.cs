namespace PlanZajec.DataModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PlanPwrContext : DbContext
    {
        public PlanPwrContext()
            : base("name=PlanPwrContext")
        {
        }

        public virtual DbSet<Blok> Blok { get; set; }
        public virtual DbSet<GrupyZajeciowe> GrupyZajeciowe { get; set; }
        public virtual DbSet<Kursy> Kursy { get; set; }
        public virtual DbSet<Plany> Plany { get; set; }
        public virtual DbSet<Prowadzacy> Prowadzacy { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blok>()
                .HasMany(e => e.Kursy)
                .WithOptional(e => e.Blok1)
                .HasForeignKey(e => e.Blok);

            modelBuilder.Entity<Plany>()
                .HasMany(e => e.GrupyZajeciowe)
                .WithMany(e => e.Plany)
                .Map(m => m.ToTable("Nalezy").MapLeftKey("IdPlanu").MapRightKey("KodGrupy"));
        }
    }
}
