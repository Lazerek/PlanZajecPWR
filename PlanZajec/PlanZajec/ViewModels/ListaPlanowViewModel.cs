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
    class ListaPlanowViewModel
    {
        public ObservableCollection<Plany> Items { get; set; }
        public ListaPlanowViewModel()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = new ObservableCollection<Plany>(uw.Plany.GetAll().ToList());
            }
        }
    }
}
