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
    public partial class AjouterClientPartForm : Form
    {
        private MySqlConnection connexion;
        public AjouterClientPartForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion; 
        }
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            string nom = textBox1.Text;
            string prenom = textBox2.Text;
            string tel = textBox3.Text;
            string mail = textBox4.Text;
            string numero = textBox5.Text;
            string rue = textBox6.Text;
            string ville = textBox7.Text;

            if (!string.IsNullOrWhiteSpace(nom) && !string.IsNullOrWhiteSpace(prenom) && !string.IsNullOrWhiteSpace(tel) && !string.IsNullOrWhiteSpace(mail) && !string.IsNullOrWhiteSpace(rue) && !string.IsNullOrWhiteSpace(ville))
            {
                string requeteAdresse = $"INSERT INTO ADRESSE (numero,rue,ville) VALUES('{numero}', '{rue}', '{ville}');";
                MySqlCommand cmdAd = new MySqlCommand(requeteAdresse, connexion);
                cmdAd.ExecuteNonQuery();

                string idAdresse = Convert.ToString(cmdAd.LastInsertedId);

                string requete = $"INSERT INTO PARTICULIER (nom_client,prenom_client,idAdresse,no_tel,courriel) VALUES('{nom}', '{prenom}', '{idAdresse}','{tel}','{mail}');";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                int lignesModifiees = cmd.ExecuteNonQuery();

                string idParticulier = Convert.ToString(cmd.LastInsertedId);

                string requeteClientele = $"INSERT INTO CLIENTELE (idParticulier) VALUES('{idParticulier}');";
                MySqlCommand cmdC = new MySqlCommand(requeteClientele, connexion);
                cmdC.ExecuteNonQuery();

                if (lignesModifiees > 0)
                {
                    MessageBox.Show("Client ajouté avec succès !");
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show("Erreur lors de l'ajout du client.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
            }
        }
    }
}
