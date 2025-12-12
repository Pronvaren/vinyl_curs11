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
    public partial class izm_tov : Form
    {
        private string userRole;
        private string userFio;
        private int tovId;
        string selectedPhotoName = "non.jpg";
        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public izm_tov(string fio, string role, int id, string name, string comp, string artist, string relyear, string manyear, string genre, string medform, string label, string manuf, string supp, string cost, string quantity, string desc, string photo)
        {
            InitializeComponent();

            userRole = role;
            userFio = fio;
            tovId = id;

            selectedPhotoName = photo;
            if (selectedPhotoName == "" || selectedPhotoName == null)
            {
                selectedPhotoName = "non.jpg";
            }

            string photoPath = Application.StartupPath + "\\" + selectedPhotoName;
            if (File.Exists(photoPath))
            {
                pictureBox1.Image = Image.FromFile(photoPath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\non.jpg");
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }

            textBox1.Text = name;
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            textBox2.Text = relyear;
            textBox3.Text = manyear;
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            comboBox5.Items.Clear(); ;
            comboBox6.Items.Clear();
            comboBox7.Items.Clear();
            textBox5.Text = cost;
            textBox6.Text = quantity;
            textBox7.Text = desc;

            // КОМПОЗИТОР
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string compSql = "SELECT id, Name FROM Composers";
                MySqlCommand compCmd = new MySqlCommand(compSql, conn);
                MySqlDataReader reader = compCmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader["id"]), reader["Name"].ToString()));
                }
                reader.Close();
            }

            // ИСПОЛНИТЕЛИ
            using (MySqlConnection conn1 = new MySqlConnection(connStr))
            {
                conn1.Open();

                string artSql = "SELECT id, Name FROM Artists";
                MySqlCommand artCmd = new MySqlCommand(artSql, conn1);
                MySqlDataReader reader1 = artCmd.ExecuteReader();
                while (reader1.Read())
                {
                    comboBox2.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader1["id"]), reader1["Name"].ToString()));
                }
                reader1.Close();
            }
            // ЖАНРЫ
            using (MySqlConnection conn2 = new MySqlConnection(connStr))
            {
                conn2.Open();
                string genreSql = "SELECT id, Name FROM Genres";
                MySqlCommand genreCmd = new MySqlCommand(genreSql, conn2);
                MySqlDataReader reader2 = genreCmd.ExecuteReader();
                while (reader2.Read())
                {
                    comboBox3.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader2["id"]), reader2["Name"].ToString()));
                }
                reader2.Close();
            }

            // ФОРМАТЫ НОСИТЕЛЯ
            using (MySqlConnection conn3 = new MySqlConnection(connStr))
            {
                conn3.Open();
                string medSql = "SELECT id, Name FROM MediaFormats";
                MySqlCommand medCmd = new MySqlCommand(medSql, conn3);
                MySqlDataReader reader3 = medCmd.ExecuteReader();
                while (reader3.Read())
                {
                    comboBox4.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader3["id"]), reader3["Name"].ToString()));
                }
                reader3.Close();
            }

            // ЛЕЙБЛЫ
            using (MySqlConnection conn4 = new MySqlConnection(connStr))
            {
                conn4.Open();
                string labelSql = "SELECT id, Name FROM Labels";
                MySqlCommand labelCmd = new MySqlCommand(labelSql, conn4);
                MySqlDataReader reader4 = labelCmd.ExecuteReader();
                while (reader4.Read())
                {
                    comboBox5.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader4["id"]), reader4["Name"].ToString()));
                }
                reader4.Close();
            }

            // ПРОИЗВОДИТЕЛИ
            using (MySqlConnection conn5 = new MySqlConnection(connStr))
            {
                conn5.Open();
                string manufSql = "SELECT id, Name FROM Manufacturers";
                MySqlCommand manufCmd = new MySqlCommand(manufSql, conn5);
                MySqlDataReader reader5 = manufCmd.ExecuteReader();
                while (reader5.Read())
                {
                    comboBox6.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader5["id"]), reader5["Name"].ToString()));
                }
                reader5.Close();
            }

            // ПОСТАВЩИКИ
            using (MySqlConnection conn6 = new MySqlConnection(connStr))
            {
                conn6.Open();
                string suppSql = "SELECT id, Name FROM Suppliers";
                MySqlCommand suppCmd = new MySqlCommand(suppSql, conn6);
                MySqlDataReader reader6 = suppCmd.ExecuteReader();
                while (reader6.Read())
                {
                    comboBox7.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader6["id"]), reader6["Name"].ToString()));
                }
                reader6.Close();
            }

            comboBox1.DisplayMember = "Value"; comboBox1.ValueMember = "Key"; comboBox1.SelectedIndex = comboBox1.FindStringExact(comp);
            comboBox2.DisplayMember = "Value"; comboBox2.ValueMember = "Key"; comboBox2.SelectedIndex = comboBox2.FindStringExact(artist);
            comboBox3.DisplayMember = "Value"; comboBox3.ValueMember = "Key"; comboBox3.SelectedIndex = comboBox3.FindStringExact(genre);
            comboBox4.DisplayMember = "Value"; comboBox4.ValueMember = "Key"; comboBox4.SelectedIndex = comboBox4.FindStringExact(medform);
            comboBox5.DisplayMember = "Value"; comboBox5.ValueMember = "Key"; comboBox5.SelectedIndex = comboBox5.FindStringExact(label);
            comboBox6.DisplayMember = "Value"; comboBox6.ValueMember = "Key"; comboBox6.SelectedIndex = comboBox6.FindStringExact(manuf);
            comboBox7.DisplayMember = "Value"; comboBox7.ValueMember = "Key"; comboBox7.SelectedIndex = comboBox7.FindStringExact(supp);

            textBox1.MaxLength = 255;
            textBox2.MaxLength = 4;
            textBox3.MaxLength = 4;
            textBox5.MaxLength = 255;
            textBox6.MaxLength = 255;
            textBox7.MaxLength = 255;
        }

        private void izm_tov_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            tov tov = new tov(userFio, userRole);
            this.Hide();
            tov.ShowDialog();
            this.Close();
        }

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

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show("Вы уверены, что хотите изменить запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            string name = textBox1.Text.Trim();
            int composerId = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
            int artistId = ((KeyValuePair<int, string>)comboBox2.SelectedItem).Key;
            int genreId = ((KeyValuePair<int, string>)comboBox3.SelectedItem).Key;
            int mediaId = ((KeyValuePair<int, string>)comboBox4.SelectedItem).Key;
            int labelId = ((KeyValuePair<int, string>)comboBox5.SelectedItem).Key;
            int manufId = ((KeyValuePair<int, string>)comboBox6.SelectedItem).Key;
            int suppId = ((KeyValuePair<int, string>)comboBox7.SelectedItem).Key;
            int relYear = Convert.ToInt32(textBox2.Text);
            int manYear = Convert.ToInt32(textBox3.Text);
            decimal cost = Convert.ToDecimal(textBox5.Text);
            int quantity = Convert.ToInt32(textBox6.Text);
            string desc = textBox7.Text.Trim();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "UPDATE Products SET " +
                             "Name = @name, Composer = @composerId, Artist = @artistId, " +
                             "ReleaseYear = @relYear, ManufactureYear = @manYear, Genre = @genreId, " +
                             "MediaFormat = @mediaId, Label = @labelId, Manufacturer = @manufId, " +
                             "Supplier = @suppId, Cost = @cost, QuantityWarehouse = @quantity, Description = @desc, Photo = @photo " +
                             "WHERE ID = @id";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@composerId", composerId);
                cmd.Parameters.AddWithValue("@artistId", artistId);
                cmd.Parameters.AddWithValue("@relYear", relYear);
                cmd.Parameters.AddWithValue("@manYear", manYear);
                cmd.Parameters.AddWithValue("@genreId", genreId);
                cmd.Parameters.AddWithValue("@mediaId", mediaId);
                cmd.Parameters.AddWithValue("@labelId", labelId);
                cmd.Parameters.AddWithValue("@manufId", manufId);
                cmd.Parameters.AddWithValue("@suppId", suppId);
                cmd.Parameters.AddWithValue("@cost", cost);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@id", tovId);
                cmd.Parameters.AddWithValue("@photo", selectedPhotoName);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Запись успешно изменена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите изображение";
            ofd.Filter = "Файлы изображений|*.jpg;*.jpeg;*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);
                if (fi.Length > 2 * 1024 * 1024)
                {
                    MessageBox.Show("Размер изображения не должен превышать 2 МБ!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Создание копию изображения в той же папке
                string folder = fi.DirectoryName;
                string nameWithoutExt = Path.GetFileNameWithoutExtension(fi.Name);
                string ext = fi.Extension;
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string newFileName = $"{nameWithoutExt}_{timestamp}{ext}";
                string newFilePath = Path.Combine(folder, newFileName);

                File.Copy(ofd.FileName, newFilePath);

                selectedPhotoName = newFileName;
            }
        }
    }
}
