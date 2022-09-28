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
using System.IO;

namespace PhoneShop
{
    public partial class apple : Form
    {
        public apple()
        {
            InitializeComponent();
        }

        MySqlConnection con = Login.con;
        Stock stock = new Stock();

        private string cat = Shop.categories;
        public static string[] product(string cat) //ฟังก์ชันการเลือกสินค้าในประเภทนั้นๆ 
        {
            MySqlConnection con = Login.con;
            string sql = "SELECT * FROM stock WHERE category = '" + cat + "'"; //select from stock เลือกสินค้ายี่ห้ออะไร
            con.Open();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> list = new List<string>();  
            while (reader.Read()) //วนลูปทั้งหมดไปเก็บไว้ในตัวแปรlist 
            {
                list.Add(reader.GetString("model") + "-" + reader.GetString("color"));
            }

            con.Close();
            return list.Distinct().ToArray(); //returnค่าของตัวแปรlistโดยที่จะลบค่าที่ซ้ำ 
        }

        public static string[] getfrommodel(string model, bool hascolor, string color) //ฟังก์ชันเช็คgb ดึงจาก model สี
        {
            MySqlConnection con = Login.con;
            string sql = "";
            if (hascolor == true) //ถ้ามีสีก็จะดึงข้อมูลจาก model color
            {
                sql = "SELECT * FROM stock WHERE model = '" + model + "' AND color = '" + color + "'";
            }
            else //ถ้าไม่มี color ก็จะดึงมาแค่ model
            {
                sql = "SELECT * FROM stock WHERE model = '" + model + "'";
            }
            con.Open();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> list = new List<string>();
            while (reader.Read()) //ใช้ while เพื่อที่จะลูปค่าไว้ในตัวแปรlist 
            {
                if (hascolor == true) //เช็คสีว่าสีนั้นมีไหม 
                {
                    list.Add(reader.GetString("gb")); //ถ้ามีก็จะเก็บข้อมูลความจุไว้ในlist
                }
                else
                {
                    list.Add(reader.GetString("color")); //ถ้าไม่มีก็จะเก็บข้อมูลสีไว้ในlist
                }
            }

            con.Close();
            return list.ToArray(); 
        }

        public static string[] getdata(string model, string gb, string color) //ฟังก์ชันดึงข้อมูลสินค้า โดยที่จะมี model gb color
        {
            MySqlConnection con = Login.con;
            string sql = "SELECT * FROM stock WHERE model = '" + model + "' AND gb = '" + gb + "' AND color = '" + color + "'"; 
            con.Open();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> list = new List<string>();
            while (reader.Read()) //วูปเก็บ ราคา รายละเอียด ไฟล์รูปภาพ
            {
                list.Add(reader.GetString("price"));
                list.Add(reader.GetString("details"));
                list.Add(reader.GetString("file"));
            }

            con.Close();
            return list.ToArray();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e) //ล็อกไม่ให้พิมพ์ในcombobox
        {
            e.Handled = true;
        }

        private void button7_Click(object sender, EventArgs e) //ย้อนกลับหน้าเลือกยี่ห้อสินค้า
        {
            Shop back1 = new Shop();
            back1.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e) //ไปที่หน้าตะกร้า
        {
            basket dasket = new basket();
            basket.ppp = 3;
            dasket.Show();
            this.Hide();
        }

        private int checkBasket(string model, string gb, string color, int idorder) //เช็คจำนวนสินค้าในตะกร้าที่เลือก
        {
            string sql = $"SELECT * FROM basket WHERE model = '{model}' AND gb = '{gb}' AND color = '{color}' AND idorder = '{idorder}'";
            MySqlCommand cmd = new MySqlCommand(sql, Login.con);
            con.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            int quantity = 0;
            if (dr.Read())  //เซ็ตค่าเข้าไปในตัวแปรquantity
            {
                quantity = dr.GetInt32("quantity");
            }
            con.Close();

            return quantity;
        }


