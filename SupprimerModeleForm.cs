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
    public partial class SupprimerModeleForm : Form
    {
        private MySqlConnection connexion;

        public SupprimerModeleForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            ChargerModeles();
        }

        private void ChargerModeles()
        {
            comboBox1.Items.Clear();

            // Requête pour récupérer les noms des modèles avec leurs IDs et leurs tailles
            string requete = "SELECT CONCAT(idModele, ': ', nom, ' - ', grandeur) AS modele FROM MODELE;";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["modele"].ToString());
                }
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string modeleSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = modeleSelectionne.IndexOf(':');
                string idModele = modeleSelectionne.Substring(0, indexSeparateur).Trim();
                DialogResult confirmation = MessageBox.Show($"Voulez-vous vraiment supprimer le modèle '{modeleSelectionne}' ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmation == DialogResult.Yes)
                {
                    string requete1 = "DELETE FROM ASSEMBLAGE WHERE idModele = @idModele;";
                    string requete = "DELETE FROM MODELE WHERE idModele = @idModele;";
                    MySqlCommand cmd1 = new MySqlCommand(requete1, connexion);
                    cmd1.Parameters.AddWithValue("@idModele", idModele);
                    cmd1.ExecuteNonQuery();
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    cmd.Parameters.AddWithValue("@idModele", idModele);

                    int lignesModifiees = cmd.ExecuteNonQuery();
                    if (lignesModifiees > 0)
                    {
                        MessageBox.Show("Modèle supprimé avec succès !");
                        ChargerModeles(); // Rafraîchir la liste des modèles
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression du modèle.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un modèle à supprimer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
