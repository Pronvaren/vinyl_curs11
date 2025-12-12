using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vinyl_curs
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new aut());
        }

        //Form2 f2 = new Form2();
        //f2.Show();       // показываем новую форму
        //this.Close();


        //Form2 f2 = new Form2();
        //this.Hide();     // скрываем текущую форму
        //f2.ShowDialog(); // открываем новую как основную
        //this.Close();

        //Form2 f2 = new Form2();
        //f2.Show();   // открываем новую форму
        //// this.Close();  // НЕ пишем!
        ///

        //Form2 f2 = new Form2();
        //f2.ShowDialog(); // открывается новая форма, пока её не закроют — к Form1 не вернёшься
    }
}
