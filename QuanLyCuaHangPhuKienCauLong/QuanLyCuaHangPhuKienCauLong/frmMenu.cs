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
            frmNhanVien nv = new frmNhanVien();
            int desiredWidth =nv.Width; 
            int desiredHeight = nv.Height+20; 
            this.Size = new Size(desiredWidth, desiredHeight);
            nv.FormBorderStyle = FormBorderStyle.None;
            nv.TopLevel = false;
            panel_cha.Controls.Clear();
            nv.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(nv);
            nv.Show();
            pictureBox1.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(pictureBox1);
            pictureBox1.Show();
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
            string ma = MaNV;
            frmHoaDon HD = new frmHoaDon(ma);
            int desiredWidth = HD.Width;
            int desiredHeight = HD.Height + 20;
            this.Size = new Size(desiredWidth, desiredHeight);
            HD.FormBorderStyle = FormBorderStyle.None;
            HD.TopLevel = false;
            panel_cha.Controls.Clear();
            HD.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(HD);
            HD.Show();
            pictureBox1.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(pictureBox1);
            pictureBox1.Show();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void phiếuNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ma=MaNV;
            frmPhieuNhap PN=new frmPhieuNhap(ma);
            int desiredWidth = PN.Width;
            int desiredHeight = PN.Height + 20;
            this.Size = new Size(desiredWidth, desiredHeight);
            PN.FormBorderStyle = FormBorderStyle.None;
            PN.TopLevel = false;
            panel_cha.Controls.Clear();
            PN.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(PN);
            PN.Show();
            pictureBox1.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(pictureBox1);
            pictureBox1.Show();
        }

        private void nhàCungCấpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNhaCungCap ncc = new frmNhaCungCap();
            int desiredWidth = ncc.Width;
            int desiredHeight = ncc.Height + 20;
            this.Size = new Size(desiredWidth, desiredHeight);
            ncc.FormBorderStyle = FormBorderStyle.None;
            ncc.TopLevel = false;
            panel_cha.Controls.Clear();
            ncc.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(ncc);
            ncc.Show();
            pictureBox1.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(pictureBox1);
            pictureBox1.Show();
        }

        private void sảnPhẩmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSanPham sp = new frmSanPham();
            int desiredWidth = sp.Width;
            int desiredHeight = sp.Height + 20;
            this.Size = new Size(desiredWidth, desiredHeight);
            sp.FormBorderStyle = FormBorderStyle.None;
            sp.TopLevel = false;
            panel_cha.Controls.Clear();
            sp.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(sp);
            sp.Show();
            pictureBox1.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(pictureBox1);
            pictureBox1.Show();
        }

        private void thốngKêHàngTồnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongKeHangTon frm = new frmThongKeHangTon();
            int desiredWidth = frm.Width;
            int desiredHeight = frm.Height + 20;
            this.Size = new Size(desiredWidth, desiredHeight);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            panel_cha.Controls.Clear();
            frm.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(frm);
            frm.Show();
            pictureBox1.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(pictureBox1);
            pictureBox1.Show();
        }

        private void thốngKêDoanhThuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmThongKeDoanhThu frm = new frmThongKeDoanhThu();
            int desiredWidth = frm.Width;
            int desiredHeight = frm.Height + 20;
            this.Size = new Size(desiredWidth, desiredHeight);
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.TopLevel = false;
            panel_cha.Controls.Clear();
            frm.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(frm);
            frm.Show();
            pictureBox1.Dock = DockStyle.Fill;
            panel_cha.Controls.Add(pictureBox1);
            pictureBox1.Show();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_cha.Controls.Clear(); 
            pictureBox1.Width = 924;
            pictureBox1.Height = 587;
            panel_cha.Controls.Add(pictureBox1);
        }
    }
}
