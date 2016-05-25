﻿using System.Windows;

namespace PlanZajec.Views
{
    /// <summary>
    /// Klasa do obsługi okna edycji danych
    /// </summary>
    public partial class OknoDanychEdukacji : Window
    {
        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
        public OknoDanychEdukacji()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Klasa pobierająca dane
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sciagnij(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Klasa do anulowania działania
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Anuluj(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
