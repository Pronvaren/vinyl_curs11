using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;

namespace vinyl_curs
{
    class IdleManager
    {
        private Timer timer;
        private int idleSeconds;
        private int currentIdle;
        private Form currentForm;

        public IdleManager(Form form)
        {
            currentForm = form;

            idleSeconds = int.Parse(
                ConfigurationManager.AppSettings["IdleTimeoutSeconds"]);

            timer = new Timer();
            timer.Interval = 1000; 
            timer.Tick += Timer_Tick;

            form.MouseMove += ResetIdle;
            form.KeyDown += ResetIdle;

            timer.Start();
        }

        private void ResetIdle(object sender, EventArgs e)
        {
            currentIdle = 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currentIdle++;

            if (currentIdle >= idleSeconds)
            {
                timer.Stop();

                MessageBox.Show(
                    "Блокировка системы из-за бездействия!","Блокировка",MessageBoxButtons.OK,MessageBoxIcon.Warning);

                currentForm.Hide();

                aut authForm = new aut();
                authForm.ShowDialog();

                currentForm.Close();
            }
        }
    }
}
