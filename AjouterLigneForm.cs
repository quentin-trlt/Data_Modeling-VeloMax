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
    public partial class AjouterLigneForm : Form
    {
        private MySqlConnection connexion;
        public AjouterLigneForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            RemplirCommandes();
            RemplirChoixLC();
            AfficherLignes();
            MettreAJourPrixTotal();
        }
        private void RemplirCommandes()
        {
            string requeteC = "SELECT idCommande FROM COMMANDE";
            MySqlCommand cmdC = new MySqlCommand(requeteC, connexion);

            using (MySqlDataReader reader = cmdC.ExecuteReader())
            {
                while (reader.Read())
                {
                    string id = reader["idCommande"].ToString();
                    comboBox4.Items.Add($"{id}");
                }
            }
        }
        private void RemplirChoixLC()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add($"Vélo");
            comboBox1.Items.Add($"Pièce de rechange");
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = comboBox1.SelectedItem.ToString();
            comboBox2.Items.Clear();
            RemplirTypes(text);
            MettreAJourPrixTotal();
            AfficherLignes();
        }
        private void RemplirTypes(string text)
        {
            if (text == "Vélo")
            {
                string requeteType = "SELECT DISTINCT ligne FROM MODELE";
                MySqlCommand cmdMagasins = new MySqlCommand(requeteType, connexion);

                using (MySqlDataReader reader = cmdMagasins.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string ligne = reader["ligne"].ToString();
                        comboBox2.Items.Add($"{ligne}");
                    }
                }
            }
            else if (text == "Pièce de rechange")
            {
                string requeteType = "SELECT DISTINCT libelle FROM PIECE";
                MySqlCommand cmdMagasins = new MySqlCommand(requeteType, connexion);

                using (MySqlDataReader reader = cmdMagasins.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string libelle = reader["libelle"].ToString();
                        comboBox2.Items.Add($"{libelle}");
                    }
                }
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = comboBox1.SelectedItem.ToString();
            string reference = comboBox2.SelectedItem.ToString();
            comboBox3.Items.Clear();
            RemplirReferences(text, reference);
        }
        private void RemplirReferences(string text, string reference)
        {
            if (text == "Vélo")
            {
                string requeteType = $"SELECT idModele FROM MODELE WHERE ligne = '{reference}';";
                MySqlCommand cmdMagasins = new MySqlCommand(requeteType, connexion);

                using (MySqlDataReader reader = cmdMagasins.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idModele = reader["idModele"].ToString();
                        comboBox3.Items.Add($"{idModele}");
                    }
                }
            }
            else if (text == "Pièce de rechange")
            {
                string requeteType = $"SELECT idPiece FROM PIECE WHERE libelle = '{reference}'";
                MySqlCommand cmdMagasins = new MySqlCommand(requeteType, connexion);

                using (MySqlDataReader reader = cmdMagasins.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idPiece = reader["idPiece"].ToString();
                        comboBox3.Items.Add($"{idPiece}");
                    }
                }
            }
        }
        private void AfficherLignes()
        {           
            listBox1.Items.Clear();

            if (comboBox4.SelectedItem != null)
            {
                string idCommande = comboBox4.SelectedItem.ToString();
                string requeteLignesModele = $"SELECT * FROM LIGNE_COMMANDE_MODELE WHERE idCommande = '{idCommande}'";
                MySqlCommand cmdLignesModele = new MySqlCommand(requeteLignesModele, connexion);

                using (MySqlDataReader reader = cmdLignesModele.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idModele = reader["idModele"].ToString();
                        string quantite = reader["quantite"].ToString();
                        string prixLigne = reader["prix_ligne"].ToString();
                        string ligneCommande = $"Modèle: {idModele}, Quantité: {quantite}, Prix: {prixLigne}";
                        listBox1.Items.Add(ligneCommande);
                    }
                }
                string requeteLignesPiece = $"SELECT * FROM LIGNE_COMMANDE_PIECE WHERE idCommande = '{idCommande}'";
                MySqlCommand cmdLignesPiece = new MySqlCommand(requeteLignesPiece, connexion);

                using (MySqlDataReader reader = cmdLignesPiece.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idPiece = reader["idPiece"].ToString();
                        string quantite = reader["quantite"].ToString();
                        string prixLigne = reader["prix_ligne"].ToString();
                        string ligneCommande = $"Pièce: {idPiece}, Quantité: {quantite}, Prix: {prixLigne}";
                        listBox1.Items.Add(ligneCommande);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande.");
            }
        }
        private void AjouterLigneCommande()
        {
            string idCommande = comboBox4.SelectedItem?.ToString();
            string quantite = textBox1.Text;
            string text = comboBox1.SelectedItem?.ToString();
            string reference = comboBox3.SelectedItem?.ToString();
            string prix = "0";

            if (!string.IsNullOrWhiteSpace(idCommande) && !string.IsNullOrWhiteSpace(quantite) && !string.IsNullOrWhiteSpace(reference))
            {
                // Obtention du prix et insertion de la ligne de commande
                if (text == "Vélo")
                {
                    string requetePrix = $"SELECT prix FROM MODELE WHERE idModele = '{reference}';";
                    MySqlCommand cmdPrix = new MySqlCommand(requetePrix, connexion);
                    using (MySqlDataReader mySqlDataReader = cmdPrix.ExecuteReader())
                    {
                        if (mySqlDataReader.Read())
                        {
                            prix = mySqlDataReader["prix"].ToString();
                        }
                    }
                    int prix_ligne = Convert.ToInt32(quantite) * Convert.ToInt32(prix);

                    string requete = $"INSERT INTO LIGNE_COMMANDE_MODELE (idCommande, idModele, quantite, prix_ligne) VALUES('{idCommande}', '{reference}', '{quantite}','{prix_ligne}');";
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    int lignesModifiees = cmd.ExecuteNonQuery();
                    if (lignesModifiees > 0)
                    {
                        MessageBox.Show("Ligne de commande ajoutée avec succès !");
                        // Affichage de la ligne de commande ajoutée dans la listBox
                        string ligneCommande = $"Modèle: {reference}, Quantité: {quantite}, Prix: {prix_ligne}";
                        listBox1.Items.Add(ligneCommande);
                        MettreAJourPrixTotal();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout de la ligne de commande.");
                    }
                }
                else if (text == "Pièce de rechange")
                {
                    string requetePrix = $"SELECT min(prixUnitaire) FROM PIECE_FOURNIT WHERE idPiece = '{reference}';";
                    MySqlCommand cmdPrix = new MySqlCommand(requetePrix, connexion);
                    using (MySqlDataReader mySqlDataReader = cmdPrix.ExecuteReader())
                    {
                        if (mySqlDataReader.Read())
                        {
                            prix = mySqlDataReader["min(prixUnitaire)"].ToString();
                        }
                    }
                    int prix_ligne = Convert.ToInt32(quantite) * Convert.ToInt32(prix);

                    string requete = $"INSERT INTO LIGNE_COMMANDE_PIECE (idCommande, idPiece, quantite, prix_ligne) VALUES('{idCommande}', '{reference}', '{quantite}','{prix_ligne}');";
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    int lignesModifiees = cmd.ExecuteNonQuery();
                    if (lignesModifiees > 0)
                    {
                        MessageBox.Show("Ligne de commande ajoutée avec succès !");
                        // Affichage de la ligne de commande ajoutée dans la listBox
                        string ligneCommande = $"Pièce: {reference}, Quantité: {quantite}, Prix: {prix_ligne}";
                        listBox1.Items.Add(ligneCommande);
                        MettreAJourPrixTotal();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout de la ligne de commande.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
            }
        }
        private void MettreAJourPrixTotal()
        {
            string idCommande = comboBox4.SelectedItem?.ToString();
            if (!string.IsNullOrWhiteSpace(idCommande))
            {
                string requetePrixTotal = $"SELECT SUM(prix_ligne) AS prix_total FROM LIGNE_COMMANDE_MODELE WHERE idCommande = '{idCommande}'" +
                                          $"UNION ALL SELECT SUM(prix_ligne) AS prix_total FROM LIGNE_COMMANDE_PIECE WHERE idCommande = '{idCommande}'";
                MySqlCommand cmdPrixTotal = new MySqlCommand(requetePrixTotal, connexion);
                double prixTotal = 0;

                using (MySqlDataReader reader = cmdPrixTotal.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prixTotal += Convert.ToDouble(reader["prix_total"]);
                    }
                }

                string requeteUpdatePrixTotal = $"UPDATE COMMANDE SET prix_total = '{prixTotal}' WHERE idCommande = '{idCommande}'";
                MySqlCommand cmdUpdatePrixTotal = new MySqlCommand(requeteUpdatePrixTotal, connexion);
                int lignesModifiees = cmdUpdatePrixTotal.ExecuteNonQuery();

                if (lignesModifiees > 0)
                {
                    MessageBox.Show("Prix total mis à jour avec succès !");
                    // Affichage du prix total dans la TextBox
                    textBox3.Text = prixTotal.ToString();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du prix total.");
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande.");
            }
        }

        private void btnAjouterLigne_Click(object sender, EventArgs e)
        {
            AjouterLigneCommande();
            AfficherLignes();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AjouterLigneForm_Load(object sender, EventArgs e)
        {

        }

   
    }
}
