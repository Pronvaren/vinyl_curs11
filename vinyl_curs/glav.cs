using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vinyl_curs
{
    public partial class glav : Form
    {
        private string userRole;
        private string userFio;

        public glav(string fio, string role)
        {
            InitializeComponent();

            userFio = fio;
            userRole = role;


        }

        private void glav_Load(object sender, EventArgs e)
        {
            label1.Text = userFio;
            label3.Text = userRole;

            if (userRole == "Продавец")
            {
                button4.Visible = false;
                button5.Visible = false; 
            }
            else if (userRole == "Товаровед")
            {

            }
            else if (userRole == "Администратор")
            {
                button2.Enabled = false;
                button4.Enabled = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tov tov = new tov(userFio, userRole);
            this.Hide();     
            tov.ShowDialog(); 
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            zak zak = new zak(userFio, userRole);
            this.Hide();
            zak.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            post post = new post(userFio, userRole);
            this.Hide();
            post.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sprav sprav = new sprav(userFio, userRole);
            this.Hide();
            sprav.ShowDialog();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите вернуться на форму авторизации?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                aut aut = new aut();
                this.Hide();
                aut.ShowDialog();
                this.Close();
            }
            else
            {
            }
        }
    }
}
