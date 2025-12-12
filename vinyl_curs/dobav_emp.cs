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
using System.Security.Cryptography;

namespace vinyl_curs
{
    public partial class dobav_emp : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public dobav_emp(string role, string fio)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;

            textBox4.MaxLength = 255;
            textBox1.MaxLength = 255;
            textBox2.MaxLength = 255;
            maskedTextBox1.MaxLength = 11;
        }

        private void dobav_emp_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
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
            conn.Close();

            comboBox1.DisplayMember = "Value"; 
            comboBox1.ValueMember = "Key";    
            comboBox1.SelectedIndex = -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            emp emp = new emp(userRole, userFio);
            this.Hide();
            emp.ShowDialog();
            this.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string text = textBox4.Text;

            if (string.IsNullOrEmpty(text))
                return;

            string[] words = text.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }

            string newText = string.Join(" ", words);

            int selectionStart = textBox4.SelectionStart;
            textBox4.Text = newText;
            textBox4.SelectionStart = selectionStart;
        }

        // ЛОГИН
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

        // ПАРОЛЬ
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(maskedTextBox1.Text) || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо заполнить обязательные поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите добавить запись?", "Добавление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (result != DialogResult.Yes) return;

                    int roleId = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
                    string fio = textBox4.Text.Trim();
                    string login = textBox1.Text.Trim();
                    string phone = maskedTextBox1.Text.Trim();

                    string password;
                    using (var sha = System.Security.Cryptography.SHA256.Create())
                    {
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(textBox2.Text.Trim());
                        byte[] hash = sha.ComputeHash(bytes);
                        password = BitConverter.ToString(hash).Replace("-", "").ToLower();
                    }

                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();

                    string checkSql = $"SELECT COUNT(*) FROM Employees WHERE Login = '{login}' OR PhoneNumber = '{phone}'";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Такой логин или телефон уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string insertSql = $"INSERT INTO Employees (Name, Role, Login, Password, PhoneNumber) VALUES ('{fio}', {roleId}, '{login}', '{password}', '{phone}')";
                        MySqlCommand cmd = new MySqlCommand(insertSql, conn);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Сотрудник добавлен!", "Добавление записи");

                        textBox4.Clear();
                        textBox1.Clear();
                        textBox2.Clear();
                        maskedTextBox1.Clear();
                        comboBox1.SelectedIndex = -1;
                    }

                    conn.Close();
                }
                else
                {
                }
            }
        }

        // ФИО
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

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
