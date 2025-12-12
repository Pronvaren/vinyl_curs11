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
    public partial class sprav : Form
    {
        private string userRole;
        private string userFio;

        public sprav(string fio, string role)
        {
            InitializeComponent();

            userRole = role;
            userFio = fio;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            glav glav = new glav(userFio, userRole);
            this.Hide();
            glav.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            supp supp = new supp(userFio, userRole);
            this.Hide();
            supp.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            manuf manuf = new manuf(userFio, userRole);
            this.Hide();
            manuf.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            emp emp = new emp(userFio, userRole);
            this.Hide();
            emp.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            role role = new role(userFio, userRole);
            this.Hide();
            role.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            artist artist = new artist(userFio, userRole);
            this.Hide();
            artist.ShowDialog();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            genre genre = new genre(userFio, userRole);
            this.Hide();
            genre.ShowDialog();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            med_format med_format = new med_format(userFio, userRole);
            this.Hide();
            med_format.ShowDialog();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label label = new label(userFio, userRole);
            this.Hide();
            label.ShowDialog();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            composer composer = new composer(userFio, userRole);
            this.Hide();
            composer.ShowDialog();
            this.Close();
        }

        private void sprav_Load(object sender, EventArgs e)
        {
            if (userRole == "Продавец")
            {

            }
            else if (userRole == "Товаровед")
            {
                button2.Visible = false;
                button3.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                button9.Visible = false;

            }
            else if (userRole == "Администратор")
            {
                button1.Enabled = false;
                button4.Enabled = false;
            }
        }
    }
}
