using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanZajec.EdukacjaIntegration;

namespace TestyEdukacjiCL
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines;
            try
            {
                lines = File.ReadAllLines("../../daneKonta.txt");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Nie znaleziono pliku w projekcie!\n" +
                                  "Utwórz swój własny plik w głównym folderze tego projektu. W pierwszej lini umieść swój login w drugiej hasło\n" +
                                  "Pamiętaj, żeby nie wysyłać na gita swoich danych (upewnij sie, że przy zmianach plik z danymi nie jest w plikach śledzonych)");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Inny błąd");
                return;
            }

            if (lines.Length != 2)
            {
                Console.WriteLine("Błędna ilośc lini - popraw plik");
                return;
            }
            EdukacjaConnector edu = new EdukacjaConnector(lines[0],lines[1]);
            edu.Run();
        }
    }
}
