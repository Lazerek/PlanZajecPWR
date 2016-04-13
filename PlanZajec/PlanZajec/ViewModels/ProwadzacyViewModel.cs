using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;

namespace PlanZajec.ViewModels
{
    public class ProwadzacyViewModel : INotifyPropertyChanged
    {
        public List<Prowadzacy> Items { get; set; }

        public ProwadzacyViewModel()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.Prowadzacy.GetAll().ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

