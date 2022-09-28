using MySql.Data.MySqlClient;
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
    public partial class basket : Form
    {

        public static string datetime2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
        public static int idoer = 0;
        public static int ppp;
        public basket()
        {
            InitializeComponent();
        }

        private void button_goback_Click(object sender, EventArgs e) //ย้อนกลับเลือกสินค้า
        {
                apple backshop = new apple();
                backshop.Show();
                this.Hide();
        }

        public static void lastID () //หาidorderล่าสุดว่าเป็นเลขอะไร
        {
            MySqlConnection con = new MySqlConnection("host=localhost;user=root;password=;database=phoneshop;");
            string sql = "SELECT * FROM basket";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            con.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            List<int> list = new List<int>();
            while (dr.Read())
            {
                list.Add(dr.GetInt32("idorder"));
            }
            list = list.OrderBy(x => x).ToList();
            int length = list.Count;
            int newID = list[length - 1]+1;
            con.Close();
            idoer = newID; //เอาไปเก็บในตัวแปรidorder
        }

        private void button2_Click(object sender, EventArgs e) //ปุ่มชำระเงิน
        {
            receipt re = new receipt(); //ไปหน้าใบเสร็จ
            re.Show();
            this.Close(); //ปิดหน้าตระกร้า

            connectionmysql con = new connectionmysql(); //นำข้อมูลusernameและidorder มาแสดงบนdatagide
            string sql = "SELECT  * FROM basket WHERE username = '" + Login.user + "' AND idorder = '"+ idoer +"'" ; //เลือกข้อมูลในตระกร้าที่สั่ง
            DataTable dtGrid = con.GetDataTable(sql);

            sql = "SELECT * FROM `member` WHERE USErNAME='" + Login.user + "'"; //นำข้อมูลจากmemberมาแสดงบนdatagide
            DataTable dtmember = con.GetDataTable(sql);

            for (int i = 0; i < dtGrid.Rows.Count; i++) //จะforเพื่อจะinsertข้อมูลตามที่จำนวนสินค้าที่เราสั่งไป
            {
                sql = "INSERT INTO `history`(`member`, `username`, `phone`, `model`, `gb`, `color`, `quantity`, `price`, `total`, `idorder`) VALUES (" +
                    "'" + dtmember.Rows[0]["fname"].ToString() + "'," +
                    "'" + Login.user + "'," +
                    "'" + dtmember.Rows[0]["phone"].ToString() + "'," +
                    "'" + dtGrid.Rows[i]["model"].ToString() + "'," +
                    "'" + dtGrid.Rows[i]["GB"].ToString() + "'," +
                    "'" + dtGrid.Rows[i]["color"].ToString() + "'," +
                    "'" + dtGrid.Rows[i]["quantity"].ToString() + "'," +
                    "'" + dtGrid.Rows[i]["price"].ToString() + "'," +
                    "'" + Convert.ToInt32(dtGrid.Rows[i]["price"]) + "'," +
                    "'" + idoer + "')";
                con.ExecuteNonQuery(sql);
            }
            sql = "INSERT INTO `order` (`idorder`, `datetime`) VALUES ('"+idoer+"', '"+datetime2+"')"; //insert order สั่งสินค้าไปกี่โมง
            con.ExecuteNonQuery(sql);
            lastID();
        }

        private void basket_Shown(object sender, EventArgs e) //โชว์ของในตะกร้า
        {
            MySqlConnection conn = new MySqlConnection("host=localhost;user=root;password=;database=phoneshop;");
          
            DataSet ds = new DataSet();
            conn.Open();

            MySqlCommand cmd;

            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM basket WHERE username = '" + Login.user + "' AND idorder = '" + idoer + "'";//เลือกข้อมูลในฐานข้อมูลตารางbasket เช็คจากusername กับ idorder

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(ds);

            conn.Close();
            dataGridView1.DataSource = ds.Tables[0].DefaultView; 
            dataGridView1.Columns[0].Visible = false;
        }
        private void button5_Click(object sender, EventArgs e) //ปุ่มลบสินค้า
        { //try catch เพื่อดักจับ error
            try
            {
                if (MessageBox.Show("คุณต้องการลบข้อมูลใช่หรือไม่ ?", "ยืนยันการลบข้อมูล", MessageBoxButtons.YesNo,
            MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (dataGridView1.CurrentCell != null)
                    {
                        int selectedRow = dataGridView1.CurrentCell.RowIndex;
                        connectionmysql con = new connectionmysql();
                        string model = dataGridView1.Rows[selectedRow].Cells["model"].FormattedValue.ToString();
                        string gb = dataGridView1.Rows[selectedRow].Cells["GB"].FormattedValue.ToString();
                        string color = dataGridView1.Rows[selectedRow].Cells["color"].FormattedValue.ToString();
                        int quantity = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells["quantity"].FormattedValue.ToString());
                        int amount = Stock.quan(model, gb, color); //อัปเดตสินค้าในตะกร้า
                        string sql = "UPDATE stock SET quantity = '" + (amount + quantity) + "' WHERE model =  '" + model + "'AND gb ='" + gb + "'AND color ='" + color + "'";
                        con.ExecuteNonQuery(sql);

                        string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString(); //คืนสินค้าเข้าตะกร้า
                        sql = "DELETE FROM `basket` WHERE ID='" + id + "'";
                        con.ExecuteNonQuery(sql);
                        MessageBox.Show("ลบสำเร็จ");

                        sql = "SELECT * FROM basket WHERE username = '" + Login.user + "' AND idorder = '" + idoer + "'";
                        dataGridView1.DataSource = con.GetDataTable(sql);

                    } else
                    {
                        MessageBox.Show("เลือกสินค้าที่ท่านต้องการลบ");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //เป็นการเลือกทั้งแถวของตาราง
        {
            dataGridView1.CurrentRow.Selected = true;
        }


    }
}
