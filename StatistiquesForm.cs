using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using VeloMax;

namespace VeloMax
{
    public partial class StatistiquesForm : Form
    {
        private MySqlConnection connexion;

        public StatistiquesForm(MySqlConnection connexion)
        {
            InitializeComponent();
            LoadPrograms1(connexion);
            LoadPrograms2(connexion);
            LoadPrograms3(connexion);
            LoadPrograms4(connexion);


            LoadBestClient(connexion);
            LoadAVGCommande(connexion);
            LoadBestMagasin(connexion);
            LoadAVGRabais(connexion);
            //SommePiece(connexion);

            Bonus(connexion);
            this.connexion = connexion;

        }

        public void LoadPrograms1(MySqlConnection connexion)
        {
            string requete = "SELECT nom_client,prenom_client,(SELECT dateDebut FROM ADHESION WHERE ADHESION.idClient=PARTICULIER.idParticulier),(SELECT duree FROM FIDELIO WHERE FIDELIO.idProgramme=PARTICULIER.idProgramme) FROM PARTICULIER WHERE idProgramme = '1'";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                listBox1.Items.Clear();
                while (reader.Read())
                {
                    string nomprog1 = reader["nom_client"].ToString();
                    string prenomprog1 = reader["prenom_client"].ToString();
                    /*
                    string dateDebut = reader["dateDebut"].ToString();
                    string duree = reader["duree"].ToString();

                    TimeSpan dureeTimeSpan;
                    TimeSpan dureeabo = TimeSpan.Parse((duree));
                    DateTime dateDebutDateTime = DateTime.Parse(dateDebut);
                    DateTime dateFin = dateDebutDateTime + dureeabo;
                    string dateFinString = dateFin.ToString();



                    listBox1.Items.Add($"{nomprog1},{prenomprog1},{dateFinString}");*/

                    listBox1.Items.Add($"{nomprog1},{prenomprog1}");

                }
            }
        }

        public void LoadPrograms2(MySqlConnection connexion)
        {
            string requete = "SELECT nom_client,prenom_client FROM PARTICULIER WHERE idProgramme = '2'";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                listBox2.Items.Clear();
                while (reader.Read())
                {
                    string nomprog2 = reader["nom_client"].ToString();
                    string prenomprog2 = reader["prenom_client"].ToString();

                    listBox2.Items.Add($"{nomprog2},{prenomprog2}");
                }
            }
        }

        public void LoadPrograms3(MySqlConnection connexion)
        {
            string requete = "SELECT nom_client,prenom_client FROM PARTICULIER WHERE idProgramme = '3'";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                listBox3.Items.Clear();
                while (reader.Read())
                {
                    string nomprog3 = reader["nom_client"].ToString();
                    string prenomprog3 = reader["prenom_client"].ToString();

                    listBox3.Items.Add($"{nomprog3},{prenomprog3}");
                }
            }
        }

        public void LoadPrograms4(MySqlConnection connexion)
        {
            string requete = "SELECT nom_client,prenom_client FROM PARTICULIER WHERE idProgramme = '4'";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                listBox4.Items.Clear();
                while (reader.Read())
                {
                    string nomprog4 = reader["nom_client"].ToString();
                    string prenomprog4 = reader["prenom_client"].ToString();

                    listBox4.Items.Add($"{nomprog4},{prenomprog4}");
                }
            }
        }

        public void LoadBestClient(MySqlConnection connexion)
        {
            string requete = "SELECT PARTICULIER.nom_client, PARTICULIER.prenom_client, COMMANDE.idClient, SUM(COMMANDE.prix_total) AS total FROM COMMANDE JOIN PARTICULIER ON PARTICULIER.idParticulier = COMMANDE.idClient GROUP BY COMMANDE.idClient ORDER BY total DESC LIMIT 1;";
            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                listBox5.Items.Clear();
                while (reader.Read())
                {
                    string nomC = reader["nom_client"].ToString();
                    string prenomC = reader["prenom_client"].ToString();
                    string montant = reader["total"].ToString();


                    listBox5.Items.Add($"{nomC} {prenomC} {montant} €");
                }
            }
        }


        public void LoadAVGCommande(MySqlConnection connexion)
        {
            string requete = "SELECT AVG(prix_total) FROM COMMANDE";
            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                listBox6.Items.Clear();
                while (reader.Read())
                {
                    string avg = reader["AVG(prix_total)"].ToString();

                    listBox6.Items.Add($"{avg} €");
                }
            }
        }

        public void LoadBestMagasin(MySqlConnection connexion)
        {
            string requete = "SELECT SUM(idCommande) FROM COMMANDE";
            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                listBox7.Items.Clear();
                while (reader.Read())
                {
                    string count = reader["SUM(idCommande)"].ToString();

                    listBox7.Items.Add($"{count} commandes");
                }
            }
        }

        public void LoadAVGRabais(MySqlConnection connexion)
        {
            string requete = "SELECT AVG(rabais) FROM RABAIS JOIN COMMANDE ON RABAIS.idRabais = COMMANDE.idRabais";
            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                listBox8.Items.Clear();
                while (reader.Read())
                {
                    string rabébé = reader["AVG(rabais)"].ToString();

                    listBox8.Items.Add($"{rabébé}%");
                }
            }
        }

        public void Bonus(MySqlConnection connexion)
        {
            string requete = "SELECT idEmploye, SUM(prix_total) FROM COMMANDE GROUP BY idEmploye";
            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                listBox9.Items.Clear();
                while (reader.Read())
                {
                    string employe = reader["idEmploye"].ToString();
                    string bonus = reader["SUM(prix_total)"].ToString();
                    float vraibonus = float.Parse(bonus) / 20; // on lui met 5% de bonus de vente 
                    listBox9.Items.Add($"{employe} à {vraibonus}");
                }
            }
        }

        


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}