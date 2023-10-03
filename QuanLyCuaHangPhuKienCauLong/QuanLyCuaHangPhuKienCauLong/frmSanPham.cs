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
    public partial class frmSanPham : Form
    {
        public frmSanPham()
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
            txtMaSanPham.Text = "";
            txtTenSanPham.Text = "";
            txtXuatXu.Text = "";
            txtMoTa.Text = "";
            txtGiaBan.Text = "";
            txtAnh.Text = "";
            updowSL.Value = 0;
            cmbTimkiem.Text = "";
            pbAnhSanPham.Image = null;
            btnLuu.Enabled = false;
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            fill_combobox();
        }
        private void loaDataGridView()
        {
            try
            {
                string query = "select * from SanPham ";
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].HeaderText = "Mã sản phẩm";
                dataGridView1.Columns[1].HeaderText = "Tên sản phẩm";
                dataGridView1.Columns[2].HeaderText = "Xuất xứ";
                dataGridView1.Columns[3].HeaderText = "Mô tả";
                dataGridView1.Columns[4].HeaderText = "Giá bán";
                dataGridView1.Columns[5].HeaderText = "Số lượng";
                dataGridView1.Columns[6].HeaderText = "Ảnh";
                dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                cmbTimkiem.Items.Clear();
                string query = "select TenSP from SanPham";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmbTimkiem.Items.Add(reader["TenSP"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
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
            string tiento = "SP";
            string MaNCC = tiento + d + t;
            txtMaSanPham.Text = MaNCC;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            loaDataGridView();
            fill_combobox();
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = string.Format("insert into SanPham values ('{0}',N'{1}',N'{2}',N'{3}','{4}','{5}',N'{6}')",
                   txtMaSanPham.Text, txtTenSanPham.Text, txtXuatXu.Text, txtMoTa.Text, txtGiaBan.Text.Replace(",", "."), updowSL.Value, txtAnh.Text);
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
                MessageBox.Show("Lỗi: " + ex.Message);                
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lammoi();
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtMaSanPham.Text = row.Cells["MaSP"].Value.ToString();
                txtTenSanPham.Text = row.Cells["TenSP"].Value.ToString();
                txtXuatXu.Text = row.Cells["XuatXu"].Value.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value.ToString();
                txtGiaBan.Text = row.Cells["GiaBan"].Value.ToString();
                txtAnh.Text = row.Cells["Anh"].Value.ToString();
                try
                {
                    if (!string.IsNullOrEmpty(txtAnh.Text))
                    {
                        string imagePath = txtAnh.Text;
                        pbAnhSanPham.Image = Image.FromFile(imagePath);
                        pbAnhSanPham.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi đọc ảnh do sai đường dẫn ảnh: " + ex.Message);
                }
                updowSL.Text = dataGridView1[5, dataGridView1.CurrentRow.Index].Value.ToString();
                btnThem.Enabled = false;
                btnXoa.Enabled = true;
                btnSua.Enabled = true;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = string.Format("update SanPham set TenSP=N'{1}', XuatXu=N'{2}',MoTa=N'{3}',GiaBan='{4}',SoLuong='{5}',Anh=N'{6}' where  MaSP='{0}'",
                    txtMaSanPham.Text, txtTenSanPham.Text, txtXuatXu.Text, txtMoTa.Text, txtGiaBan.Text.Replace(",", "."), updowSL.Value, txtAnh.Text);
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

        private void btnLammoi_Click(object sender, EventArgs e)
        {
            lammoi();
            loaDataGridView();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = string.Format("select count(MaSP) from ChiTietHD where masp='{0}'", txtMaSanPham.Text);
                SqlCommand cmd = new SqlCommand(query, conn);
                int SL = (int)cmd.ExecuteScalar();
                conn.Close();
                if (SL > 0)
                {
                    MessageBox.Show("Sản phẩm này đang được bán. Không được phép xóa");
                }
                if (SL <= 0)
                {
                    if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        conn.Open();
                        string query1 = string.Format("delete from SanPham where MaSP='{0}'",
                           txtMaSanPham.Text);
                        SqlCommand cmd1 = new SqlCommand(query1, conn);
                        int rowsAffected = cmd1.ExecuteNonQuery();
                        conn.Close();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa thành công");
                            btnLammoi.PerformClick();
                        }
                    }   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("lỗi" + ex);
            }
        }

        private void btnTimkiem_Click(object sender, EventArgs e)
        {
            try
            {
                string query = string.Format("select * from SanPham where TenSP like N'%{0}%' ", cmbTimkiem.Text);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi" + ex.Message);
            }
            
        }

        private void btnMoAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*";
            dlgOpen.FilterIndex = 2;
            dlgOpen.Title = "Chọn ảnh minh hoạ cho sản phẩm";
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                txtAnh.Text = dlgOpen.FileName; // Gán đường dẫn với Encoding UTF-8
                pbAnhSanPham.Image = Image.FromFile(dlgOpen.FileName); // Hiển thị ảnh
                pbAnhSanPham.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
