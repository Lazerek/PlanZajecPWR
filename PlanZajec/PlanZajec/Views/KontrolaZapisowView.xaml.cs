using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for KontrolaZapisowView.xaml
    /// </summary>
    public partial class KontrolaZapisowView : UserControl
    {
        public KontrolaZapisowView()
        {
            InitializeComponent();
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
            }
        }
    }
}
