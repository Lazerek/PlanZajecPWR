﻿using PlanZajec.DataModel;
using System;
using System.Collections.Generic;
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

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for WyborPlanu.xaml
    /// </summary>
    public partial class WyborPlanu : UserControl
    {
        public delegate void ChosenPlanToShow(Plany plan);

        public event ChosenPlanToShow ChosenPlanToShowEventHandler;

        public WyborPlanu()
        {
            InitializeComponent();
        }

        private void ZmienKolorMouseEnter(object sender, MouseEventArgs e)
        {
            KafelPlanu plan = sender as KafelPlanu;
            plan.WypelnieniePlansza.Background = new SolidColorBrush(Colors.GreenYellow);
            
        }

        private void ZmienKolorMouseLeave(object sender, MouseEventArgs e)
        {
            KafelPlanu plan = sender as KafelPlanu;
            plan.WypelnieniePlansza.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void ButtonPlanMouseEnter(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Transparent);
        }


        private void WywierzPlan(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Transparent);
            if(ChosenPlanToShowEventHandler != null)
            {
                ChosenPlanToShowEventHandler(button.DataContext as Plany);
            }
        }
    }
}
