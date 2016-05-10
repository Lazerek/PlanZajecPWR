using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlanZajec.Parser
{
    internal class Parser
    {
        public static bool Run()
        {
            //folder with files path
            var folderPath = @"../../TestResources";
            //list of all files in folder
            var fileEntries = Directory.GetFiles(folderPath);
            //foreach file content, main functions in for loop to reduce List type variables
            foreach (var singleFileEntrie in fileEntries)
            {
                //read file
                var fullFileText = File.ReadAllLines(singleFileEntrie);
                //ready clear groupArray
                //get new blockData
                var blockData = TryGetBlockData(fullFileText);
                //get raw groups data
                var groupArray = GetGroupRawData(fullFileText).ToList();
                //if no groups then go out
                if (groupArray.Count == 0) return false;
                //iterate trougth groupArray and call SearchLine for each string[] lines
                var datas = groupArray.Select(lines => SearchLine(lines, blockData)).ToList();
                //for each group call ZapisDoBazy
                System.Diagnostics.Debug.WriteLine(datas.Count());
                foreach (var gd in datas)
                {
                    ZapisDoBazy.zapisz(gd);
                }
               
            }
            return true;
        }

        private static GroupData SearchLine(string[] lines, string[] blockStrings)
        {
            //flag that forces second part of if run first
            var startLooking = false;
            //begining search index
            var index = 0;
            //
            var result = new GroupData();
            for (var i = 0; i < lines.Length; i++)
            {
                if (!startLooking)
                {
                    
                }
                if (startLooking)
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
                else
                {
                    if (lines[i].Contains("hrefGrupyZajecioweKursuTabela"))
                    {
                        index = i;
                        startLooking = true;
                    }
                }
            }
            //przypisanie stringa zawierającego kod bloku do obiektu, jeśli null to podaj string pusty
            result.KodBloku = blockStrings==null ? "" : blockStrings[0];
            //przypisanie stringa zawierającego nazwe bloku do obiektu, jeśli null to podaj string pusty
            result.NazwaBloku = blockStrings == null ? "" : blockStrings[1];
            return result;
        }

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

        private static string[] TryGetBlockData(string[] textArrayStrings)
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
            var partialResultTab = textArrayStrings.Skip(beginingOfSubarray).Take(numberToTake).ToArray();
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
            return new[]{blockCode, blockName};
        }
    }
}