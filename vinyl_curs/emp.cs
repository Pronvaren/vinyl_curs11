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
    public partial class emp : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;


        public emp(string fio, string role)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;
        }

        private void emp_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                    SELECT e.id AS 'ID',
                    e.Name AS 'ФИО',
                    e.Role AS 'RoleID',
                    r.Name AS 'Роль',
                    e.Login AS 'Логин',
                    e.Password AS 'Пароль',
                    e.PhoneNumber AS 'Номер телефона'
                    FROM Employees e
                    JOIN Roles r ON e.Role = r.id;";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns["RoleID"].Visible = false;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;

                dataGridView1.Columns["ID"].Visible = false;

                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Crimson;

                dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);

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
            dobav_emp dobav_emp = new dobav_emp(userFio, userRole);
            this.Hide();
            dobav_emp.ShowDialog();
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
            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
            string fio = dataGridView1.SelectedRows[0].Cells["ФИО"].Value.ToString();
            string login = dataGridView1.SelectedRows[0].Cells["Логин"].Value.ToString();
            string pass = dataGridView1.SelectedRows[0].Cells["Пароль"].Value.ToString();
            string phone = dataGridView1.SelectedRows[0].Cells["Номер телефона"].Value.ToString();
            int roleId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RoleID"].Value);

            izm_emp izm_emp = new izm_emp(userFio, userRole, id, fio, login, pass, phone, roleId);
            this.Hide();
            izm_emp.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите сотрудника для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int empId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);
            string selectedFio = dataGridView1.SelectedRows[0].Cells["ФИО"].Value.ToString();

            if (selectedFio == userFio)
            {
                MessageBox.Show("Вы не можете удалить свой собственный аккаунт!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Вы уверены, что хотите удалить сотрудника: {selectedFio}?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT(*) FROM Orders WHERE Salesman = @id";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@id", empId);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Нельзя удалить сотрудника, так как он используется в заказах!",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string deleteSql = "DELETE FROM Employees WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(deleteSql, conn);
                    cmd.Parameters.AddWithValue("@id", empId);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Сотрудник успешно удалён!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);

                emp_Load(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
