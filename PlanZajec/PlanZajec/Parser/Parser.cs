using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace PlanZajec.Parser
{
    /// <summary>
    /// /Klasa służąca do parsowania danych z plików  html
    /// </summary>
    internal class Parser
    {
        public static string FolderPath { get; set; } // = @"../../TestResources";

        /// <summary>
        /// Metoda wczytująca plik i wywołująca zapis danych do bazy
        /// </summary>
        /// <returns>Powodzenia metody</returns>
        public static bool Run()
        {
            var fileEntries = Directory.GetFiles(FolderPath);
            //foreach file content, main functions in for loop to reduce List type variables
            foreach (var singleFileEntrie in fileEntries)
            {
                var fullTextInFile = File.ReadAllLines(singleFileEntrie);
                if (!RunParserForAllLine(fullTextInFile)) return false;
            }
            return true;
        }

        public static bool RunParserForAllLine(string[] fullThmlTextInFile)
        {
            //ready clear groupArray
            //TryIt(fullThmlTextInFile);
            Block blockOfCourse = TryGetBlockOfCourse(fullThmlTextInFile);
            var groupArray = GetGroupRawData(fullThmlTextInFile).ToList();
            if (groupArray.Count == 0) return false;
            //iterate trougth groupArray and call SearchLine for each string[] lines
            var datas = groupArray.Select(lines => SearchLine(lines, blockOfCourse)).ToList();
            System.Diagnostics.Debug.WriteLine(datas.Count());
            foreach (var gd in datas)
            {
                ZapisDoBazy.zapisz(gd);
            }
            return true;
        }

        /// <summary>
        /// Metoda Parsująca pojedynczy plik html
        /// </summary>
        /// <param name="lines">Linie pliku</param>
        /// <param name="block">Nazwa i kod bloku</param>
        /// <returns>Objekt klasy groupdata zawierające dane o grupie</returns>
        private static GroupData SearchLine(string[] lines, Block block)
        {
            var index = GetIndexOfFirstSignificantLine(lines);
            if (index < 0)
                return null; //TODO null or nothing
            var result = new GroupData();
            for (var i = index; i < lines.Length; i++)
            {
                switch (i - index)
                {
                    //kod grupy
                    case 2:
                        result.KodGrupy = lines[i].Trim();
                        break;
                    //kod kursu
                    case 5:
                        result.KodKursu = lines[i].Trim();
                        break;
                    //nazwa kursu
                    case 8:
                        result.NazwaKursu = lines[i].Trim();
                        break;
                    //liczba miejsc
                    case 13:
                        result.LiczbaMiejsc = lines[i].Trim();
                        break;
                    //prowadzacy
                    case 46:
                        result.Prowadzacy = Regex.Replace(lines[i].Trim(), @"\s+", " ");
                        break;
                    //forma zajęc
                    case 49:
                        result.FormaZajec = lines[i].Trim();
                        break;
                    //potok
                    case 61:
                        result.Potok = lines[i].Trim();
                        break;
                    //data i miejsce
                    case 66:
                        var indextd = lines[i].IndexOf("<td>", StringComparison.Ordinal);
                        var indextd2 = lines[i].IndexOf("</td>", StringComparison.Ordinal);
                        var arrDataIMiejsce = lines[i].Substring(indextd + 5, indextd2 - indextd - 5).Split(',');
                        var data = arrDataIMiejsce[0];
                        if (arrDataIMiejsce.Length > 2)
                        {
                            var miejsce = arrDataIMiejsce[1] + arrDataIMiejsce[2];
                            result.Miejsce = miejsce;
                        }
                        result.Data = data;
                        break;
                }
            }
            //przypisanie stringa zawierającego kod bloku do obiektu, jeśli null to podaj string pusty
            result.KodBloku = block==null ? "" : block.Code;
            //przypisanie stringa zawierającego nazwe bloku do obiektu, jeśli null to podaj string pusty
            result.NazwaBloku = block == null ? "" : block.Name;
            return result;
        }

        private static int GetIndexOfFirstSignificantLine(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("hrefGrupyZajecioweKursuTabela"))
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// Metoda szukająca grup zajęciowych
        /// </summary>
        /// <param name="textArray">Wszystkie linie pliku</param>
        /// <returns>Lista bloków gotowych do parsowania</returns>
        private static List<string[]> GetGroupRawData(string[] textArray)
        {
            var regex = new Regex(@"<a name=.hrefGrupyZajecioweKursuTabela\d{6}.> </a>");
            var result = new List<string[]>();

            var indexes = new List<int>();

            for (var i = 0; i < textArray.Length; i++)
            {
                var mt = regex.Match(textArray[i]);
                if (mt.Success)
                {
                    indexes.Add(i);
                }
            }
            //var groupSize = indexes[1] - indexes[0];
            var groupSize = 87;
            for (var i = 0; i < indexes.Count; i++)
            {
                var partOfResult = new string[groupSize];
                for (var j = 0; j < groupSize; j++)
                {
                    partOfResult[j] = textArray[indexes[i] + j];
                }
                result.Add(partOfResult);
            }
            return result;
        }

        private static Block TryGetBlockOfCourse(string[] textArrayStrings)
        {
            var regex = new Regex(@"<a name=.hrefKursyGrupyBlokiTabelaBlok\d{6,}.> </a>");
            var beginingOfSubarray = -1;
            for (var i = 0; i < textArrayStrings.Length; i++)
            {
                if (regex.Match(textArrayStrings[i]).Success)
                {
                    beginingOfSubarray = i;
                }
            }
            if (beginingOfSubarray < 0)
                return null;
            var numberToTake =  26;
            var beginTrimLines = textArrayStrings.Skip(beginingOfSubarray);
            var partialResultTab = beginTrimLines.Take(numberToTake).ToArray();
            var blockCode = "";
            var blockName = "";
            if (partialResultTab[14].Contains("INZ"))
            {
                blockCode = partialResultTab[14].Trim();
                //cell 14 - code
                blockName = partialResultTab[23].Trim();
                //cell 23 - name
            }
            else
            {
                blockCode = partialResultTab[15].Trim();
                //cell 14 - code
                blockName = partialResultTab[24].Trim();
                //cell 23 - name
            }
            return new Block(blockCode, blockName);
        }

        class Block
        {
            public string Code { get; set; }

            public string Name { get; set; }

            public Block(string code, string name)
            {
                Code = code;
                Name = name;
            }
        }

        public static void TryIt(string[] array)
        {
            HtmlDocument document2 = new HtmlDocument();
            StringBuilder builder = new StringBuilder();
            foreach (string line in array)
            {
                builder.Append(line+"\n");
            }
            document2.LoadHtml(builder.ToString());
            var cos = document2.DocumentNode.SelectNodes("//a");

            foreach (var node in cos)
            {
                if (node.Attributes["name"] != null)
                {
                    System.Diagnostics.Debug.WriteLine(node.Attributes["name"].Value);
                }
            }
            var cos2 = cos.SkipWhile(aNode => 
                aNode.Attributes["name"] == null
                || aNode.Attributes["name"].Value.StartsWith("hrefKursyGrupyBlokiTabelaBlok"));
            
            var cos3 = cos2.ElementAt(1);
            var cos4 = cos3.ChildNodes;
            foreach (var node in cos4)
            {
                if (node.InnerText != null)
                {
                    System.Diagnostics.Debug.WriteLine(node.InnerText);
                }
            }
        }
    }
}