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
    public partial class frmThongKeDoanhThu : Form
    {
        public frmThongKeDoanhThu()
        {
            InitializeComponent();
        }

        KetNoi kn = new KetNoi();

        static string[] chuSo = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
        static string[] donVi = { "", "nghìn", "triệu", "tỷ" };

        static string ConvertNumberToWords(decimal number)
        {
            if (number == 0)
                return "không đồng";

            string[] block = new string[4];
            int i = 0;

            while (number > 0)
            {
                decimal so = number % 1000;
                block[i] = DocBlock(so, i == 0);
                number = Math.Floor(number / 1000);
                i++;
            }

            StringBuilder words = new StringBuilder();
            for (int j = i - 1; j >= 0; j--)
            {
                if (!string.IsNullOrEmpty(block[j]))
                {
                    words.Append(block[j]);
                    words.Append(" ");
                    words.Append(donVi[j]);
                    words.Append(" ");
                }
            }

            return words.ToString().Trim();
        }

        static string DocBlock(decimal block, bool docDonVi)
        {
            string result = "";

            decimal tram = Math.Floor(block / 100);
            decimal chuc = Math.Floor((block % 100) / 10);
            decimal donVi = block % 10;

            if (tram > 0)
            {
                result += chuSo[(int)tram] + " trăm ";
            }

            if (chuc > 1)
            {
                result += chuSo[(int)chuc] + " mươi ";
                if (donVi == 1)
                {
                    result += "mốt ";
                }
            }
            else if (chuc == 1)
            {
                result += "mười ";
                if (donVi == 1)
                {
                    result += "một ";
                }
            }
            else if (docDonVi && donVi > 0)
            {
                result += "lẻ ";
            }

            if (donVi > 0 && chuc != 1)
            {
                result += chuSo[(int)donVi] + " ";
            }

            return result;
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (rdbHoaDon.Checked)
            {
                btnLamMoi.PerformClick();
                string query = string.Format("select MaHD, NgayBan, TongTien from HoaDon WHERE NgayBan BETWEEN '{0}' AND '{1}'", dtpNgayBD.Value.ToString("yyyy-MM-dd"), dtpNgayKT.Value.ToString("yyyy-MM-dd"));
                DataSet ds = kn.LayDuLieu(query);
                dgvThongKe.DataSource = ds.Tables[0];
                dgvThongKe.Columns[0].HeaderText = "Mã hóa đơn";
                dgvThongKe.Columns[1].HeaderText = "Ngày bán";
                dgvThongKe.Columns[2].HeaderText = "Tổng tiền";

                dgvThongKe.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvThongKe.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvThongKe.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                string query2 = string.Format("SELECT SUM(TongTien) FROM HoaDon WHERE NgayBan BETWEEN '{0}' AND '{1}'", dtpNgayBD.Value.ToString("yyyy-MM-dd"), dtpNgayKT.Value.ToString("yyyy-MM-dd"));
                DataSet ds2 = kn.LayDuLieu(query2);
                object tongTienObj = ds2.Tables[0].Rows[0][0];
                txtTong.Text = tongTienObj.ToString();
                decimal tongTien = 0;

                if (tongTienObj != null && decimal.TryParse(tongTienObj.ToString(), out tongTien))
                {
                    txtTong.Text = tongTien.ToString("N"); // Hiển thị giá trị số với định dạng số
                    lbBangChu1.Text = "Bằng chữ: " + ConvertNumberToWords(tongTien); // Chuyển số tiền thành chữ và gán vào lbBangChu1
                }
                else
                {
                    txtTong.Text = "";
                    lbBangChu1.Text = "";
                }
            }
            else if (rdbPhieuNhap.Checked)
            {
                btnLamMoi.PerformClick();
                string query = string.Format("select PHieuNhap.MaPN, NgayNhap, TongTien from PhieuNhap inner join ChiTietPN on PhieuNhap.MaPN = ChiTietPN.MaPN where NgayNhap BETWEEN '{0}' AND '{1}' group by PhieuNhap.MaPN, NgayNhap, TongTien", dtpNgayBD.Value.ToString("yyyy-MM-dd"), dtpNgayKT.Value.ToString("yyyy-MM-dd"));
                DataSet ds = kn.LayDuLieu(query);
                dgvThongKe.DataSource = ds.Tables[0];
                dgvThongKe.Columns[0].HeaderText = "Mã phiếu nhập";
                dgvThongKe.Columns[1].HeaderText = "Ngày nhập";
                dgvThongKe.Columns[2].HeaderText = "Tổng tiền";

                dgvThongKe.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvThongKe.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvThongKe.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                string query2 = string.Format("SELECT SUM(TongTien) FROM PhieuNhap inner join ChiTietPN on PhieuNhap.MaPN = ChiTietPN.MaPN where NgayNhap BETWEEN '{0}' AND '{1}'", dtpNgayBD.Value.ToString("yyyy-MM-dd"), dtpNgayKT.Value.ToString("yyyy-MM-dd"));
                DataSet ds2 = kn.LayDuLieu(query2);
                object tongTienObj = ds2.Tables[0].Rows[0][0];
                txtTong.Text = tongTienObj.ToString();
                decimal tongTien = 0;

                if (tongTienObj != null && decimal.TryParse(tongTienObj.ToString(), out tongTien))
                {
                    txtTong.Text = tongTien.ToString("N"); // Hiển thị giá trị số với định dạng số
                    lbBangChu1.Text = "Bằng chữ: " + ConvertNumberToWords(tongTien); // Chuyển số tiền thành chữ và gán vào lbBangChu1
                }
                else
                {
                    txtTong.Text = "";
                    lbBangChu1.Text = "";
                }
            }
        }

        public void LamMoi()
        {
            dgvThongKe.DataSource = null;
            rdbHoaDon.Checked = false;
            rdbPhieuNhap.Checked = false;
            txtTong.Text = "";
            lbBangChu1.Text = "";

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvThongKe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmThongKeDoanhThu_Load(object sender, EventArgs e)
        {

        }
    }
}
