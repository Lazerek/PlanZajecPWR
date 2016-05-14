using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace PlanZajec.ViewModels
{
    public class UsunPlanViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Plany> Items { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public UsunPlanViewModel()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = new ObservableCollection<Plany>(uw.Plany.GetAll().ToList());
            }
        }

        public void usunPlan(Plany p)
        {
            int indexToDelete = -1;
            indexToDelete = Items.IndexOf(p);
            Items.RemoveAt(indexToDelete);
            using (var unit = new UnitOfWork(new PlanPwrContext()))
            {
                unit.Plany.Remove(p);
                unit.SaveChanges();
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
