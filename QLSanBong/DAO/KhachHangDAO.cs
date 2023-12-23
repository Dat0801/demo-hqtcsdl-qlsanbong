using MyClass.DAO;
using QLSanBong.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLSanBong.DAO
{
    class KhachHangDAO
    {
        private static KhachHangDAO instance;

        public static KhachHangDAO Instance
        {
            get { if (instance == null) instance = new KhachHangDAO(); return KhachHangDAO.instance; }
            private set { KhachHangDAO.instance = value; }
        }

        private KhachHangDAO() { }
        public List<KhachHang> LoadListKH()
        {
            List<KhachHang> listKH = new List<KhachHang>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SP_GetListKhachHang");
            foreach (DataRow row in data.Rows)
            {
                KhachHang kh = new KhachHang(row);
                listKH.Add(kh);
            }
            return listKH;
        }

        public KhachHang LoadListKH(int MaKH)
        {
            List<KhachHang> listKH = new List<KhachHang>();
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * from KhachHang where MaKH = '" + MaKH + "'");
            KhachHang kh = new KhachHang(data.Rows[0]);
            return kh;
        }

        public int ThemKhachHang(string tenkh, string diachi, string sdt)
        {
            string query = "SP_ThemKhachHang @TenKH , @DiaChi , @SDT";
            //DataTable result = DataProvider.Instance.ExecuteQuery(query);
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { tenkh, diachi, sdt });
            return result;
        }
        public int SuaDanhSach(string tenkh, string diachi, string sdt, int makh)
        {
            string query = "SP_SuaDanhSachKH @TenKH , @DiaChi , @SDT , @MaKH";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { tenkh, diachi, sdt, makh });
            return result;
        }
        public List<KhachHang> timKiemKhachHang(string tenKhachHang)
        {
            List<KhachHang> ListKhachHang = new List<KhachHang>();
            string query = "SP_TimKH @TenKH";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { tenKhachHang });
            foreach (DataRow row in data.Rows)
            {
                KhachHang khachhang = new KhachHang(row);
                ListKhachHang.Add(khachhang);
            }
            return ListKhachHang;
        }
    }
}
