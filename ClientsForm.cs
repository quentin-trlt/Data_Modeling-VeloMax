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
    public partial class ClientsForm : Form
    {
        private MySqlConnection connexion;
        public ClientsForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ClientsPartForm clientsPartForm = new ClientsPartForm(connexion);
            clientsPartForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClientsProForm clientsPartForm = new ClientsProForm(connexion);
            clientsPartForm.ShowDialog();
        }
        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
