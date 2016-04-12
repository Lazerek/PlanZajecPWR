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
        public List<string> NazwyProwadzacych { get; set; } 

        public ProwadzacyViewModel()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.Prowadzacy.GetAll().ToList();
                NazwyProwadzacych = new List<string>();
                foreach (var prow in Items)
                {
                    NazwyProwadzacych.Add(prow.Imie + " " + prow.Nazwisko + " " + prow.Tytul);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

