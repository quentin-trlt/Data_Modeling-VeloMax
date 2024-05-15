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
    public partial class ModifierClientProForm : Form
    {
        private MySqlConnection connexion;
        public ModifierClientProForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            ChargerClient();
        }
        private void ChargerClient()
        {
            string requete = "SELECT nom_entreprise, E.siret, idClient FROM ENTREPRISE E JOIN CLIENTELE C ON C.siret = E.siret;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {

                while (mySqlDataReader.Read())
                {

                    string nom = mySqlDataReader["nom_entreprise"].ToString();
                    string siret = mySqlDataReader["siret"].ToString();
                    string idClient = mySqlDataReader["idClient"].ToString();
                    comboBox1.Items.Add($"{idClient} : {nom},  {siret}");
                }
            }
        }
        private void comboBoxFournisseur_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;;
            textBox5.Text = string.Empty;

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
                    $"(SELECT idAdresse FROM ENTREPRISE E JOIN CLIENTELE C ON C.siret = E.siret " +
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
        private void ChargerInfos(string idClient)
        {
            string requeteInfos = $"SELECT courriel FROM ENTREPRISE E JOIN CLIENTELE C ON C.siret = E.siret " +
                $"WHERE idClient = '{idClient}';";

            MySqlCommand cmdInfos = new MySqlCommand(requeteInfos, connexion);

            using (MySqlDataReader mySqlDataReader = cmdInfos.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    string mail = mySqlDataReader["courriel"].ToString();
                    textBox5.Text = mail;
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
                                        $"WHERE idAdresse IN (SELECT idAdresse FROM ENTREPRISE E " +
                                        $"JOIN CLIENTELE C ON C.siret = E.siret WHERE idClient = '{idClient}');";

                MySqlCommand cmdAdresse = new MySqlCommand(requeteAdresse, connexion);
                cmdAdresse.ExecuteNonQuery();

                string mail = textBox5.Text;


                string requeteInfos = $"UPDATE ENTREPRISE SET courriel = '{mail}' " +
                                      $"WHERE siret IN (SELECT siret FROM CLIENTELE WHERE idClient = '{idClient}');";

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
