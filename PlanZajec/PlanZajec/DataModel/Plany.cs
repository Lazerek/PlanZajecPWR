namespace PlanZajec.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Plany")]
    public partial class Plany : IEquatable<Plany>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Plany()
        {
            GrupyZajeciowe = new HashSet<GrupyZajeciowe>();
        }

        [Key]
        public long IdPlanu { get; set; }

        [StringLength(2147483647)]
        public string WolneDni { get; set; }

        [StringLength(2147483647)]
        public string NazwaPlanu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GrupyZajeciowe> GrupyZajeciowe { get; set; }

        public bool Equals(Plany other)
        {
            System.Diagnostics.Debug.WriteLine("@@@Grag->EqualsPlany");
            if (other == null)
                return false;
            return IdPlanu == other.IdPlanu;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return Equals(obj as Plany);
        }
        //TODO Fix warning Object.Equals(object o) but does not override Object.GetHashCode
        public string[] GetDni()
        {
            return this.WolneDni.Split(',');
        }

        public void SetDni(string str)
        {
            string[] current= this.WolneDni.Split(',');
            string nowy = "";
            foreach (string one in current)
            {
                if (one.Substring(6, 2).Equals(str.Substring(6, 2))&&(
                    (Int16.Parse(one.Substring(3,2))>=Int16.Parse(str.Substring(0, 2))&&
                    (Int16.Parse(one.Substring(3, 2)) <= Int16.Parse(str.Substring(3, 2))))||
                    (Int16.Parse(one.Substring(0, 2)) >= Int16.Parse(str.Substring(0, 2)) &&
                    (Int16.Parse(one.Substring(0, 2)) <= Int16.Parse(str.Substring(3, 2))))||
                    (Int16.Parse(str.Substring(3, 2)) >= Int16.Parse(one.Substring(0, 2)) &&
                    (Int16.Parse(str.Substring(3, 2)) <= Int16.Parse(one.Substring(3, 2)))) ||
                    (Int16.Parse(str.Substring(0, 2)) >= Int16.Parse(one.Substring(0, 2)) &&
                    (Int16.Parse(str.Substring(0, 2)) <= Int16.Parse(one.Substring(3, 2))))
                    ))
                {
                   str= Append(one, str);
                }
                else
                {
                    nowy = nowy+one+",";
                }
            }
            nowy = nowy + str;
        }

        private string Append(string str1, string str2)
        {
            string wynik = "";
            if (Int16.Parse(str1.Substring(0, 2)) <= Int16.Parse(str2.Substring(0, 2)))
            {
                wynik = wynik + str1.Substring(0, 2) + ":";
            }
            else
            {
                wynik = wynik + str2.Substring(0, 2) + ":";
            }
            if (Int16.Parse(str1.Substring(3, 2)) <= Int16.Parse(str2.Substring(3, 2)))
            {
                wynik = wynik + str1.Substring(3, 2) + ":";
            }
            else
            {
                wynik = wynik + str2.Substring(3, 2) + ":";
            }
            wynik=wynik+ str2.Substring(6, 2);
            return wynik;
        }
    }
}
