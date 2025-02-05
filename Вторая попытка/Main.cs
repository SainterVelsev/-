using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Вторая_попытка
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            App F2 = new App();
            F2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Order F3 = new Order();
            F3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Empl F4 = new Empl();
            F4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 F1 = new Form1();
            F1.Show();
        }
    }
}
