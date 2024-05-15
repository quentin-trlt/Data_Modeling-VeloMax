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
    public partial class CommandesForm : Form
    {
        private MySqlConnection connexion;

        public CommandesForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            AfficherCommandes();
            textBox1.ReadOnly = true;
        }

        private void AfficherCommandes()
        {
            string requete = "SELECT idCommande, idClient, idMagasin, prix_total, dateCommande FROM COMMANDE;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    string idCommande = mySqlDataReader["idCommande"].ToString();
                    string idClient = mySqlDataReader["idClient"].ToString();
                    string idMagasin = mySqlDataReader["idMagasin"].ToString();
                    string prix = mySqlDataReader["idMagasin"].ToString();
                    string date = mySqlDataReader["dateCommande"].ToString();

                    textBox1.Text += $"{idCommande} : Client : {idClient}, Magasin : {idMagasin} - {prix}, {date}\r\n";
                }
            }
        }

        private void FormListeModeles_Load(object sender, EventArgs e)
        {

        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterCommandeForm ajouterCommandeForm = new AjouterCommandeForm(connexion);
            ajouterCommandeForm.ShowDialog();

            textBox1.Clear();
            AfficherCommandes();
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            SupprimerCommandeForm supprimerCommandeForm = new SupprimerCommandeForm(connexion);
            supprimerCommandeForm.ShowDialog();

            textBox1.Clear();
            AfficherCommandes();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            AjouterLigneForm modifierModeleForm = new AjouterLigneForm(connexion);
            modifierModeleForm.ShowDialog();

            textBox1.Clear();
            AfficherCommandes();
        }
        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
