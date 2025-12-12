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
    public partial class izm_genre : Form
    {
        private string userRole;
        private string userFio;
        private int genreId;
        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public izm_genre(string fio, string role, int id, string name)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;
            genreId = id;

            textBox4.Text = name;

            textBox4.MaxLength = 255;
        }

        private void izm_genre_Load(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            genre genre = new genre(userFio, userRole);
            this.Hide();
            genre.ShowDialog();
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
                        string sql = "UPDATE Genres SET Name = '" + textBox4.Text + "' WHERE id = " + genreId;
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запись успешно изменена!", "Изменение");
                }
                else
                {
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
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
