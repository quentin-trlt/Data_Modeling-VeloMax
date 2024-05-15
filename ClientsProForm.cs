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
    public partial class ClientsProForm : Form
    {
        private MySqlConnection connexion;
        public ClientsProForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            AfficherClients();
        }

        private void AfficherClients()
        {

            string requete = "SELECT nom_entreprise, siret, numero, rue, ville FROM ENTREPRISE E " +
                "JOIN ADRESSE A ON A.idAdresse = E.idAdresse;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    string nom = mySqlDataReader["nom_entreprise"].ToString();
                    string siret= mySqlDataReader["siret"].ToString();
                    string adresse = mySqlDataReader["numero"].ToString() + ", " + mySqlDataReader["rue"].ToString() + ", " + mySqlDataReader["ville"].ToString();
                    textBox1.Text += $"{nom}, {siret}, {adresse}\r\n";
                }
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterClientProForm ajouterClientProForm = new AjouterClientProForm(connexion);
            ajouterClientProForm.ShowDialog();

            textBox1.Clear();
            AfficherClients();
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            SupprimerClientProForm supprimerClientProForm = new SupprimerClientProForm(connexion);
            supprimerClientProForm.ShowDialog();

            textBox1.Clear();
            AfficherClients();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            ModifierClientProForm modifierClientPartForm = new ModifierClientProForm(connexion);
            modifierClientPartForm.ShowDialog();

            textBox1.Clear();
            AfficherClients();
        }
        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Hide();

            MenuForm menuForm = new MenuForm(connexion);
            menuForm.Show();
        }


    }
}
