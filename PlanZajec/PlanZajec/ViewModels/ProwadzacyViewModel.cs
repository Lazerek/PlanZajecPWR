using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PlanZajec.DataAccessLayer;
using PlanZajec.DataModel;

namespace PlanZajec.ViewModels
{
    public class ProwadzacyViewModel : INotifyPropertyChanged
    {
        public List<Prowadzacy> Items { get; set; }

        public ProwadzacyViewModel()
        {
            
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                Items = uw.Prowadzacy.GetAll().ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void FiltrujProwadzacych(string str)
        {
            using (var uw = new UnitOfWork(new PlanPwrContext()))
            {
                List<Prowadzacy> pro = uw.Prowadzacy.GetAll().ToList();
                List<Prowadzacy> temp =new List<Prowadzacy>();
                str = Regex.Replace(str, @"\s+", " ");
                string[] arrStr = str.Trim().Split(' ');
                //
                var firstWord = true;
                foreach (var prowTemp in arrStr)
                {
                    for (int i = 0; i < pro.Count(); i++)
                    {
                        if (pro.ElementAt(i).Nazwisko.StartsWith(prowTemp, StringComparison.OrdinalIgnoreCase)
                            || pro.ElementAt(i).Imie.StartsWith(prowTemp, StringComparison.OrdinalIgnoreCase))
                        {
                            if (!temp.Contains(pro.ElementAt(i)) && firstWord)
                            {
                                temp.Add(pro.ElementAt(i));
                            }
                        }
                        else
                        {
                            if (temp.Contains(pro.ElementAt(i)))
                            {
                                temp.Remove(pro.ElementAt(i));
                            }
                        }
                    }
                    firstWord = false;
                }
                Items = temp;
                NotifyPropertyChanged("Items");
            }
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

