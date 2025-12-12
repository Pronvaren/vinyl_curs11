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
    public partial class izm_manuf : Form
    {
        private string userRole;
        private string userFio;
        private int manufId;
        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public izm_manuf(string fio, string role, int id, string name)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;
            manufId = id;

            textBox4.Text = name;

            textBox4.MaxLength = 255;
        }

        private void izm_manuf_Load(object sender, EventArgs e)
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
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Необходимо заполнить обязательные поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите изменить запись?", "Изменение записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        string sql = "UPDATE Manufacturers SET Name = '" + textBox4.Text + "' WHERE id = " + manufId;
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запись успешно изменена!", "Изменение");
                    textBox4.Clear();
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
            char c = e.KeyChar;

            if ((c >= 'A' && c <= 'z'))
            {
                e.Handled = true;
            }

            if (!char.IsControl(c) && !char.IsLetter(c) && c != ' ' && c != '-')
            {
                e.Handled = true;
            }
        }
    }
}
