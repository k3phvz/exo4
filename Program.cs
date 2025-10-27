using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoursSupDeVinci
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Lecture du fichier CSV

            const string path = @"C:\Users\kephas ASSOGBA\Desktop\CoursSupDeVinci\CoursSupDeVinci\CoursSupDeVinci\CoursSupDeVinci_C#.csv";

            Dictionary<int, Person> persons = new Dictionary<int, Person>();
            string[] lignes = File.ReadAllLines(path);

            // On commence à 1 pour ignorer l'en-tête du fichier CSV
            for (int i = 1; i < lignes.Length; i++)
            {
                string line = lignes[i];
                string[] parts = line.Split(',');

                // Création d'une personne à partir de la ligne du fichier
                Person person = new Person();
                person.Lastname = parts[1];
                person.Firstname = parts[2];
                person.Birthdate = ConvertToDateTime(parts[3]);
                person.Size = int.Parse(parts[5]);

                // Récupération des détails d’adresse
                List<string> details = parts[4].Split(';').ToList();
                person.AdressDetails = new Detail(details[0], int.Parse(details[1]), details[2]);

                persons.Add(int.Parse(parts[0]), person);
            }

            #endregion

            #region Calcul de la moyenne et filtrage des personnes

            // Calcul de la taille moyenne de la classe
            double tailleMoyenne = persons.Values.Average(p => p.Size);

            // Sélection des personnes de Nantes plus grandes que la moyenne
            List<Person> personnesFiltrees = persons.Values
                .Where(p => p.Size > tailleMoyenne && p.AdressDetails.City.ToLower() == "nantes")
                .OrderByDescending(p => p.Size)
                .ToList();

            #endregion

            #region Affichage du résultat

            Console.WriteLine($"Taille moyenne de la classe : {Math.Round(tailleMoyenne / 100, 2)} m\n");
            Console.WriteLine("Personnes de Nantes plus grandes que la moyenne :\n");

            if (personnesFiltrees.Count == 0)
            {
                Console.WriteLine("Aucune personne de Nantes n'est plus grande que la moyenne.");
            }
            else
            {
                int position = 1;
                foreach (Person p in personnesFiltrees)
                {
                    double tailleMetre = Math.Round(p.Size / 100.0, 2);
                    Console.WriteLine($"{position} - {p.Firstname} - {tailleMetre.ToString("0.00").Replace('.', ',')}");
                    position++;
                }
            }

            #endregion
        }

        #region Méthode de conversion de date

        static DateTime ConvertToDateTime(string date)
        {
            if (DateTime.TryParse(date, out DateTime birthdate))
            {
                return birthdate;
            }
            else
            {
                Console.WriteLine("Date invalide, utilisation de la date du jour.");
                return DateTime.Now;
            }
        }

        #endregion
    }
}
