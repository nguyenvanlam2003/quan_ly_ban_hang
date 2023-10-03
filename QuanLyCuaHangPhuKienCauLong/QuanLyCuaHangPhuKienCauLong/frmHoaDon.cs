using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using COMExcel = Microsoft.Office.Interop.Excel;

namespace QuanLyCuaHangPhuKienCauLong
{
    public partial class frmHoaDon : Form
    {
        public frmHoaDon()
        {
            InitializeComponent();
        }
        string MaNV = "";
        public frmHoaDon(string MaNV)
        {
            InitializeComponent();
            this.MaNV = MaNV;
        }
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-14GA20T\SQLEXPRESS;Initial Catalog=DoAn_C#;Integrated Security=True");
        private void loaDataGridView()
        {
            
            string query = "select HoaDon.MaHD,ngayban,manv,MaSP,GiaBan,SoLuong,GiamGia,ThanhTien from chitietHD,HoaDon where HoaDon.MaHD=ChiTietHD.MaHD ";
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[0].HeaderText = "Mã hóa đơn";
            dataGridView1.Columns[1].HeaderText = "Ngày bán";
            dataGridView1.Columns[2].HeaderText = "Mã nhân viên";
            dataGridView1.Columns[3].HeaderText = "Mã sản phẩm";
            dataGridView1.Columns[4].HeaderText = "Giá bán";
            dataGridView1.Columns[5].HeaderText = "Số lượng";
            dataGridView1.Columns[6].HeaderText = "Giảm giá";
            dataGridView1.Columns[7].HeaderText = "Thành tiền";
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            conn.Close();
        }
        private void loadsauluu()
        {
            try
            {
                conn.Open();
                string query = string.Format("select ChiTietHD.MaSP,TenSP,ChiTietHD.GiaBan,ChiTietHD.SoLuong,GiamGia,ThanhTien from ChiTietHD inner join SanPham on ChiTietHD.MaSP = SanPham.MaSP where MaHD='{0}'", txtMaHD.Text);
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[1].HeaderText = "Tên sản phẩm";

                conn.Open();
                string query2 = string.Format("select TongTien from HoaDon where MaHD='{0}'", txtMaHD.Text);
                SqlCommand cmd = new SqlCommand(query2, conn);
                string TongTien = cmd.ExecuteScalar().ToString();
                txtTongTien.Text = TongTien;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi " + ex);
            }
        }
        private void fill_combobox()
        {
            cmbMaSP.Items.Clear();
            string query = "select MaSP from SanPham";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cmbMaSP.Items.Add(reader["MaSP"].ToString());
            }
            conn.Close();
            cmbMaHoaDon.Items.Clear();
            string query1= "select distinct MaHD from ChiTietHD";
            conn.Open();
            SqlCommand cmd1 = new SqlCommand(query1, conn);
            SqlDataReader reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                cmbMaHoaDon.Items.Add(reader1["MaHD"].ToString());
            }
            conn.Close();
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
            string tiento = "HD";
            string MaHD = tiento + d + t;
            txtMaHD.Text = MaHD;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            conn.Open();
            string query = string.Format("insert into HoaDon(MaHD,NgayBan,MaNV) values ('{0}','{1}','{2}')",
                  txtMaHD.Text, dtpNgayBan.Value.ToString("yyyy-MM-dd"), TxtMaNV.Text);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            fill_combobox();
        }

        private void frmHoaDon_Load(object sender, EventArgs e)
        {
            fill_combobox();
            loaDataGridView();
            TxtMaNV.Text = MaNV;
            btnSua.Enabled = false;
            btnXoaSP.Enabled = false;
            btnLuu.Enabled = false;
            btnXoaHD.Enabled = false;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {

                if (cmbMaSP.Text == "")
                {
                    MessageBox.Show("Mã sản phẩm không được để trống");
                }
                else
                {
                    conn.Open();
                    string query = string.Format("select soluong from sanpham where masp='{0}'", cmbMaSP.Text);
                    SqlCommand cmd = new SqlCommand(query, conn);
                    int SL = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (upDowSoLuong.Value > SL)
                    {
                        MessageBox.Show("Số lượng trong kho còn" + SL + " sản phẩm. Vui lòng nhập lại!!!!");
                    }
                    else
                    {
                        conn.Open();
                        string query2 = string.Format("insert into ChiTietHD(MaHD,MaSP,GiaBan,SoLuong,GiamGia) values ('{0}','{1}','{2}','{3}','{4}') ",
                                           txtMaHD.Text, cmbMaSP.Text, txtDonGia.Text, upDowSoLuong.Value, upDowGiamGia.Value);
                        SqlCommand cmd2 = new SqlCommand(query2, conn);
                        int rowsAffected2;
                        rowsAffected2 = cmd2.ExecuteNonQuery();
                        conn.Close();
                        if (rowsAffected2 > 0)
                        {
                            MessageBox.Show("Lưu thành công");
                            loadsauluu();
                            cmbMaSP.Text = "";
                            upDowSoLuong.Value = 0;
                            upDowGiamGia.Value = 0;
                            btnLamMoi.PerformClick();
                        }
                        else
                        {
                            MessageBox.Show("Lưu thất bại!!!");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("loi: " + ex);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int a = dataGridView1.ColumnCount;
            if (a == 8)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    txtMaHD.Text = row.Cells["MaHD"].Value.ToString();
                    TxtMaNV.Text = row.Cells["MaNV"].Value.ToString();
                    dtpNgayBan.Text = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
                    btnThem.Enabled = false;;
                    btnLuu.Enabled = true;
                    btnXoaHD.Enabled = true;
                    loadsauluu();
                }
            }
            if (a == 6)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    txtDonGia.Text = row.Cells["GiaBan"].Value.ToString();
                    txtThanhTien.Text = row.Cells["ThanhTien"].Value.ToString();
                    cmbMaSP.Text = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                    upDowSoLuong.Text = dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
                    upDowGiamGia.Text = dataGridView1[4, dataGridView1.CurrentRow.Index].Value.ToString();
                    btnThem.Enabled = false;
                    btnSua.Enabled = true;
                    btnXoaSP.Enabled = true;
                    btnLuu.Enabled = false;
                    btnXoaHD.Enabled = false;
                    cmbMaSP.Enabled = false;
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                string query = string.Format("select soluong from sanpham where masp='{0}'", cmbMaSP.Text);
                SqlCommand cmd = new SqlCommand(query, conn);
                int SL = (int)cmd.ExecuteScalar();
                conn.Close();
                if (upDowSoLuong.Value > SL)
                {
                    MessageBox.Show("Số lượng trong kho còn" + SL + " sản phẩm. Vui lòng nhập lại!!!!");
                }
                else
                {
                    conn.Open();
                    string query1 = string.Format("update ChiTietHD set SoLuong='{2}', GiamGia='{3}' where  MaHD='{0}'and MaSP='{1}'",
                        txtMaHD.Text, cmbMaSP.Text, upDowSoLuong.Value, upDowGiamGia.Value);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    int rowsAffected = cmd1.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Sửa thành công");
                        loadsauluu();
                        cmbMaSP.Text = "";
                        upDowSoLuong.Value = 0;
                        upDowGiamGia.Value = 0;
                    }
                    else
                    {
                        MessageBox.Show("Sửa thất bại");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi: " + ex);
            }
        }

        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    conn.Open();
                    string query1 = string.Format("delete from ChiTietHD  where  MaHD='{0}'and MaSP='{1}'",
                            txtMaHD.Text, cmbMaSP.Text);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    int rowsAffected = cmd1.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa thành công");
                        loadsauluu();
                        cmbMaSP.Text = "";
                        upDowSoLuong.Value = 0;
                        upDowGiamGia.Value = 0;
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi: " + ex);
            }
        }
        private void lammoi()
        {
            txtMaHD.Text = "";
            txtDonGia.Text = "";
            txtThanhTien.Text = "";
            txtTongTien.Text = "";
            TxtMaNV.Text = MaNV;
            cmbMaSP.Text = "";
            dtpNgayBan.Text = "";
            upDowSoLuong.Value = 0;
            upDowGiamGia.Value = 0;
            cmbMaHoaDon.Text = "";
            fill_combobox();
        }
        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    conn.Open();
                    string query2 = string.Format("delete from ChiTietHD  where  MaHD='{0}'", txtMaHD.Text);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    int rowsAffected2 = cmd2.ExecuteNonQuery();
                    conn.Close();
                    conn.Open();
                    string query1 = string.Format("delete from HoaDon  where  MaHD='{0}'",
                            txtMaHD.Text);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    int rowsAffected = cmd1.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected > 0 && rowsAffected2 > 0)
                    {
                        MessageBox.Show("Xóa hóa đơn thành công");
                        btnXoaHD.Enabled = false;
                        btnThem.Enabled = true;
                        btnLuu.Enabled = false;
                        dataGridView1.Columns.Clear();
                        loaDataGridView();
                        lammoi();
                        fill_combobox();
                    }
                    else
                    {
                        MessageBox.Show("Xóa hóa đơn thất bại");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi: " + ex);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
            dataGridView1.Columns.Clear();
            loaDataGridView();
            btnThem.Enabled = true;
            btnSua.Enabled = false;
            btnXoaSP.Enabled = false;
            btnLuu.Enabled = false;
            btnXoaHD.Enabled = false;
            cmbMaSP.Enabled = true;
        }

        private void cmbMaHoaDon_SelectedIndexChanged(object sender, EventArgs e)
        {
            //fill_combobox();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Columns.Clear();
                string query = string.Format("select HoaDon.MaHD,ngayban,manv,MaSP,GiaBan,SoLuong,GiamGia,ThanhTien from chitietHD,HoaDon where HoaDon.MaHD=ChiTietHD.MaHD and ChiTietHD.MaHD='{0}'", cmbMaHoaDon.Text);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                txtMaHD.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
                dtpNgayBan.Text = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].HeaderText = "Mã hóa đơn";
                dataGridView1.Columns[1].HeaderText = "Ngày bán";
                dataGridView1.Columns[2].HeaderText = "Mã nhân viên";
                dataGridView1.Columns[3].HeaderText = "Mã sản phẩm";
                dataGridView1.Columns[4].HeaderText = "Giá bán";
                dataGridView1.Columns[5].HeaderText = "Số lượng";
                dataGridView1.Columns[6].HeaderText = "Giảm giá";
                dataGridView1.Columns[7].HeaderText = "Thành tiền";
                dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("loi: " + ex);
            }
            
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            // Khởi động chương trình Excel
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;
            int hang = 0, cot = 0;
            DataTable tblThongtinSP, tblThongtinHD;
            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            // Định dạng chung
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:Z300"].Font.Name = "Times new roman"; //Font chữ
            exRange.Range["A1:B3"].Font.Size = 10;
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 5; //Màu xanh da trời
            exRange.Range["A1:A1"].ColumnWidth = 7;
            exRange.Range["B1:B1"].ColumnWidth = 15;
            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "Shop bán phụ kiện cầu lông VNB";
            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "16 Trần Quốc Vượng, phường Dịch Vọng Hậu, quận Cầu Giấy, Hà Nội";
            exRange.Range["A3:B3"].MergeCells = true;
            exRange.Range["A3:B3"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A3:B3"].Value = "Điện thoại: 0788612959";
            exRange.Range["C2:E2"].Font.Size = 16;
            exRange.Range["C2:E2"].Font.Bold = true;
            exRange.Range["C2:E2"].Font.ColorIndex = 3; //Màu đỏ
            exRange.Range["C2:E2"].MergeCells = true;
            exRange.Range["C2:E2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:E2"].Value = "HÓA ĐƠN MUA HÀNG";
            // Biểu diễn thông tin chung của hóa đơn bán
            if (txtMaHD.Text != "")
            {
            KetNoi kn = new KetNoi();
            string query = string.Format("SELECT ChiTietHD.MaHD, NgayBan, TenNV, TongTien FROM ChiTietHD INNER JOIN HoaDon ON ChiTietHD.MaHD = HoaDon.MaHD INNER JOIN NhanVien ON HoaDon.MaNV = NhanVien.MaNV WHERE ChiTietHD.MaHD = '{0}'", txtMaHD.Text);
            DataSet ds = new DataSet();
            ds = kn.LayDuLieu(query);
            tblThongtinHD = ds.Tables[0];
            exRange.Range["B6:C9"].Font.Size = 12;
            exRange.Range["B6:B6"].Value = "Mã hóa đơn:";
            exRange.Range["C6:E6"].MergeCells = true;
            exRange.Range["C6:E6"].Value = tblThongtinHD.Rows[0][0].ToString();
            exRange.Range["B7:B7"].Value = "Ngày bán:";
            exRange.Range["C7:E7"].MergeCells = true;
            exRange.Range["C7:E7"].Value = tblThongtinHD.Rows[0][1].ToString();
            //Lấy thông tin các mặt hàng
                string query2 = string.Format("select TenSP, ChiTietHD.GiaBan, ChiTietHD.SoLuong, GiamGia, ThanhTien from ChiTietHD inner join SanPham on ChiTietHD.MaSP = SanPham.MaSP where MaHD = '{0}'", txtMaHD.Text);
                DataSet ds2 = new DataSet();
                ds2 = kn.LayDuLieu(query2);
                tblThongtinSP = ds2.Tables[0];
                conn.Close();
                //Tạo dòng tiêu đề bảng
                exRange.Range["A11:F11"].Font.Bold = true;
                exRange.Range["A11:F11"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["C11:F11"].ColumnWidth = 12;
                exRange.Range["A11:A11"].Value = "STT";
                exRange.Range["B11:B11"].Value = "Tên sản phẩm";
                exRange.Range["C11:C11"].Value = "Giá bán";
                exRange.Range["D11:D11"].Value = "Số lượng";
                exRange.Range["E11:E11"].Value = "Giảm giá";
                exRange.Range["F11:F11"].Value = "Thành tiền";
                for (hang = 0; hang < tblThongtinSP.Rows.Count; hang++)
                {
                    //Điền số thứ tự vào cột 1 từ dòng 12
                    exSheet.Cells[1][hang + 12] = hang + 1;
                    for (cot = 0; cot < tblThongtinSP.Columns.Count; cot++)
                    //Điền thông tin hàng từ cột thứ 2, dòng 12
                    {
                        exSheet.Cells[cot + 2][hang + 12] = tblThongtinSP.Rows[hang][cot].ToString();
                        if (cot == 3) exSheet.Cells[cot + 2][hang + 12] = tblThongtinSP.Rows[hang][cot].ToString() + "%";
                    }
                }

                // Đẩy cột "Tổng tiền" sang phải 2 cột
                exRange = exSheet.Cells[cot + 2][hang + 14];
                exRange.Font.Bold = true;
                exRange.Value2 = "Tổng tiền:";
                // Đặt giá trị tổng tiền
                exRange = exSheet.Cells[cot + 3][hang + 14];
                exRange.Font.Bold = true;
                exRange.Value2 = tblThongtinHD.Rows[0][3].ToString();

                // Đẩy cột "Nhân viên bán hàng" sang phải 2 cột
                exRange = exSheet.Cells[cot + 2][hang + 16];
                exRange.Range["A1:C1"].MergeCells = true;
                exRange.Range["A1:C1"].Font.Bold = true;
                exRange.Range["A1:C1"].Font.Italic = true;
                exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A1:C1"].Value = "Nhân viên bán hàng";
                exRange.Range["A6:C6"].MergeCells = true;
                exRange.Range["A6:C6"].Font.Italic = true;
                exRange.Range["A6:C6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A6:C6"].Value = tblThongtinHD.Rows[0][2];

                exSheet.Name = "Hóa đơn mua hàng";
                exApp.Visible = true;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần in");
            }

        }
    }
}