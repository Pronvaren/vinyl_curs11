using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Globalization;

namespace vinyl_curs
{
    public partial class oform_zak : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;

        private DataTable productsTable;

        public oform_zak(string fio, string role)
        {
            InitializeComponent();

            textBox1.MaxLength = 50;
            numericUpDown1.Minimum = 1;

            userRole = role;
            userFio = fio;

            textBox1.TextChanged += textBox1_TextChanged;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

        }

        private void oform_zak_Load(object sender, EventArgs e)
        {
            string query = @"
SELECT 
    p.id AS 'Артикул',
    p.Name AS 'Наименование',
    c.Name AS 'Композитор',
    a.Name AS 'Исполнитель',
    p.Cost AS 'Цена (руб.)',
    p.Photo AS 'Фото',
    g.Name AS 'Жанр',
    p.ReleaseYear AS 'Год релиза',
    p.ManufactureYear AS 'Год производства',
    m.Name AS 'Формат',
    l.Name AS 'Лейбл'
FROM Products p
LEFT JOIN Genres g ON p.Genre = g.id
LEFT JOIN Composers c ON p.Composer = c.id
LEFT JOIN Artists a ON p.Artist = a.id
LEFT JOIN MediaFormats m ON p.MediaFormat = m.id
LEFT JOIN Labels l ON p.Label = l.id;
";

            MySqlDataAdapter adapter = new MySqlDataAdapter(query, connStr);
            productsTable = new DataTable();
            adapter.Fill(productsTable);

            dataGridView1.DataSource = productsTable;

            dataGridView1.Columns["Артикул"].HeaderCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.Columns["Наименование"].HeaderCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView1.Columns["Цена (руб.)"].HeaderCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);

            dataGridView1.Columns["Жанр"].Visible = false;
            dataGridView1.Columns["Год релиза"].Visible = false;
            dataGridView1.Columns["Год производства"].Visible = false;
            dataGridView1.Columns["Композитор"].Visible = false;
            dataGridView1.Columns["Исполнитель"].Visible = false;
            dataGridView1.Columns["Формат"].Visible = false;
            dataGridView1.Columns["Лейбл"].Visible = false;
            dataGridView1.Columns["Фото"].Visible = false;

            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Crimson;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            // КОРЗИНА
            dataGridView2.Columns.Add("Артикул", "Артикул");
            dataGridView2.Columns.Add("Наименование", "Наименование");
            dataGridView2.Columns.Add("Кол-во", "Кол-во");
            dataGridView2.Columns.Add("Цена", "Цена (руб.)");

