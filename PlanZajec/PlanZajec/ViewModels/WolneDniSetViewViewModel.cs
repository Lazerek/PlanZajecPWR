using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.ViewModels
{
    class WolneDniSetViewViewModel
    {
        public ObservableCollection<Plany> Plany { get; private set; }


        private static WolneDniSetViewViewModel instance = new WolneDniSetViewViewModel();

        public static WolneDniSetViewViewModel Instance
        {
            get { return instance; }
        }
    }
}
