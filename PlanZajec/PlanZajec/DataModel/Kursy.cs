namespace PlanZajec.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Klasa z danymi dotycz�ca kurs�w, zawieraj�ca dane takie jak kod kursu, nazwa kursu, blok i liczba punkt�w ECTS
    /// </summary>
    [Table("Kursy")]
    public partial class Kursy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kursy()
        {
            GrupyZajeciowe = new HashSet<GrupyZajeciowe>();
        }

        [Key]
        [StringLength(2147483647)]
        public string KodKursu { get; set; }

        [Required]
        [StringLength(2147483647)]
        public string NazwaKursu { get; set; }

        public long? ECTS { get; set; }

        [StringLength(2147483647)]
        public string Blok { get; set; }

        public virtual Blok Blok1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupyZajeciowe> GrupyZajeciowe { get; set; }
        /// <summary>
        /// Funkcja zwracaj�ca skr�con� nazw� kursu
        /// </summary>
        [NotMapped]
        public string SkrotKursu
        {
            get
            {
                string[] sp = NazwaKursu.Split();
                string result = "";
                for(int i=0; i<sp.Length; i++)
                {
                    if(sp.Length > 0)
                    {
                        result += Char.ToUpper(sp[i][0]);
                    }
                }
                return result;
            }
        }
        override
            public string ToString()
        {
            if (Blok1 != null)
                return KodKursu + "," + NazwaKursu + "," + ECTS+ "," + Blok1;
            else
                return KodKursu + "," + NazwaKursu + "," + ECTS + ",,";
        }
        

    }
}
