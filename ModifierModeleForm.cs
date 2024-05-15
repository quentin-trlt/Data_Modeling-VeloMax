using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VeloMax
{
    public partial class ModifierModeleForm : Form
    {
        private MySqlConnection connexion;

        public ModifierModeleForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            ChargerModeles();
            ChargerPieces();
        }

        private void ChargerModeles()
        {
            string requete = "SELECT idModele, nom, grandeur FROM MODELE;";

            MySqlCommand cmd = connexion.CreateCommand();
            cmd.CommandText = requete;

            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
            {

                while (mySqlDataReader.Read())
                {

                    string idModele = mySqlDataReader["idModele"].ToString();
                    string nom = mySqlDataReader["nom"].ToString();
                    string taille = mySqlDataReader["grandeur"].ToString();

                    comboBox1.Items.Add($"{idModele} : {nom} - {taille}");
                }
            }
        }

        private void ChargerListe()
        {
            if (comboBox1.SelectedItem != null)
            {
                string modeleSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = modeleSelectionne.IndexOf(':');
                string idModele = modeleSelectionne.Substring(0, indexSeparateur - 1).Trim();

                string requete = "SELECT P.libelle, P.idPiece FROM PIECE P JOIN ASSEMBLAGE A ON A.idPiece = P.idPiece " +
                                 " JOIN MODELE M ON M.idModele = A.idModele WHERE A.idModele = @idModele;";
                MySqlCommand cmd = new MySqlCommand(requete, connexion);
                cmd.Parameters.AddWithValue("@idModele", idModele);
                StringBuilder listePieces = new StringBuilder();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idPiece = reader["idPiece"].ToString();
                        string libellePiece = reader["libelle"].ToString();
                        listePieces.AppendLine($"{libellePiece} : {idPiece}");

                    }
                }

            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un modèle.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ChargerPieces()
        {
            string requete = "SELECT idPiece, libelle FROM PIECE;";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string idPiece = reader["idPiece"].ToString();
                    string libellePiece = reader["libelle"].ToString();
                    switch (libellePiece)
                    {
                        case "Cadre":
                            comboBox2.Items.Add($"{idPiece}");
                            break;
                        case "Guidon":
                            comboBox3.Items.Add($"{idPiece}");
                            break;
                        case "Selle":
                            comboBox4.Items.Add($"{idPiece}");
                            break;
                        case "Frein":
                            comboBox5.Items.Add($"{idPiece}");
                            break;
                        case "Dérailleur avant":
                            comboBox6.Items.Add($"{idPiece}");
                            break;
                        case "Dérailleur arrière":
                            comboBox7.Items.Add($"{idPiece}");
                            break;
                        case "Roue avant":
                            comboBox8.Items.Add($"{idPiece}");
                            break;
                        case "Roue arrière":
                            comboBox9.Items.Add($"{idPiece}");
                            break;
                        case "Réflecteur":
                            comboBox10.Items.Add($"{idPiece}");
                            break;
                        case "Ordinateur":
                            comboBox11.Items.Add($"{idPiece}");
                            break;
                        case "Panier":
                            comboBox12.Items.Add($"{idPiece}");
                            break;
                        case "Pédalier":
                            comboBox13.Items.Add($"{idPiece}");
                            break;                    
                    }
                }
            }
        }

        private void comboBoxModele_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string modeleSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = modeleSelectionne.IndexOf(':');
                string idModele = modeleSelectionne.Substring(0, indexSeparateur).Trim();
                if (idModele != null)
                {
                    ChargerPiecesModele(idModele);
                }
            }
        }

        private void ChargerPiecesModele(string idModele)
        {
            string requete = "SELECT P.idPiece, libelle FROM PIECE P JOIN ASSEMBLAGE ON ASSEMBLAGE.idPiece = P.idPiece WHERE idModele = @idModele;";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);
            cmd.Parameters.AddWithValue("@idModele", idModele);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string idPiece = reader["idPiece"].ToString();
                    string libellePiece = reader["libelle"].ToString();

                    // Recherche de la pièce dans les autres comboBox et sélection
                    foreach (Control control in Controls)
                    {
                        if (control is System.Windows.Forms.ComboBox comboBox && comboBox.Name != "comboBox1") // Ignorer la comboBox1
                        {
                            if (comboBox.Items.Contains(idPiece))
                            {
                                comboBox.SelectedItem = idPiece;
                            }
                        }
                    }
                }
            }
        }

        

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            string modeleSelectionne = comboBox1.SelectedItem.ToString();
            int indexSeparateur = modeleSelectionne.IndexOf(':');
            string idModele = modeleSelectionne.Substring(0, indexSeparateur).Trim();
            if (modeleSelectionne != null)
            {
                // Enregistrer les modifications des pièces associées au modèle sélectionné dans la base de données
                EnregistrerPiecesModele(idModele);
                MessageBox.Show("Modifications enregistrées avec succès !");
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un modèle.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EnregistrerPiecesModele(string idModele)
        {
            
            string requeteSuppression = "DELETE FROM ASSEMBLAGE WHERE idModele = @idModele;";
            MySqlCommand cmdSuppression = new MySqlCommand(requeteSuppression, connexion);
            cmdSuppression.Parameters.AddWithValue("@idModele", idModele);
            cmdSuppression.ExecuteNonQuery();

            if (comboBox2.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox2.SelectedItem).ToString());
            if (comboBox3.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox3.SelectedItem).ToString());
            if (comboBox4.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox4.SelectedItem).ToString());
            if (comboBox5.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox5.SelectedItem).ToString());
            if (comboBox6.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox6.SelectedItem).ToString());
            if (comboBox7.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox7.SelectedItem).ToString());
            if (comboBox8.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox8.SelectedItem).ToString());
            if (comboBox9.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox9.SelectedItem).ToString());
            if (comboBox10.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox10.SelectedItem).ToString());
            if (comboBox11.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox11.SelectedItem).ToString());
            if (comboBox12.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox12.SelectedItem).ToString());
            if (comboBox13.SelectedItem != null)
                EnregistrerPieceModele(idModele, (comboBox13.SelectedItem).ToString());

            string requeteUpdateNom = "UPDATE MODELE SET nom = @nom WHERE idModele = @idModele;";
            MySqlCommand cmdUpdateNom = new MySqlCommand(requeteUpdateNom, connexion);
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                cmdUpdateNom.Parameters.AddWithValue("@nom", textBox2.Text);
                cmdUpdateNom.Parameters.AddWithValue("@idModele", idModele);
                cmdUpdateNom.ExecuteNonQuery();
            }

            string requeteUpdatePrix = "UPDATE MODELE SET prix = @prix WHERE idModele = @idModele;";
            MySqlCommand cmdUpdatePrix = new MySqlCommand(requeteUpdatePrix, connexion);
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                cmdUpdatePrix.Parameters.AddWithValue("@prix", textBox3.Text);
                cmdUpdatePrix.Parameters.AddWithValue("@idModele", idModele);
                cmdUpdatePrix.ExecuteNonQuery();
            }

            string requeteUpdateDateFin = "UPDATE MODELE SET date_fin = @date_fin WHERE idModele = @idModele;";
            MySqlCommand cmdUpdateDateFin = new MySqlCommand(requeteUpdateDateFin, connexion);
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                cmdUpdateDateFin.Parameters.AddWithValue("@date_fin", textBox4.Text); 
                cmdUpdateDateFin.Parameters.AddWithValue("@idModele", idModele);
                cmdUpdateDateFin.ExecuteNonQuery();
            }
        }

        private void EnregistrerPieceModele(string idModele, string idPiece)
        {
            if (!string.IsNullOrEmpty(idPiece))
            {
                string requeteInsertion = "INSERT INTO ASSEMBLAGE (idModele, idPiece) VALUES (@idModele, @idPiece);";
                MySqlCommand cmdInsertion = new MySqlCommand(requeteInsertion, connexion);
                cmdInsertion.Parameters.AddWithValue("@idModele", idModele);
                cmdInsertion.Parameters.AddWithValue("@idPiece", idPiece);
                cmdInsertion.ExecuteNonQuery();
            }
        }

        private void ModifierModeleForm_Load(object sender, EventArgs e)
        {

        }
    }

}
