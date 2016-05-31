namespace PlanZajec.DataModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Blok")]
    public partial class Blok
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Blok()
        {
            Kursy = new HashSet<Kursy>();
        }

        [Key]
        [StringLength(2147483647)]
        public string KodBloku { get; set; }
        [StringLength(2147483647)]
        public string NazwaBloku { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kursy> Kursy { get; set; }

        override
            public string ToString()
        {
            return KodBloku + "," + NazwaBloku;
        }
    }
}
