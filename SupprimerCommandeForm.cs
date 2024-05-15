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
    public partial class SupprimerCommandeForm : Form
    {
        private MySqlConnection connexion;
        public SupprimerCommandeForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            ChargerCommandes();
        }

        private void ChargerCommandes()
        {
            comboBox1.Items.Clear();

            string requete = "SELECT CONCAT(idCommande, ' : ', idMagasin, ', ', dateCommande) AS commande FROM COMMANDE;";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["commande"].ToString());
                }
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string commandeSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = commandeSelectionne.IndexOf(':');
                string idCommande = commandeSelectionne.Substring(0, indexSeparateur - 1).Trim();
                DialogResult confirmation = MessageBox.Show($"Voulez-vous vraiment supprimer la commande '{commandeSelectionne}' ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmation == DialogResult.Yes)
                {
                    string requete1 = "DELETE FROM LIGNE_COMMANDE WHERE idCommande = @idCommande;";
                    string requete = "DELETE FROM COMMANDE WHERE idCommande = @idCommande;";
                    MySqlCommand cmd1 = new MySqlCommand(requete1, connexion);
                    cmd1.Parameters.AddWithValue("@idCommande", idCommande);
                    cmd1.ExecuteNonQuery();
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    cmd.Parameters.AddWithValue("@idCommande", idCommande);

                    int lignesModifiees = cmd.ExecuteNonQuery();
                    if (lignesModifiees > 0)
                    {
                        MessageBox.Show("Commande supprimée avec succès !");
                        ChargerCommandes();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de la commande.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}