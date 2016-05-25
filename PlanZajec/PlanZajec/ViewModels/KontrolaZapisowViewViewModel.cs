using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.ViewModels
{
    class KontrolaZapisowViewViewModel
    {
        public ObservableCollection<Plany> Plany { get; private set; }


        private static KontrolaZapisowViewViewModel instance = new KontrolaZapisowViewViewModel();

        public static KontrolaZapisowViewViewModel Instance
        {
            get { return instance; }
        }
    }
}
