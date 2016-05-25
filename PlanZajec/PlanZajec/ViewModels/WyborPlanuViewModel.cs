using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace PlanZajec.ViewModels
{
    /// <summary>
    /// Klasa pomagająca wybrać plan
    /// </summary>
    class WyborPlanuViewModel
    {
        public ObservableCollection<Plany> Plany { get; private set; }


        private static WyborPlanuViewModel instance = new WyborPlanuViewModel();
        /// <summary>
        /// Instancja wyboru planu
        /// </summary>
        public static WyborPlanuViewModel Instance
        {
            get { return instance; }
        }
        /// <summary>
        /// Domyślny konstruktor pobierający plany do bazy
        /// </summary>
        private WyborPlanuViewModel()
        {
            using(var unitOfWork = new UnitOfWork(new PlanPwrContext()))
            {
                Plany = new ObservableCollection<Plany>(unitOfWork.Plany.GetAll().ToList());
            }
        }
        /// <summary>
        /// Metoda usuwania planów
        /// </summary>
        /// <param name="plan">Plan zajęć</param>
        public void UsunPlan(Plany plan)
        {
            if (Plany.Count <= 1)
            {
                MessageBox.Show("Nie można usunąć jedynego planu");
                return;
            }
            int indexToDelete = -1;
            indexToDelete = Plany.IndexOf(plan);
            Plany.RemoveAt(indexToDelete);
        }
        /// <summary>
        /// Metoda dodawania planów
        /// </summary>
        /// <param name="plan">Plan zajęć</param>
        public void DodajPlan(Plany plan)
        {
            Plany.Add(plan);
        }

    }
}
