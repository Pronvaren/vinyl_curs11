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
    public partial class izm_emp : Form
    {
        private string userRole;
        private string userFio;
        private int empId;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public izm_emp(string fio, string role, int id, string empFio, string login, string pass, string phone, int roleId)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;
            empId = id;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT id, Name FROM Roles";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                comboBox1.Items.Clear();
                while (reader.Read())
                {
                    comboBox1.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["id"]), reader["Name"].ToString()));
                }

                reader.Close();
            }

            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";

            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                var item = (KeyValuePair<int, string>)comboBox1.Items[i];
                if (item.Key == roleId)
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

            textBox4.Text = empFio;
            textBox1.Text = login;
            maskedTextBox1.Text = phone;
            comboBox1.SelectedValue = roleId;

            textBox4.MaxLength = 255;
            textBox1.MaxLength = 255;
            textBox2.MaxLength = 255;
            maskedTextBox1.MaxLength = 11;
        }

        private void izm_emp_Load(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            emp emp = new emp(userFio, userRole);
            this.Hide();
            emp.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(maskedTextBox1.Text))
            {
                MessageBox.Show("Необходимо заполнить обязательные поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Вы уверены, что хотите изменить запись?", "Изменение записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            int roleId = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
            string fio = textBox4.Text.Trim();
            string login = textBox1.Text.Trim();
            string phone = maskedTextBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string checkSql = $"SELECT COUNT(*) FROM Employees WHERE (Login = '{login}' OR PhoneNumber = '{phone}') AND ID <> {empId}";
                MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Такой логин или телефон уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string updateSql;

                if (!string.IsNullOrEmpty(password))
                {
                    using (var sha = System.Security.Cryptography.SHA256.Create())
                    {
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                        byte[] hash = sha.ComputeHash(bytes);
                        password = BitConverter.ToString(hash).Replace("-", "").ToLower();
                    }

                    updateSql = $"UPDATE Employees SET Name = '{fio}', Role = {roleId}, Login = '{login}', Password = '{password}', PhoneNumber = '{phone}' WHERE ID = {empId}";
                }
                else
                {
                    updateSql = $"UPDATE Employees SET Name = '{fio}', Role = {roleId}, Login = '{login}', PhoneNumber = '{phone}' WHERE ID = {empId}";
                }

                MySqlCommand cmd = new MySqlCommand(updateSql, conn);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Запись успешно изменена!", "Изменение записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if ((c >= 'A' && c <= 'z'))
            {
                e.Handled = true;
            }
            if (c >= '0' && c <= '9')
            {
                e.Handled = true;
            }

            if (!char.IsControl(c) && !char.IsLetter(c) && c != ' ' && c != '-')
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if (!char.IsControl(c) && !char.IsLetterOrDigit(c))
            {
                e.Handled = true;
            }

            if ((c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё')
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;

            if ((c >= 'А' && c <= 'я') || c == 'ё' || c == 'Ё')
            {
                e.Handled = true;
            }

            if (c == ' ')
            {
                e.Handled = true;
            }
        }
    }
}
