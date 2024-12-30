using Builder;
using System;
using System.IO;

namespace Builder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Entrez le nom du vendeur :");
            string vendeurNom = Console.ReadLine();

            Console.WriteLine("Choisissez le format de liasse (html/pdf) :");
            string choix = Console.ReadLine()?.ToLower();

            ConstructeurLiasseVehicule constructeur;

            if (choix == "html")
            {
                constructeur = new ConstructeurLiasseVehiculeHtml();
            }
            else if (choix == "pdf")
            {
                constructeur = new ConstructeurLiasseVehiculePdf();
            }
            else
            {
                Console.WriteLine("Format inconnu. Par défaut, PDF sera utilisé.");
                constructeur = new ConstructeurLiasseVehiculePdf();
            }

            Directeur directeur = new Directeur(constructeur);

            Console.WriteLine("Entrez le nom du client :");
            string client = Console.ReadLine();

            Console.WriteLine("Entrez les informations du véhicule :");
            string informationsVehicule = Console.ReadLine();

            Console.WriteLine("Entrez l'adresse de facturation du client :");
            string adresseFacturation = Console.ReadLine();

            string immatriculation = GenererImmatriculation();
            string numeroCommande = GenererNumeroCommande();
            string dateGeneration = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            Liasse liasse = directeur.Construit(client, informationsVehicule);
            liasse.AjouteDocument($"Vendeur : {vendeurNom}");
            liasse.AjouteDocument($"Adresse de facturation : {adresseFacturation}");
            liasse.AjouteDocument($"Numéro de commande : {numeroCommande}");
            liasse.AjouteDocument($"Date de génération : {dateGeneration}");
            liasse.AjouteDocument($"Immatriculation générée : {immatriculation}");
            liasse.AjouteDocument("Signature du client : ___________________________");
            liasse.AjouteDocument($"Signature du vendeur ({vendeurNom}) : ___________________________");

            Console.WriteLine("======================================");
            Console.WriteLine("           Liasse de Documents        ");
            Console.WriteLine("======================================");
            liasse.Imprime();
            Console.WriteLine("======================================");

            string dossierExport = @"C:\Users\Mylan\Documents\Cours\ESI 4\DesignPatternsCsharp\Builder\LiasseGenerer";
            string dateFichier = DateTime.Now.ToString("ddMMyy"); // Format JJMMAA
            string nomFichier = $"{client.Replace(" ", "_")}_{dateFichier}.txt";
            string cheminComplet = Path.Combine(dossierExport, nomFichier);

            try
            {
                if (!Directory.Exists(dossierExport))
                {
                    Directory.CreateDirectory(dossierExport);
                }

                File.WriteAllLines(cheminComplet, liasse.documents);
                Console.WriteLine($"Liasse exportée avec succès sous '{cheminComplet}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exportation : {ex.Message}");
            }

            Console.WriteLine("Liasse générée avec succès !");
        }

        static string GenererImmatriculation()
        {
            Random random = new Random();
            string lettres1 = $"{(char)random.Next('A', 'Z' + 1)}{(char)random.Next('A', 'Z' + 1)}";
            string chiffres = random.Next(100, 999).ToString();
            int departement = random.Next(1, 99); // Département fictif (01 à 99)
            string lettres2 = $"{(char)random.Next('A', 'Z' + 1)}{(char)random.Next('A', 'Z' + 1)}";
            return $"{lettres1}-{chiffres}-{lettres2} {departement:D2}";
        }

        static string GenererNumeroCommande()
        {
            Random random = new Random();
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sequence = random.Next(1000, 9999).ToString();
            return $"CMD-{date}-{sequence}";
        }
    }
}
