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
        private static string relativePath = "../../daneKonta.txt";

        static void Main(string[] args)
        {
            string[] lines;
            if (!File.Exists(relativePath))
            {
                Console.WriteLine("Nie znaleziono pliku w projekcie!\n" +
                   "Utwórz swój własny plik w głównym folderze tego projektu. W pierwszej lini umieść swój login w drugiej hasło\n" +
                   "Pamiętaj, żeby nie wysyłać na gita swoich danych (upewnij sie, że przy zmianach plik z danymi nie jest w plikach śledzonych)");
                return;
            }
            try
            {
                lines = File.ReadAllLines(relativePath);
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
