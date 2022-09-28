using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PhoneShop
{
    public partial class history : Form
    {
        MySqlConnection con = Login.con;
        public history()
        {
            InitializeComponent();
        }
        private DataTable dt = null;

        private int[] idlist () //ฟังก์ชันการดึง idorder ของวันที่เราต้องการดูยอดขายรายวัน
        {
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd"); //ประกาศตัวแปรdateเพื่อที่จะเก็บค่าจากdatetimepicker

            string sql = "SELECT * FROM `order` WHERE `datetime` LIKE '%" + date + "%' "; //selectจากorderโดยเช็คจากวัน/เวลา
            MySqlCommand cmd = new MySqlCommand(sql, con);
            con.Open();
            List<int> list = new List<int>();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())      //วนลูปดึงค่า id มาเก็บไว้ที่ตัวแปร list
            {
                list.Add(reader.GetInt32(0));
            }
            con.Close();
            return list.Distinct().ToArray(); //returnตัวแปรlistกลับไป แล้วลบตัวที่ซ้ำด้วย Distinct
        }

        private void show() //showประวัติที่ต้องการดู
        {
            string sql = "SELECT * FROM `history` "; 
            if (checkBox1.Checked == true) //เช็คประวัติจากusernameหรือชื่อจริง
            {
                if (!string.IsNullOrEmpty(textBox1.Text)) //ถ้าติ๊กดูทั้งหมดก็จะแสดงชื่อที่ต้องการจะดู
                {

                    sql += " WHERE username LIKE '%" + textBox1.Text + "%' OR member LIKE '%" + textBox1.Text + "%'"; //แสดงแค่รายชื่อที่ต้องการดู
                }
            }
            else //ถ้าไม่ได้ติ๊กเช็คในวันนั้นทั้งหมด
            {
                if (string.IsNullOrEmpty(textBox1.Text)) //เช็คtextboxว่าง
                {
                    sql += " WHERE idorder IN ( ";

                    for (int i = 0; i < idlist().Length; i++) //ลูปจากidlistที่เราประกาศฟังก์ชันไว้ข้างบน
                    {
                        if (i == idlist().Length - 1) 
                        {
                            sql += "" + idlist()[i] + "";
                        }
                        else
                        {
                            sql += "" + idlist()[i] + ", ";
                        }
                    }
                    sql += " ) ";
                }
                else if (!string.IsNullOrEmpty(textBox1.Text) && checkBox1.Checked == false) //เช็คจากชื่อและวัน
                {
                    sql += " WHERE (username LIKE '%" + textBox1.Text + "%' OR member LIKE '%" + textBox1.Text + "%') AND idorder IN ( ";

                    for (int i = 0; i < idlist().Length; i++) 
                    {
                        if (i == idlist().Length - 1)
                        {
                            sql += "" + idlist()[i] + "";
                        }
                        else
                        {
                            sql += "" + idlist()[i] + ", ";
                        }
                    }
                    sql += " ) ";
                }
            }
            
            connectionmysql con = new connectionmysql(); 
            dt = con.GetDataTable(sql);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e) //ปุ่มค้นหา
        {
            show();
        }

        private void history_Load(object sender, EventArgs e) //เป็นฟังก์ชันการทำงานของหน้าประวัติลูกค้าโชว์ขึ้นมา
        {
            connectionmysql con = new connectionmysql();
            string sql = "SELECT  * FROM history "; //โชว์ประวัติทั้งหมดในตาราง history
            dt = con.GetDataTable(sql);
            dataGridView1.DataSource = dt; 
        }

        private void button2_Click(object sender, EventArgs e) //คำนวณยอดขาย
        {
            if (dt != null) //ถ้าตารางมีข้อมุลจะคำนวณยอดขายให้ 
            {
                int sum = 0;
                for (int i = 0; i < dt.Rows.Count; i++) //ลูปตามจำนวนแถวที่มี
                {
                    sum += Convert.ToInt32(dt.Rows[i]["total"]);//บวกค่าเข้าไปเรื่อยๆ จนครบ
                }
                textBox2.Text = sum.ToString(); //เอาไปใส่ใน
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) //ฟังก์ชันเปลี่ยนวันที่ ก็จะโชว์วันที่เราเปลี่ยน
        {
            show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //ฟังก์ชั้นติ๊กทั้งหมดไม่ให้กดวันที่
        {
            if (checkBox1.Checked)
            {
                dateTimePicker1.Enabled = false; //ถ้าไม่ติ๊กปฏิทินก็จะกดไม่ได้
                show();
            }
            else
            {
                dateTimePicker1.Enabled = true; //ถ้าติ๊กปฏิทินก็จะกดได้
                show();
            }
        }

    }
}
