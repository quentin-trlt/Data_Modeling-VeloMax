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
    public partial class StockPieceForm : Form
    {
        private MySqlConnection connexion;
        public StockPieceForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
        }
        private void ChargerTypesPieces()
        {
            string requete = "SELECT DISTINCT libelle FROM PIECE";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {                
                checkedListBox1.Items.Clear();
                while (reader.Read())
                {
                    string libellePiece = reader["libelle"].ToString();
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
                string requete = "SELECT idPiece, quantite FROM STOCK_PIECE WHERE idMagasin = @idMagasin";

                List<string> typesSelectionnes = new List<string>();
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    typesSelectionnes.Add(item.ToString());
                }

                if (typesSelectionnes.Count > 0)
                {
                    string types = string.Join(",", typesSelectionnes.Select(t => $"'{t}'"));
                    requete += $" AND idPiece IN (SELECT idPiece FROM PIECE WHERE libelle IN ({types}))";
                }

                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@idMagasin", idMagasin);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idPiece = reader["idPiece"].ToString();
                        int quantite = Convert.ToInt32(reader["quantite"]);
                        listBox1.Items.Add($"{idPiece} - Quantité : {quantite}");
                    }
                }
            }
            else if (Convert.ToString(comboBox1.SelectedItem) == "Tous les magasins")
            {
                string requete = "SELECT idPiece, SUM(quantite) AS quantite_totale " +
                                 "FROM STOCK_PIECE " +
                                 "GROUP BY idPiece";

                List<string> typesSelectionnes = new List<string>();
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    typesSelectionnes.Add(item.ToString());
                }

                if (typesSelectionnes.Count > 0)
                {
                    string types = string.Join(",", typesSelectionnes.Select(t => $"'{t}'"));
                    requete += $" HAVING idPiece IN (SELECT idPiece FROM PIECE WHERE libelle IN ({types}))";
                }

                MySqlCommand cmd = new MySqlCommand(requete, connexion);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idPiece = reader["idPiece"].ToString();
                        int quantiteTotale = Convert.ToInt32(reader["quantite_totale"]);
                        listBox1.Items.Add($"{idPiece} - Quantité totale : {quantiteTotale}");
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
        private void buttonAjouterPiece_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && !string.IsNullOrWhiteSpace(textBox1.Text) && Convert.ToString(comboBox1.SelectedItem) != "Tous les magasins")
            {
                string magasinSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = magasinSelectionne.IndexOf('-');
                string idMagasin = magasinSelectionne.Substring(0, indexSeparateur - 1).Trim();

                string idPiece = textBox1.Text;
                int quantite = Convert.ToInt32(textBox2.Text);

                AjouterPiece(idMagasin ,idPiece, quantite);
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs pour ajouter une pièce.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void AjouterPiece(string idMagasin, string idPiece, int quantite)
        {
            string requeteVerification = $"SELECT COUNT(*) FROM STOCK_PIECE WHERE idMagasin = '{idMagasin}' AND idPiece = '{idPiece}';";
            MySqlCommand cmdVerification = new MySqlCommand(requeteVerification, connexion);
            int nombrePieces = Convert.ToInt32(cmdVerification.ExecuteScalar());

            if (nombrePieces > 0)
            {
                string requeteMiseAJour = $"UPDATE STOCK_PIECE SET quantite = quantite + {quantite} " +
                                          $"WHERE idMagasin = '{idMagasin}' AND idPiece = '{idPiece}';";
                MySqlCommand cmdMiseAJour = new MySqlCommand(requeteMiseAJour, connexion);
                cmdMiseAJour.ExecuteNonQuery();
            }
            else
            {
                string requeteAjout = $"INSERT INTO STOCK_PIECE (idMagasin, idPiece, quantite) " +
                                      $"VALUES ('{idMagasin}', '{idPiece}', {quantite});";
                MySqlCommand cmdAjout = new MySqlCommand(requeteAjout, connexion);
                cmdAjout.ExecuteNonQuery();
            }
            listBox1.Items.Clear();
            ChargerStock();
        }
    }
}
