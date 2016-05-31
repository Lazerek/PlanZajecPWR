namespace PlanZajec.Parser
{
    /// <summary>
    /// Klasa GroupData zawierająca informacje o grupie zajęciowej
    /// </summary>
    internal class GroupData
    {
        public string KodGrupy { get; set; }
        public string KodKursu { get; set; }
        public string NazwaKursu { get; set; }
        public string Prowadzacy { get; set; }
        public string FormaZajec { get; set; }
        public string Data { get; set; }
        public string Miejsce { get; set; }
        public string Potok { get; set; }
        public string LiczbaMiejsc { get; set; }
        public string KodBloku { get; set; }
        public string NazwaBloku { get; set; }
        /// <summary>
        /// Konstruktor GroupDaty zawierający informacje o grupie zajęć, data i miejsce są pojedynyczmy stringiem
        /// </summary>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="prowadzacy">Prowadzący</param>
        /// <param name="formaZajec">Forma/typ zajęć</param>
        /// <param name="dataIMiejsce">Data i miejsce</param>
        /// <param name="potok">Potok</param>
        /// <param name="liczbaMiejsc">Liczba miejsca w grupie</param>
        /// <param name="kodBloku">Kod Bloku</param>
        /// <param name="nazwaBloku">Nazwa Bloku</param>
        public GroupData(string kodGrupy, string kodKursu, string nazwaKursu, string prowadzacy, string formaZajec,
            string dataIMiejsce, string potok, string liczbaMiejsc, string kodBloku, string nazwaBloku)
        {
            KodGrupy = kodGrupy;
            KodKursu = kodKursu;
            NazwaKursu = nazwaKursu;
            Prowadzacy = prowadzacy;
            FormaZajec = formaZajec;
            var arrDataIMiejsce = dataIMiejsce.Split(',');
            Data = arrDataIMiejsce[0];
            Miejsce = arrDataIMiejsce[1] + arrDataIMiejsce[2];
            Potok = potok;
            LiczbaMiejsc = liczbaMiejsc;
            KodBloku = kodBloku;
            NazwaBloku = nazwaBloku;
        }
        /// <summary>
        /// Konstruktor GroupDaty zawierający informacje o grupie zajęć, data i miejsce są osobnymi stringami
        /// </summary>
        /// <param name="kodGrupy">Kod Grupy</param>
        /// <param name="kodKursu">Kod Kursu</param>
        /// <param name="nazwaKursu">Nazwa Kursu</param>
        /// <param name="prowadzacy">Prowadzący</param>
        /// <param name="formaZajec">Forma/typ zajęć</param>
        /// <param name="data">Data odbywania się zajęć</param>
        /// <param name="miejsce">Miejsce odbywania się zajęć</param>
        /// <param name="potok">Potok</param>
        /// <param name="liczbaMiejsc">Liczba miejsca w grupie</param>
        /// <param name="kodBloku">Kod Bloku</param>
        /// <param name="nazwaBloku">Nazwa Bloku</param>
        public GroupData(string kodGrupy, string kodKursu, string nazwaKursu, string prowadzacy, string formaZajec,
            string data, string miejsce, string potok, string liczbaMiejsc, string kodBloku, string nazwaBloku)
        {
            KodGrupy = kodGrupy;
            KodKursu = kodKursu;
            NazwaKursu = nazwaKursu;
            Prowadzacy = prowadzacy;
            FormaZajec = formaZajec;
            Data = data;
            Miejsce = miejsce;
            Potok = potok;
            LiczbaMiejsc = liczbaMiejsc;
            KodBloku = kodBloku;
            NazwaBloku = nazwaBloku;
        }
        /// <summary>
        /// Domyślny konstruktor
        /// </summary>
        public GroupData()
        {}
        /// <summary>
        /// Metoda nadpisania metody ToString
        /// </summary>
        /// <returns>String zawierający najważniejsze informacje o grupie</returns>
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