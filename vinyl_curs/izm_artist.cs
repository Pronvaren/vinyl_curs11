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
    public partial class izm_artist : Form
    {
        private string userRole;
        private string userFio;
        private int artistId;
        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public izm_artist(string fio, string role, int id, string name)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;
            artistId = id;

            textBox4.Text = name;

            textBox4.MaxLength = 255;
        }

        private void izm_artist_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            artist artist = new artist(userFio, userRole);
            this.Hide();
            artist.ShowDialog();
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
                        string sql = "UPDATE Artists SET Name = '" + textBox4.Text + "' WHERE id = " + artistId;
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запись успешно изменена!", "Изменение");
                    textBox4.Clear();
                }
            }
        }
    }
}
