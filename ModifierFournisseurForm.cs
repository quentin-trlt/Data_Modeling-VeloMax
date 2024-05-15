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
    public partial class ModifierFournisseurForm : Form
    {
        private MySqlConnection connexion;
        public ModifierFournisseurForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            ChargerFournisseur();
        }
        private void ChargerFournisseur()
        {
            string requete = "SELECT nom_entreprise, siret FROM FOURNISSEUR F JOIN ADRESSE A ON A.idAdresse = F.idAdresse;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {

                while (mySqlDataReader.Read())
                {

                    string nom = mySqlDataReader["nom_entreprise"].ToString();
                    string siret = mySqlDataReader["siret"].ToString();
                    comboBox1.Items.Add($"{nom} : {siret}" );
                }
            }
        }
        private void comboBoxFournisseur_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            listBox1.Items.Clear();

            if (comboBox1.SelectedItem != null)
            {
                string fournisseurSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = fournisseurSelectionne.IndexOf(':');
                string siret = fournisseurSelectionne.Substring(indexSeparateur + 1).Trim();

                ChargerAdresse(siret);
                ChargerPiecesFournies(siret); 
            }
        }

        private void ChargerAdresse(string siret)
        {
            string requeteAdresse = $"SELECT numero, rue, ville FROM ADRESSE WHERE idAdresse IN (SELECT idAdresse FROM FOURNISSEUR WHERE siret = '{siret}');";

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

        private void ChargerPiecesFournies(string siret)
        {
            string requetePieces = $"SELECT idPiece, prixUnitaire, delai FROM PIECE_FOURNIT WHERE siret = '{siret}';";

            MySqlCommand cmdPieces = new MySqlCommand(requetePieces, connexion);

            using (MySqlDataReader mySqlDataReader = cmdPieces.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    string idPiece = mySqlDataReader["idPiece"].ToString();
                    double prixUnitaire = Convert.ToDouble(mySqlDataReader["prixUnitaire"]);
                    int delai = Convert.ToInt32(mySqlDataReader["delai"]);

                    listBox1.Items.Add($"{idPiece} - Prix unitaire: {prixUnitaire} - Délai: {delai}");
                }
            }
        }
        private void buttonSupprimerPiece_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string fournisseurSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = fournisseurSelectionne.IndexOf(':');
                string siret = fournisseurSelectionne.Substring(indexSeparateur + 1).Trim();

                string pieceSelectionnee = listBox1.SelectedItem.ToString();
                int indexSeparateurPiece = pieceSelectionnee.IndexOf('-');
                string idPiece = pieceSelectionnee.Substring(0, indexSeparateurPiece - 1).Trim();

                SupprimerPiece(siret, idPiece);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une pièce à supprimer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void SupprimerPiece(string siret, string idPiece)
        {
            string requeteSuppression = $"DELETE FROM PIECE_FOURNIT WHERE siret = '{siret}' AND idPiece = '{idPiece}';";

            MySqlCommand cmdSuppression = new MySqlCommand(requeteSuppression, connexion);
            cmdSuppression.ExecuteNonQuery();
            listBox1.Items.Clear();
            ChargerPiecesFournies(siret);

        }

        private void buttonAjouterPiece_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && !string.IsNullOrWhiteSpace(textBox4.Text) && !string.IsNullOrWhiteSpace(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox6.Text))
            {
                string fournisseurSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = fournisseurSelectionne.IndexOf(':');
                string siret = fournisseurSelectionne.Substring(indexSeparateur + 1).Trim();

                string idPiece = textBox4.Text;
                double prixUnitaire = Convert.ToDouble(textBox5.Text);
                int delai = Convert.ToInt32(textBox6.Text);

                AjouterPiece(siret, idPiece, prixUnitaire, delai);
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs pour ajouter une pièce.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void AjouterPiece(string siret, string idPiece, double prixUnitaire, int delai)
        {
            string requeteAjout = $"INSERT INTO PIECE_FOURNIT (idPiece, siret, prixUnitaire, delai) " +
                                  $"VALUES ('{idPiece}', '{siret}', {prixUnitaire}, {delai});";

            MySqlCommand cmdAjout = new MySqlCommand(requeteAjout, connexion);
            cmdAjout.ExecuteNonQuery();
            listBox1.Items.Clear();
            ChargerPiecesFournies(siret);
        }

        private void buttonEnregistrer_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string fournisseurSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = fournisseurSelectionne.IndexOf(':');
                string siret = fournisseurSelectionne.Substring(indexSeparateur + 1).Trim();

                // Logique pour enregistrer les modifications de l'adresse
                string numero = textBox1.Text;
                string rue = textBox2.Text;
                string ville = textBox3.Text;

                string requeteAdresse = $"UPDATE ADRESSE SET numero = '{numero}', rue = '{rue}', ville = '{ville}' " +
                                        $"WHERE idAdresse IN (SELECT idAdresse FROM FOURNISSEUR WHERE siret = '{siret}');";

                MySqlCommand cmdAdresse = new MySqlCommand(requeteAdresse, connexion);
                cmdAdresse.ExecuteNonQuery();
                MessageBox.Show("Modifications enregistrées avec succès.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un fournisseur.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


    }
}
