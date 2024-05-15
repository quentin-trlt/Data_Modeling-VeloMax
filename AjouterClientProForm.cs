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
    public partial class AjouterClientProForm : Form
    {
        private MySqlConnection connexion;
        public AjouterClientProForm(MySqlConnection connexion)
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


            if (!string.IsNullOrWhiteSpace(nom) && !string.IsNullOrWhiteSpace(siret) && !string.IsNullOrWhiteSpace(rue) && !string.IsNullOrWhiteSpace(ville))
            {
                string requeteAdresse = $"INSERT INTO ADRESSE (numero,rue,ville) VALUES('{numero}', '{rue}', '{ville}');";
                MySqlCommand cmdAd = new MySqlCommand(requeteAdresse, connexion);
                cmdAd.ExecuteNonQuery();

                string idAdresse = Convert.ToString(cmdAd.LastInsertedId);

                string requete = $"INSERT INTO ENTREPRISE (siret,nom_entreprise,idAdresse) VALUES('{siret}', '{nom}', '{idAdresse}');";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                int lignesModifiees = cmd.ExecuteNonQuery();


                string requeteClientele = $"INSERT INTO CLIENTELE (siret) VALUES('{siret}');";
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
