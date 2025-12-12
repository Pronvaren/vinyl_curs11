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
    public partial class dobav_composer : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public dobav_composer(string fio, string role)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;

            textBox4.MaxLength = 255;
        }

        private void dobav_composer_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            composer composer = new composer(userFio, userRole);
            this.Hide();
            composer.ShowDialog();
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

                    string checkSql = "SELECT COUNT(*) FROM Composers WHERE Name = '" + textBox4.Text + "'";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Такая запись уже существует!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox4.Clear();
                    }
                    else
                    {
                        string sql = "INSERT INTO Composers (Name) VALUES ('" + textBox4.Text + "')";
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
