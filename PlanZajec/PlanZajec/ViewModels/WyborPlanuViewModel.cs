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
        public List<Plany> Plany { get; private set; }

        public WyborPlanuViewModel()
        {
            using(var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                Plany = unitOfWork.Plany.GetAll().ToList();
            }
        }


    }
}
