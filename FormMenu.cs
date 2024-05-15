using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VeloMax
{
    public partial class MenuForm : Form
    {
        private MySqlConnection connexion;

        public MenuForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            StockForm stockForm = new StockForm(connexion);
            stockForm.Show();
        }

        private void btnClients_Click(object sender, EventArgs e)
        {
            ClientsForm clientsForm = new ClientsForm(connexion);
            clientsForm.Show();
        }

        private void btnCommandes_Click(object sender, EventArgs e)
        {
            CommandesForm commandesForm = new CommandesForm(connexion);
            commandesForm.Show();
        }

        private void btnStatistiques_Click(object sender, EventArgs e)
        {
            StatistiquesForm statistiquesForm = new StatistiquesForm(connexion);
            statistiquesForm.Show();
        }

        private void buttonModele_Click(object sender, EventArgs e)
        {
            FormListeModeles modeleForm = new FormListeModeles(connexion);
            modeleForm.Show();
        }
        private void btnFournisseur_Click(object sender, EventArgs e)
        {
            FournisseurForm fournisseurForm = new FournisseurForm(connexion);
            fournisseurForm.Show();
        }
        private void btnEmploye_Click(object sender, EventArgs e)
        {
            EmployesForm employeForm = new EmployesForm(connexion);
            employeForm.Show();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            string[,] piecesAReapprovisionner;
            string[,] modelesAReapprovisionner;

            {
                string requetePieces = "SELECT S.idPiece, idMagasin FROM PIECE P " +
                    "JOIN STOCK_PIECE S ON S.idPiece = P.idPiece " +
                    "WHERE quantite <= 0";


                MySqlCommand cmdPieces = new MySqlCommand(requetePieces, connexion);
                MySqlDataReader readerPieces = cmdPieces.ExecuteReader();

                // Comptage du nombre de lignes
                int rowCount = 0;
                while (readerPieces.Read())
                {
                    rowCount++;
                }

                // Réinitialisation du lecteur
                readerPieces.Close();
                readerPieces = cmdPieces.ExecuteReader();

                int colCount = 2;
                piecesAReapprovisionner = new string[rowCount, colCount];
                int rowIndex = 0;
                while (readerPieces.Read())
                {
                    piecesAReapprovisionner[rowIndex, 0] = readerPieces["idPiece"].ToString();
                    piecesAReapprovisionner[rowIndex, 1] = readerPieces["idMagasin"].ToString();
                    rowIndex++;
                }

                readerPieces.Close();


                modelesAReapprovisionner = new string[rowCount, colCount];
                string requeteModeles = "SELECT S.idModele, idMagasin FROM MODELE M " +
                    "JOIN STOCK_VELO S ON S.idModele = M.idModele " +
                    "WHERE quantite <= 0";
                MySqlCommand cmdModeles = new MySqlCommand(requeteModeles, connexion);
                MySqlDataReader readerModeles = cmdModeles.ExecuteReader();
                int rowCount1 = 0;
                while (readerModeles.Read())
                {
                    rowCount++;
                }
                readerModeles.Close();

                readerModeles = cmdModeles.ExecuteReader();
                int colCount1 = 2;
                int rowIndex1 = 0;
                modelesAReapprovisionner = new string[rowCount1, colCount1];
                while (readerModeles.Read())
                {
                    modelesAReapprovisionner[rowIndex1, 0] = readerModeles["idModele"].ToString();
                    modelesAReapprovisionner[rowIndex1, 1] = readerModeles["idMagasin"].ToString();
                    rowIndex1++;
                }

                readerModeles.Close();
            }

            if (piecesAReapprovisionner.GetLength(0) > 0 || modelesAReapprovisionner.GetLength(0) > 0)
            {
                AfficherPopUp(piecesAReapprovisionner, modelesAReapprovisionner);
            }
        }
        private void AfficherPopUp(string[,] pieces, string[,] modeles)
        {
            PopUpForm popUpForm = new PopUpForm(connexion, pieces, modeles);
            popUpForm.ShowDialog(); 
        }


    }
}
