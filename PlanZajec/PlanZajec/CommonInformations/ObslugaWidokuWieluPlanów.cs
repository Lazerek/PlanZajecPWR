using PlanZajec.ViewModels;
using PlanZajec.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.CommonInformations
{
    public class ObslugaWidokuWieluPlanów
    {
        private static ObslugaWidokuWieluPlanów instance = new ObslugaWidokuWieluPlanów();

        private Dictionary<long, PlanView> miejscePrzechowywania;


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