        private void button11_Click_1(object sender, EventArgs e) //ปุ่มบันทึกการสั่งซื้อ
        {
            //ประกาศตัวแปรเก็บค่าของcombobox
            string model = comboBox1.Text;
            string gb = comboBox3.Text;
            string color = comboBox2.Text;
            int quantity = checkBasket(model, gb, color, basket.idoer); //จำนวนที่เอาไว้เช็คของเมื่อกี้
            if (Stock.quan(model, gb, color) >= numericUpDown1.Value) //เช็คว่าสินค้าพอกับการสั่งซื้อของลูกค้าไหม
            {
                if (quantity == 0) //ถ้าจำนวนสินค้าคือ0จะinsertสินค้าเข้าไปในตาราง
                {

                    string sql = "INSERT INTO `basket` (model,GB,color,quantity,price,username,idorder) VALUES";
                    sql += "('" + model + "',";
                    sql += "'" + gb + "',";
                    sql += "'" + color + "',";
                    sql += "'" + numericUpDown1.Value + "',";
                    sql += "'" + (Convert.ToInt32(textBox1.Text.Replace(",", "")) * numericUpDown1.Value) + "',";
                    sql += "'" + Login.user + "',";
                    sql += "'" + basket.idoer + "')";


                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    con.Open();

                    int rows = cmd.ExecuteNonQuery();
                    con.Close();

                    //การอัปเดตสินค้าในstock
                    int amount = Stock.quan(model, gb, color); //ดึงจำนวนสินค้าที่มีอยู่ในstock
                    sql = "UPDATE stock SET quantity = '" + (amount - numericUpDown1.Value) + "' WHERE model =  '" + model + "'AND gb ='" + gb + "'AND color ='" + color + "'";
                    cmd = new MySqlCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (rows > 0) //ถ้าเพิ่มสินค้าสำเร็จ
                    {
                        MessageBox.Show("เพิ่มในตะกร้า", "Succeed");
                    }
                    else //ถ้าเพิ่มสินค้าไม่สำเร็จ
                    {
                        MessageBox.Show("โปรดกรอกชื่อของท่านเป็นตัวอักษรเท่านั้น", "ERROR");
                    }
                }
                else //ถ้าจำนวนสินค้าไม่เป็น0จะinsertสินค้าเข้าไปในตาราง
                {
                    string sql = $"UPDATE basket SET quantity = '{(quantity + numericUpDown1.Value)}' WHERE model = '{model}' AND gb = '{gb}' AND color = '{color}' AND idorder = '{basket.idoer}'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close(); //อัปเดตจำนวนสินค้าเข้าไปในตระกร้า

                    int amount = Stock.quan(model, gb, color);
                    sql = "UPDATE stock SET quantity = '" + (amount - numericUpDown1.Value) + "' WHERE model =  '" + model + "'AND gb ='" + gb + "'AND color ='" + color + "'";
                    cmd = new MySqlCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();  //อัปเดตจำนวนสินค้าเข้าไปในstock

                    if (rows > 0) //เพิ่มสำเร็จ
                    {
                        MessageBox.Show("เพิ่มในตะกร้า", "Succeed");
                    }
                }
            }
            else //สินค้าหมดหรือสั่งเยอะกว่าที่มี
            {
                MessageBox.Show("สินค้าไม่พอต่อการสั่งซื้อของท่าน");
            }

        }

        private void clearBox() //ฟังก์ชันเคลียร์ข้อความ
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox3.Text = "";
            Address.Text = "";

            pictureBox1.Image = null;
        }

        private void button6_Click(object sender, EventArgs e) //ที่หน้าสั่งซื้อสินค้าหรือตระกร้าสินค้า
        {
            basket buy1 = new basket();
            buy1.Show();
            this.Hide();
        }

