using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.ViewModels
{
    class WyborPlanuViewModel
    {
        public ObservableCollection<Plany> Plany { get; private set; }


        private static WyborPlanuViewModel instance = new WyborPlanuViewModel();

        public static WyborPlanuViewModel Instance
        {
            get { return instance; }
        }

        private WyborPlanuViewModel()
        {
            using(var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                Plany = new ObservableCollection<Plany>(unitOfWork.Plany.GetAll().ToList());
            }
        }

        public void UsunPlan(Plany plan)
        {
            int indexToDelete = -1;
            indexToDelete = Plany.IndexOf(plan);
            Plany.RemoveAt(indexToDelete);
        }

        public void DodajPlan(Plany plan)
        {
            Plany.Add(plan);
        }

    }
}
