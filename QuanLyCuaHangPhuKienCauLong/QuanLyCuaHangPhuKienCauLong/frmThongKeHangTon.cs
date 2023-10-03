using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;

namespace QuanLyCuaHangPhuKienCauLong
{
    public partial class frmThongKeHangTon : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-14GA20T\SQLEXPRESS;Initial Catalog=DoAn_C#;Integrated Security=True");

        public frmThongKeHangTon()
        {
            InitializeComponent();
        }

        public DataTable getSanPham()
        {
            DataTable dataTable = new DataTable();
            string query = "select MaSP, TenSP, XuatXu, MoTa, GiaBan, SoLuong from SanPham where SoLuong > 0";
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.Fill(dataTable);
            conn.Close();
            return dataTable;
        }
        private void frmThongKeHangTon_Load(object sender, EventArgs e)
        {
            try
            {
                reportViewer1.LocalReport.ReportEmbeddedResource = "QuanLyCuaHangPhuKienCauLong.Report1.rdlc";
                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DataSet1";
                reportDataSource.Value = getSanPham();
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                

                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
