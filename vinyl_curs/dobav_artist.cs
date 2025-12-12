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
    public partial class dobav_artist : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;


        public dobav_artist(string fio, string role)
        {
            InitializeComponent();

            textBox4.MaxLength = 255;

            userFio = fio;
            userRole = role;
        }

        private void dobav_artist_Load(object sender, EventArgs e)
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
                DialogResult result = MessageBox.Show("Вы уверены, что хотите добавить запись?", "Добавление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MySqlConnection conn = new MySqlConnection(connStr);
                    conn.Open();

                    string checkSql = "SELECT COUNT(*) FROM Artists WHERE Name = '" + textBox4.Text + "'";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Такая запись уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox4.Clear();
                    }
                    else
                    {
                        string sql = "INSERT INTO Artists (Name) VALUES ('" + textBox4.Text + "')";
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
    }
}
