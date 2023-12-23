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
                ReportDataSource reportDataSource = new ReportDataSource("DataSetHoaDon", listHD);
                reportViewer2.LocalReport.DataSources.Clear();
                reportViewer2.LocalReport.DataSources.Add(reportDataSource);
                List<ChiTietHoaDon> listcthd = ChiTietHDDAO.Instance.getCTHoaDon(MaHD);
                foreach (var item in listcthd)
                {
                    List<DichVu> dv = DichVuDAO.Instance.getDichVu(item.MaDV);
                    ReportDataSource reportDataSource2 = new ReportDataSource("DataSetDichVu", dv);
                    reportViewer2.LocalReport.DataSources.Add(reportDataSource2);
                }
                foreach (var item in listHD)
                {
                    List<KhachHang> listKH = KhachHangDAO.Instance.LoadListKH(item.MaKH);
                    ReportDataSource reportDataSource2 = new ReportDataSource("DataSetKhachHang", listKH);
                    reportViewer2.LocalReport.DataSources.Add(reportDataSource2);
                }
                this.reportViewer2.RefreshReport();
            }
        }
    }
}
