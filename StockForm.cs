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
    public partial class StockForm : Form
    {
        private MySqlConnection connexion;
        public StockForm(MySqlConnection connexion)
        {
            InitializeComponent();
            this.connexion = connexion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StockVeloForm stockVeloForm = new StockVeloForm(connexion);
            stockVeloForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StockPieceForm stockPieceForm = new StockPieceForm(connexion);
            stockPieceForm.ShowDialog();
        }
        private void btnQuitter_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
