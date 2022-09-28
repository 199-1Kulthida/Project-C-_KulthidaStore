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
    public partial class Stock : Form
    {
        MySqlConnection con = new MySqlConnection("host=localhost;user=root;password=;database=phoneshop;");

        public Stock()
        {
            InitializeComponent();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e) //ปิดcombobox1ไม่ให้พิมพ์ได้
        {
            e.Handled = true;
        }

        public byte[] image(string fileLocation)
        {
            byte[] image = null;

            if (image_check(fileLocation))
            {
                FileStream stream = new FileStream(fileLocation, FileMode.Open, FileAccess.Read);
                BinaryReader brs = new BinaryReader(stream);
                image = brs.ReadBytes((int)stream.Length);
            }
            return image;
        }

        private bool image_check(string fileLocation)
        {
            if (File.Exists(fileLocation)) //เช็ครูปภาพ รูปที่เลือกว่ามีอยู่ในเครื่อง?
            {
                return true;
            }
            MessageBox.Show("Image not found"); //ถ้าไม่มี
            return false;
        }

        private void cleartext() //เคลียร์textbox combobox
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            comboBox1.Text = "";
            numericUpDown1.Value = 1;
            Address.Clear();
            fileLoc.Clear();
        }

        private void button2_Click(object sender, EventArgs e) //เพิ่มสินค้า
        {
            //ประกาศตัวแปรเพื่อที่จะเอาไว้เก็บค่า
            string cat = comboBox1.Text;
            string model = textBox3.Text;
            string gb = textBox5.Text;
            string color = textBox4.Text;
            string price = textBox1.Text;
            decimal quantity = numericUpDown1.Value;
            try //เช็คสต๊อกว่าในstock
            {
                if (check_stock(model, gb, color) == false) //ถ้าไม่มีก็จะinsertข้อมูลเข้าไปเก็บในstock
                {
                    string fileLocation = fileLoc.Text; 
                    fileLocation = fileLocation.Replace("\\", "\\\\");
                    if (image_check(fileLocation)) //เช็ครูปมีอยู่ในเครื่องไหม
                    {
                        string sql = "INSERT INTO stock (category,model,gb,color,price,quantity,file,image,details) VALUES ('" + cat + "' , '" + model + "' ,'" + gb + "','" + color + "','" + price + "','" + quantity + "','" + fileLocation + "',@images,'" + Address.Text + "')";
                        MySqlCommand cmd = new MySqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@images", image(fileLocation));
                        con.Open();
                        int rows = cmd.ExecuteNonQuery();
                        con.Close();
                        if (rows > 0) //ถ้าเพิ่มสำเร็จ
                        {
                            MessageBox.Show("เพิ่มสินค้าเสร็จเรียบร้อยแล้ว");
                            showEquipment();
                            cleartext();
                        }

                    }
                }
                else 
                {
                    int instock = quan(model, gb, color);  //เช็คสินค้าในstockว่ามีเท่าไร
                    int addquan = Convert.ToInt32(numericUpDown1.Value); //เพิ่มเท่าไร
                    string sql = "UPDATE stock SET quantity = '" + (instock + addquan) + "' WHERE model =  '" + model + "'AND gb ='" + gb + "'AND color ='" + color + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rows > 0) //เมื่อเพิ่มสินค้าเรียบร้อย
                    {
                        MessageBox.Show("แก้ไขข้อมูลสำเร็จ");
                        showEquipment();
                        cleartext();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_goback_Click(object sender, EventArgs e) //ย้อนกลับไปที่หน้าร้านสินค้า
        {
            Loginadmin loginadmin = new Loginadmin();
            loginadmin.Show();
            this.Hide();
        }
        private void showEquipment() //โชว์ข้อมูลสินค้าในdatagridview
        {
            DataSet dt = new DataSet();
            con.Open();

            MySqlCommand cmd;

            cmd = con.CreateCommand();
            cmd.CommandText = "SELECT id, category ,model, gb, color,price,quantity FROM stock";

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dt);
            con.Close();
            datastock.DataSource = dt.Tables[0].DefaultView;
        }

        private bool check_stock(string model,string gb,string color) //เช็คสินค้าในสต๊อก
        {
            string sql = $"SELECT * FROM stock WHERE model = '{model}' AND gb = '{gb}' AND color = '{color}'"; 
            MySqlCommand cmd = new MySqlCommand(sql, con);

            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows) //อ่านค่ามีแถวที่ต้องการ
            {
                con.Close();
                return true;
            }
            con.Close();
            return false;
        }

        private void Stock_Load(object sender, EventArgs e)  //โชว์ข้อมูลในdatagridview
        {
            showEquipment();
        }

        private void deleteBtn_Click(object sender, EventArgs e)  //ลบสินค้า
        {
            int selectedRow = datastock.CurrentCell.RowIndex;
            datastock.CurrentRow.Selected = true;
            string model = datastock.Rows[selectedRow].Cells["model"].FormattedValue.ToString(); //เมื่อกดเลือกช่องในตารางจะนำข้อมูลไปเก็บในตัวแปร
            string gb = datastock.Rows[selectedRow].Cells["gb"].FormattedValue.ToString();
            string color = datastock.Rows[selectedRow].Cells["color"].FormattedValue.ToString();
            DialogResult result = MessageBox.Show("คุณต้องการลบข้อมูลสินค้าหรือไหม", "",MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string sql = "DELETE FROM `stock` WHERE model =  '" + model + "'AND gb ='" + gb + "'AND color ='" + color + "'";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                con.Open();

                int rows = cmd.ExecuteNonQuery();
                con.Close();

                if (rows > 0)
                {
                    MessageBox.Show("ลบข้อมูลเรียบร้อย");
                    showEquipment();
                }
            }
        }
        public static int quan (string model,string gb,string color) //เช็คจำนวนสินค้าในstock
        {
            MySqlConnection con = new MySqlConnection("host=localhost;user=root;password=;database=phoneshop;");
            string sql = "SELECT quantity FROM stock WHERE model =  '" + model + "'AND gb ='" + gb + "'AND color ='" + color + "'";
            MySqlCommand cmd = new MySqlCommand(sql, con);

            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            int amount = 0;
            while (reader.Read()) //นำมาเก็บไว้ในตัวแปรamount
            {
                amount = reader.GetInt32("quantity");
            }
            con.Close();
            return amount;
        }

        private void button1_Click(object sender, EventArgs e) //ไปที่หน้าประวัติการขาย
        {
            history his = new history();
            his.ShowDialog(); 
        }

        private void button4_Click(object sender, EventArgs e)  //ปุ่มเลือกรูป
        {
            OpenFileDialog dialog = new OpenFileDialog(); //หน้าต่างเลือกรูป
            dialog.Filter = "Image Files (*.jpg;*.png;*.gif) |*.jpg;*.png;*.gif"; //เลือกได้เฉพาะไฟล์ jpg;*.png;*.gif

            if (dialog.ShowDialog() == DialogResult.OK) //เมื่อกด ok จะนำรูปภาพเข้ามาใส่
            {
                string fileLocation = dialog.FileName.ToString();
                pictureBox1.ImageLocation = fileLocation; //นำมาโชว์pictureBox
                fileLoc.Text = fileLocation; //โชว์ที่อยู่ไฟล์ของภาพ
            }
        }

        private void datastock_CellClick(object sender, DataGridViewCellEventArgs e)  
        {
            int selectedRow = datastock.CurrentCell.RowIndex;
            string cat = datastock.Rows[selectedRow].Cells["category"].FormattedValue.ToString();
            string model = datastock.Rows[selectedRow].Cells["model"].FormattedValue.ToString();
            string gb = datastock.Rows[selectedRow].Cells["gb"].FormattedValue.ToString();
            string color = datastock.Rows[selectedRow].Cells["color"].FormattedValue.ToString();
            string price = datastock.Rows[selectedRow].Cells["price"].FormattedValue.ToString();
            string quantity = datastock.Rows[selectedRow].Cells["quantity"].FormattedValue.ToString();
            //กำหนดตัวแปรไว้เก็บค่า
            comboBox1.Text = cat;
            textBox3.Text = model;
            textBox5.Text = gb;
            textBox4.Text = color;
            textBox1.Text = price;
            numericUpDown1.Text = quantity;

            try  
            {
                string sql = "SELECT * FROM stock WHERE category = '"+cat+"' AND model = '" + model + "' AND gb = '" + gb + "' AND color = '" + color + "'";
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con); 
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) 
                {
                    fileLoc.Text = reader["file"].ToString(); 
                    byte[] img = (byte[])(reader["image"]); 
                    Address.Text = reader.GetString("details"); 

                    if (img == null) 
                    {
                        pictureBox1.Image = null;
                    }
                    else 
                    {
                        MemoryStream ms = new MemoryStream(img);
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)  //แก้ไขข้อมูล
        {
            int selectedRow = datastock.CurrentCell.RowIndex; 
            int id = Convert.ToInt32(datastock.Rows[selectedRow].Cells["id"].FormattedValue.ToString()); //เก็บค่าที่คลิกเพื่อที่จะไปเก็บในid
            string fileLocation = fileLoc.Text; //เก็บที่อยู่ของไฟล์
            fileLocation = fileLocation.Replace("\\", "\\\\");
            if (image_check(fileLocation))   
            {
                string sql = "UPDATE stock SET category = '" + comboBox1.Text + "', model = '" + textBox3.Text + "', gb = '" + textBox5.Text + "', color = '" + textBox4.Text + "', price = '" + textBox1.Text + "', quantity = '" + numericUpDown1.Value + "', file = '" + fileLocation + "', image = @images , details = '" + Address.Text + "' WHERE id = '" + id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, con); 
                cmd.Parameters.AddWithValue("@images", image(fileLocation));

                con.Open();

                int rows = cmd.ExecuteNonQuery();
                con.Close();

                if (rows > 0)
                {
                    MessageBox.Show("แก้ไขข้อมูลสำเร็จ");
                    showEquipment();
                    cleartext();
                }
            }
        }

        private void keyPress(object sender, KeyPressEventArgs e)  //เช็คที่พิมพ์ว่าเป็นตัวเลขไหม 
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == (char) Keys.Back) //ถ้าเป็นตัวเลขพิมพ์ได้
            {
                e.Handled = false;
            } 
            else   //ถ้าไม่เป็นตัวเลขพิมพ์ไม่ได้
            {
                e.Handled = true;
            }
        }

        private void fileLoc_TextChanged(object sender, EventArgs e) 
        {
            bool enable = true;

            foreach (Control c in groupBox1.Controls)
            {
                if (c is TextBox)
                {
                    if (string.IsNullOrEmpty(((TextBox)c).Text))
                    {
                        enable = false;
                    }
                }
                if (c is ComboBox)
                {
                    if (string.IsNullOrEmpty(((ComboBox)c).Text))
                    {
                        enable = false;
                    }
                }
            }
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    if (string.IsNullOrEmpty(((TextBox)c).Text))
                    {
                        enable = false;
                    }
                }
                if (c is ComboBox)
                {
                    if (string.IsNullOrEmpty(((ComboBox)c).Text))
                    {
                        enable = false;
                    }
                }
            }
            button2.Enabled = enable;
            button3.Enabled = enable;
        }

    }
}