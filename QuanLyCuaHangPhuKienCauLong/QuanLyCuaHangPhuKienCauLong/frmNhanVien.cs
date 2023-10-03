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
    public partial class frmNhanVien : Form
    {
        public frmNhanVien()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-14GA20T\SQLEXPRESS;Initial Catalog=DoAn_C#;Integrated Security=True");
        private void loaDataGridView()
        {
            try
            {
                string query = "select nhanvien.manv ,tennv , goitinh, ngaysinh, diachi, email, sdt, maquyen from nhanvien, TaiKhoan where taikhoan.manv=nhanvien.manv";
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgvNhanVien.DataSource = ds.Tables[0];

                dgvNhanVien.Columns[0].HeaderText = "Mã nhân viên";
                dgvNhanVien.Columns[1].HeaderText = "Tên nhân viên";
                dgvNhanVien.Columns[2].HeaderText = "Giới tính";
                dgvNhanVien.Columns[3].HeaderText = "Ngày sinh";
                dgvNhanVien.Columns[4].HeaderText = "Địa chỉ";
                dgvNhanVien.Columns[5].HeaderText = "Email";
                dgvNhanVien.Columns[6].HeaderText = "Số điện thoại";
                dgvNhanVien.Columns[7].HeaderText = "Mã quyền";
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void fill_combobox()
        {
            try
            {
                cmbQuyen.Items.Clear();
                string query = "select MaQuyen from Quyen";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmbQuyen.Items.Add(reader["MaQuyen"].ToString());
                }
                conn.Close();

                cmbTenNV.Items.Clear();
                string query1 = "select TenNV from NhanVien";
                conn.Open();
                SqlCommand cmd1 = new SqlCommand(query1, conn);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    cmbTenNV.Items.Add(reader1["TenNV"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            loaDataGridView();
            fill_combobox();
            cmbQuyen.SelectedIndex = 0;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
        }
        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }

       
        private void btnThem_Click(object sender, EventArgs e)
        {
            string[] partsDay = DateTime.Now.ToShortDateString().Split('/');
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            if (partsTime[2].Substring(3, 2) == "CH")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "SA")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            string tiento = "NV";
            string MaNV = tiento + d + t;
            txtMaNhanVien.Text = MaNV;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void lammoi()
        {
            txtMaNhanVien.Text = "";
            txtTenNhanVien.Text = "";
            txtDiaChi.Text ="";
            txtEmail.Text ="";
            txtSDT.Text = "";
            dtpNgaySinh.Text = "";
            cmbGioiTinh.Text = "";
            cmbQuyen.SelectedIndex =0;
            txttaiKhoan.Text = "";
            txtMatKhau.Text = "";
            cmbTenNV.Text = "";
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            txttaiKhoan.Enabled = true;
            txtMatKhau.Enabled = true;
            cmbQuyen.Enabled = true;
            fill_combobox();
        }
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];
                txtMaNhanVien.Text = row.Cells["MaNV"].Value.ToString();
                txtTenNhanVien.Text = row.Cells["TenNV"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtSDT.Text = row.Cells["SDT"].Value.ToString();
                dtpNgaySinh.Text = dgvNhanVien[3, dgvNhanVien.CurrentRow.Index].Value.ToString();
                cmbGioiTinh.Text = dgvNhanVien[2, dgvNhanVien.CurrentRow.Index].Value.ToString();
                cmbQuyen.Text = dgvNhanVien[7, dgvNhanVien.CurrentRow.Index].Value.ToString();
                btnThem.Enabled = false;
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                txttaiKhoan.Enabled = false;
                txtMatKhau.Enabled = false;
                cmbQuyen.Enabled = false;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = string.Format("update nhanvien set TenNV=N'{1}', DiaChi=N'{2}',GoiTinh=N'{3}',Email=N'{4}', NgaySinh='{5}',SDT='{6}' where  MaNV='{0}'",
                    txtMaNhanVien.Text, txtTenNhanVien.Text, txtDiaChi.Text, cmbGioiTinh.Text, txtEmail.Text, dtpNgaySinh.Value.ToString("yyyy-MM-dd"), txtSDT.Text);
                SqlCommand cmd = new SqlCommand(query, conn);
                int rowsAffected = cmd.ExecuteNonQuery();
                string query2 = string.Format("update taikhoan set MaQuyen='{1}' where  MaNV='{0}'",
                    txtMaNhanVien.Text, cmbQuyen.Text);
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                int rowsAffected2 = cmd2.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0 && rowsAffected2> 0)
                {
                    MessageBox.Show("Sửa thành công");
                    btnLamMoi.PerformClick();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    conn.Open();
                    string query = string.Format("delete from TaiKhoan where MaNV='{0}'",
                       txtMaNhanVien.Text);
                    SqlCommand cmd = new SqlCommand(query, conn);
                    string query2 = string.Format("delete from NhanVien where MaNV='{0}'",
                       txtMaNhanVien.Text);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    int rowsAffected, rowsAffected2;
                    rowsAffected = cmd.ExecuteNonQuery();
                    rowsAffected2 = cmd2.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected > 0 && rowsAffected2 > 0)
                    {
                        MessageBox.Show("Xóa thành công");
                        btnLamMoi.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try {
             conn.Open();
                string query = string.Format("insert into NhanVien values ('{0}',N'{1}',N'{2}','{3}',N'{4}', '{5}','{6}')",
                   txtMaNhanVien.Text, txtTenNhanVien.Text, cmbGioiTinh.Text, dtpNgaySinh.Value.ToString("yyyy-MM-dd"), txtDiaChi.Text, txtEmail.Text, txtSDT.Text);
                SqlCommand cmd = new SqlCommand(query, conn);
                string query2 = string.Format("insert into TaiKhoan values ('{0}','{1}','{2}','{3}')",
                   txtMaNhanVien.Text, txttaiKhoan.Text, txtMatKhau.Text, cmbQuyen.Text);
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                if (txtMatKhau.Text != "" && txttaiKhoan.Text != "" && cmbQuyen.Text!="")
                {
                    int rowsAffected, rowsAffected2;
                    rowsAffected = cmd.ExecuteNonQuery();
                    rowsAffected2 = cmd2.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected > 0 && rowsAffected2 > 0)
                    {
                        MessageBox.Show("Lưu thành công");
                        btnLamMoi.PerformClick();
                    }
                }
                else
                {
                    MessageBox.Show("Không được bỏ trống tài khoản, mật khẩu, mã quyền !!!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnHienThi_Click(object sender, EventArgs e)
        {
            loaDataGridView();
            lammoi();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = string.Format("select nhanvien.manv ,tennv ,goitinh,ngaysinh,diachi,email, sdt,maquyen from nhanvien, TaiKhoan where taikhoan.manv=nhanvien.manv and tennv like N'%{0}%'", cmbTenNV.Text);
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgvNhanVien.DataSource = ds.Tables[0];
                conn.Close();
                btnThem.Enabled = false;
                btnLuu.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void cmbQuyen_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
