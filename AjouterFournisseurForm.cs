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
    public partial class AjouterFournisseurForm : Form
    {
        private MySqlConnection connexion;

        public AjouterFournisseurForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            string siret = textBox1.Text;
            string nom = textBox2.Text;
            string numero = textBox3.Text;
            string rue = textBox4.Text;
            string ville = textBox5.Text;


            if (!string.IsNullOrWhiteSpace(siret) && !string.IsNullOrWhiteSpace(nom) && !string.IsNullOrWhiteSpace(rue) && !string.IsNullOrWhiteSpace(ville))
            {
                string requeteAdresse = $"INSERT INTO ADRESSE (numero,rue,ville) VALUES('{numero}', '{rue}', '{ville}');";
                MySqlCommand cmdAd = new MySqlCommand(requeteAdresse, connexion);
                cmdAd.ExecuteNonQuery();

                string idAdresse = Convert.ToString(cmdAd.LastInsertedId);

                string requete = $"INSERT INTO FOURNISSEUR (siret,nom_entreprise,idAdresse,categorie) VALUES('{siret}', '{nom}', '{idAdresse}',null);";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                int lignesModifiees = cmd.ExecuteNonQuery();
                if (lignesModifiees > 0)
                {
                    MessageBox.Show("Fournisseur ajouté avec succès !");
                    this.Close(); // Fermer le formulaire après l'ajout
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du fournisseur.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
            }
        }
    }
}
