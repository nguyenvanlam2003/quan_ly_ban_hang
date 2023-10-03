using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyCuaHangPhuKienCauLong
{
    public partial class frmMenu : Form
    {
        string TaiKhoan = "", MatKhau = "", Quyen = "",MaNV="";
        public frmMenu()
        {
            InitializeComponent();
           
        }
        public frmMenu(string TaiKhoan, string MatKhau, string Quyen, string MaNV)
        {
            InitializeComponent();
            this.TaiKhoan = TaiKhoan;
            this.MatKhau = MatKhau;
            this.Quyen = Quyen;
            this.MaNV = MaNV;
        }
        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmNhanVien nv = new frmNhanVien();
            nv.ShowDialog();
            this.Show();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            if (Quyen == "NVBH")
            {
                nhânViênToolStripMenuItem.Enabled = false;
                hóaĐơnToolStripMenuItem.Enabled = true;
                sảnPhẩmToolStripMenuItem.Enabled = false;
                phiếuNhậpToolStripMenuItem.Enabled = false;
                nhàCungCấpToolStripMenuItem.Enabled = false;
                báoCáoToolStripMenuItem.Enabled = false;
            }
            if (Quyen == "NVK")
            {
                nhânViênToolStripMenuItem.Enabled = false;
                hóaĐơnToolStripMenuItem.Enabled = false;
                nhàCungCấpToolStripMenuItem.Enabled = false;
                báoCáoToolStripMenuItem.Enabled = false;
                sảnPhẩmToolStripMenuItem.Enabled = true;
                phiếuNhậpToolStripMenuItem.Enabled = true;
            }
           
          
        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            string ma = MaNV;
            frmHoaDon HD = new frmHoaDon(ma);
            HD.ShowDialog();
            this.Show();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void phiếuNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            string ma=MaNV;
            frmPhieuNhap PN=new frmPhieuNhap(ma);
            PN.ShowDialog();
            this.Show();
        }

        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmNhaCungCap ncc = new frmNhaCungCap();
            ncc.ShowDialog();
            this.Show();
        }

        private void sảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSanPham sp = new frmSanPham();
            sp.ShowDialog();
            this.Show();
        }

        private void thốngKêHàngTồnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmThongKeHangTon frm = new frmThongKeHangTon();
            frm.ShowDialog();
            this.Show();
        }

        private void thốngKêDoanhThuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmThongKeDoanhThu frm = new frmThongKeDoanhThu();
            frm.ShowDialog();
            this.Show();
        }
    }
}
