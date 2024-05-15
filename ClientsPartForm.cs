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
    public partial class ClientsPartForm : Form
    {
        private MySqlConnection connexion;
        public ClientsPartForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            AfficherClients();
        }

        private void AfficherClients()
        {

            string requete = "SELECT nom_client, prenom_client, numero, rue, ville, no_tel, courriel FROM PARTICULIER P " +
                "JOIN ADRESSE A ON A.idAdresse = P.idAdresse;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    string nom = mySqlDataReader["nom_client"].ToString();
                    string prenom = mySqlDataReader["prenom_client"].ToString();
                    string adresse = mySqlDataReader["numero"].ToString() + ", " + mySqlDataReader["rue"].ToString() + ", " + mySqlDataReader["ville"].ToString();
                    string noTel = mySqlDataReader["no_tel"].ToString();
                    string mail = mySqlDataReader["courriel"].ToString();

                    textBox1.Text += $"{nom}, {prenom}, {adresse}, {noTel}, {mail}\r\n";
                }
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterClientPartForm ajouterClientPartForm = new AjouterClientPartForm(connexion);
            ajouterClientPartForm.ShowDialog();

            textBox1.Clear();
            AfficherClients();
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            SupprimerClientPartForm supprimerClientPartForm = new SupprimerClientPartForm(connexion);
            supprimerClientPartForm.ShowDialog();

            textBox1.Clear();
            AfficherClients();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            ModifierClientPartForm modifierClientPartForm = new ModifierClientPartForm(connexion);
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
