using System.Linq;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace PlanZajec.ViewModels
{
    /// <summary>
    /// Klasa pozwalająća usuwać plany
    /// </summary>
    public class UsunPlanViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Plany> Items { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public static UsunPlanViewModel instance = new UsunPlanViewModel();
        /// <summary>
        /// Instancja usuwania planu
        /// </summary>
        public static UsunPlanViewModel Instance
        {
            get { return instance; }
        }
        /// <summary>
        /// Domyślny konstruktor pobierająca plany
        /// </summary>
        public UsunPlanViewModel()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = new ObservableCollection<Plany>(uw.Plany.GetAll().ToList());
            }
        }
        /// <summary>
        /// Metoda usuwania konkretnego planu
        /// </summary>
        /// <param name="p">Plan</param>
        public void UsunPlan(Plany p)
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
        /// <summary>
        /// Metoda dodająca plan
        /// </summary>
        /// <param name="plan">Plan</param>
        public void DodajPlan(Plany plan)
        {
            Items.Add(plan);
        }
        /// <summary>
        /// Metoda zmieniająca własności
        /// </summary>
        /// <param name="propertyName">Nazwa własności</param>
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
