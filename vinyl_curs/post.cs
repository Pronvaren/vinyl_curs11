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
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace vinyl_curs
{
    public partial class post : Form
    {
        private string userRole;
        private string userFio;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
        public post(string fio, string role)
        {
            InitializeComponent();

            dateTimePicker2.MinDate = dateTimePicker1.Value;

            userFio = fio;
            userRole = role;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int postId = Convert.ToInt32(row.Cells["ID"].Value);
            string productName = row.Cells["Товар"].Value.ToString();

            DialogResult result = MessageBox.Show($"Вы уверены, что хотите удалить запись с товаром: {productName}?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    string deleteSql = "DELETE FROM Supplies WHERE id = @postId";
                    MySqlCommand cmd = new MySqlCommand(deleteSql, conn);
                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Запись успешно удалена!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                post_Load(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            string date = row.Cells["Дата"].Value.ToString();
            string product = row.Cells["Товар"].Value.ToString();
            string supplier = row.Cells["Поставщик"].Value.ToString();
            string quantity = row.Cells["Количество"].Value.ToString();

            izm_post izm_post = new izm_post(userFio, userRole, id, date, product, supplier, quantity);
            this.Hide();
            izm_post.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dobav_post dobav_post = new dobav_post(userFio, userRole);
            this.Hide();
            dobav_post.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            glav glav = new glav(userFio,userRole);
            this.Hide();
            glav.ShowDialog();
            this.Close();
        }

        private void post_Load(object sender, EventArgs e)
        {
            if (userRole == "Продавец")
            {

            }
            else if (userRole == "Товаровед")
            {

            }
            else if (userRole == "Администратор")
            {

            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                    SELECT s.id AS 'ID', s.Date AS 'Дата', p.Name AS 'Товар', ss.Name AS 'Поставщик', s.Quantity AS 'Количество' FROM Supplies s LEFT JOIN Products p ON s.Product = p.id LEFT JOIN Suppliers ss ON s.Supplier = ss.id
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

        private void button5_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = @"
            SELECT 
                s.id AS ID,
                s.Date AS Date,
                p.Name AS Product,
                sup.Name AS Supplier,
                s.Quantity AS Quantity,
                (s.Quantity * p.Cost) AS TotalCost
            FROM Supplies s
            LEFT JOIN Products p ON s.Product = p.id
            LEFT JOIN Suppliers sup ON p.Supplier = sup.id
            WHERE s.Date BETWEEN @from AND @to
        ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@from", dateTimePicker1.Value.Date);
                cmd.Parameters.AddWithValue("@to", dateTimePicker2.Value.Date);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных за выбранный период!");
                return;
            }

            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet ws = workbook.ActiveSheet;

            excelApp.Visible = true;

            // Заголовок
            ws.Range["A1:F1"].Merge();
            ws.Range["A1"].Value = "Отчет по поставкам";
            ws.Range["A1"].Font.Size = 16;
            ws.Range["A1"].Font.Bold = true;
            ws.Range["A1"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            // Период
            ws.Range["A2:F2"].Merge();
            ws.Range["A2"].Value = $"Период: с {dateTimePicker1.Value:dd.MM.yyyy} по {dateTimePicker2.Value:dd.MM.yyyy}";
            ws.Range["A2"].Font.Size = 12;
            ws.Range["A2"].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

            // Заголовки столбцов
            string[] headers = { "Дата", "Продукт", "Поставщик", "Количество", "Сумма поставки" };
            for (int i = 0; i < headers.Length; i++)
            {
                ws.Cells[4, i + 1] = headers[i];
                ws.Cells[4, i + 1].Font.Bold = true;
                ws.Cells[4, i + 1].Interior.Color = System.Drawing.Color.LightGray;
                ws.Columns[i + 1].AutoFit();
                ws.Cells[4, i + 1].Borders.Weight = Excel.XlBorderWeight.xlThin;
            }

            // Данные
            int row = 5;
            decimal totalQuantity = 0;
            decimal totalSum = 0;

            foreach (DataRow dr in dt.Rows)
            {
                ws.Cells[row, 1] = Convert.ToDateTime(dr["Date"]).ToString("dd.MM.yyyy");
                ws.Cells[row, 2] = dr["Product"];
                ws.Cells[row, 3] = dr["Supplier"];
                ws.Cells[row, 4] = dr["Quantity"];
                ws.Cells[row, 5] = dr["TotalCost"];

                totalQuantity += Convert.ToDecimal(dr["Quantity"]);
                totalSum += Convert.ToDecimal(dr["TotalCost"]);

                for (int c = 1; c <= 5; c++)
                {
                    ws.Cells[row, c].Borders.Weight = Excel.XlBorderWeight.xlThin;
                }

                row++;
            }

            // Итоговая строка
            ws.Cells[row, 3] = "ИТОГО:";
            ws.Cells[row, 3].Font.Bold = true;
            ws.Cells[row, 4] = totalQuantity;
            ws.Cells[row, 5] = totalSum;

            for (int c = 3; c <= 5; c++)
            {
                ws.Cells[row, c].Borders.Weight = Excel.XlBorderWeight.xlThin;
                ws.Cells[row, c].Interior.Color = System.Drawing.Color.LightYellow;
            }

            ws.Columns.AutoFit();

            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = $"Отчет_по_поставкам_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";
            string filePath = Path.Combine(folderPath, fileName);

            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();

            System.Diagnostics.Process.Start(filePath);

            MessageBox.Show("Отчет по поставкам успешно создан!\n" + filePath);
        }
    }
}
