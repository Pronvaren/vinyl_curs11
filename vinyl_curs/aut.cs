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
    public partial class aut : Form
    {
        int trying = 0;
        public aut()
        {
            InitializeComponent();

            textBox1.MaxLength = 255;
            textBox2.MaxLength = 255;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;

            if (trying > 0 && (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)))
            {
                MessageBox.Show("Необходимо заполнить поля логина и пароля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"SELECT Employees.Name AS FIO, Roles.Name AS RoleName
                    FROM Employees 
                    JOIN Roles ON Employees.Role = Roles.id
                    WHERE Employees.Login=@login AND Employees.Password=@pass";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@pass", GetHashPass(password));

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string fio = reader["FIO"].ToString();
                        string role = reader["RoleName"].ToString();

                        MessageBox.Show("Вход выполнен!");
                        this.Hide();
                        glav main = new glav(fio, role);
                        main.Show();
                    }
                    else
                    {
                        // ПУСТЫЕ ПОЛЯ

                        if ((textBox1.Text == "" && textBox2.Text == "") && (textBox1.Text == "" || textBox2.Text == ""))
                        {
                            MessageBox.Show("Есть пустые поля!", "Ошибка!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к БД!\nПроверьте строку подключения!");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите выйти из приложения?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
            }
        }

        // ХЭШ
        string GetHashPass(string password)
        {
            //получаем байтовое представление строки
            //byte[] bytesPass = Encoding.Unicode.GetBytes(password);
            byte[] bytesPass = Encoding.UTF8.GetBytes(password);

            //экземпляр класса для работы с алгоритмом SHA256
            SHA256Managed hashstring = new SHA256Managed();

            //получаем хеш из строки в виде массива байтов
            byte[] hash = hashstring.ComputeHash(bytesPass);

            //очищаем строку
            string hashPasswd = string.Empty;

            //собираем полученный хеш воедино
            foreach (byte x in hash)
            {
                hashPasswd += String.Format("{0:x2}", x);
            }

            //освобождаем все ресурсы связанные с экземпляром объекта SHA256Managed
            hashstring.Dispose();

            return hashPasswd;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

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
    }

}
