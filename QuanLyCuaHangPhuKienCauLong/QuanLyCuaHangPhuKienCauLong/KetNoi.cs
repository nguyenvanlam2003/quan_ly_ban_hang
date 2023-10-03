using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace QuanLyCuaHangPhuKienCauLong
{
    class KetNoi
    {
        private string conStr = @"Data Source=DESKTOP-14GA20T\SQLEXPRESS;Initial Catalog=DoAn_C#;Integrated Security=True";
        private SqlConnection conn;

        public KetNoi ()
        {
            conn = new SqlConnection(conStr);
        }

        public DataSet LayDuLieu(string TruyVan) 
        {
            try
            {
                DataSet ds = new DataSet();
                // B1: Tao doi tuong thuc thi truy van
                SqlDataAdapter da = new SqlDataAdapter(TruyVan, conn);
                // B2: Do du lieu truy van duoc luu len Dataset
                da.Fill(ds);
                // B3 : Return
                return ds;
            }
            catch
            {
                return null;
            }
        }

        public bool ThucThi(string TruyVan)
        {
            // B1: Mo ket noi den csdl
            conn.Open();
            // B2: Tao thuc thi truy van
            SqlCommand cmd = new SqlCommand(TruyVan, conn);
            // B3: Thuc thi truy van
            int r = cmd.ExecuteNonQuery();
            // B4: Dong ket noi den csdl
            conn.Close();
            return r > 0;
        }
    }
}
