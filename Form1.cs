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
    public partial class Form1 : Form
    {
        private MySqlConnection connexion;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnecter_Click(object sender, EventArgs e)
        {
            string nomUtilisateur = textBox1.Text;
            string motDePasse = textBox4.Text;

            // Mot de passe correct, connectez-vous à la base de données
            try
            {
                string connexionString;
                if (nomUtilisateur == "root" && motDePasse == "root" || nomUtilisateur == "bozo" && motDePasse == "bozo")
                {
                    connexionString = $"server=localhost;port=3306;database=VeloMax;uid={nomUtilisateur};password={motDePasse}";
                }
                else
                {
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect. Veuillez réessayer.");
                    return; // Arrêter le traitement car les informations d'identification sont incorrectes
                }

                connexion = new MySqlConnection(connexionString);
                connexion.Open();
                MessageBox.Show("Connexion réussie !");
                this.Hide();

                MenuForm menuForm = new MenuForm(connexion);
                menuForm.Show();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Erreur de connexion: " + ex.Message);
            }
        }


    }
}
