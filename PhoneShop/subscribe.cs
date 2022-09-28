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
    public partial class subscribe : Form
    {
        MySqlConnection con = new MySqlConnection("host=localhost;user=root;password=;database=phoneshop;");
        public subscribe()
        {
            InitializeComponent();
        }
        private void button_goback_Click(object sender, EventArgs e) //ย้อนกลับไปหน้าร้าน
        {
            KulthidaStore back1 = new KulthidaStore();
            back1.Show();
            this.Hide();
        }
        private bool checkcharacter(string str) //ฟังก์ชันเอาไว้เช็คตัวอักษร
        {
            string word = "กขฃคฅฆงจฉชซฌญฎฏฐฑฒณดตถทธนบปผฝพฟภมยรลวศษสหฬอฮะาำิีึืุูเแโัๆใไๅฤฦ่้๊๋์ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < str.Length; i++)  //ใช้ for เช็คตัวอักษรทีละตัวของข้อความ
            {
                if (!(word.Contains(str[i])))
                {
                    return false;         //ถ้ามีตัวที่ไม่มีในตัวแปร word ก็จะreturn...
                }
            }
            return true;
        }

        private void sign(string username, string password, string fname, string lname, string phone, string Address, string province, string postcode) 
        {
            string sql = $"INSERT INTO member (username,password,fname,lname,phone,Address,province,postcode) VALUES ('{username}','{password}','{fname}','{lname}','{phone}','{Address}','{province}','{postcode}')";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            con.Open();

            int rows = cmd.ExecuteNonQuery();
            con.Close();

            if (rows > 0)
            {
                MessageBox.Show("เข้าสู่ระบบ");
            }
            else
            {
                MessageBox.Show("โปรดกรอกชื่อของท่านเป็นตัวอักษรเท่านั้น", "ERROR");
            }
        }

        private bool checkNum(string str)  //ทำงานเพื่อเช็คตัวเลข เรียกใช้ทีหลัง
        {
            if (string.IsNullOrEmpty(str))
                return false;

            for (int i = 0; i < str.Length; i++) //เช็คตัวอักษตรแต่ละวัน
            {
                if (char.IsNumber(str[i])) //ลูปเช็คทีละตัวถ้าเป็นตัวเลขจะ....
                {
                    return true;
                }
            }
            return false;
        }

        private void signin_Click(object sender, EventArgs e) //หลังจากที่คลิกปุ่มสมัครสมาชิก
        {
            //ประกาศตัวแปรเพื่อเก็บค่าในtextboxทั้งหมด 
            string user = username.Text;
            string pass1 = password1.Text;
            string pass2 = password2.Text;
            string f_name = fname.Text;
            string l_name = lname.Text;
            string call = phone.Text;
            string home = Address.Text;
            string pro = province.Text;
            string pc = postcode.Text;

            if (checkcharacter(user) && checkcharacter(f_name) && checkcharacter(l_name)) //เช็คตัวอักษตร fname,lname
            {
                if (checkNum(call) && call.Length == 10) //เช็คเบอร์โทร
                {
                    if (checkNum(pc) && pc.Length == 5) //เช็ครหัสไปรษณีย์
                    {
                        if (pass1.Length >= 8) //เช็ครหัส
                        {
                            if (pass1 == pass2) //เช็ครหัสยืนยัน
                            {
                                MessageBox.Show("ลงทะเบียนสมัครสมาชิกเรียบร้อยแล้ว", "Succeed");
                                sign(user, pass1, f_name, l_name, call, home, pro, pc);

                                Login logins = new Login();
                                logins.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Password ไม่ตรงกัน", "โปรดกรอกอีกครั้ง");
                            }
                        } else
                        {
                            MessageBox.Show("รหัสผ่านต้องมีไม่น้อยกว่า 8 ตัวอักษร", "โปรดกรอกอีกครั้ง");
                        }
                    }
                    else
                    {
                        MessageBox.Show("รหัสไปรษณีย์ไม่ถูกต้อง", "โปรดกรอกอีกครั้ง");
                    }
                }
                else
                {
                    MessageBox.Show("เบอร์โทรศัพท์ไม่ถูกต้อง", "โปรดกรอกอีกครั้ง");
                }

            }
            else
            {
                MessageBox.Show("ชื่อผู้ใช้หรือชื่อ-นามสกุล ไม่ถูกต้อง", "โปรดกรอกอีกครั้ง");
            }
        }
    }
}
