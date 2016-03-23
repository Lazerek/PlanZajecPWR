namespace PlanZajec.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prowadzacy")]
    public partial class Prowadzacy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prowadzacy()
        {
            GrupyZajeciowe = new HashSet<GrupyZajeciowe>();
        }

        [Key]
        public long IdProwadzacego { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Tytul { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Imie { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Nazwisko { get; set; }

        [Column(TypeName = "real")]
        public double? Ocena { get; set; }

        [StringLength(2147483647)]
        public string Opis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupyZajeciowe> GrupyZajeciowe { get; set; }
    }
}
