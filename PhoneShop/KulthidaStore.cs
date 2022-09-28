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
    public partial class KulthidaStore : Form
    {
        public KulthidaStore()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) //ไปหน้าสมัครสมาชิก
        {
            subscribe sub = new subscribe();
            sub.Show();
            this.Hide();  
        }

        private void Login_Click(object sender, EventArgs e) //ไปหน้าสมัครเข้าสู่ระบบ
        {
            Login sign = new Login();
            sign.Show();
            this.Hide();
        }

        private void buynow_Click(object sender, EventArgs e) //ไปหน้าเลือกรุ่น
        {
            Shop shoppp = new Shop();
            shoppp.Show();
            this.Hide();
        }

        private void Admin_Click(object sender, EventArgs e) //ไปหน้าเข้าสู่ระบบหลังร้าน
        {
            Loginadmin loginadmin = new Loginadmin();
            loginadmin.Show();
            this.Hide();
        }
    }
}
