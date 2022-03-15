using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using Font = iTextSharp.text.Font;

namespace MaxtalbQuotingWinForm
{
    public partial class MaxtalbQuotingGenerator : Form
    {
        public MaxtalbQuotingGenerator()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, EventArgs e)
        {
            GeneratePdf();
        }

        public void GeneratePdf()
        {
            // Définition de l'emplacement du fichier généré: chemin actuel/doss.format
            string outFile = Environment.CurrentDirectory + "/devis.pdf";
            // Création du nouveau document
            Document doc = new Document();
            // Pour écrire dans le document créé qu'on enregistre au chemin spécifié.
            PdfWriter.GetInstance(doc, new FileStream(outFile, FileMode.Create));
            // On ouvre le document pour travailler dedans
            doc.Open();
            // Standardisation visuelle
            // Palette de couleurs
            BaseColor blue = new BaseColor(0, 75, 155);
            BaseColor grey = new BaseColor(240, 240, 240);
            BaseColor white = new BaseColor(255, 255, 255);

            // Définissons les polices
            // Pour le titre:
            Font policeTitre = new Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20f, iTextSharp.text.Font.BOLD, blue);
            // Pour les th (tableHeader)
            Font policeTh = new Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16f, iTextSharp.text.Font.BOLD, white);

            // Page
            // Création de paragraphe que l'on place en haut à gauche du devis
            Paragraph p1 = new Paragraph("Maxtalb, " + textBoxCoordonnees.Text + "\n\n", policeTitre);
            p1.Alignment = Element.ALIGN_LEFT;
            // Ajout de l'élément au document
            doc.Add(p1);
            // Client
            Paragraph p2 = new Paragraph("Client: " + textBoxClient.Text + "\n\n", policeTitre);
            p2.Alignment = Element.ALIGN_RIGHT;
            doc.Add(p2);
            // Devis
            Paragraph p3 = new Paragraph("Devis: " + textBoxTitreDevis.Text + "\n\n", policeTitre);
            p3.Alignment = Element.ALIGN_LEFT;
            doc.Add(p3);

            // Création du tableau(nombreColonnes)
            PdfPTable tableau = new PdfPTable(3);
            tableau.WidthPercentage = 100;

            // Cellules headers du tableau
            AddCellToTab("Désignation", policeTh, blue, tableau);
            AddCellToTab("Quantité", policeTh, blue, tableau);
            AddCellToTab("Prix", policeTh, blue, tableau);

            // Lister les produits dans le tableau
            string[] infosProduit = new string[3];
            infosProduit[0] = textBoxNomProduit.Text;
            infosProduit[1] = textBoxQuantite.Text;
            infosProduit[2] = textBoxPrix.Text;
            foreach(string info in infosProduit)
            {
                PdfPCell cell = new PdfPCell(new Phrase(info));
                cell.BackgroundColor = grey;
                cell.Padding = 7;
                cell.BorderColor = grey;
                tableau.AddCell(cell);
            }
            doc.Add(tableau);
            // création d'un espacement à la fin du tableau, avant le prix
            doc.Add(new Phrase("\n"));

            // Prix
            int prixTotal = int.Parse(textBoxPrix.Text) * int.Parse(textBoxQuantite.Text);
            Paragraph p4 = new Paragraph("Prix: " + prixTotal + "\n\n", policeTitre);
            p4.Alignment = Element.ALIGN_RIGHT;
            doc.Add(p4);

            // Fermer le document
            doc.Close();
            // Démarrer le processus pour exécuter le devis et ouvrir le fichier PDF
            Process.Start(@"cmd.exe", @"/c" + outFile);
        }
        // Fonction d'ajout des cellulles
        public void AddCellToTab(string str, Font police, BaseColor couleur, PdfPTable tableau )
        {
            PdfPCell cell1 = new PdfPCell(new Phrase(str, police));
            cell1.BackgroundColor = couleur;
            cell1.Padding = 7;
            cell1.BorderColor = couleur;
            tableau.AddCell(cell1);
        }
    }
}