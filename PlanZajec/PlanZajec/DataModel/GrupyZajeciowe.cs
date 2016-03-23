namespace PlanZajec.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GrupyZajeciowe")]
    public partial class GrupyZajeciowe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GrupyZajeciowe()
        {
            Plany = new HashSet<Plany>();
        }

        [Key]
        [StringLength(2147483647)]
        public string KodGrupy { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string TypZajec { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Dzień { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Godzina { get; set; }

        [StringLength(2147483647)]
        public string Sala { get; set; }

        [StringLength(2147483647)]
        public string Budynek { get; set; }

        public long? Miejsca { get; set; }

        public long? ZajeteMiejsca { get; set; }

        [StringLength(2147483647)]
        public string Potok { get; set; }

        public long? IdProwadzacego { get; set; }

        [StringLength(2147483647)]
        public string KodKursu { get; set; }

        public virtual Kursy Kursy { get; set; }

        public virtual Prowadzacy Prowadzacy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plany> Plany { get; set; }
    }
}
