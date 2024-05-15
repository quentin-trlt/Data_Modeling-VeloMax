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
    public partial class AjouterModeleForm : Form
    {
        private MySqlConnection connexion;

        public AjouterModeleForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            string idModele = textBox1.Text;
            string nom = textBox2.Text;
            string taille = textBox3.Text;
            string prix = textBox4.Text;
            string ligne = textBox5.Text;
            string date_debut = textBox6.Text;

            if (!string.IsNullOrWhiteSpace(idModele) && !string.IsNullOrWhiteSpace(nom) && !string.IsNullOrWhiteSpace(taille))
            {             
                string requete = $"INSERT INTO MODELE (idModele,nom,grandeur,prix,ligne,date_debut,date_fin) VALUES('{Convert.ToInt32(idModele)}', '{nom}', '{taille}','{Convert.ToInt32(prix)}','{ligne}' ,'{date_debut}', null);";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                int lignesModifiees = cmd.ExecuteNonQuery();
                if (lignesModifiees > 0)
                {
                    MessageBox.Show("Modèle ajouté avec succès !");
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du modèle.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
            }
        }


    }
}
