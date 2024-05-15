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
    public partial class FournisseurForm : Form
    {
        private MySqlConnection connexion;
        public FournisseurForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            AfficherFournisseurs();
        }

        private void AfficherFournisseurs()
        {

            string requete = "SELECT nom_entreprise, numero, rue, ville, categorie FROM FOURNISSEUR F " +
                "JOIN ADRESSE A ON A.idAdresse = F.idAdresse;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    string nom = mySqlDataReader["nom_entreprise"].ToString();
                    string categorie = mySqlDataReader["categorie"].ToString();
                    string adresse = mySqlDataReader["numero"].ToString() +", " + mySqlDataReader["rue"].ToString() + ", " + mySqlDataReader["ville"].ToString();

                    textBox1.Text += $"{nom}, {adresse}, {categorie}\r\n";
                }
            }
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterFournisseurForm ajouterFournisseurForm = new AjouterFournisseurForm(connexion);
            ajouterFournisseurForm.ShowDialog();

            textBox1.Clear();
            AfficherFournisseurs();
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            SupprimerFournisseurForm supprimerFournisseurForm = new SupprimerFournisseurForm(connexion);
            supprimerFournisseurForm.ShowDialog();

            textBox1.Clear();
            AfficherFournisseurs();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            ModifierFournisseurForm modifierFournisseurForm = new ModifierFournisseurForm(connexion);
            modifierFournisseurForm.ShowDialog();

            textBox1.Clear();
            AfficherFournisseurs();
        }
        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void FournisseurForm_Load(object sender, EventArgs e)
        {

        }
    }
}
