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
    public partial class frmPhieuNhap : Form
    {
        public frmPhieuNhap()
        {
            InitializeComponent();
        }
        string MaNV = "";
        public frmPhieuNhap(string MaNV)
        {

            InitializeComponent();
            this.MaNV = MaNV;
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

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-14GA20T\SQLEXPRESS;Initial Catalog=DoAn_C#;Integrated Security=True");
        private void loaDataGridView()
        {
            try
            {
                string query = "select distinct phieunhap.MaPN,MaNCC,MaNV,NgayNhap,TongTien from phieunhap,ChiTietPN where phieunhap.MaPN=ChiTietPN.MaPN";
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].HeaderText = "Mã phiếu nhập";
                dataGridView1.Columns[1].HeaderText = "Mã nhà cung cấp";
                dataGridView1.Columns[2].HeaderText = "Mã nhân viên";
 //               dataGridView1.Columns[3].HeaderText = "Mã sản phẩm";
   //             dataGridView1.Columns[4].HeaderText = "Giá nhập";
    //            dataGridView1.Columns[5].HeaderText = "Số lượng";
               dataGridView1.Columns[3].HeaderText = "Ngày nhập";
                dataGridView1.Columns[4].HeaderText = "Tổng tiền";
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
                cmbNCC.Items.Clear();
                string query1 = "select MaNCC from NhaCungCap";
                conn.Open();
                SqlCommand cmd1 = new SqlCommand(query1, conn);
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    cmbNCC.Items.Add(reader1["MaNCC"].ToString());
                }
                conn.Close();

                cmbMaPhieuNhap.Items.Clear();
                string query2 = "select distinct MaPN from ChiTietPN";
                conn.Open();
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                SqlDataReader reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    cmbMaPhieuNhap.Items.Add(reader2["MaPN"].ToString());
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void loadsauluu()
        {
            try
            {
                conn.Open();
                string query = string.Format("select ChiTietPN.MaSP, TenSP, GiaNhap, ChiTietPN.SoLuong, NgayNhap, thanhtien from ChiTietPN  inner join SanPham on ChiTietPN.MaSP = SanPham.MaSP where MaPN='{0}' ", txtMaPhieuNhap.Text);
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[1].HeaderText = "Tên sản phẩm";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                conn.Close();

                conn.Open();
                string query2 = string.Format("select TongTien from PhieuNhap where MaPN='{0}'", txtMaPhieuNhap.Text);
                SqlCommand cmd = new SqlCommand(query2, conn);
              //  string TongTien = cmd.ExecuteScalar().ToString();
            //    txtTongTien.Text = TongTien;
                object result = cmd.ExecuteScalar().ToString();
                string TongTien = result.ToString();
                double tongTien;
                if (double.TryParse(TongTien, out tongTien))
                {
                    txtTongTien.Text = TongTien;
                    lblTongTien.Text = "Bằng chữ: " + NumberToWordsConverter.ChuyenSoSangChuoi(tongTien);
                }
                else
                {
                    txtTongTien.Text = "";
                    lblTongTien.Text = "";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi:  " + ex.Message);
            }
        }
        private void frmPhieuNhap_Load(object sender, EventArgs e)
        {
            loaDataGridView();
            fill_combobox();
            txtMaNhanVien.Text = MaNV;
            btnXoaMH.Enabled = false;
            btnXoaPhieuNhap.Enabled = false;
            btnSuaPhieuNhap.Enabled = false;
            btnLuu.Enabled = false;
        }

        private void btnThemPhieuNhap_Click(object sender, EventArgs e)
        {
            if (cmbNCC.Text != "")
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
                string tiento = "PN";
                string MaPN = tiento + d + t;
                txtMaPhieuNhap.Text = MaPN;
                btnThemPhieuNhap.Enabled = false;
                btnLuu.Enabled = true;
                try
                {
                    conn.Open();
                    string query = string.Format("insert into PhieuNhap(MaPN,MaNCC,MaNV) values ('{0}','{1}','{2}')",
                          txtMaPhieuNhap.Text, cmbNCC.Text, txtMaNhanVien.Text);
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    fill_combobox();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Mã nhà cung cấp không được bỏ trống!!!");
            }

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

                    if (upDowSL.Value < 0)
                    {
                        MessageBox.Show("Số lượng phải lớn hơn 0. Vui lòng nhập lại!!!!");
                    }
                    else
                    {
                        conn.Open();
                        string query2 = string.Format("insert into ChiTietPN(MaPN,MaSP,GiaNhap,SoLuong,NgayNhap) values ('{0}','{1}','{2}','{3}','{4}') ",
                                           txtMaPhieuNhap.Text, cmbMaSP.Text, txtGiaNhap.Text.Replace(",", "."), upDowSL.Value, dtpNgayNhap.Value.ToString("yyyy-MM-dd"));
                        SqlCommand cmd2 = new SqlCommand(query2, conn);
                        int rowsAffected2;
                        rowsAffected2 = cmd2.ExecuteNonQuery();
                        conn.Close();
                        if (rowsAffected2 > 0)
                        {
                            MessageBox.Show("Lưu thành công");
                            loadsauluu();
                            cmbMaSP.Text = "";
                            upDowSL.Value = 0;
                            txtGiaNhap.Text = "";
                            txtThanhTien.Text = "";
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
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnSuaPhieuNhap_Click(object sender, EventArgs e)
        {
            try
            {

                if (upDowSL.Value < 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0. Vui lòng nhập lại!!!!");
                }
                else
                {
                    conn.Open();

                    string query1 = string.Format("update ChiTietPN set SoLuong='{2}', GiaNhap='{3}' where  MaPN='{0}'and MaSP='{1}'",
                        txtMaPhieuNhap.Text, cmbMaSP.Text, upDowSL.Value, txtGiaNhap.Text.Replace(",", "."));
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    int rowsAffected = cmd1.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Sửa thành công");
                        loadsauluu();
                        cmbMaSP.Text = "";
                        upDowSL.Value = 0;
                        txtGiaNhap.Text = "";
                        txtThanhTien.Text = "";
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int a = dataGridView1.ColumnCount;
            if (a == 5)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    txtMaPhieuNhap.Text = row.Cells["MaPN"].Value.ToString();
                    txtMaNhanVien.Text = row.Cells["MaNV"].Value.ToString();
                    dtpNgayNhap.Text = dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
                    cmbNCC.Text = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
                    btnThemPhieuNhap.Enabled = false; ;
                    btnLuu.Enabled = true;
                    btnXoaPhieuNhap.Enabled = true;
                    cmbNCC.Enabled = false;
                    loadsauluu();
                }
            }
            if (a == 6)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    txtGiaNhap.Text = row.Cells["GiaNhap"].Value.ToString();
                    txtThanhTien.Text = row.Cells["ThanhTien"].Value.ToString();
                    cmbMaSP.Text = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
                    upDowSL.Text = dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();
                    btnThemPhieuNhap.Enabled = false;
                    btnSuaPhieuNhap.Enabled = true;
                    btnXoaMH.Enabled = true;
                    btnLuu.Enabled = false;
                    btnXoaPhieuNhap.Enabled = false;
                    cmbMaSP.Enabled = false;
                }
            }
        }
        private void lammoi()
        {
            txtMaPhieuNhap.Text = "";
            txtGiaNhap.Text = "";
            txtThanhTien.Text = "";
            txtTongTien.Text = "";
            txtMaNhanVien.Text = MaNV;
            cmbMaSP.Text = "";
            dtpNgayNhap.Text = "";
            upDowSL.Value = 0;
            cmbNCC.Text = "";
            cmbMaPhieuNhap.Text = "";

            btnThemPhieuNhap.Enabled = true;
            btnSuaPhieuNhap.Enabled = false;
            btnXoaMH.Enabled = false;
            btnLuu.Enabled = false;
            btnXoaPhieuNhap.Enabled = false;
            cmbNCC.Enabled = true;
            cmbMaSP.Enabled = true;
            fill_combobox();
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            lammoi();
            dataGridView1.Columns.Clear();
            loaDataGridView();
        }

        private void btnInPhieuNhap_Click(object sender, EventArgs e)
        {
            // Khởi động chương trình Excel
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; //Trong 1 chương trình Excel có nhiều Workbook
            COMExcel.Worksheet exSheet; //Trong 1 Workbook có nhiều Worksheet
            COMExcel.Range exRange;
            int hang = 0, cot = 0;
            DataTable tblThongtinSP;
            DataTable tblThongtinPN;
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
            exRange.Range["C2:E2"].Value = "Phiếu nhập";
            // Biểu diễn thông tin chung của phiếu nhập
            if (txtMaPhieuNhap.Text != "")
            {
                KetNoi kn = new KetNoi();
                //conn.Open();
                //string query = string.Format("SELECT ChiTietPN.MaPN, NgayNhap, TenNV, TenNCC, TongTien FROM ChiTietPN INNER JOIN PhieuNhap ON ChiTietPN.MaPN = PhieuNhap.MaPN INNER JOIN NhanVien ON PhieuNhap.MaNV = NhanVien.MaNV  INNER JOIN NhaCungCap on PhieuNhap.MaNCC = NhaCungCap.MaNCC WHERE ChiTietPN.MaPN = '{0}'", txtMaPhieuNhap.Text);
                //SqlDataAdapter da = new SqlDataAdapter(query, conn);
                //DataSet ds = new DataSet();
                //da.Fill(ds);

                string query = string.Format("SELECT ChiTietPN.MaPN, NgayNhap, TenNV, TenNCC, TongTien FROM ChiTietPN INNER JOIN PhieuNhap ON ChiTietPN.MaPN = PhieuNhap.MaPN INNER JOIN NhanVien ON PhieuNhap.MaNV = NhanVien.MaNV  INNER JOIN NhaCungCap on PhieuNhap.MaNCC = NhaCungCap.MaNCC WHERE ChiTietPN.MaPN = '{0}'", txtMaPhieuNhap.Text);
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                ds = kn.LayDuLieu(query);
                tblThongtinPN = ds.Tables[0];

                exRange.Range["B6:C9"].Font.Size = 12;
                exRange.Range["B6:B6"].Value = "Mã phiếu nhập:";
                exRange.Range["C6:E6"].MergeCells = true;
                exRange.Range["C6:E6"].Value = tblThongtinPN.Rows[0][0].ToString();
                exRange.Range["B7:B7"].Value = "Ngày nhập:";
                exRange.Range["C7:E7"].MergeCells = true;
                exRange.Range["C7:E7"].Value = tblThongtinPN.Rows[0][1].ToString();
                exRange.Range["B8:B8"].Value = "Tên nhà cung cấp:";
                exRange.Range["C8:E8"].MergeCells = true;
                exRange.Range["C8:E8"].Value = tblThongtinPN.Rows[0][3].ToString();
                //Lấy thông tin các mặt hàng
                //conn.Open();
                //string query2 = string.Format("select TenSP, GiaBan, ChiTietPN.SoLuong, ThanhTien from ChiTietPN inner join SanPham on ChiTietPN.MaSP = SanPham.MaSP where MaPN = '{0}'", txtMaPhieuNhap.Text);
                //SqlDataAdapter da2 = new SqlDataAdapter(query2, conn);
                //DataSet ds2 = new DataSet();
                //da2.Fill(ds2);
                string query2 = string.Format("select TenSP, GiaBan, ChiTietPN.SoLuong, ThanhTien from ChiTietPN inner join SanPham on ChiTietPN.MaSP = SanPham.MaSP where MaPN = '{0}'", txtMaPhieuNhap.Text);
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
                exRange.Range["E11:E11"].Value = "Thành tiền";
                for (hang = 0; hang < tblThongtinSP.Rows.Count; hang++)
                {
                    //Điền số thứ tự vào cột 1 từ dòng 12
                    exSheet.Cells[1][hang + 12] = hang + 1;
                    for (cot = 0; cot < tblThongtinSP.Columns.Count; cot++)
                    //Điền thông tin hàng từ cột thứ 2, dòng 12
                    {
                        exSheet.Cells[cot + 2][hang + 12] = tblThongtinSP.Rows[hang][cot].ToString();
                    }
                }

                // Đẩy cột "Tổng tiền" sang phải 2 cột
                exRange = exSheet.Cells[cot + 2][hang + 14];
                exRange.Font.Bold = true;
                exRange.Value2 = "Tổng tiền:";
                // Đặt giá trị tổng tiền
                exRange = exSheet.Cells[cot + 3][hang + 14];
                exRange.Font.Bold = true;
                exRange.Value2 = tblThongtinPN.Rows[0][4].ToString();

                // Đẩy cột "Nhân viên bán hàng" sang phải 2 cột
                exRange = exSheet.Cells[cot + 2][hang + 16];
                exRange.Range["A1:C1"].MergeCells = true;
                exRange.Range["A1:C1"].Font.Bold = true;
                exRange.Range["A1:C1"].Font.Italic = true;
                exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A1:C1"].Value = "Nhân viên kho hàng";
                exRange.Range["A6:C6"].MergeCells = true;
                exRange.Range["A6:C6"].Font.Italic = true;
                exRange.Range["A6:C6"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
                exRange.Range["A6:C6"].Value = tblThongtinPN.Rows[0][2];

                exSheet.Name = "Phiếu nhập hàng";
                exApp.Visible = true;
            }
            else
            {
                MessageBox.Show("vui long chon phieu nhap can in");
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXoaMH_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    conn.Open();
                    string query1 = string.Format("delete from ChiTietPN  where  MaPN='{0}'and MaSP='{1}'",
                            txtMaPhieuNhap.Text, cmbMaSP.Text);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    int rowsAffected = cmd1.ExecuteNonQuery();
                    conn.Close();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Xóa thành công");
                        loadsauluu();
                        cmbMaSP.Text = "";
                        upDowSL.Value = 0;
                        txtGiaNhap.Text = "";
                        txtThanhTien.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoaPhieuNhap_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    conn.Open();
                    string query2 = string.Format("delete from ChiTietPN  where  MaPN='{0}'",
                            txtMaPhieuNhap.Text);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    int rowsAffected2 = cmd2.ExecuteNonQuery();
                    conn.Close();
                    conn.Open();
                    string query1 = string.Format("delete from PhieuNhap  where  MaPN='{0}'",
                            txtMaPhieuNhap.Text);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    int rowsAffected = cmd1.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected > 0 && rowsAffected2 > 0)
                    {
                        MessageBox.Show("Xóa phiếu nhập thành công");
                        btnLamMoi.PerformClick();
                    }
                    else
                    {
                        MessageBox.Show("Xóa phiếu nhập thất bại");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            } 
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Columns.Clear();
                string query = string.Format("select distinct phieunhap.MaPN,MaNCC,MaNV,NgayNhap,TongTien from phieunhap,ChiTietPN where phieunhap.MaPN=ChiTietPN.MaPN and phieunhap.MaPN ='{0}'", cmbMaPhieuNhap.Text);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                txtMaPhieuNhap.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
                dtpNgayNhap.Text = dataGridView1[3, dataGridView1.CurrentRow.Index].Value.ToString();

                dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns[0].HeaderText = "Mã phiếu nhập";
                dataGridView1.Columns[1].HeaderText = "Mã nhà cung cấp";
                dataGridView1.Columns[2].HeaderText = "Mã nhân viên";
           //     dataGridView1.Columns[3].HeaderText = "Mã sản phẩm";
            //    dataGridView1.Columns[4].HeaderText = "Giá nhập";
           //     dataGridView1.Columns[5].HeaderText = "Số lượng";
                dataGridView1.Columns[3].HeaderText = "Ngày nhập";
                dataGridView1.Columns[4].HeaderText = "Tổng tiền";
                dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            
        }
    }
}