namespace PlanZajec.DataModel
{
    using DataAccessLayer;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Klasa przechowuj�ca informacje o planie
    /// </summary>
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
        /// <summary>
        /// Metoda sprawdzaj�ca istnienie planu o tym samym ID
        /// </summary>
        /// <param name="other">Plan do por�wnania</param>
        /// <returns>Informacja czy plany maj� taki sam id</returns>
        public bool Equals(Plany other)
        {
            if (other == null)
                return false;
            return IdPlanu == other.IdPlanu;
        }
        /// <summary>
        /// Metoda por�wnuj�ca 2 plany jako obiekty
        /// </summary>
        /// <param name="obj">Por�wnywany obiekt</param>
        /// <returns>Informacja czy plany s� identyczne</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return Equals(obj as Plany);
        }
        /// <summary>
        /// Metoda zwracaj�ca dni wyj�te z planu
        /// </summary>
        /// <returns>Tablica z wykluczonymi dniami do uk�adania planu</returns>
        public string[] GetWolneDni()
        {
            return WolneDni?.Split(',');
        }
        /// <summary>
        /// Metoda dodaj�ca dni i godziny, w kt�rych nie chcemy dodawa� zaj�� do planu
        /// </summary>
        /// <param name="str"></param>
        public void AddWolneDni(string str)
        {
            string nowy = "";
            if (WolneDni != null)
            {
                string[] current = this.WolneDni.Split(',');
                foreach (string one in current)
                {
                    if (one.Substring(6, 2).Equals(str.Substring(6, 2)) && (
                        (Int16.Parse(one.Substring(3, 2)) >= Int16.Parse(str.Substring(0, 2)) &&
                         (Int16.Parse(one.Substring(3, 2)) <= Int16.Parse(str.Substring(3, 2)))) ||
                        (Int16.Parse(one.Substring(0, 2)) >= Int16.Parse(str.Substring(0, 2)) &&
                         (Int16.Parse(one.Substring(0, 2)) <= Int16.Parse(str.Substring(3, 2)))) ||
                        (Int16.Parse(str.Substring(3, 2)) >= Int16.Parse(one.Substring(0, 2)) &&
                         (Int16.Parse(str.Substring(3, 2)) <= Int16.Parse(one.Substring(3, 2)))) ||
                        (Int16.Parse(str.Substring(0, 2)) >= Int16.Parse(one.Substring(0, 2)) &&
                         (Int16.Parse(str.Substring(0, 2)) <= Int16.Parse(one.Substring(3, 2))))
                        ))
                    {
                        str = AppendWolneDni(one, str);
                    }
                    else
                    {
                        nowy = nowy + one + ",";
                    }
                }
            }
            nowy = nowy + str;
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
               var plany= uw.Plany.GetAll();
                foreach (var plan in plany)
                {
                    if (plan.IdPlanu == this.IdPlanu)
                        plan.WolneDni = nowy;
                }
                uw.SaveChanges();
            }
            WolneDni = nowy;
        }
        /// <summary>
        /// Funkcja dodaj�ca wolne dni do stringa
        /// </summary>
        /// <param name="str1">Godziny pocz�tkowe wolnego dnia</param>
        /// <param name="str2">Godziny ko�cowe wolnego dnia</param>
        /// <returns>Wolne dni</returns>
        private string AppendWolneDni(string str1, string str2)
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
            if (Int16.Parse(str1.Substring(3, 2)) >= Int16.Parse(str2.Substring(3, 2)))
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

        public void UsunWolnyPrzedzial(string dzien, string godzPocz)
        {
            var wolneDni = GetWolneDni();
            string noweDni = "";
            foreach (var jeden in wolneDni)
            {
                var temp = jeden.Split(':');
                if (!(temp[2] == dzien && temp[0] == godzPocz))
                    noweDni += jeden;
            }
            if (noweDni == "")
                noweDni = null;
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                var plany = uw.Plany.GetAll();
                foreach (var plan in plany)
                {
                    if (plan.IdPlanu == this.IdPlanu)
                        plan.WolneDni = noweDni;
                }
                uw.SaveChanges();
            }
            WolneDni = noweDni;
        }
    }
}
