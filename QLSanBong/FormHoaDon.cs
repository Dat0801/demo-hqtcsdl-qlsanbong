using Microsoft.Reporting.WinForms;
using QLSanBong.DAO;
using QLSanBong.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSanBong
{
    public partial class FormHoaDon : Form
    {
        public FormHoaDon()
        {
            InitializeComponent();
            LoadHoaDon();
        }

        private void LoadHoaDon()
        {
            List<HoaDon> listHD = HoaDonDAO.Instance.LoadListHoaDonDaThanhToan();
            cbo_HoaDon.DataSource = listHD;
            cbo_HoaDon.DisplayMember = "MaHD";
            cbo_HoaDon.ValueMember = "MaHD";
        }

        private void FormHoaDon_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'QLSANBONGDataSet.HOADON' table. You can move, or remove it, as needed.
            this.HOADONTableAdapter.Fill(this.QLSANBONGDataSet.HOADON);

        }

        private void btn_InHoaDon_Click(object sender, EventArgs e)
        {
            int MaHD = int.Parse(cbo_HoaDon.SelectedValue.ToString());
            List<HoaDon> listHD = HoaDonDAO.Instance.LoadListHoaDon(MaHD);
            List<ChiTietHoaDon> listCTHD = ChiTietHDDAO.Instance.LoadListLoadCTHD();
            int flag = 0;
            foreach (var item in listCTHD)
            {
                if (item.MaHD == MaHD)
                    flag = 1;
            }
            if(flag == 1)
            {
                reportViewer2.LocalReport.ReportPath = "ReportHoaDon.rdlc";
                ReportDataSource reportDataSourceHD = new ReportDataSource("DataSetHoaDon", listHD);
                reportViewer2.LocalReport.DataSources.Clear();
                reportViewer2.LocalReport.DataSources.Add(reportDataSourceHD);
                List<ChiTietHoaDon> listcthd = ChiTietHDDAO.Instance.getCTHoaDon(MaHD);
                ReportDataSource reportDataSourceCTHD = new ReportDataSource("DataSetCTHD", listcthd);
                reportViewer2.LocalReport.DataSources.Add(reportDataSourceCTHD);
                List<San> listSan = new List<San>();
                foreach (var item in listHD)
                {
                    San san = SanDAO.Instance.LoadListSan(item.MaSan);
                    listSan.Add(san);
                }
                ReportDataSource reportDataSourceSan = new ReportDataSource("DataSetSan", listSan);
                reportViewer2.LocalReport.DataSources.Add(reportDataSourceSan);
                List<DichVu> listDV = new List<DichVu>();
                foreach (var item in listcthd)
                {
                    DichVu dv = DichVuDAO.Instance.getDichVu(item.MaDV);
                    listDV.Add(dv);
                }
                ReportDataSource reportDataSourceDV = new ReportDataSource("DataSetDichVu", listDV);
                reportViewer2.LocalReport.DataSources.Add(reportDataSourceDV);
                List<KhachHang> listKH = new List<KhachHang>();
                foreach (var item in listHD)
                {
                    KhachHang kh = KhachHangDAO.Instance.LoadListKH(item.MaKH);
                    listKH.Add(kh);
                }
                ReportDataSource reportDataSourceKH = new ReportDataSource("DataSetKhachHang", listKH);
                reportViewer2.LocalReport.DataSources.Add(reportDataSourceKH);
                this.reportViewer2.RefreshReport();
            } else
            {
                reportViewer2.LocalReport.ReportPath = "ReportHoaDonKhongDV.rdlc";
                ReportDataSource reportDataSourceHD = new ReportDataSource("DataSetHoaDon", listHD);
                reportViewer2.LocalReport.DataSources.Clear();
                reportViewer2.LocalReport.DataSources.Add(reportDataSourceHD);
                List<ChiTietHoaDon> listcthd = ChiTietHDDAO.Instance.getCTHoaDon(MaHD);
                ReportDataSource reportDataSourceCTHD = new ReportDataSource("DataSetCTHD", listcthd);
                reportViewer2.LocalReport.DataSources.Add(reportDataSourceCTHD);
                List<San> listSan = new List<San>();
                foreach (var item in listHD)
                {
                    San san = SanDAO.Instance.LoadListSan(item.MaSan);
                    listSan.Add(san);
                }
                ReportDataSource reportDataSourceSan = new ReportDataSource("DataSetSan", listSan);
                reportViewer2.LocalReport.DataSources.Add(reportDataSourceSan);
                List<KhachHang> listKH = new List<KhachHang>();
                foreach (var item in listHD)
                {
                    KhachHang kh = KhachHangDAO.Instance.LoadListKH(item.MaKH);
                    listKH.Add(kh);
                }
                ReportDataSource reportDataSourceKH = new ReportDataSource("DataSetKhachHang", listKH);
                reportViewer2.LocalReport.DataSources.Add(reportDataSourceKH);
                this.reportViewer2.RefreshReport();
            }
        }
    }
}
