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
    public partial class FormListeModeles : Form
    {
        private MySqlConnection connexion;

        public FormListeModeles(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion; 
            AfficherModeles();
            textBox1.ReadOnly = true;
        }

        private void AfficherModeles()
        {
            string requete = "SELECT * FROM MODELE;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    string idModele = mySqlDataReader["idModele"].ToString();
                    string nom = mySqlDataReader["nom"].ToString();
                    string taille = mySqlDataReader["grandeur"].ToString();

                    textBox1.Text += $"ID: {idModele}, Nom: {nom}, Taille: {taille}\r\n";
                }
            }
        }

        private void FormListeModeles_Load(object sender, EventArgs e)
        {

        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            AjouterModeleForm ajouterModeleForm = new AjouterModeleForm(connexion);
            ajouterModeleForm.ShowDialog();

            textBox1.Clear();
            AfficherModeles();
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            SupprimerModeleForm supprimerModeleForm = new SupprimerModeleForm(connexion);
            supprimerModeleForm.ShowDialog();

            textBox1.Clear();
            AfficherModeles();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            ModifierModeleForm modifierModeleForm = new ModifierModeleForm(connexion);
            modifierModeleForm.ShowDialog();

            textBox1.Clear();
            AfficherModeles();
        }
        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Hide();

            MenuForm menuForm = new MenuForm(connexion);
            menuForm.Show();
        }
    }
}
