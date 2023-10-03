using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace QuanLyCuaHangPhuKienCauLong
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-14GA20T\SQLEXPRESS;Initial Catalog=DoAn_C#;Integrated Security=True");
      

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDangNhap_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            string str = string.Format("select Username,Matkhau,MaQuyen,MaNV from TaiKhoan where Username='{0}' and Matkhau='{1}'",
                txtTaiKhoan.Text, txtMatKhau.Text);
            SqlDataAdapter da = new SqlDataAdapter(str, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                MessageBox.Show("Đăng nhập thành công!");
                this.Hide();
                frmMenu f = new frmMenu(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString(), dt.Rows[0][3].ToString());
                f.ShowDialog();
                this.Show();
                txtTaiKhoan.Text = "";
                txtMatKhau.Text = "";
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại!");
            }
            conn.Close();
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {

        }

    }
}
