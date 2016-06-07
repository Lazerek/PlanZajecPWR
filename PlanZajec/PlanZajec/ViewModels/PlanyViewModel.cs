using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanZajec.ViewModels
{
    /// <summary>
    /// Klasa będąca viewModelem planu, zawierająca plany
    /// </summary>
    class PlanyViewModel:ViewModel
    {
        public ObservableCollection<Plany> Plany { get; private set; }


        /// <summary>
        /// Instancja wyboru planu
        /// </summary>
        public static PlanyViewModel Instance { get; } = new PlanyViewModel();

        /// <summary>
        /// Domyślny konstruktor pobierający plany do bazy
        /// </summary>
        private PlanyViewModel()
        {
            using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
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

