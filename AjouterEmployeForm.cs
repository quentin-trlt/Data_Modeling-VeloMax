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
    public partial class AjouterEmployeForm : Form
    {
        private MySqlConnection connexion;

        public AjouterEmployeForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            string nom = textBox1.Text;
            string prenom = textBox2.Text;
            string idMagasin = textBox3.Text;
            string position = textBox4.Text;
            string salaire = textBox5.Text;


            if (!string.IsNullOrWhiteSpace(nom) && !string.IsNullOrWhiteSpace(prenom) && !string.IsNullOrWhiteSpace(idMagasin) && !string.IsNullOrWhiteSpace(position) && !string.IsNullOrWhiteSpace(salaire))
            {
                string requete = $"INSERT INTO EMPLOYE (nomEmploye,prenomEmploye,idMagasin, position, salaire) VALUES('{nom}', '{prenom}', '{idMagasin}','{position}',{salaire});";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                int lignesModifiees = cmd.ExecuteNonQuery();
                if (lignesModifiees > 0)
                {
                    MessageBox.Show("Employé ajouté avec succès !");
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show("Problème lors de l'ajout de l'employé.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}