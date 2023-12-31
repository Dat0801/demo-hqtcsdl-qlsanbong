﻿using Microsoft.Reporting.WinForms;
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
    public partial class FrmReportLichDatSan : Form
    {
        public FrmReportLichDatSan()
        {
            InitializeComponent();
        }

        private void FrmReportLichDatSan_Load(object sender, EventArgs e)
        {
            List<LichDatSan> listLDS = LichDatSanDAO.Instance.LoadListLoaiSan();
            reportViewer2.LocalReport.ReportPath = "ReportLDS.rdlc";
            var source = new ReportDataSource("LichDatSanDataSet", listLDS);
            reportViewer2.LocalReport.DataSources.Clear();
            reportViewer2.LocalReport.DataSources.Add(source);
            List<San> listSan = new List<San>();
            List<KhachHang> listKH = new List<KhachHang>();
            foreach (var item in listLDS)
            {
                San san = SanDAO.Instance.LoadListSan(item.MaSan);
                listSan.Add(san);
                KhachHang kh = KhachHangDAO.Instance.LoadListKH(item.MaKH);
                listKH.Add(kh);
            }
            ReportDataSource reportDataSourceSan = new ReportDataSource("DataSetSan", listSan);
            reportViewer2.LocalReport.DataSources.Add(reportDataSourceSan);
            ReportDataSource reportDataSourceKH = new ReportDataSource("DataSetKhachHang", listKH);
            reportViewer2.LocalReport.DataSources.Add(reportDataSourceKH);
            this.reportViewer2.RefreshReport();
        }
    }
}
