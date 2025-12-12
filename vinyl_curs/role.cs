using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace vinyl_curs
{
    public partial class role : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public role(string fio, string role)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;
        }

        private void role_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                    SELECT id AS 'ID', Name AS 'Название' FROM Roles
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
            dobav_role dobav_role = new dobav_role(userFio, userRole);
            this.Hide();
            dobav_role.ShowDialog();
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

            // Запрет изменения стандартных ролей
            if (name == "Администратор" || name == "Товаровед" || name == "Продавец")
            {
                MessageBox.Show("Эту стандарьная роль! Её нельзя изменять!", "Запрещено", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            izm_role izm_role = new izm_role(userFio, userRole, id, name);
            this.Hide();
            izm_role.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int roleId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
            string roleName = dataGridView1.SelectedRows[0].Cells["Название"].Value.ToString();

            if (roleName == "Администратор" && roleName == "Товаровед" && roleName == "Продавец")
            {
                MessageBox.Show("Эту роль нельзя удалить!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить роль: {roleName}?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string deleteSql = "DELETE FROM Roles WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(deleteSql, conn);
                    cmd.Parameters.AddWithValue("@id", roleId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Роль успешно удалена!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                role_Load(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Эту роль нельзя удалить!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
