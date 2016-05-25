using PlanZajec.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PlanZajec.Views
{
    /// <summary>
    /// Interaction logic for OknoOpcji.xaml
    /// </summary>
    /// 
    public partial class OknoOpcji : Window
    {
        private PrzegladanieGrupViewModel viewModel;
        public OknoOpcji()
        {
            InitializeComponent();
        }

       

        private void buttonFiltr_Click(object sender, RoutedEventArgs e)
        {
             String potoczek;
             potoczek = textBoxFiltr.Text;
           //  PrzegladanieGrupViewModel.temp.przegladanieFiltrowanie(potoczek);
        }

        private void textBoxprowadzacy_TextChanged(object sender, TextChangedEventArgs e)
        {
           // PrzegladanieGrupViewModel.przegladanieGrupViewModel.PrzegladanieProwadzacy(textBoxprowadzacy.Text);
        }
    }
}
