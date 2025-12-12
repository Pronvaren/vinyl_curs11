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
using System.IO;

namespace vinyl_curs
{
    public partial class tov : Form
    {
        private string userRole;
        private string userFio;
        

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
        public tov(string fio, string role)
        {
            InitializeComponent();

            textBox1.MaxLength = 255;
            textBox2.MaxLength = 50;
            textBox3.MaxLength = 50;

            userRole = role;
            userFio = fio;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tov_Load(object sender, EventArgs e)
        {
            
            if (userRole == "Продавец")
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
            else if (userRole == "Товаровед")
            {
                button4.Visible = false;
            }
            else if (userRole == "Администратор")
            {
                button4.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }

            string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        p.id AS 'ID',
                        p.Name AS 'Название',
                        c.Name AS 'Композитор',
                        a.Name AS 'Исполнитель',
                        p.ReleaseYear AS 'Год релиза',
                        p.ManufactureYear AS 'Год производства',
                        g.Name AS 'Жанр',
                        m.Name AS 'Формат носителя',
                        l.Name AS 'Лейбл',
                        mf.Name AS 'Производитель',
                        p.Cost AS 'Цена',
                        p.QuantityWarehouse AS 'Количество',
                        p.Description AS 'Описание',
                        p.Photo
                    FROM Products p
                    LEFT JOIN Composers c ON p.Composer = c.id
                    LEFT JOIN Artists a ON p.Artist = a.id
                    LEFT JOIN Genres g ON p.Genre = g.id
                    LEFT JOIN MediaFormats m ON p.MediaFormat = m.id
                    LEFT JOIN Labels l ON p.Label = l.id
                    LEFT JOIN Manufacturers mf ON p.Manufacturer = mf.id;
                ";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;

                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Crimson;

                dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);

                dataGridView1.Columns["ID"].Visible = false;

                LoadData("");
                LoadGenres();
                LoadMedForms();
                LoadLabels();
            }
        }


        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dobav_tov dobav_tov = new dobav_tov(userFio, userRole);
            this.Hide();
            dobav_tov.ShowDialog();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            glav glav = new glav(userFio, userRole);
            this.Hide();
            glav.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для изменения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["ID"].Value);
            string name = row.Cells["Название"].Value.ToString();
            string comp = row.Cells["Композитор"].Value.ToString();
            string artist = row.Cells["Исполнитель"].Value.ToString();
            string relyear = row.Cells["Год релиза"].Value.ToString();
            string manyear = row.Cells["Год производства"].Value.ToString();
            string genre = row.Cells["Жанр"].Value.ToString();
            string medform = row.Cells["Формат носителя"].Value.ToString();
            string label = row.Cells["Лейбл"].Value.ToString();
            string manuf = row.Cells["Производитель"].Value.ToString();
            string supp = row.Cells["Поставщик"].Value.ToString();
            string cost = row.Cells["Цена"].Value.ToString();
            string quantity = row.Cells["Количество"].Value.ToString();
            string desc = row.Cells["Описание"].Value.ToString();

            string photo;
            if (row.Cells["Photo"].Value == null)
            {
                photo = "non.jpg";
            }
            else
            {
                photo = row.Cells["Photo"].Value.ToString();
            }

            izm_tov izm_tov = new izm_tov(userFio, userRole, id, name, comp, artist, relyear, manyear, genre, medform, label, manuf, supp, cost, quantity, desc, photo);
            this.Hide();
            izm_tov.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            oform_zak oform_zak = new oform_zak(userFio, userRole);
            this.Hide();
            oform_zak.ShowDialog();
            this.Close();
        }

        // ПРИМЕНЕИЕ ПОИСКА ФИЛЬТРОВ И СОРТИРОВКИ
        private void LoadData(string filter, string genreId = "", string medformId = "", string labelId = "")
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = @"
            SELECT 
        p.id AS 'ID',
        p.Name AS 'Название',
        c.Name AS 'Композитор',
        a.Name AS 'Исполнитель',
        p.ReleaseYear AS 'Год релиза',
        p.ManufactureYear AS 'Год производства',
        g.Name AS 'Жанр',
        m.Name AS 'Формат носителя',
        l.Name AS 'Лейбл',
        mf.Name AS 'Производитель',
        ps.Name AS 'Поставщик',
        p.Cost AS 'Цена',
        p.QuantityWarehouse AS 'Количество',
        p.Description AS 'Описание',
        p.Photo
    FROM Products p
    LEFT JOIN Composers c ON p.Composer = c.id
    LEFT JOIN Artists a ON p.Artist = a.id
    LEFT JOIN Genres g ON p.Genre = g.id
    LEFT JOIN MediaFormats m ON p.MediaFormat = m.id
    LEFT JOIN Labels l ON p.Label = l.id
    LEFT JOIN Manufacturers mf ON p.Manufacturer = mf.id
    LEFT JOIN Suppliers ps ON p.Supplier = ps.id
    WHERE (p.Name LIKE @filter OR c.Name LIKE @filter OR a.Name LIKE @filter)
        ";
                if (!string.IsNullOrEmpty(genreId))
                {
                    query += " AND p.Genre = @genreId";
                }

                if (!string.IsNullOrEmpty(medformId))
                {
                    query += " AND p.MediaFormat = @medformId";
                }

                if (!string.IsNullOrEmpty(labelId))
                {
                    query += " AND p.Label = @labelId";
                }

                if (!string.IsNullOrEmpty(textBox2.Text))
                    query += $" AND p.Cost >= {textBox2.Text}";
                if (!string.IsNullOrEmpty(textBox3.Text))
                    query += $" AND p.Cost <= {textBox3.Text}";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@filter", "%" + filter + "%");

                if (!string.IsNullOrEmpty(genreId))
                    cmd.Parameters.AddWithValue("@genreId", genreId);

                if (!string.IsNullOrEmpty(medformId))
                    cmd.Parameters.AddWithValue("@medformId", medformId);

                if (!string.IsNullOrEmpty(labelId))
                    cmd.Parameters.AddWithValue("@labelId", labelId);


                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Contains("Фото"))
                    dataGridView1.Columns.Remove("Фото");

                DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
                imgCol.HeaderText = "Фото";
                imgCol.Name = "Фото";
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dataGridView1.Columns.Insert(0, imgCol);

                if (dataGridView1.Columns.Contains("Photo"))
                    dataGridView1.Columns["Photo"].Visible = false;

                string basePath = Application.StartupPath;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["Фото"] == null) continue;

                    string fileName = row.Cells["Photo"].Value?.ToString();

                    string path;

                    if (string.IsNullOrEmpty(fileName))
                        path = Path.Combine(basePath, "non.jpg");
                    else
                        path = Path.Combine(basePath, fileName);

                    if (File.Exists(path))
                        row.Cells["Фото"].Value = Image.FromFile(path);
                    else
                        row.Cells["Фото"].Value = Image.FromFile(Path.Combine(basePath, "non.jpg"));
                }

                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string genreId = "";
            string medformId = "";
            string labelId = "";

            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Все жанры")
                genreId = comboBox1.SelectedItem.ToString().Split('-')[0].Trim();

            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() != "Все форматы")
                medformId = comboBox2.SelectedItem.ToString().Split('-')[0].Trim();

            if (comboBox3.SelectedItem != null && comboBox3.SelectedItem.ToString() != "Все лейблы")
                labelId = comboBox3.SelectedItem.ToString().Split('-')[0].Trim();

            LoadData(textBox1.Text.Trim(), genreId, medformId, labelId);
        }

        // ЖАНРЫ
        private void LoadGenres()
        {
            comboBox1.Items.Add("Все жанры");
            comboBox1.SelectedIndex = 0;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, Name FROM Genres", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["id"].ToString() + " - " + reader["Name"].ToString());
                }
            }
        }

        // ФОРМАТЫ НОСИТЕЛЕЙ
        private void LoadMedForms()
        {
            comboBox2.Items.Add("Все форматы");
            comboBox2.SelectedIndex = 0;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, Name FROM MediaFormats", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["id"].ToString() + " - " + reader["Name"].ToString());
                }
            }
        }

        // ЛЕЙБЛЫ
        private void LoadLabels()
        {
            comboBox3.Items.Add("Все лейблы");
            comboBox3.SelectedIndex = 0;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, Name FROM Labels", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox3.Items.Add(reader["id"].ToString() + " - " + reader["Name"].ToString());
                }
            }
        }

        // ЖАНРЫ
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string genreId = "";
            string medformId = "";
            string labelId = "";
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Все жанры")
                genreId = comboBox1.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() != "Все форматы")
                medformId = comboBox2.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox3.SelectedItem != null && comboBox3.SelectedItem.ToString() != "Все лейблы")
                labelId = comboBox3.SelectedItem.ToString().Split('-')[0].Trim();

            LoadData(textBox1.Text, genreId, medformId, labelId);

        }

        // ФОРМАТЫ НОСИТЕЛЕЙ    
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string genreId = "";
            string medformId = "";
            string labelId = "";
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Все жанры")
                genreId = comboBox1.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() != "Все форматы")
                medformId = comboBox2.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox3.SelectedItem != null && comboBox3.SelectedItem.ToString() != "Все лейблы")
                labelId = comboBox3.SelectedItem.ToString().Split('-')[0].Trim();

            LoadData(textBox1.Text, genreId, medformId, labelId);
        }

        // ЛЕЙБЛЫ
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string genreId = "";
            string medformId = "";
            string labelId = "";
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Все жанры")
                genreId = comboBox1.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() != "Все форматы")
                medformId = comboBox2.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox3.SelectedItem != null && comboBox3.SelectedItem.ToString() != "Все лейблы")
                labelId = comboBox3.SelectedItem.ToString().Split('-')[0].Trim();

            LoadData(textBox1.Text, genreId, medformId, labelId);
        }

        // ОТ
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string genreId = "";
            string medformId = "";
            string labelId = "";
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Все жанры")
                genreId = comboBox1.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() != "Все форматы")
                medformId = comboBox2.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox3.SelectedItem != null && comboBox3.SelectedItem.ToString() != "Все лейблы")
                labelId = comboBox3.SelectedItem.ToString().Split('-')[0].Trim();

            LoadData(textBox1.Text, genreId, medformId, labelId);
        }

        // ДО
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string genreId = "";
            string medformId = "";
            string labelId = "";
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Все жанры")
                genreId = comboBox1.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() != "Все форматы")
                medformId = comboBox2.SelectedItem.ToString().Split('-')[0].Trim();
            if (comboBox3.SelectedItem != null && comboBox3.SelectedItem.ToString() != "Все лейблы")
                labelId = comboBox3.SelectedItem.ToString().Split('-')[0].Trim();

            LoadData(textBox1.Text, genreId, medformId, labelId);
        }

        // ОЧИСТКА СОРТИРОВКИ
        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            LoadData("");
        }

        // ВВОД ТОЛЬКО ЦИФР
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int productId = Convert.ToInt32(row.Cells["ID"].Value);
            string productName = row.Cells["Название"].Value.ToString();

            DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить товар: {productName}?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT(*) FROM Supplies WHERE Product = @id";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@id", productId);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Нельзя удалить товар, так как он используется в поставках и заказах!",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string deleteSql = "DELETE FROM Products WHERE id = @productId";
                    MySqlCommand cmd = new MySqlCommand(deleteSql, conn);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Товар успешно удален!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadData(textBox1.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
