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
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto.Macs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VeloMax
{
    public partial class AjouterCommandeForm : Form
    {
        private MySqlConnection connexion;
        public AjouterCommandeForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            RemplirChoixClient();
            RemplirListeMagasins();
        }
        private void RemplirChoixClient()
        {
            comboBox4.Items.Clear();
            comboBox4.Items.Add($"Particulier");
            comboBox4.Items.Add($"Professionel");
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedItem != null)
            {
                string text = comboBox4.SelectedItem.ToString();
                comboBox1.Items.Clear();
                RemplirListeClients(text);
            }
        }
        private void RemplirListeClients(string text)
        {
            if (text == "Particulier")
            {
                string requeteClients = "SELECT idClient, nom_client, prenom_client FROM PARTICULIER P " +
                    "JOIN CLIENTELE C ON C.idParticulier = P.idParticulier;";
                MySqlCommand cmdClients = new MySqlCommand(requeteClients, connexion);
                using (MySqlDataReader reader = cmdClients.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idClient = reader.GetInt32("idClient");
                        string nom = reader.GetString("nom_client");
                        string prenom = reader.GetString("prenom_client");
                        comboBox1.Items.Add($"{idClient} : '{nom}', '{prenom}'");
                    }
                }
            }
            else
            {
                string requeteClients = "SELECT idClient, nom_entreprise FROM ENTREPRISE E " +
                    "JOIN CLIENTELE C ON C.siret = E.siret;";
                MySqlCommand cmdClients = new MySqlCommand(requeteClients, connexion);
                using (MySqlDataReader reader = cmdClients.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idClient = reader.GetInt32("idClient");
                        string nom = reader.GetString("nom_entreprise");
                        comboBox1.Items.Add($"{idClient} : '{nom}'");
                    }
                }
            }
        }
        private void RemplirListeMagasins()
        {
            string requeteMagasins = "SELECT idMagasin FROM MAGASIN";
            MySqlCommand cmdMagasins = new MySqlCommand(requeteMagasins, connexion);

            using (MySqlDataReader reader = cmdMagasins.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idMagasin = reader.GetInt32("idMagasin");
                    comboBox2.Items.Add($"{idMagasin}");
                }
            }
        }

        private void RemplirListeEmployes(int idMagasin)
        {
            string requeteEmployes = "SELECT idEmploye FROM EMPLOYE WHERE idMagasin = @idMagasin";
            MySqlCommand cmdEmployes = new MySqlCommand(requeteEmployes, connexion);
            cmdEmployes.Parameters.AddWithValue("@idMagasin", idMagasin);

            using (MySqlDataReader reader = cmdEmployes.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idEmploye = reader.GetInt32("idEmploye");
                    comboBox3.Items.Add($"{idEmploye}");
                }
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                int idMagasin = Convert.ToInt32(comboBox2.SelectedItem.ToString());
                comboBox3.Items.Clear();
                RemplirListeEmployes(idMagasin);
            }
        }
        private void btnOuvrir_Click(object sender, EventArgs e)
        {
            string idMagasin = comboBox2.SelectedItem?.ToString();
            string idEmploye = comboBox3.SelectedItem?.ToString();
            
            string clientSelectionne = comboBox1.SelectedItem.ToString();
            int indexSep = clientSelectionne.IndexOf(':');
            string idClient = clientSelectionne.Substring(0, indexSep - 1).Trim();

            

            if (!string.IsNullOrWhiteSpace(idMagasin) && !string.IsNullOrWhiteSpace(idClient) && !string.IsNullOrWhiteSpace(idEmploye))
            {
                string requete = $"INSERT INTO COMMANDE (idClient, idMagasin, idEmploye, dateCommande) VALUES('{idClient}', '{idMagasin}','{idEmploye}', CURDATE());";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                int lignesModifiees = cmd.ExecuteNonQuery();
                string idCommande = cmd.LastInsertedId.ToString();

                if (lignesModifiees > 0)
                {
                    MessageBox.Show("Commande ajoutée avec succès !");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout de la commande.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
            }
        }
    }
}
