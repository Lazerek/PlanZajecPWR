using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlanZajec.Parser
{
    class Parser
    {
        public static bool Run()
        {
            var folderPath =
                @"../../TestResources";
            var fileEntries = Directory.GetFiles(folderPath);
            List<string[]> groupArray = new List<string[]>();
            foreach (var fullFileText in fileEntries.Select(File.ReadAllLines))
            {
                foreach (var rawGroup in GetGroupRawData(fullFileText))
                {
                    groupArray.Add(rawGroup);
                }
            }
            var datas = new List<GroupData>();

            if (groupArray.Count==0) return false;
            datas.AddRange(groupArray.Select(SearchLine));
            foreach (var gd in datas)
            {
                ZapisDoBazy.zapisz(gd);
            }
            return true;
        }

        static GroupData SearchLine(string[] lines)
        {
            var startLooking = false;
            var index = 0;
            var temporary = new GroupData();
            for (var i = 0; i < lines.Length; i++)
            {
                if (startLooking)
                {
                    switch (i - index)
                    {
                        //kod grupy
                        case 2:
                            temporary.KodGrupy = lines[i].Trim();
                            break;
                        //kod kursu
                        case 5:
                            temporary.KodKursu = lines[i].Trim();
                            break;
                        //nazwa kursu
                        case 8:
                            temporary.NazwaKursu = lines[i].Trim();
                            break;
                        //liczba miejsc
                        case 13:
                            temporary.LiczbaMiejsc = lines[i].Trim();
                            break;
                        //prowadzacy
                        case 46:
                            temporary.Prowadzacy = Regex.Replace(lines[i].Trim(), @"\s+", " ");
                            break;
                        //forma zajęc
                        case 49:
                            temporary.FormaZajec = lines[i].Trim();
                            break;
                        //potok
                        case 61:
                            temporary.Potok = lines[i].Trim();
                            break;
                        //data i miejsce
                        case 66:
                            int indextd = lines[i].IndexOf("<td>", StringComparison.Ordinal);
                            int indextd2 = lines[i].IndexOf("</td>", StringComparison.Ordinal);
                            var arrDataIMiejsce = lines[i].Substring(indextd + 5, indextd2 - indextd - 5).Split(',');
                            string data = arrDataIMiejsce[0];
                            if (arrDataIMiejsce.Length > 2)
                            {
                                string miejsce = arrDataIMiejsce[1] + arrDataIMiejsce[2];
                                temporary.Miejsce = miejsce;
                            }
                            temporary.Data = data;
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
            return temporary;
        }

        static List<string[]> GetGroupRawData(string[] textArray)
        {
            Regex regex = new Regex(@"<a name=.hrefGrupyZajecioweKursuTabela\d{6}.> </a>");
            List<string[]> result = new List<string[]>();

            var indexes = new List<int>();

            for (int i = 0; i < textArray.Length; i++)
            {
                Match mt = regex.Match(textArray[i]);
                if (mt.Success)
                {
                    indexes.Add(i);
                }
            }
            //var groupSize = indexes[1] - indexes[0];
            var groupSize = 87;
            for (int i = 0; i < indexes.Count; i++)
            {
                string[] partOfResult = new string[groupSize];
                for (int j = 0; j < groupSize; j++)
                {
                    partOfResult[j] = textArray[indexes[i] + j];
                }
                result.Add(partOfResult);
            }
            return result;
        }
    }
}