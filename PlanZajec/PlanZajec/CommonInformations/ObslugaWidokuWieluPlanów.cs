using PlanZajec.ViewModels;
using PlanZajec.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.CommonInformations
{
    /// <summary>
    /// klasa wspomagająca obsługę wielu planów
    /// </summary>
    public class ObslugaWidokuWieluPlanów
    {
        private static ObslugaWidokuWieluPlanów instance = new ObslugaWidokuWieluPlanów();

        private Dictionary<long, PlanView> miejscePrzechowywania;

        /// <summary>
        /// Konstruktor tworzący słownik przechowujący wiele planów
        /// </summary>
        private ObslugaWidokuWieluPlanów() {
            miejscePrzechowywania = new Dictionary<long, PlanView>();
        }

        public static ObslugaWidokuWieluPlanów Instance
        {
            get
            {
                return instance;
            }
        }
        /// <summary>
        /// Metoda zwaracają widok planu i dodająca go do słownika
        /// </summary>
        /// <param name="idPlanu">id poszukiwanego planu</param>
        /// <returns>Widok Planu</returns>
        public PlanView getPlanView(long idPlanu)
        {
            PlanView result;
            bool zaaGregowane = miejscePrzechowywania.TryGetValue(idPlanu, out result);
            if (!zaaGregowane)
            {
                result = new PlanView(new PlanViewModel(idPlanu));
                miejscePrzechowywania.Add(idPlanu, result); //TODO
            }
            return result;
        }
        /// <summary>
        /// Metoda usuwająca plan view ze słownika
        /// </summary>
        /// <param name="idPlanu">id poszukiwanego planu</param>
        /// <returns>Informacja o powodzeniu operacji</returns>
        public bool deletePlanView(long idPlanu)
        {
            if (miejscePrzechowywania.ContainsKey(idPlanu)){
                miejscePrzechowywania.Remove(idPlanu);
                return true;
            }
            return false;
        }



    }
}
