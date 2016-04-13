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
    public class ProwadzacyOpinieViewModel : INotifyPropertyChanged
    {
        public List<Prowadzacy> Items { get; set; }
        public List<Prowadzacy> ComboBoxItems { get; set; }

        public ProwadzacyOpinieViewModel()
        {
            ComboBoxItems = new List<Prowadzacy>();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                ComboBoxItems = uw.Prowadzacy.GetAll().ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

