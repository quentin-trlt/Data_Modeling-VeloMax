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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VeloMax
{
    public partial class EmployesForm : Form
    {
        private MySqlConnection connexion;
        public EmployesForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            AfficherEmployes();
        }
        private void AfficherEmployes()
        {

            string requete = "SELECT idEmploye, nomEmploye, prenomEmploye, idMagasin, position FROM EMPLOYE;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    string id = mySqlDataReader["idEmploye"].ToString();
                    string nom = mySqlDataReader["nomEmploye"].ToString();
                    string prenom = mySqlDataReader["prenomEmploye"].ToString();
                    string idMagasin = mySqlDataReader["idMagasin"].ToString();
                    string poste = mySqlDataReader["position"].ToString();

                    textBox1.Text += $"{id} : {nom}, {prenom}, {poste}, {idMagasin}\r\n";
                }
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterEmployeForm ajouterEmployeForm = new AjouterEmployeForm(connexion);
            ajouterEmployeForm.ShowDialog();

            textBox1.Clear();
            AfficherEmployes();
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            SupprimerEmployeForm supprimerClientPartForm = new SupprimerEmployeForm();
            supprimerClientPartForm.ShowDialog();

            textBox1.Clear();
            AfficherEmployes();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            ModifierClientPartForm modifierClientPartForm = new ModifierClientPartForm(connexion);
            modifierClientPartForm.ShowDialog();

            textBox1.Clear();
            AfficherEmployes();
        }
        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


    }
}


