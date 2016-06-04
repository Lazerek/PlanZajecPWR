namespace PlanZajec.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Klasa Grup Zajęciowych, która zawiera informacje o konkretnej grupie
    /// </summary>
    [Table("GrupyZajeciowe")]
    public partial class GrupyZajeciowe : IEquatable<GrupyZajeciowe>
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
        public string Tydzien { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string Godzina { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string GodzinaKoniec { get; set; }

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
        /// <summary>
        /// Funkcja informująca czy są jeszcze wolne miejsca w grupie
        /// </summary>
        [NotMapped]
        public bool Wolna
        {
            get
            {
                return Miejsca.HasValue && ZajeteMiejsca.HasValue ? ZajeteMiejsca.Value < Miejsca.Value : true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plany> Plany { get; set; }
        /// <summary>
        /// Funkcja służąca do sprawdzania identyczności grup pod względem kodu grupy
        /// </summary>
        /// <param name="other">Grupa zajęciowa, którą porównujemy</param>
        /// <returns>Czy grupa jest z tym samym kodem grupy</returns>
        public bool Equals(GrupyZajeciowe other)
        {
            return other != null && KodGrupy.Equals(other.KodGrupy);
        }
        override
        public string ToString()
        {
            return KodGrupy + "," + TypZajec + "," + Dzień + "," + Tydzien + "," + Godzina + "," + GodzinaKoniec + "," + Sala + "," + Budynek + "," + Miejsca + "," + ZajeteMiejsca + "," + Potok+","+Kursy+ "," + Prowadzacy;
        }

        
    }
}
