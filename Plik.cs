using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlownikObcy
{
    class Plik
    {
        public struct Dane                     //Struktura wykorzystana do przechowywania informacji o haśle słownikowym
        {
            public string obcy;
            public string polski;
        };

        public readonly List<Dane> BazaNazw = new List<Dane>();

        public Plik(string Nazwa)
        {
            System.IO.StreamReader plik = new StreamReader(Nazwa);


            string linia;
            Dane temp;
            BazaNazw.Clear();
            //_________ZAŁADOWANIE Z PLIKU___________

            while ((linia = plik.ReadLine()) != null)                           //Dopóki są zapełnione linie tekstu
            {
                string[] linie = linia.Split(';');                              //Tablica przechowuje pocięte średnikami linie tekstu
                string obcy = linie[0];                                        //Poszczególne elementy linii przechowują odpowiednie dane
                string polski = linie[1];
                temp.obcy = obcy;
                temp.polski = polski;
                BazaNazw.Add(temp);
            }

            plik.Close();
        }
        public Plik ()
        {

        }

        public void Zapisz(string Nazwa)            //Zapis do pliku
        {
            System.IO.StreamWriter plik = new StreamWriter(Nazwa);

            BazaNazw.Sort((s1, s2) => s1.obcy.CompareTo(s2.obcy));      //Sortowanie pod względem obcego słowa
            foreach(Dane haslo in BazaNazw)                             
            {
                plik.WriteLine(haslo.obcy + ";" + haslo.polski);
            }

            plik.Close();                                               
        }
    }
}
