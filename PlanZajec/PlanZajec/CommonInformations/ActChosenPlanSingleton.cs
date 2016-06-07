using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.CommonInformations
{
    /// <summary>
    /// Klasa tworząca singleton planu
    /// </summary>
    class ActChosenPlanSingleton
    {
        private static ActChosenPlanSingleton instance = new ActChosenPlanSingleton();

        private ActChosenPlanSingleton() { }



        public static ActChosenPlanSingleton Instance
        {
            get
            {
                return instance;
            }
        }

        public long IdPlanu { get; private set; }
        /// <summary>
        /// Metoda ustawiająca plan
        /// </summary>
        /// <param name="plan">Plan</param>
        public void SetPlan(Plany plan)
        {
            IdPlanu = plan.IdPlanu; 
        }
        /// <summary>
        /// Metoda ustawiająca plan po idPlanu
        /// </summary>
        /// <param name="idPlanu">idPlanu</param>
        public void SetPlan(long idPlanu)
        {
            IdPlanu = idPlanu;
        }

    }
}
