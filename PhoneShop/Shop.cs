using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneShop
{
    public partial class Shop : Form
    {
        public static string categories = "";
        public Shop()
        {
            InitializeComponent();
        }

        private void button_goback_Click(object sender, EventArgs e) //กลับสู่หน้าแรก
        {
            KulthidaStore fpage = new KulthidaStore();
            fpage.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e) //เข้าสู่หน้าสินค้าiphone
        {
            categories = "iPhone";
            apple phone1 = new apple();
            phone1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e) //เข้าสู่หน้าสินค้าandroid
        {
            categories = "Android";
            apple phone1 = new apple();
            phone1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e) //เข้าสู่หน้าสินค้าอุปกรณ์เสริม
        {
            categories = "Accessories";
            apple phone1 = new apple();
            phone1.Show();
            this.Hide();
        }
    }
}
