﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;

namespace PlanZajec.Views
{
    /// <summary>
    ///     Interaction logic for WolneDniSetView.xaml
    /// </summary>
    public partial class WolneDniSetView : UserControl
    {
        public WolneDniSetView()
        {
            InitializeComponent();

            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                plany = new ObservableCollection<Plany>(uw.Plany.GetAll().ToList());
            }

            foreach (var plan in plany)
            {
                SelectPlanComboBox.Items.Add(plan.NazwaPlanu);
            }
        }

        public ObservableCollection<Plany> plany { get; }

        private void DodajWolneButton_OnClick(object sender, RoutedEventArgs e)
        {
            getSelectedPlan(SelectPlanComboBox.SelectedValue as string)
                .AddWolneDni(getBeaginingHour() + ":" + getEndHour() + ":" + getShortDay());
            InfoLabel.Content = "Dodano";
            var col = Colors.ForestGreen;
            Brush colorBrush = new SolidColorBrush(col);
            InfoLabel.Background = colorBrush;
        }

        private Plany getSelectedPlan(string name)
        {
            return plany.FirstOrDefault(plan => plan.NazwaPlanu == name);
        }

        private string getBeaginingHour()
        {
            var beginingHour = "";
            switch (GodzinaRozpoczeciaComboBox.Text)
            {
                case "7.30":
                    beginingHour = "07";
                    break;
                case "9.15":
                    beginingHour = "09";
                    break;
                case "11.15":
                    beginingHour = "11";
                    break;
                case "13.15":
                    beginingHour = "13";
                    break;
                case "15.15":
                    beginingHour = "15";
                    break;
                case "17.05":
                    beginingHour = "17";
                    break;
                case "18.55":
                    beginingHour = "19";
                    break;
            }
            return beginingHour;
        }

        private string getEndHour()
        {
            var endHour = "";
            switch (GodzinaZakonczeniaComboBox.Text)
            {
                case "9.00":
                    endHour = "09";
                    break;
                case "11.00":
                    endHour = "11";
                    break;
                case "13.00":
                    endHour = "13";
                    break;
                case "15.00":
                    endHour = "15";
                    break;
                case "16.55":
                    endHour = "17";
                    break;
                case "18.45":
                    endHour = "19";
                    break;
                case "20.35":
                    endHour = "21";
                    break;
            }
            return endHour;
        }

        private string getShortDay()
        {
            var dayShort = "";
            switch (DaySelectComboBox.SelectedIndex)
            {
                case 0:
                    dayShort = "pn";
                    break;
                case 1:
                    dayShort = "wt";
                    break;
                case 2:
                    dayShort = "śr";
                    break;
                case 3:
                    dayShort = "cz";
                    break;
                case 4:
                    dayShort = "pt";
                    break;
                case 5:
                    dayShort = "sb";
                    break;
                case 6:
                    dayShort = "nd";
                    break;
            }
            return dayShort;
        }
    }
}