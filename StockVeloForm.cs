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
    public partial class StockVeloForm : Form
    {
        private MySqlConnection connexion;
        public StockVeloForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
        }
        private void ChargerTypesPieces()
        {
            string requete = "SELECT DISTINCT ligne FROM MODELE";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                checkedListBox1.Items.Clear();
                while (reader.Read())
                {
                    string libellePiece = reader["ligne"].ToString();
                    checkedListBox1.Items.Add(libellePiece);
                }
            }
        }

        private void ChargerMagasins()
        {
            string requete = "SELECT idMagasin, numero, rue, ville FROM MAGASIN M JOIN ADRESSE A ON A.idAdresse = M.idAdresse";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                comboBox1.Items.Clear();
                while (reader.Read())
                {
                    string idMagasin = reader["idMagasin"].ToString();
                    string adresse = reader["numero"].ToString() + ", " + reader["rue"].ToString() + ", " + reader["ville"].ToString();
                    comboBox1.Items.Add($"{idMagasin} - {adresse}");
                }
                comboBox1.Items.Add($"Tous les magasins");
            }
        }

        private void ChargerStock()
        {
            listBox1.Items.Clear();
            if (comboBox1.SelectedItem != null && Convert.ToString(comboBox1.SelectedItem) != "Tous les magasins")
            {
                string magasinSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = magasinSelectionne.IndexOf('-');
                string idMagasin = magasinSelectionne.Substring(0, indexSeparateur - 1).Trim();
                string requete = "SELECT S.idModele, nom, grandeur, quantite FROM STOCK_VELO S " +
                    "JOIN MODELE M ON M.idModele = S.idModele WHERE idMagasin = @idMagasin " +
                    "GROUP BY S.idModele, M.nom, M.grandeur";

                List<string> typesSelectionnes = new List<string>();
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    typesSelectionnes.Add(item.ToString());
                }

                if (typesSelectionnes.Count > 0)
                {
                    string types = string.Join(",", typesSelectionnes.Select(t => $"'{t}'"));
                    requete += $" AND S.idModele IN (SELECT idModele FROM MODELE WHERE ligne IN ({types}))";
                }

                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@idMagasin", idMagasin);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idModele = reader["idModele"].ToString();
                        string nom= reader["nom"].ToString();
                        string taille = reader["grandeur"].ToString();
                        int quantite = Convert.ToInt32(reader["quantite"]);
                        listBox1.Items.Add($"{idModele} - {nom} - {taille} - Quantité : {quantite}");
                    }
                }
            }
            else if (Convert.ToString(comboBox1.SelectedItem) == "Tous les magasins")
            {
                string requete = "SELECT S.idModele, nom, grandeur, SUM(quantite) AS quantite_totale FROM STOCK_VELO S " +
                    "JOIN MODELE M ON M.idModele = S.idModele " +
                    "GROUP BY S.idModele, M.nom, M.grandeur";

                List<string> typesSelectionnes = new List<string>();
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    typesSelectionnes.Add(item.ToString());
                }

                if (typesSelectionnes.Count > 0)
                {
                    string types = string.Join(",", typesSelectionnes.Select(t => $"'{t}'"));
                    requete += $" AND S.idModele IN (SELECT idModele FROM MODELE WHERE ligne IN ({types}))";
                }

                MySqlCommand cmd = new MySqlCommand(requete, connexion);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idModele = reader["idModele"].ToString();
                        string nom = reader["nom"].ToString();
                        string taille = reader["grandeur"].ToString();
                        int quantite = Convert.ToInt32(reader["quantite"]);
                        listBox1.Items.Add($"{idModele} - {nom} - {taille} - Quantité : {quantite}");
                    }
                }
            }
        }
        private void ClearListBox()
        {
            listBox1.Items.Clear();
        }

        private void comboBoxMagasin_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearListBox();
            ChargerStock();
        }

        private void checkedListBoxTypes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ClearListBox();
            ChargerStock();
        }

        private void StockForm_Load(object sender, EventArgs e)
        {
            ChargerMagasins();
            ChargerTypesPieces();
        }
        private void buttonAjouterVelo_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && !string.IsNullOrWhiteSpace(textBox1.Text) && Convert.ToString(comboBox1.SelectedItem) != "Tous les magasins")
            {
                string magasinSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = magasinSelectionne.IndexOf('-');
                string idMagasin = magasinSelectionne.Substring(0, indexSeparateur - 1).Trim();

                string idVelo = textBox1.Text;
                int quantite = Convert.ToInt32(textBox2.Text);

                AjouterVelo(idMagasin, idVelo, quantite);
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs pour ajouter un vélo.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void AjouterVelo(string idMagasin, string idVelo, int quantite)
        {
            string requeteVerification = $"SELECT COUNT(*) FROM STOCK_VELO WHERE idMagasin = '{idMagasin}' AND idModele = '{idVelo}';";
            MySqlCommand cmdVerification = new MySqlCommand(requeteVerification, connexion);
            int nombreVelos = Convert.ToInt32(cmdVerification.ExecuteScalar());

            if (nombreVelos > 0)
            {
                string requeteMiseAJour = $"UPDATE STOCK_VELO SET quantite = quantite + {quantite} " +
                                          $"WHERE idMagasin = '{idMagasin}' AND idModele = '{idVelo}';";
                MySqlCommand cmdMiseAJour = new MySqlCommand(requeteMiseAJour, connexion);
                cmdMiseAJour.ExecuteNonQuery();
            }
            else
            {
                string requeteAjout = $"INSERT INTO STOCK_VELO(idMagasin, idModele, quantite) " +
                                      $"VALUES ('{idMagasin}', '{idVelo}', {quantite});";
                MySqlCommand cmdAjout = new MySqlCommand(requeteAjout, connexion);
                cmdAjout.ExecuteNonQuery();
            }
            listBox1.Items.Clear();
            ChargerStock();
        }
    }
}
