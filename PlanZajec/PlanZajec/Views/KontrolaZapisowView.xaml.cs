using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using PlanZajec.ViewModels;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa odpowiedzialna za kontrolę zapisów
    /// </summary>
    public partial class KontrolaZapisowView : UserControl
    {
        /// <summary>
        /// Domyślny konstruktor, sprawdza poprawność zapisu
        /// </summary>
        public KontrolaZapisowView()
        {
            InitializeComponent();
            this.DataContext = PlanyViewModel.Instance;
            /*
            List<Expander> schedulesExpanders = new List<Expander>();
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            { 
                var schedules = uw.Plany.GetAll().ToList();
                foreach (var schedule in schedules)
                {
                    Expander exp = new Expander();
                    exp.Header = "Kontrola planu nr " + schedule.IdPlanu;
                    StackPanel tpanel = new StackPanel();

                    var allCourses = uw.Kursy.GetAll().ToList();
                    var groupsInScheduleCodes = schedule.GrupyZajeciowe.Select(grupa => grupa.KodKursu).ToList();

                    foreach (var course in allCourses)
                    {
                        Color inScheduleColor;
                        if (groupsInScheduleCodes.Contains(course.KodKursu))
                        {
                            inScheduleColor = Colors.Green;
                        }
                        else
                        {
                            inScheduleColor = Colors.DarkRed;
                        }
                        TextBlock courseTextBlock = new TextBlock {Background = new SolidColorBrush(inScheduleColor), Text = course.NazwaKursu};

                        tpanel.Children.Add(courseTextBlock);
                    }

                    exp.Content = tpanel;
                    KontroleZapisowPanel.Children.Add(exp);
                }
                */
        }
    }
}
