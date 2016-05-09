using PlanZajec.ViewModels;
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

        private Dictionary<long, PlanViewModel> miejscePrzechowywania;


        private ObslugaWidokuWieluPlanów() {
            miejscePrzechowywania = new Dictionary<long, PlanViewModel>();
        }

        public static ObslugaWidokuWieluPlanów Instance
        {
            get
            {
                return instance;
            }
        }

        public PlanViewModel getPlanViewModel(long idPlanu)
        {
            PlanViewModel result;
            bool zaaGregowane = miejscePrzechowywania.TryGetValue(idPlanu, out result);
            if (!zaaGregowane)
            {
                result = new PlanViewModel(idPlanu);
                //miejscePrzechowywania.Add(idPlanu, result); TODO
            }
            return result;
        }

        public bool deletePlanViewModel(long idPlanu)
        {
            if (miejscePrzechowywania.ContainsKey(idPlanu)){
                miejscePrzechowywania.Remove(idPlanu);
                return true;
            }
            return false;
        }



    }
}
