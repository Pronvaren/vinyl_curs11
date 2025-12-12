using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace vinyl_curs
{
    public partial class dobav_tov : Form
    {
        private string userRole;
        private string userFio;

        string selectedPhotoName = "non.jpg";

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        public dobav_tov(string fio, string role)
        {
            InitializeComponent();

            userRole = role;
            userFio = fio;

            textBox1.MaxLength = 255;
            textBox2.MaxLength = 4;
            textBox3.MaxLength = 4;
            textBox5.MaxLength = 255;
            textBox6.MaxLength = 255;
            textBox7.MaxLength = 255;
        }

        private void dobav_tov_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            
            // КОМПОЗИТОРЫ
            conn.Open();
            string sql = "SELECT id, Name FROM Composers";
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

            // ИСПОЛНИТЕЛИ
            conn.Open();
            string sql2 = "SELECT id, Name FROM Artists";
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

            // ЖАНР
            conn.Open();
            string sql3 = "SELECT id, Name FROM Genres";
            MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
            MySqlDataReader reader3 = cmd3.ExecuteReader();

            comboBox3.Items.Clear();
            while (reader3.Read())
            {
                comboBox3.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader3["id"]), reader3["Name"].ToString()));
            }
            reader3.Close();
            comboBox3.DisplayMember = "Value";
            comboBox3.ValueMember = "Key";
            comboBox3.SelectedIndex = -1;
            conn.Close();

            // ФОРМАТ НОСИТЕЛЯ
            conn.Open();
            string sql4 = "SELECT id, Name FROM MediaFormats";
            MySqlCommand cmd4 = new MySqlCommand(sql4, conn);
            MySqlDataReader reader4 = cmd4.ExecuteReader();

            comboBox4.Items.Clear();
            while (reader4.Read())
            {
                comboBox4.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader4["id"]), reader4["Name"].ToString()));
            }
            reader4.Close();
            comboBox4.DisplayMember = "Value";
            comboBox4.ValueMember = "Key";
            comboBox4.SelectedIndex = -1;
            conn.Close();

            // ЛЕЙБЛ
            conn.Open();
            string sql5 = "SELECT id, Name FROM Labels";
            MySqlCommand cmd5 = new MySqlCommand(sql5, conn);
            MySqlDataReader reader5 = cmd5.ExecuteReader();

            comboBox5.Items.Clear();
            while (reader5.Read())
            {
                comboBox5.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader5["id"]), reader5["Name"].ToString()));
            }
            reader5.Close();
            comboBox5.DisplayMember = "Value";
            comboBox5.ValueMember = "Key";
            comboBox5.SelectedIndex = -1;
            conn.Close();

            // ПРОИЗВОДИТЕЛЬ
            conn.Open();
            string sql6 = "SELECT id, Name FROM Manufacturers";
            MySqlCommand cmd6 = new MySqlCommand(sql6, conn);
            MySqlDataReader reader6 = cmd6.ExecuteReader();

            comboBox6.Items.Clear();
            while (reader6.Read())
            {
                comboBox6.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader6["id"]), reader6["Name"].ToString()));
            }
            reader6.Close();
            comboBox6.DisplayMember = "Value";
            comboBox6.ValueMember = "Key";
            comboBox6.SelectedIndex = -1;
            conn.Close();

            // ПОСТАВЩИК
            conn.Open();
            string sql7 = "SELECT id, Name FROM Suppliers";
            MySqlCommand cmd7 = new MySqlCommand(sql7, conn);
            MySqlDataReader reader7 = cmd7.ExecuteReader();

            comboBox7.Items.Clear();
            while (reader7.Read())
            {
                comboBox7.Items.Add(new KeyValuePair<int, string>(Convert.ToInt32(reader7["id"]), reader7["Name"].ToString()));
            }
            reader7.Close();
            comboBox7.DisplayMember = "Value";
            comboBox7.ValueMember = "Key";
            comboBox7.SelectedIndex = -1;
            conn.Close();

            string defaultImagePath = Application.StartupPath + "\\non.jpg";
            if (System.IO.File.Exists(defaultImagePath))
            {
                pictureBox1.Image = Image.FromFile(defaultImagePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tov tov = new tov(userFio, userRole);
            this.Hide();
            tov.ShowDialog();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) || string.IsNullOrEmpty(textBox7.Text) || comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1 || comboBox4.SelectedIndex == -1 || comboBox5.SelectedIndex == -1 || comboBox6.SelectedIndex == -1 || comboBox7.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо заполнить обязательные поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int releaseYear = int.Parse(textBox2.Text.Trim());
            int manufactureYear = int.Parse(textBox3.Text.Trim());

            if (releaseYear > manufactureYear)
            {
                MessageBox.Show("Год выпуска не может быть больше года производства!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите добавить запись?", "Добавление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


                if (result != DialogResult.Yes) return;

                int composerId = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
                int artistId = ((KeyValuePair<int, string>)comboBox2.SelectedItem).Key;
                int genreId = ((KeyValuePair<int, string>)comboBox3.SelectedItem).Key;
                int medformsId = ((KeyValuePair<int, string>)comboBox4.SelectedItem).Key;
                int labelId = ((KeyValuePair<int, string>)comboBox5.SelectedItem).Key;
                int manufId = ((KeyValuePair<int, string>)comboBox6.SelectedItem).Key;
                int suppId = ((KeyValuePair<int, string>)comboBox7.SelectedItem).Key;
                int realdate = int.Parse(textBox2.Text.Trim());
                int mandate = int.Parse(textBox3.Text.Trim());
                int cost = int.Parse(textBox5.Text.Trim());
                int quantity = int.Parse(textBox6.Text.Trim());
                string name = textBox1.Text;
                string desc = textBox7.Text;

                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                string insertSql = @"INSERT INTO Products 
        (Name, Composer, Artist, ReleaseYear, ManufactureYear, Genre, MediaFormat, Label, Manufacturer, Supplier, Cost, QuantityWarehouse, Description, Photo)
        VALUES (@Name, @Composer, @Artist, @ReleaseYear, @ManufactureYear, @Genre, @MediaFormat, @Label, @Manufacturer, @Supplier, @Cost, @Quantity, @Description, @Photo)";

                MySqlCommand cmd = new MySqlCommand(insertSql, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Composer", composerId);
                cmd.Parameters.AddWithValue("@Artist", artistId);
                cmd.Parameters.AddWithValue("@ReleaseYear", realdate);
                cmd.Parameters.AddWithValue("@ManufactureYear", mandate);
                cmd.Parameters.AddWithValue("@Genre", genreId);
                cmd.Parameters.AddWithValue("@MediaFormat", medformsId);
                cmd.Parameters.AddWithValue("@Label", labelId);
                cmd.Parameters.AddWithValue("@Manufacturer", manufId);
                cmd.Parameters.AddWithValue("@Supplier", suppId);
                cmd.Parameters.AddWithValue("@Cost", cost);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Description", desc);
                cmd.Parameters.AddWithValue("@Photo", selectedPhotoName);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Товар добавлен!", "Добавление записи");

                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                comboBox4.SelectedIndex = -1;
                comboBox5.SelectedIndex = -1;
                comboBox6.SelectedIndex = -1;
                comboBox7.SelectedIndex = -1;

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();

                selectedPhotoName = "non.jpg";

                conn.Close();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите изображение";
            ofd.Filter = "Файлы изображений|*.jpg;*.jpeg;*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    if (fi.Length > 2 * 1024 * 1024) 
                    {
                        MessageBox.Show("Размер изображения не должен превышать 2 МБ!.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

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
                catch
                {
                    MessageBox.Show("Не удалось загрузить выбранное изображение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