            dataGridView2.Columns["Артикул"].HeaderCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView2.Columns["Наименование"].HeaderCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView2.Columns["Кол-во"].HeaderCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            dataGridView2.Columns["Цена"].HeaderCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);

            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.Crimson;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ДАТА ПРИЕМА
            CultureInfo ru = new CultureInfo("ru-RU");
            string year = DateTime.Now.ToString("d MMMM yyyy", ru);
            label5.Text = $"Дата приема: {year} г.";

            // ДАТА ВЫПОЛНЕНИЯ
            dateTimePicker1.MinDate = DateTime.Today;

            // НОМЕР ЗАКАЗА
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query1 = "SELECT MAX(id) FROM Orders";
                MySqlCommand cmd = new MySqlCommand(query1, conn);

                object result = cmd.ExecuteScalar();

                int nextId = 1;

                if (result != DBNull.Value)
                    nextId = Convert.ToInt32(result) + 1;

                label6.Text = $"Заказ №: {nextId}";
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            tov tov = new tov(userFio, userRole);
            this.Hide();
            tov.ShowDialog();
            this.Close();
        }

        // ВВОД ТОЛЬКО ЦИФР
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (productsTable != null)
            {
                DataView dv = productsTable.DefaultView;

                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    dv.RowFilter = "";
                }
                else
                {
                    dv.RowFilter = $"Convert([Артикул], 'System.String') LIKE '%{textBox1.Text}%'";
                }

                dataGridView1.DataSource = dv;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                string artikul = row.Cells["Артикул"].Value.ToString();
                string name = row.Cells["Наименование"].Value.ToString();
                string genre = row.Cells["Жанр"].Value.ToString();
                string comp = row.Cells["Композитор"].Value.ToString();
                string artist = row.Cells["Исполнитель"].Value.ToString();
                string relYear = row.Cells["Год релиза"].Value.ToString();
                string manYear = row.Cells["Год производства"].Value.ToString();
                string medform = row.Cells["Формат"].Value.ToString();
                string label = row.Cells["Лейбл"].Value.ToString();


                label12.Text = $"Артикул: {artikul}";
                label11.Text = $"Наименование: {name}";
                label13.Text = $"Жанр: {genre}";
                label14.Text = $"Композитор: {comp}";
                label15.Text = $"Исполнитель: {artist}";
                label16.Text = $"Год выпуска: {relYear}";
                label17.Text = $"Год производства: {manYear}";
                label18.Text = $"Формат носителя: {medform}";
                label19.Text = $"Лейбл: {label}";

                // ФОТО
                string photoFileName = row.Cells["Фото"].Value.ToString();
                string fullPath = System.IO.Path.Combine(Application.StartupPath, photoFileName);

                if (!System.IO.File.Exists(fullPath))
                {
                    fullPath = System.IO.Path.Combine(Application.StartupPath, "non.jpg");
                }

                if (System.IO.File.Exists(fullPath))
                {
                    pictureBox1.Image = Image.FromFile(fullPath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для добавления в корзину!");
                return;
            }

            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

            string artikul = selectedRow.Cells["Артикул"].Value.ToString();
            string name = selectedRow.Cells["Наименование"].Value.ToString();
            int quantity = (int)numericUpDown1.Value;
            decimal price = Convert.ToDecimal(selectedRow.Cells["Цена (руб.)"].Value);
            decimal totalPrice = price * quantity;
            int currentQtyInCart = 0;
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["Артикул"].Value.ToString() == artikul)
                {
                    currentQtyInCart = Convert.ToInt32(row.Cells["Кол-во"].Value);
                    break;
                }
            }

            int qtyWarehouse = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT QuantityWarehouse FROM Products WHERE id = " + artikul;
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();

                if (result == null)
                {
                    MessageBox.Show("Ошибка: товар не найден!");
                    return;
                }

                qtyWarehouse = Convert.ToInt32(result);
            }

            int newTotal = currentQtyInCart + quantity;

            if (newTotal > qtyWarehouse)
            {
                MessageBox.Show(
                    $"Нельзя добавить {quantity} шт.\n" +
                    $"В корзине уже: {currentQtyInCart} шт.\n" +
                    $"На складе всего: {qtyWarehouse} шт.",
                    "Недостаточно товара",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            bool found = false;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["Артикул"].Value.ToString() == artikul)
                {
                    int oldQty = Convert.ToInt32(row.Cells["Кол-во"].Value);
                    int newQty = oldQty + quantity;
                    row.Cells["Кол-во"].Value = oldQty + quantity;

                    decimal unitPrice = Convert.ToDecimal(selectedRow.Cells["Цена (руб.)"].Value);
                    row.Cells["Цена"].Value = unitPrice * newQty;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                dataGridView2.Rows.Add(artikul, name, quantity, totalPrice);
            }

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["Цена"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["Цена"].Value);
                }
            }

            // Скидка
            if (total > 20000)
            {
                decimal discount = total * 0.15m;
                decimal totalWithDiscount = total - discount;
                label7.Text = $"Сумма заказа: {totalWithDiscount} руб. (скидка 15%)";
            }
            else
            {
                label7.Text = $"Сумма заказа: {total} руб.";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления из корзины!");
                return;
            }

            DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

            DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить товар '{selectedRow.Cells["Наименование"].Value}' из корзины?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                dataGridView2.Rows.Remove(selectedRow);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        // КНОПКА ОФОРМИТЬ ЗАКАЗ
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }

            int userId = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sqlUser = "SELECT id FROM Employees WHERE Name = @name";
                MySqlCommand cmdUser = new MySqlCommand(sqlUser, conn);
                cmdUser.Parameters.AddWithValue("@name", userFio);
                object resultUser = cmdUser.ExecuteScalar();

                if (resultUser != null)
                    userId = Convert.ToInt32(resultUser);
                else
                {
                    MessageBox.Show("Сотрудник не найден в базе!");
                    return;
                }
            }

            decimal totalOrderSum = 0;

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (row.Cells["Цена"].Value != null)
                    totalOrderSum += Convert.ToDecimal(row.Cells["Цена"].Value);
            }

            decimal discount = 0;
            if (totalOrderSum > 20000)
                discount = totalOrderSum * 0.15m;

            decimal totalWithDiscount = totalOrderSum - discount;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                try
                {
                    string insertOrder = @"
INSERT INTO Orders (Date, Status, Salesman, CostSumm, DeliveryDate)
VALUES (@date, @status, @salesman, @costsumm, @delivery);";
                    MySqlCommand orderCmd = new MySqlCommand(insertOrder, conn);
                    orderCmd.Parameters.AddWithValue("@date", DateTime.Now);
                    orderCmd.Parameters.AddWithValue("@status", 1); 
                    orderCmd.Parameters.AddWithValue("@salesman", userId);
                    orderCmd.Parameters.AddWithValue("@costsumm", totalWithDiscount);
                    orderCmd.Parameters.AddWithValue("@delivery", dateTimePicker1.Value);
                    orderCmd.ExecuteNonQuery();

                    int newOrderId = Convert.ToInt32(new MySqlCommand("SELECT LAST_INSERT_ID();", conn).ExecuteScalar());

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        if (row.Cells["Артикул"].Value == null) continue;

                        string insertPos = @"
INSERT INTO `OrderComposition` (OrderID, ProductID, Quantity, Cost, CostSumm)
VALUES (@order, @product, @qty, @cost, @costsumm);";

                        MySqlCommand posCmd = new MySqlCommand(insertPos, conn);
                        int productId = Convert.ToInt32(row.Cells["Артикул"].Value);
                        int quantity = Convert.ToInt32(row.Cells["Кол-во"].Value);
                        decimal price = Convert.ToDecimal(row.Cells["Цена"].Value);

                        posCmd.Parameters.AddWithValue("@order", newOrderId);
                        posCmd.Parameters.AddWithValue("@product", productId);
                        posCmd.Parameters.AddWithValue("@qty", quantity);
                        posCmd.Parameters.AddWithValue("@cost", price / quantity);
                        posCmd.Parameters.AddWithValue("@costsumm", price); 
                        posCmd.ExecuteNonQuery();

                        string updateQty = @"UPDATE Products SET QuantityWarehouse = QuantityWarehouse - @qty WHERE id = @id";
                        MySqlCommand updCmd = new MySqlCommand(updateQty, conn);
                        updCmd.Parameters.AddWithValue("@qty", quantity);
                        updCmd.Parameters.AddWithValue("@id", productId);
                        updCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show($"Заказ №{newOrderId} успешно оформлен!\nСумма: {totalWithDiscount} руб. \n\n Ожидайте чек...", "Готово");

                    var wordApp = new Microsoft.Office.Interop.Word.Application();
                    wordApp.Visible = true;
                    var doc = wordApp.Documents.Add();
                    var para = doc.Paragraphs.Add();
                    para.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    para.Range.Font.Name = "Times New Roman";
                    para.Range.Font.Size = 14;
                    para.Range.Font.Bold = 1;
                    para.Range.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorWhite;
                    para.Range.Shading.BackgroundPatternColor = Microsoft.Office.Interop.Word.WdColor.wdColorRed;

                    // Заголовок с красным фоном и белым текстом
                    para.Range.Text = "ЧЕК ПОКУПКИ\n";
                    para.Range.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorWhite;
                    para.Range.Font.Size = 14;
                    para.Range.Font.Bold = 1;
                    para.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    para.Range.Shading.BackgroundPatternColor = Microsoft.Office.Interop.Word.WdColor.wdColorRed;
                    para.Range.InsertParagraphAfter();

                    // Номер заказа и дата
                    para.Range.Text = $"ЗАКАЗ №{newOrderId}\nДАТА: {DateTime.Now:d}\nПРОДАВЕЦ: {userFio}\n";
                    para.Range.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                    para.Range.Font.Size = 12;
                    para.Range.Font.Bold = 1;
                    para.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    para.Range.Shading.BackgroundPatternColor = Microsoft.Office.Interop.Word.WdColor.wdColorAutomatic;
                    para.Range.InsertParagraphAfter();

                    // Таблица заказа
                    var table = doc.Tables.Add(para.Range, dataGridView2.Rows.Count + 1, 4);
                    table.Borders.Enable = 1;
                    table.Range.Font.Name = "Times New Roman";
                    table.Range.Font.Size = 12;

                    // Заголовки колонок
                    table.Cell(1, 1).Range.Text = "НАИМЕНОВАНИЕ";
                    table.Cell(1, 2).Range.Text = "КОЛ-ВО";
                    table.Cell(1, 3).Range.Text = "ЦЕНА ЗА ШТ.";
                    table.Cell(1, 4).Range.Text = "СУММА";

                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        table.Cell(i + 2, 1).Range.Text = dataGridView2.Rows[i].Cells["Наименование"].Value.ToString();
                        table.Cell(i + 2, 2).Range.Text = dataGridView2.Rows[i].Cells["Кол-во"].Value.ToString();
                        decimal pricePerUnit = Convert.ToDecimal(dataGridView2.Rows[i].Cells["Цена"].Value) / Convert.ToInt32(dataGridView2.Rows[i].Cells["Кол-во"].Value);
                        table.Cell(i + 2, 3).Range.Text = pricePerUnit.ToString("0.00");
                        table.Cell(i + 2, 4).Range.Text = Convert.ToDecimal(dataGridView2.Rows[i].Cells["Цена"].Value).ToString("0.00");
                    }

                    para.Range.InsertParagraphAfter();

                    para.Range.Shading.BackgroundPatternColor = Microsoft.Office.Interop.Word.WdColor.wdColorRed;
                    para.Range.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorWhite;

                    // Итоговая сумма
                    para.Range.Text = $"\nИТОГ: {totalWithDiscount:0.00} руб.\n";
                    para.Range.Font.Size = 12;
                    para.Range.Font.Bold = 1;
                    para.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

                    dataGridView2.Rows.Clear();
                    label7.Text = "Сумма заказа: 0 руб.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при оформлении заказа:\n" + ex.Message);
                }
            }
        }
    }
}
