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
    public partial class izm_post : Form
    {
        private string userRole;
        private string userFio;
        private int postId;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public izm_post(string fio, string role, int id, string date, string product, string supplier, string quantity)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;
            postId = id;

            textBox1.Text = quantity;
            dateTimePicker1.Value = DateTime.Parse(date);
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            // ТОВАРЫ
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string prodSql = "SELECT id, Name FROM Products";
                MySqlCommand prodCmd = new MySqlCommand(prodSql, conn);
                MySqlDataReader reader = prodCmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["id"]), reader["Name"].ToString()));
                }
                reader.Close();
            }

            // ПОСТАВЩИКИ
            using (MySqlConnection conn1 = new MySqlConnection(connStr))
            {
                conn1.Open();

                string suppSql = "SELECT id, Name FROM Suppliers";
                MySqlCommand suppCmd = new MySqlCommand(suppSql, conn1);
                MySqlDataReader reader1 = suppCmd.ExecuteReader();
                while (reader1.Read())
                {
                    comboBox2.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader1["id"]), reader1["Name"].ToString()));
                }
                reader1.Close();
            }

            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            comboBox1.SelectedIndex = comboBox1.FindStringExact(product);

            comboBox2.DisplayMember = "Value";
            comboBox2.ValueMember = "Key";
            comboBox2.SelectedIndex = comboBox2.FindStringExact(supplier);

            textBox1.MaxLength = 255;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            post post = new post(userFio, userRole);
            this.Hide();
            post.ShowDialog();
            this.Close();
        }

        private void izm_post_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Необходимо заполнить обязательные поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите изменить запись?", "Изменение записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;

                string quantity = textBox1.Text.Trim();
                int productId = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
                DateTime date = dateTimePicker1.Value;

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string updateSql = $"UPDATE Supplies SET Date = '{date:yyyy-MM-dd}', Product = {productId}, Quantity = '{quantity}' WHERE id = {postId}";
                    MySqlCommand cmd = new MySqlCommand(updateSql, conn);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Запись успешно изменена!", "Изменение записи", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }
    }
}
