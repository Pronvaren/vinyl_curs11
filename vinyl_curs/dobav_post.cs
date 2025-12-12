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
    public partial class dobav_post : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
        public dobav_post(string fio, string role)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;

            textBox1.MaxLength = 255;

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dobav_post_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connStr);

            // ТОВАРЫ
            conn.Open();
            string sql = "SELECT id, Name FROM Products";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            comboBox1.Items.Clear();
            while (reader.Read())
            {
                comboBox1.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["id"]), reader["Name"].ToString()));
            }
            reader.Close();
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            comboBox1.SelectedIndex = -1;
            conn.Close();

            // ПОСТАВЩИКИ
            conn.Open();
            string sql2 = "SELECT id, Name FROM Suppliers";
            MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
            MySqlDataReader reader2 = cmd2.ExecuteReader();

            comboBox2.Items.Clear();
            while (reader2.Read())
            {
                comboBox2.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader2["id"]), reader2["Name"].ToString()));
            }
            reader2.Close();
            comboBox2.DisplayMember = "Value";
            comboBox2.ValueMember = "Key";
            comboBox2.SelectedIndex = -1;
            conn.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            post post = new post(userFio, userRole);
            this.Hide();
            post.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо заполнить обязательные поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите добавить запись?", "Добавление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                int productId = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
                int supplierId = ((KeyValuePair<int, string>)comboBox2.SelectedItem).Key;
                DateTime supplyDate = dateTimePicker1.Value;
                int quantity = int.Parse(textBox1.Text.Trim());

                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                // Добавление записи о поставке
                string insertSql = @"
                INSERT INTO Supplies (Date, Product, Supplier, Quantity)
                VALUES (@date, @product, @supplier, @quantity)";
                MySqlCommand cmd = new MySqlCommand(insertSql, conn);

                cmd.Parameters.AddWithValue("@date", supplyDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@product", productId);
                cmd.Parameters.AddWithValue("@supplier", supplierId);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.ExecuteNonQuery();

                // Увеличение количество товара
                string updateSql = @"
                UPDATE Products
                SET QuantityWarehouse = QuantityWarehouse + @quantity
                WHERE id = @productId";
                MySqlCommand cmd2 = new MySqlCommand(updateSql, conn);

                cmd2.Parameters.AddWithValue("@quantity", quantity);
                cmd2.Parameters.AddWithValue("@productId", productId);
                cmd2.ExecuteNonQuery();

                MessageBox.Show("Поставка добавлена!", "Добавление записи");

                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                textBox1.Clear();
                dateTimePicker1.Value = DateTime.Now;

                conn.Close();
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

