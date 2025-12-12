using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace vinyl_curs
{
    public partial class genre : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public genre(string fio, string role)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;
        }

        private void genre_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                    SELECT id AS 'ID', Name AS 'Название' FROM Genres
                ";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;

                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Crimson;

                dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);

                dataGridView1.Columns["ID"].Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sprav sprav = new sprav(userFio, userRole);
            this.Hide();
            sprav.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dobav_genre dobav_genre = new dobav_genre(userFio, userRole);
            this.Hide();
            dobav_genre.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для изменения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
            string name = selectedRow.Cells["Название"].Value.ToString();

            izm_genre izm_genre = new izm_genre(userFio, userRole, id, name);
            this.Hide();
            izm_genre.ShowDialog();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
