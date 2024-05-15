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
using System.Configuration;

namespace VeloMax
{
    public partial class PopUpForm : Form
    {
        private MySqlConnection connexion;
        private string[,] referencesAReapprovisionner;

        public PopUpForm(MySqlConnection connexion, string[,] pieces, string[,] modeles = null)
        {
            InitializeComponent();
            this.connexion = connexion;
            int rowCount = pieces.GetLength(0) + modeles.GetLength(0);
            int colCount = 2;
            this.referencesAReapprovisionner = new string[rowCount, colCount];
            for (int i = 0; i < pieces.GetLength(0); i++)
            {
                this.referencesAReapprovisionner[i, 0] = pieces[i, 0]; 
                this.referencesAReapprovisionner[i, 1] = pieces[i, 1]; 
            }
            for (int i = 0; i < modeles.GetLength(0); i++)
            {
                int index = pieces.GetLength(0) + i;
                this.referencesAReapprovisionner[index, 0] = modeles[i, 0]; 
                this.referencesAReapprovisionner[index, 1] = modeles[i, 1]; 
            }
            AfficherReferencesManquantes();
        }

        private void AfficherReferencesManquantes()
        {
            for (int i = 0; i < referencesAReapprovisionner.GetLength(0); i++)
            {
                string reference = referencesAReapprovisionner[i, 0];
                string magasin = referencesAReapprovisionner[i, 1];
                string texteReference = $"Référence : {reference} - Magasin : {magasin}\r\n";

                listBox1.Items.Add(texteReference);
            }

            this.ClientSize = new System.Drawing.Size(300, 100 + referencesAReapprovisionner.GetLength(0) * 20 + 50);
        }

        private void BtnStockForm_Click(object sender, EventArgs e)
        {
            StockForm stockForm = new StockForm(connexion);
            this.Close();
            stockForm.Show();
        }

        private void AfficherPopUp(string[,] references)
        {
            //bool afficherPopUp = Properties.Settings.Default.AfficherPopUp;

            // Afficher la fenêtre pop-up uniquement si l'indicateur est vrai
            //if (afficherPopUp)
            {
                // PopUpForm popUpForm = new PopUpForm(connexion, references);
                // DialogResult result = popUpForm.ShowDialog();

                // Si l'utilisateur a choisi de ne plus afficher la fenêtre pop-up
                // if (result == DialogResult.OK)
                {
                    // Définir l'indicateur sur faux dans les paramètres de configuration
                    //Properties.Settings.Default.AfficherPopUp = false;
                    // Properties.Settings.Default.Save();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}