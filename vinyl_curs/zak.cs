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
using Microsoft.Office.Interop.Word;
using System.IO;
using System.Globalization;

namespace vinyl_curs
{
    public partial class zak : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
        public zak(string fio, string role)
        {
            InitializeComponent();

            dateTimePicker2.MinDate = dateTimePicker1.Value;

            textBox1.MaxLength = 50;

            userRole = role;
            userFio = fio;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            glav glav = new glav(userFio, userRole);
            this.Hide();
            glav.ShowDialog();
            this.Close();
        }

        private void zak_Load(object sender, EventArgs e)
        {
            if (userRole == "Продавец")
            {
                button8.Visible = false;
                label8.Visible = false;
                dateTimePicker1.Visible = false;
                label9.Visible = false;
                dateTimePicker2.Visible = false;
            }
            else if (userRole == "Товаровед")
            {
                button4.Visible = false;
            }
            else if (userRole == "Администратор")
            {
                button4.Visible = false;
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
            SELECT 
                o.id AS 'Номер заказа',
                o.Date AS 'Дата заказа',
                os.Name AS 'Статус',
                e.Name AS 'Продавец',
                o.CostSumm AS 'Сумма',
                o.DeliveryDate AS 'Дата доставки'
            FROM Orders o
            LEFT JOIN Employees e ON o.Salesman = e.id
            LEFT JOIN OrderStatus os ON o.Status = os.id;
        ";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Crimson;

                LoadOrders("");
                LoadStatus();

                // СУММА ОПЛАЧЕНЫХ ЗАКАЗОВ
                using (MySqlConnection conn1 = new MySqlConnection(connStr))
                {
                    conn1.Open();

                    string query1 = "SELECT SUM(CostSumm) FROM Orders WHERE Status <> 3";

                    MySqlCommand cmd = new MySqlCommand(query1, conn1);
                    object result = cmd.ExecuteScalar();

                    decimal total = 0;
                    if (result != DBNull.Value)
                        total = Convert.ToDecimal(result);

                    label4.Text = $"Сумма оплаченных заказов: {total} руб.";

                    string queryCount = "SELECT COUNT(*) FROM Orders";
                    MySqlCommand cmdCount = new MySqlCommand(queryCount, conn1);
                    object resultCount = cmdCount.ExecuteScalar();
                    int totalCount = (resultCount != DBNull.Value) ? Convert.ToInt32(resultCount) : 0;
                    label5.Text = $"Кол-во записей: {totalCount}";
                }

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите отменить заказ?", "Отмена заказа", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Заказ отменен!", "Отмена заказа");
            }
            else
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заказ для просмотра!");
                return;
            }

            int orderId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Номер заказа"].Value);

            prosm_zak prosm_zak = new prosm_zak(userFio, userRole, orderId);
            this.Hide();
            prosm_zak.ShowDialog();
            this.Close();
        }

        // ФИЛЬТРАЦИЯ ПО СТАТУСУ
        private void LoadStatus()
        {
            comboBox1.Items.Add("Все статусы");
            comboBox1.SelectedIndex = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, Name FROM OrderStatus", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["id"].ToString() + " - " + reader["Name"].ToString());
                }
            }
        }

        private void LoadOrders(string statusId = "", string idFilter = "")
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
        SELECT 
            o.id AS 'Номер заказа',
            o.Date AS 'Дата заказа',
            os.Name AS 'Статус',
            e.Name AS 'Продавец',
            o.CostSumm AS 'Сумма',
            o.DeliveryDate AS 'Дата доставки'
        FROM Orders o
        LEFT JOIN Employees e ON o.Salesman = e.id
        LEFT JOIN OrderStatus os ON o.Status = os.id
        WHERE 1=1
        ";

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;

                if (!string.IsNullOrEmpty(statusId))
                {
                    query += " AND o.Status = @statusId";
                    cmd.Parameters.AddWithValue("@statusId", statusId);
                }

                if (!string.IsNullOrEmpty(idFilter))
                {
                    query += " AND o.id LIKE @idFilter";
                    cmd.Parameters.AddWithValue("@idFilter", "%" + idFilter + "%");
                }

                cmd.CommandText = query;

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        // СТАТУСЫ
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string statusId = "";
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Все статусы")
                statusId = comboBox1.SelectedItem.ToString().Split('-')[0].Trim();

            LoadOrders(statusId);
        }

        // ПОИСК ПО НОМЕР ЗАКАЗА
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string statusId = "";
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Все статусы")
                statusId = comboBox1.SelectedItem.ToString().Split('-')[0].Trim();

            LoadOrders(statusId, textBox1.Text.Trim());
        }

        // ОЧИСТКА
        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            comboBox1.SelectedIndex = 0;
            LoadOrders("");
        }

        // ВВОД ТОЛЬКО ЦИФР
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string dateFrom = dateTimePicker1.Value.ToString("dd.MM.yyyy");
            string dateTo = dateTimePicker2.Value.ToString("dd.MM.yyyy");
            string fio = userFio;
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            DataTable dtOrders = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string q = @"
            SELECT 
                o.id AS 'Номер заказа',
                o.Date AS 'Дата заказа',
                os.Name AS 'Статус',
                e.Name AS 'Продавец',
                o.CostSumm AS 'Сумма',
                o.DeliveryDate AS 'Дата доставки',
                o.Status AS 'StatusId'
            FROM Orders o
            LEFT JOIN Employees e ON o.Salesman = e.id
            LEFT JOIN OrderStatus os ON o.Status = os.id
            WHERE o.Date BETWEEN @from AND @to
        ";

                MySqlCommand cmd = new MySqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@from", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@to", dateTimePicker2.Value.Date);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dtOrders);
            }

            if (dtOrders.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных за выбранный период!");
                return;
            }

            // Создание отчета
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            word.Visible = false;
            Document doc = word.Documents.Add();

            // Заголовок
            var p1 = doc.Content.Paragraphs.Add();
            p1.Range.Text = "ОТЧЁТ О ВЫПОЛНЕННЫХ ЗАКАЗАХ";
            p1.Range.Font.Size = 16;
            p1.Range.Font.Bold = 1;
            p1.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            p1.Range.InsertParagraphAfter();

            var p2 = doc.Content.Paragraphs.Add();
            p2.Range.Text = $"Период: с {dateFrom} по {dateTo}\nСформирован: {date}";
            p2.Range.Font.Size = 12;
            p2.Range.Font.Bold = 0;
            p2.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            p2.Range.InsertParagraphAfter();

            // Заголовок таблицы
            var p3 = doc.Content.Paragraphs.Add();
            p3.Range.Text = "Таблица заказов:";
            p3.Range.Font.Bold = 1;
            p3.Range.InsertParagraphAfter();

            // Создание таблицы
            int rows = dtOrders.Rows.Count + 1;
            int cols = dtOrders.Columns.Count - 1;

            var table = doc.Tables.Add(p3.Range, rows, cols);
            table.Borders.Enable = 1;

            // Заголовки
            for (int c = 0; c < cols; c++)
            {
                table.Cell(1, c + 1).Range.Text = dtOrders.Columns[c].ColumnName;
                table.Cell(1, c + 1).Range.Bold = 1;
            }

            // Данные
            for (int r = 0; r < dtOrders.Rows.Count; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    table.Cell(r + 2, c + 1).Range.Text = dtOrders.Rows[r][c].ToString();
                }
            }

            int completed = dtOrders.AsEnumerable().Count(r => Convert.ToInt32(r["StatusId"]) == 4);
            int processing = dtOrders.AsEnumerable().Count(r => Convert.ToInt32(r["StatusId"]) == 2);
            int created = dtOrders.AsEnumerable().Count(r => Convert.ToInt32(r["StatusId"]) == 1);
            int cancelled = dtOrders.AsEnumerable().Count(r => Convert.ToInt32(r["StatusId"]) == 3);
            decimal sum = dtOrders.AsEnumerable().Sum(r => Convert.ToDecimal(r["Сумма"]));

            var p4 = doc.Content.Paragraphs.Add();
            p4.Range.InsertParagraphAfter();
            p4.Range.Text =
                $"ИТОГИ:\n" +
                $"Успешно завершено: {completed}\n" +
                $"В обработке: {processing}\n" +
                $"Оформлены: {created}\n" +
                $"Отменено: {cancelled}\n" +
                $"Общее количество заказов: {completed + processing + created + cancelled}\n" +
                $"Общая сумма заказов(доход): {sum}";
            p4.Range.Font.Bold = 0;

            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = $"Отчет_по_заказам_{DateTime.Now:yyyy-MM-dd}.docx";
            string filePath = Path.Combine(folderPath, fileName);

            doc.SaveAs(filePath);
            doc.Close();
            word.Quit();

            System.Diagnostics.Process.Start(filePath);

            MessageBox.Show("Отчёт успешно создан!\n" + filePath);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
        }
    }
}
