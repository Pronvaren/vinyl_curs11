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

namespace vinyl_curs
{
    public partial class prosm_zak : Form
    {
        private string userRole;
        private string userFio;
        private int orderId;

        string connStr = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
        public prosm_zak(string fio, string role, int orderId)
        {
            InitializeComponent();

            userRole = role;
            userFio = fio;
            this.orderId = orderId;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void prosm_zak_Load(object sender, EventArgs e)
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

            comboBox2.Items.Clear();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT id, Name FROM OrderStatus", conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["id"].ToString() + " - " + reader["Name"].ToString());
                }
            }

            // Загрузка данных самого заказа
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = @"
                SELECT o.id, o.Date, o.CostSumm, o.Status, o.DeliveryDate, os.Name AS StatusName 
                FROM Orders o
                LEFT JOIN OrderStatus os ON o.Status = os.id
                WHERE o.id = @orderId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    label7.Text = $"Заказ №: {reader["id"]}";
                    label1.Text = $"Дата заказа: {Convert.ToDateTime(reader["Date"]).ToString("dd.MM.yyyy")}";
                    label5.Text = $"Сумма: {Convert.ToDecimal(reader["CostSumm"])} руб.";

                    // Установка статуса в comboBox2
                    string statusText = reader["Status"].ToString() + " - " + reader["StatusName"].ToString();
                    comboBox2.SelectedItem = statusText;

                    if (reader["DeliveryDate"] != DBNull.Value)
                        dateTimePicker1.Value = Convert.ToDateTime(reader["DeliveryDate"]);
                }
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
SELECT 
    oc.ProductID AS 'Артикул',
    p.Name AS 'Наименование',
    oc.Quantity AS 'Кол-во',
    oc.Cost AS 'Цена за единицу',
    oc.CostSumm AS 'Сумма'
FROM OrderComposition oc
LEFT JOIN Products p ON oc.ProductID = p.id
WHERE oc.OrderID = @orderId";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.MultiSelect = false;
                dataGridView1.ReadOnly = true;

                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Crimson;

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    col.HeaderCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string selectedStatus = comboBox2.SelectedItem?.ToString();
            int newStatusId = 0;
            if (!string.IsNullOrEmpty(selectedStatus))
                newStatusId = int.Parse(selectedStatus.Split('-')[0].Trim());

            DateTime newDeliveryDate = dateTimePicker1.Value.Date;

            bool isStatusChanged = false;
            bool isDeliveryDateChanged = false;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT Status, DeliveryDate FROM Orders WHERE id = @orderId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);

                MySqlDataReader reader = cmd.ExecuteReader();
                int currentStatus = 0;
                DateTime currentDelivery = DateTime.MinValue;
                if (reader.Read())
                {
                    currentStatus = Convert.ToInt32(reader["Status"]);
                    currentDelivery = Convert.ToDateTime(reader["DeliveryDate"]).Date;

                    if (currentStatus != newStatusId) isStatusChanged = true;
                    if (currentDelivery != newDeliveryDate) isDeliveryDateChanged = true;
                }
                reader.Close();

                if (isStatusChanged || isDeliveryDateChanged)
                {
                    DialogResult result = MessageBox.Show(
                        "Статус заказа или дата доставки были изменены.\nСохранить изменения?",
                        "Сохранение изменений",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        if (newStatusId == 3 && currentStatus != 3)
                        {
                            // Возврат товара на склад
                            string returnQuery = @"
                            UPDATE Products p
                            JOIN OrderComposition oc ON p.id = oc.ProductID
                            SET p.QuantityWarehouse = p.QuantityWarehouse + oc.Quantity
                            WHERE oc.OrderID = @orderId";

                            MySqlCommand returnCmd = new MySqlCommand(returnQuery, conn);
                            returnCmd.Parameters.AddWithValue("@orderId", orderId);
                            returnCmd.ExecuteNonQuery();
                        }

                        // Обновление самого заказа
                        string updateQuery = @"
                        UPDATE Orders
                        SET Status = @status, DeliveryDate = @delivery
                        WHERE id = @orderId";

                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@status", newStatusId);
                        updateCmd.Parameters.AddWithValue("@delivery", newDeliveryDate);
                        updateCmd.Parameters.AddWithValue("@orderId", orderId);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }

            zak zak = new zak(userFio, userRole);
            this.Hide();
            zak.ShowDialog();
            this.Close();
        }
    }
}
