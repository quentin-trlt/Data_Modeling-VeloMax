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
    public partial class ModifierClientPartForm : Form
    {
        private MySqlConnection connexion;
        public ModifierClientPartForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            ChargerClient();
            ChargerFidelio();
        }
        private void ChargerClient()
        {
            string requete = "SELECT nom_client, prenom_client, idClient FROM PARTICULIER P JOIN CLIENTELE C ON C.idParticulier = P.idParticulier;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {

                while (mySqlDataReader.Read())
                {

                    string nom = mySqlDataReader["nom_client"].ToString();
                    string prenom = mySqlDataReader["prenom_client"].ToString();
                    string idClient = mySqlDataReader["idClient"].ToString();
                    comboBox1.Items.Add($"{idClient} : {nom},  {prenom}");
                }
            }
        }
        private void comboBoxFournisseur_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            textBox5.Text = string.Empty;
            comboBox2.Text = string.Empty;

            if (comboBox1.SelectedItem != null)
            {
                string clientSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = clientSelectionne.IndexOf(':');
                string idClient = clientSelectionne.Substring(0, indexSeparateur - 1).Trim();

                ChargerAdresse(idClient);
                ChargerInfos(idClient);
            }
        }

        private void ChargerAdresse(string idClient)
        {
            string requeteAdresse = $"SELECT numero, rue, ville FROM ADRESSE WHERE idAdresse IN " +
                    $"(SELECT idAdresse FROM PARTICULIER P JOIN CLIENTELE C ON C.idParticulier = P.idParticulier " +
                    $"WHERE idClient = '{idClient}');";

            MySqlCommand cmdAdresse = new MySqlCommand(requeteAdresse, connexion);

            using (MySqlDataReader mySqlDataReader = cmdAdresse.ExecuteReader())
            {
                if (mySqlDataReader.Read())
                {
                    string numero = mySqlDataReader["numero"].ToString();
                    string rue = mySqlDataReader["rue"].ToString();
                    string ville = mySqlDataReader["ville"].ToString();

                    textBox1.Text = numero;
                    textBox2.Text = rue;
                    textBox3.Text = ville;
                }
            }
        }
        private void ChargerFidelio()
        {
            comboBox2.Items.Add("null - Pas de programme");

            string requeteFidelio = "SELECT idProgramme, description FROM FIDELIO";
            MySqlCommand cmdFidelio = new MySqlCommand(requeteFidelio, connexion);
            using (MySqlDataReader readerFidelio = cmdFidelio.ExecuteReader())
            {
                while (readerFidelio.Read())
                {
                    string idFidelio = readerFidelio["idProgramme"].ToString();
                    string descriptionFidelio = readerFidelio["description"].ToString();
                    comboBox2.Items.Add($"{idFidelio} - {descriptionFidelio}");
                }
            }
        }
        private void ChargerInfos(string idClient)
        {
            string requeteInfos = $"SELECT no_tel, courriel, P.idProgramme, F.description FROM PARTICULIER P " +
                                  $"JOIN CLIENTELE C ON C.idParticulier = P.idParticulier " +
                                  $"LEFT JOIN FIDELIO F ON F.idProgramme = P.idProgramme " +
                                  $"WHERE idClient = '{idClient}';";

            MySqlCommand cmdInfos = new MySqlCommand(requeteInfos, connexion);

            using (MySqlDataReader mySqlDataReader = cmdInfos.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    string tel = mySqlDataReader["no_tel"].ToString();
                    string mail = mySqlDataReader["courriel"].ToString();
                    textBox4.Text = tel;
                    textBox5.Text = mail;
                    string idFidelio = mySqlDataReader["idProgramme"].ToString();
                    string descriptionFidelio = mySqlDataReader["description"].ToString();
                    comboBox2.SelectedItem = $"{idFidelio} - {descriptionFidelio}";
                }
            }
        }

        private void buttonEnregistrer_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string clientSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = clientSelectionne.IndexOf(':');
                string idClient = clientSelectionne.Substring(0, indexSeparateur - 1).Trim();

                string numero = textBox1.Text;
                string rue = textBox2.Text;
                string ville = textBox3.Text;

                string requeteAdresse = $"UPDATE ADRESSE SET numero = '{numero}', rue = '{rue}', ville = '{ville}' " +
                                        $"WHERE idAdresse IN (SELECT idAdresse FROM PARTICULIER P " +
                                        $"JOIN CLIENTELE C ON C.idParticulier = P.idParticulier WHERE idClient = '{idClient}');";

                MySqlCommand cmdAdresse = new MySqlCommand(requeteAdresse, connexion);
                cmdAdresse.ExecuteNonQuery();

                string tel = textBox4.Text;
                string mail = textBox5.Text;
                string programmeSelectionne = comboBox2.SelectedItem.ToString();
                int indexSep = programmeSelectionne.IndexOf('-');
                string idFidelio = programmeSelectionne.Substring(0, indexSep - 1).Trim();

                string requeteInfos = $"UPDATE PARTICULIER SET no_tel = '{tel}', courriel = '{mail}', idProgramme = '{idFidelio}' " +
                                      $"WHERE idParticulier IN (SELECT idParticulier FROM CLIENTELE WHERE idClient = '{idClient}');";

                MySqlCommand cmdInfos = new MySqlCommand(requeteInfos, connexion);
                cmdInfos.ExecuteNonQuery();
                MessageBox.Show("Modifications enregistrées avec succès.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


    }
}