        private void apples(object sender, EventArgs e) 
        {
            string[] productlist = product(cat); //เก็บสินค้าของประเภทนั้นๆ
            comboBox1.Items.Clear(); 
            comboBox2.Items.Clear();
            foreach (string p in productlist)  //ลูปเพิ่มตัวของmodelเข้าไปcombobox1
            {
                string model = p.Split('-')[0]; //ตัวแรกเก็บiphone 13-blue แยกขีด
                if (!comboBox1.Items.Contains(model)) //เช็คว่าสินค้ามีรึยัง มีซ้ำไหม ถ้ายังไม่มีเพิ่มเข้าไป
                {
                    comboBox1.Items.Add(model);
                }
            }
            if (cat == "iPhone") //เปลี่ยนข้อความข้างบน
            {
                label1.Text = "Apple iPhone";
            }
            else if (cat == "Android") 
            {
                label1.Text = "Android";
            }
            else if (cat == "Accessories")
            {
                label1.Text = "Accessories";
                label6.Visible = false; //ซ่อนความจุ
                comboBox3.Visible = false; 
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //เลือกรุ่น สีจะขึ้นตามรุ่นนั้นๆ
        {
            string model = comboBox1.Text;
            string[] colorlist = getfrommodel(model, false, "");
            clearBox();
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox2.Items.Clear();
            foreach (string c in colorlist)
            {
                if (!comboBox2.Items.Contains(c))
                {
                    comboBox2.Items.Add(c);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) //เลือกสี ความจุจะขึ้นตามรุ่นนั้นๆ
        {
            //ประกาศตัวแปรเก็บค่าของcombobox
            string model = comboBox1.Text;
            string color = comboBox2.Text;
            string[] gblist = getfrommodel(model, true, color);
            clearBox();
            comboBox3.Text = "";
            comboBox3.Items.Clear();
            foreach (string c in gblist) //ลูปเพิ่มข้อมูลความจุเข้าไปในcombobox3
            {
                if (!comboBox3.Items.Contains(c))
                {
                    comboBox3.Items.Add(c);
                }
            }
            if (cat == "Accessories") //ถ้าเป็น Accessories จะไปทำฟังก์ชัน set
            {
                set();
            }
        }

        public void set() 
        {
            //ประกาศตัวแปรเก็บค่าของcombobox
            string model = comboBox1.Text;
            string color = comboBox2.Text;
            string gb = comboBox3.Text;
            int quantity = Stock.quan(model, gb, color); //เรียกจำนวนของstock 
            textBox2.Text = quantity.ToString();
            if (quantity > 0) //ถ้าจำนวนของในstockมากกว่า0
            {
                numericUpDown1.Minimum = 1; //ค่าต่ำสุด=1
                numericUpDown1.Maximum = quantity; //ค่าสูงสุด=จำนวนสินค้าในstock
                numericUpDown1.Value = 1; //เซ็ตค่าปัจจุบันเป็น1
                button11.Enabled = true; 
                button6.Enabled = true;
                //ปุ่มจะกดได้
            }
            else //ถ้าจำนวนของในstockน้อยกว่าเท่ากับ0
            {
                numericUpDown1.Maximum = 0; //ค่าต่ำสุด=0
                numericUpDown1.Minimum = 0; //ค่าสูงสุด=จำนวน0
                numericUpDown1.Value = 0; //เซ็ตค่าปัจจุบันเป็น0
                button11.Enabled = false;
                button6.Enabled = false;
                //ปุ่มจะกดไม่ได้
            }
            byte[] img = stock.image(getdata(model, gb, color)[2]); //ดึงรูปมาเก็บไว้ในตัวแปร img
            string detail = getdata(model, gb, color)[1]; //ประกาศตัวแปรเก็บรายละเอียดสินค้า
            string price = getdata(model, gb, color)[0];  //ประกาศตัวแปรเก็บราคาสินค้า

            MemoryStream ms = new MemoryStream(img); //รูปที่เก็บไว้ในตัวแปรimgมาเก็บไว้ที่pictureBox
            pictureBox1.Image = Image.FromStream(ms);

            Address.Text = detail; //รายละเอียดไปโชว์ที่textbox...

            textBox1.Text = (Convert.ToInt32(price) * numericUpDown1.Value).ToString(); // textBox1แสดงราคารวมของสินค้า
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) //หลังจากที่เลือกความจุก็จะทำฟังก์ชันset
        {
            set();
        }
    }
}
