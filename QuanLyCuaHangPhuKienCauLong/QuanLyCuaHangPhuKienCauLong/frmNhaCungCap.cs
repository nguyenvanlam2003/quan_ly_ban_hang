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
    public partial class frmNhaCungCap : Form
    {
        public frmNhaCungCap()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-14GA20T\SQLEXPRESS;Initial Catalog=DoAn_C#;Integrated Security=True");    
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
        private void lammoi()
        {
            txtMaNhaCungCap.Text = "";
            txtTenNhaCungCap.Text = "";
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtSoDienThoai.Text = "";
            cmbTimKiem.Text = "";
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            btnThem.Enabled = true;
            fill_combobox();
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
            string tiento = "NCC";
            string MaNCC = tiento + d + t;
            txtMaNhaCungCap.Text = MaNCC;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
        }
        private void loaDataGridView()
        {
            try
            {
                string query = "select * from NhaCungCap ";
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgvNhaCungCap.DataSource = ds.Tables[0];

                dgvNhaCungCap.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvNhaCungCap.Columns[0].HeaderText = "Mã nhà cung cấp";
                dgvNhaCungCap.Columns[1].HeaderText = "Tên nhà cung cấp";
                dgvNhaCungCap.Columns[2].HeaderText = "Địa chỉ";
                dgvNhaCungCap.Columns[3].HeaderText = "Email";
                dgvNhaCungCap.Columns[4].HeaderText = "Số điện thoại";
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
                cmbTimKiem.Items.Clear();
                string query = "select TenNCC from NhaCungCap";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmbTimKiem.Items.Add(reader["TenNCC"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi:" + ex.Message);
            }
            
        }
        private void frmNhaCungCap_Load(object sender, EventArgs e)
        {
            loaDataGridView();
            lammoi();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("select * from NhaCungCap where TenNCC like N'%{0}%' ", cmbTimKiem.Text);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dgvNhaCungCap.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = string.Format("insert into NhaCungCap values ('{0}',N'{1}',N'{2}','{3}','{4}')",
                   txtMaNhaCungCap.Text, txtTenNhaCungCap.Text, txtDiaChi.Text,  txtEmail.Text, txtSoDienThoai.Text);
                SqlCommand cmd = new SqlCommand(query, conn);
                int r = cmd.ExecuteNonQuery();
                conn.Close();
                if (r > 0)
                {
                    MessageBox.Show("Lưu thành công");
                    btnLammoi.PerformClick();
                }
                else
                {
                    MessageBox.Show("Lưu thất bại");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi " +ex.Message);
            }
        }

        private void dgvNhaCungCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvNhaCungCap.Rows[e.RowIndex];
                txtMaNhaCungCap.Text = row.Cells["MaNCC"].Value.ToString();
                txtTenNhaCungCap.Text = row.Cells["TenNCC"].Value.ToString();
                txtDiaChi.Text = row.Cells["DiaChi"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtSoDienThoai.Text = row.Cells["SDT"].Value.ToString();
                btnThem.Enabled = false;
                btnXoa.Enabled = true;
                btnSua.Enabled = true;
            }
        }
        
        private void btnLammoi_Click(object sender, EventArgs e)
        {
            lammoi();
            loaDataGridView();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    conn.Open();
                    string query = string.Format("delete from NhaCungCap where MaNCC='{0}'", txtMaNhaCungCap.Text);
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa thành công");
                        btnLammoi.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = string.Format("update NhaCungCap set TenNCC=N'{1}', DiaChi=N'{2}',Email=N'{3}',SDT='{4}' where  MaNCC='{0}'",
                    txtMaNhaCungCap.Text, txtTenNhaCungCap.Text, txtDiaChi.Text,  txtEmail.Text, txtSoDienThoai.Text);
                SqlCommand cmd = new SqlCommand(query, conn);
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Sửa thành công");
                    btnLammoi.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
