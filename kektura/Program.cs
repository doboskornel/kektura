using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dolgozat_kektura
{
    class Turaclass
    {
        string kiindulo;
        string vegpont;
        int hossz; //meterben
        int emelkedes;
        int lejtes;
        bool pecsetelo;

        public Turaclass(string sor)
        {
            string[] spl = sor.Split(';');
            Kiindulo = spl[0];
            Vegpont = spl[1];
            string[] hspl = spl[2].Split(',');
            Hossz = Convert.ToInt32(hspl[0]) * 1000 + Convert.ToInt32(hspl[1]);
            Emelkedes = Convert.ToInt32(spl[3]);
            Lejtes = Convert.ToInt32(spl[4]);
            if (spl[5] == "i")
            {
                Pecsetelo = true;
            }
            else
            {
                Pecsetelo = false;
            }
        }

        public string Kiindulo { get => kiindulo; set => kiindulo = value; }
        public string Vegpont { get => vegpont; set => vegpont = value; }
        public int Hossz { get => hossz; set => hossz = value; }
        public int Emelkedes { get => emelkedes; set => emelkedes = value; }
        public int Lejtes { get => lejtes; set => lejtes = value; }
        public bool Pecsetelo { get => pecsetelo; set => pecsetelo = value; }
    }


    class Program
    {
        static bool HianyosNev(Turaclass tc)
        {
            if (tc.Pecsetelo && tc.Vegpont.Contains("pecsetelohely") == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static void Main(string[] args)
        {
            int tengerszintfeletti = 0;
            List<Turaclass> turaclasslista = new List<Turaclass>();
            StreamReader sr = new StreamReader("kektura.csv", Encoding.UTF8);
            tengerszintfeletti = Convert.ToInt32(sr.ReadLine());
            while (!sr.EndOfStream)
            {
                turaclasslista.Add(new Turaclass(sr.ReadLine()));
            }
            Console.WriteLine("3. feladat: Szakaszok száma: " + turaclasslista.Count + " db");
            int osszhossz = 0;
            for (int i = 0; i < turaclasslista.Count; i++)
            {
                osszhossz += turaclasslista[i].Hossz;
            }
            Console.WriteLine("4. feladat: A túra teljes hossza: " + osszhossz / 1000 + "," + osszhossz % 1000 + " km");

            int legrovidebb = 0;
            for (int i = 0; i < turaclasslista.Count; i++)
            {
                if (turaclasslista[i].Hossz < turaclasslista[legrovidebb].Hossz)
                {
                    legrovidebb = i;
                }
            }
            Console.WriteLine("5. feladat: A legrövidebb útszakasz adatai:");
            Console.WriteLine("        Kezdete: " + turaclasslista[legrovidebb].Kiindulo);
            Console.WriteLine("        Vége: " + turaclasslista[legrovidebb].Vegpont);
            Console.WriteLine("        Távolság: " + turaclasslista[legrovidebb].Hossz / 1000 + "," + turaclasslista[legrovidebb].Hossz % 1000 + " km");

            Console.WriteLine("7. feladat: Hiányzó állomásnevek:");
            List<int> hianyosaklista = new List<int>();
            for (int i = 0; i < turaclasslista.Count; i++)
            {
                if (HianyosNev(turaclasslista[i]))
                {
                    hianyosaklista.Add(i);
                }
            }
            if (hianyosaklista.Count == 0)
            {
                Console.WriteLine("Nincs hiányos állomásnév");
            }
            else
            {
                for (int i = 0; i < hianyosaklista.Count; i++)
                {
                    Console.WriteLine("        " + turaclasslista[hianyosaklista[i]].Vegpont);
                }
            }

            int magassag = tengerszintfeletti;
            int maxmagassag = 0;
            int legmagasabbpont = 0;
            for (int i = 0; i < turaclasslista.Count; i++)
            {
                magassag += turaclasslista[i].Emelkedes - turaclasslista[i].Lejtes;
                if (magassag > maxmagassag)
                {
                    maxmagassag = magassag;
                    legmagasabbpont = i;
                }
            }
            Console.WriteLine("8. feladat: A túra legmagasabban fekvő végpontja:");
            Console.WriteLine("        A végpont neve: " + turaclasslista[legmagasabbpont].Vegpont);
            Console.WriteLine("        A végpont tengerszint feletti magassága: " + maxmagassag + " m");

            for (int i = 0; i < hianyosaklista.Count; i++)
            {
                turaclasslista[hianyosaklista[i]].Vegpont = turaclasslista[hianyosaklista[i]].Vegpont + " pecsetelohely";
            }

            StreamWriter sw = new StreamWriter("kektura2.csv");
            sw.WriteLine(Convert.ToString(tengerszintfeletti));
            string s = "";
            foreach (Turaclass tc in turaclasslista)
            {
                s = tc.Kiindulo + ";";
                s = s + tc.Vegpont + ";";
                s = s + Convert.ToString(tc.Hossz / 1000) + "," + Convert.ToString(tc.Hossz % 1000) + ";";
                s = s + Convert.ToString(tc.Emelkedes) + ";";
                s = s + Convert.ToString(tc.Lejtes) + ";";
                if (tc.Pecsetelo)
                {
                    s = s + "i";
                }
                else
                {
                    s = s + "n";
                }
                sw.WriteLine(s);
            }
            sw.Close();

            Console.ReadKey();
        }
    }
}
