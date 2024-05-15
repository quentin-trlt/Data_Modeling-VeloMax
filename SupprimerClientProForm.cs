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
    public partial class SupprimerClientProForm : Form
    {
        private MySqlConnection connexion;
        public SupprimerClientProForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
            ChargerClients();
        }
        private void ChargerClients()
        {
            comboBox1.Items.Clear();
            string requete = "SELECT CONCAT(idClient, ' - ', E.siret, ' : ', nom_entreprise) AS client FROM ENTREPRISE E" +
                " JOIN CLIENTELE C ON C.siret = E.siret;";

            MySqlCommand cmd = new MySqlCommand(requete, connexion);

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["client"].ToString());
                }
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                string clientSelectionne = comboBox1.SelectedItem.ToString();
                int indexSeparateur = clientSelectionne.IndexOf('-');
                string idClient = clientSelectionne.Substring(0, indexSeparateur - 1).Trim();

                string requeteIdParticulier = "SELECT siret FROM CLIENTELE WHERE idClient = @idClient;";
                MySqlCommand cmdIdParticulier = new MySqlCommand(requeteIdParticulier, connexion);
                cmdIdParticulier.Parameters.AddWithValue("@idClient", idClient);

                string siret = null;
                using (MySqlDataReader reader = cmdIdParticulier.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        siret = Convert.ToString(reader["siret"]);
                    }
                }

                DialogResult confirmation = MessageBox.Show($"Voulez-vous vraiment supprimer le client '{clientSelectionne}' ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmation == DialogResult.Yes)
                {
                    string requete1 = "DELETE FROM CLIENTELE WHERE idClient = @idClient;";
                    string requete = "DELETE FROM ENTREPRISE WHERE siret = @siret;";
                    MySqlCommand cmd1 = new MySqlCommand(requete1, connexion);
                    cmd1.Parameters.AddWithValue("@idClient", idClient);
                    cmd1.ExecuteNonQuery();
                    MySqlCommand cmd = new MySqlCommand(requete, connexion);
                    cmd.Parameters.AddWithValue("@siret", siret);

                    int lignesModifiees = cmd.ExecuteNonQuery();
                    if (lignesModifiees > 0)
                    {
                        MessageBox.Show("Client supprimé avec succès !");
                        comboBox1.Refresh();
                        ChargerClients();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression du client.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à supprimer.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}