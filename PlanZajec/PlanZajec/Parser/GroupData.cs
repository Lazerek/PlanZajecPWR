namespace TestPlanParser
{
    internal class GroupData
    {
        public string KodGrupy { get; set; }
        public string KodKursu { get; set; }
        public string NazwaKursu { get; set; }
        public string Prowadzacy { get; set; }
        public string FormaZajec { get; set; }
        public string Data { get; set; }
        public string Miejsce { get; set; }

        public GroupData(string kodGrupy, string kodKursu, string nazwaKursu, string prowadzacy, string formaZajec,
            string dataIMiejsce)
        {
            KodGrupy = kodGrupy;
            KodKursu = kodKursu;
            NazwaKursu = nazwaKursu;
            Prowadzacy = prowadzacy;
            FormaZajec = formaZajec;
            var arrDataIMiejsce = dataIMiejsce.Split(',');
            Data = arrDataIMiejsce[0];
            Miejsce = arrDataIMiejsce[1] + arrDataIMiejsce[2];
        }

        public GroupData(string kodGrupy, string kodKursu, string nazwaKursu, string prowadzacy, string formaZajec,
            string data, string miejsce)
        {
            KodGrupy = kodGrupy;
            KodKursu = kodKursu;
            NazwaKursu = nazwaKursu;
            Prowadzacy = prowadzacy;
            FormaZajec = formaZajec;
            Data = data;
            Miejsce = miejsce;
        }
        public GroupData()
        {}
        public override string ToString()
        {
            return "Kod grupy: " + KodGrupy
                   + " Kod kursu: " + KodKursu
                   + " Nazwa kursu: " + NazwaKursu
                   + " Prowadzący: " + Prowadzacy
                   + " Forma zajęć: " + FormaZajec
                   + " Data: " + Data
                   + " Miejsce: " + Miejsce;
        }
    }
}