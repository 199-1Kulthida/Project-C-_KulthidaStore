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
    public partial class Loginadmin : Form
    {
        public Loginadmin()
        {
            InitializeComponent();
        }

        private void button_goback_Click(object sender, EventArgs e) //ย้อนกลับไปหน้าร้าน
        {
            KulthidaStore Back = new KulthidaStore();
            Back.Show();
            this.Hide();
        }


        private string username = "adminkulthida";
        private string password = "7441";
        public static bool a1 = false; //สถานะlogin

        private void kryptonButton1_Click(object sender, EventArgs e)    //ฟังก์ชัน...
        {
            string n = textBox1.Text; //username
            string p = textBox2.Text; //password

            if (username == n) 
            {
                if (password == p)
                {
                    a1 = true;
                    MessageBox.Show("เข้าสู่ระบบพนักงานเรียบร้อยแล้ว");
                    this.Hide();
                    Stock stock = new Stock();  
                    stock.Show();               //ไปหน้าaddstore
                } else
                {
                    MessageBox.Show("รหัสผ่านไม่ถูกต้อง");
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
            }
            else
            {
                MessageBox.Show("ชื่อผู้ใช้ Admin ไม่ถูกต้อง");
                textBox1.Text = "";
                textBox2.Text = "";

            }
        }
    }
}
