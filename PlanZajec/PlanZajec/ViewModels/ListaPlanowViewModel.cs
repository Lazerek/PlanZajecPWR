using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace PlanZajec.ViewModels
{
    /// <summary>
    /// Klasa typu ViewModel zawierająca listę planów
    /// </summary>
    class ListaPlanowViewModel
    {
        public ObservableCollection<Plany> Items { get; set; }
        /// <summary>
        /// Domyślny konstruktor pobierający listę planów z bazy
        /// </summary>
        public ListaPlanowViewModel()
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = new ObservableCollection<Plany>(uw.Plany.GetAll().ToList());
            }
        }
    }
}
