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
    public partial class dobav_manuf : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public dobav_manuf(string fio, string role)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;

            textBox4.MaxLength = 255;
            //textBox1.MaxLength = 255;
        }

        private void dobav_manuf_Load(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            manuf manuf = new manuf(userFio, userRole);
            this.Hide();
            manuf.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text)/* || string.IsNullOrEmpty(textBox1.Text)*/)
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

                    string checkSql = "SELECT COUNT(*) FROM Manufacturers WHERE Name = '" + textBox4.Text + "'";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Такая запись уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox4.Clear();
                    }
                    else
                    {
                        string sql = "INSERT INTO Manufacturers (Name) VALUES ('" + textBox4.Text + "')";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();

                        textBox4.Clear();
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
            char c = e.KeyChar;

            if (!char.IsControl(c) && !char.IsLetter(c) && c != ' ' && c != '-' && c != '"')
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //char c = e.KeyChar;

            //if ((c >= 'A' && c <= 'z'))
            //{
            //    e.Handled = true;
            //}

            //if (!char.IsControl(c) && !char.IsLetter(c) && c != ' ' && c != '-')
            //{
            //    e.Handled = true;
            //}
        }
    }
}
