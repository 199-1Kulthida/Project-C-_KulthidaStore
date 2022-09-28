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

namespace PhoneShop
{
    public partial class Login : Form
    {
        public static bool loginstatus = false;
        public static MySqlConnection con = new MySqlConnection("host=localhost;user=root;password=;database=phoneshop;");
        public Login()
        {
            InitializeComponent();
        }

        private bool checkPass(string text, string password) //ฟังก์ชันเช็คpasssword
        {
            con.Open();

            string sql = "SELECT password FROM member WHERE username = '" + text + "'";
            
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader dr = cmd.ExecuteReader();
            
            string pass = "";  //ประกาศตัวแปรpassเป็นค่าว่างประเภทstring 

            while (dr.Read()) //ใช้ while อ่านค่าที่มี
            {
                pass = dr.GetString("password");
            }
            con.Close();

            if (pass == password) //เช็คค่าที่ผู้ใช้ป้อนเข้ามาว่าถูกมั้ย 
            {
                return true;
            }

            return false;
        }

        private void subscribe_Click(object sender, EventArgs e) //ไปหน้าสมัครสมาชิก
        {
            subscribe backsub = new subscribe();
            backsub.Show();
            this.Hide();
        }

        private void button_goback_Click(object sender, EventArgs e) //ย้อนกลับไปหน้าร้าน
        {
            KulthidaStore back1_1 = new KulthidaStore();
            back1_1.Show();
            this.Hide();
        }
        public static string user;  //ประกาศตัวแปรuser

        private void kryptonButton1_Click(object sender, EventArgs e)  //ไปหน้าเข้าสู่ระบบเพื่อที่จะเลือกรุ่น
        {
            basket.lastID();
            user = textBox1.Text;
            string pass = textBox2.Text;

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
            {
                if (checkPass(user, pass)==true) //เช็คpass เมื่อถูกต้อง...
                {
                    loginstatus = true;
                    MessageBox.Show("เข้าสู่ระบบเรียบร้อย", "พร้อม Shop จย้าา");
                    this.Hide();
                    Shop shopp = new Shop();
                    shopp.Show();
                }
                else //เช็คpass เมื่อไม่ถูกต้อง...
                {
                    MessageBox.Show("รหัสผ่านไม่ถูกต้อง", "ไม่ถูกจย้าาา");
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
            }
            else //ผู้ใช้กรอกไม่ถูกต้อง
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน", "");
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }
    }
}
