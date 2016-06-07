using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;

namespace PlanZajec.ViewModels
{
    /// <summary>
    /// Klasa pozwalająca wyświetlać kafelki na planie
    /// </summary>
    public class GrupaTamplateViewModel
    {
        public GrupyZajeciowe GrupZaj
        {
            get; set;
        }
        public string nazwa;
        /// <summary>
        /// Tworzy obiek kafelka, reprezentujący grupę zajęciowej
        /// </summary>
        /// <param name="Kod">Kod grupy</param>
        public GrupaTamplateViewModel(string Kod)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
               var grupa = uw.GrupyZajeciowe.Get(Kod);
               GrupZaj = grupa;
               nazwa = grupa.Kursy.NazwaKursu;
                //ListaGrupZajeciowych[0];
            }
        }
    }
}
