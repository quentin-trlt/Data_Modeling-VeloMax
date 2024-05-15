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
    public partial class SupprimerFournisseurForm : Form
    {
        private MySqlConnection connexion;
        public SupprimerFournisseurForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            ChargerModeles();
        }

        private void ChargerModeles()
        {
            comboBox1.Items.Clear();

            string requete = "SELECT CONCAT(nom_entreprise, ' : ', siret) AS fournisseur FROM FOURNISSEUR;";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["fournisseur"].ToString());
                }
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string fournisseurSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = fournisseurSelectionne.IndexOf(':');
                string siret = fournisseurSelectionne.Substring(indexSeparateur + 1).Trim();
                DialogResult confirmation = MessageBox.Show($"Voulez-vous vraiment supprimer le fournisseur '{fournisseurSelectionne}' ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmation == DialogResult.Yes)
                {
                    string requete1 = "DELETE FROM PIECE_FOURNIT WHERE siret = @siret;";
                    string requete = "DELETE FROM FOURNISSEUR WHERE siret = @siret;";
                    MySqlCommand cmd1 = new MySqlCommand(requete1, connexion);
                    cmd1.Parameters.AddWithValue("@siret", siret);
                    cmd1.ExecuteNonQuery();
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    cmd.Parameters.AddWithValue("@siret", siret);

                    int lignesModifiees = cmd.ExecuteNonQuery();
                    if (lignesModifiees > 0)
                    {
                        MessageBox.Show("Fournisseur supprimé avec succès !");
                        ChargerModeles();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression du fournisseur.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un fournisseur à supprimer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}