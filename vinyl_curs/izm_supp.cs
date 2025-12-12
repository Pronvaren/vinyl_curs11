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
    public partial class izm_supp : Form
    {
        private string userRole;
        private string userFio;
        private int suppId;
        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public izm_supp(string fio, string role, int id, string name, string phonenum)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;
            suppId = id;

            textBox4.Text = name;
            maskedTextBox1.Text = phonenum;

            textBox4.MaxLength = 255;
            maskedTextBox1.MaxLength = 11;
        }

        private void izm_supp_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(maskedTextBox1.Text))
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
                        string sql = "UPDATE Suppliers SET Name = '" + textBox4.Text + "', PhoneNumber = '" + maskedTextBox1.Text + "' WHERE id = " + suppId;
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запись успешно изменена!", "Изменение");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            supp supp = new supp(userFio, userRole);
            this.Hide();
            supp.ShowDialog();
            this.Close();
        }

        private void izm_supp_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {

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
