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
    public partial class dobav_supp : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public dobav_supp(string fio, string role)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;

            textBox4.MaxLength = 255;
            maskedTextBox1.MaxLength = 11;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dobav_supp_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            supp supp = new supp(userFio, userRole);
            this.Hide();
            supp.ShowDialog();
            this.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(maskedTextBox1.Text))
            {
                MessageBox.Show("Необходимо заполнить обязательные поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите добавить запись?", "Добавление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();

                    string checkNameSql = "SELECT COUNT(*) FROM Suppliers WHERE Name = '" + textBox4.Text + "'";
                    MySqlCommand checkNameCmd = new MySqlCommand(checkNameSql, conn);
                    int nameCount = Convert.ToInt32(checkNameCmd.ExecuteScalar());

                    string checkPhoneSql = "SELECT COUNT(*) FROM Suppliers WHERE PhoneNumber = '" + maskedTextBox1.Text + "'";
                    MySqlCommand checkPhoneCmd = new MySqlCommand(checkPhoneSql, conn);
                    int phoneCount = Convert.ToInt32(checkPhoneCmd.ExecuteScalar());

                    if (nameCount > 0)
                    {
                        MessageBox.Show("Такое имя уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox4.Clear();
                        maskedTextBox1.Clear();
                    }
                    else if (phoneCount > 0)
                    {
                        MessageBox.Show("Такой телефон уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox4.Clear();
                        maskedTextBox1.Clear();
                    }
                    else
                    {
                        string sql = "INSERT INTO Suppliers (Name, PhoneNumber) VALUES ('" + textBox4.Text + "', '" + maskedTextBox1.Text + "')";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();

                        textBox4.Clear();
                        maskedTextBox1.Clear();

                        MessageBox.Show("Запись добавлена!", "Добавление записи");
                    }

                    conn.Close();
                }
                else
                {
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            //{
            //    e.Handled = true;
            //}
        }
    }
}
