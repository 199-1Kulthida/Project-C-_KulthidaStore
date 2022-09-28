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
    public partial class receipt : Form
    {
        public receipt()
        {
            InitializeComponent();
        }

        private void receipt_Load(object sender, EventArgs e)
        { 
            try
            {
                connectionmysql con = new connectionmysql();
                string sql = "SELECT  model,gb,color,quantity,price FROM basket WHERE username = '" + Login.user + "'AND idorder = '" + basket.idoer + "'"; 
                DataTable dtGrid = con.GetDataTable(sql);
                dataGridView1.DataSource = dtGrid;
                
                int sum = 0; 
                for (int i = 0; i < dtGrid.Rows.Count; i++) //ลูปเพื่อหาราคารวมทั้งหมด
                {
                    sum += Convert.ToInt32(dtGrid.Rows[i]["price"]);
                }

                sql = "SELECT * FROM `member` WHERE USERNAME='" + Login.user + "'"; //select member เพื่อที่จะมาโชว์ใน textbox ใบเสร็จ
                DataTable dtmember = con.GetDataTable(sql);
                lblNAme.Text = dtmember.Rows[0]["fname"].ToString();
                lblTel.Text = dtmember.Rows[0]["Phone"].ToString();
                lbladd.Text = dtmember.Rows[0]["Address"].ToString();
                lblpro.Text = dtmember.Rows[0]["province"].ToString();
                lblpost.Text = dtmember.Rows[0]["postcode"].ToString();
                lbltime.Text = basket.datetime2.ToString();
                lblSum.Text = sum.ToString();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void receipt_FormClosed(object sender, FormClosedEventArgs e) //ปิดหน้าformก็จะไปที่หน้า shop
        {
            Shop b1 = new Shop();
            b1.Show();
        }
    }
}
