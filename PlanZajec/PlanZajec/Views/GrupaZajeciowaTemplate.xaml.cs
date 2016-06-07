using PlanZajec.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;
using System.Windows.Input;

namespace PlanZajec.Views
{
    /// <summary>
    /// Template grupy zajęciowej używany do planview
    /// </summary>
    public partial class GrupaZajeciowaTemplate : UserControl
    {
        private GrupaTamplateViewModel viewModel;
        /// <summary>
        /// Tworzy instancję kafelka grupy zajęciowej wyświetlanego w planview
        /// </summary>
        /// <param name="viewModel">ViewModel grupy zajęciowej</param>
        public GrupaZajeciowaTemplate(GrupaTamplateViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            Start.Text = viewModel.GrupZaj.Godzina;
            Koniec.Text = viewModel.GrupZaj.GodzinaKoniec;

            Nazwa.Text = viewModel.nazwa;
            Nazwa.ToolTip = viewModel.nazwa;

            if(!viewModel.GrupZaj.Tydzien.Equals("//"))
                Tydzen.Text = viewModel.GrupZaj.Tydzien;
            Budynek.Text = viewModel.GrupZaj.Budynek;
            Sala.Text = viewModel.GrupZaj.Sala;
            String typZajec = viewModel.GrupZaj.TypZajec;
            Color colResult;
            switch (typZajec)
            {
                case "Wykład": colResult = Colors.Green; break;
                case "Zajęcia laboratoryjne": colResult = Colors.Orange; break;
                case "Projekt": colResult = Colors.Orchid; break;
                case "Seminarium": colResult = Colors.LightBlue; break;
                case "Praktyka": colResult = Colors.Yellow; break;
                default: colResult = Colors.Black; break;
            }
            SolidColorBrush solidResult = new SolidColorBrush(colResult);
            Kafel.Background = solidResult;
        }
        /// <summary>
        /// Metoda reagująza na zmianę rozmiaru
        /// </summary>

        private void GrupaZajeciowaTemplate_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Nazwa.MaxWidth = e.NewSize.Width;
        }
        /// <summary>
        /// Metoda pozwalajaca usunąć kafelek z planu przy pomocy podwójnego przyciśnięcia myszki
        /// </summary>
        private void Usun_DoubleClik(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                using (var unitOfWork = new UnitOfWork(new PlanPwrContext()))
                {
                    unitOfWork.Plany.UsunGrupeZajeciowaZPlanu(
                        ((GrupaTamplateViewModel) ((Grid) sender).DataContext).GrupZaj);
                    PlanView.RefreshSchedule();
                }
            }
        }
    }
}
