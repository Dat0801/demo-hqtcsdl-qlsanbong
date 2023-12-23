using MyClass.DAO;
using QLSanBong.DAO;
using QLSanBong.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSanBong
{
    public partial class FormQLKDNhanVien : Form
    {
        public FormQLKDNhanVien()
        {
            InitializeComponent();
            loadKhachHang();
            loadHoaDon();
            loadCTHD();
        }

        private void loadHoaDon()
        {
            List<HoaDon> listHD = HoaDonDAO.Instance.LoadListHoaDon();
            dgv_HoaDon.DataSource = listHD;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_HoaDon.SelectedCells.Count > 0)
                {

                    int selectedRowIndex = dgv_HoaDon.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dgv_HoaDon.Rows[selectedRowIndex];
                    int maHD = Convert.ToInt32(selectedRow.Cells["MaHD"].Value);
                    string TrangThai = Convert.ToString(selectedRow.Cells["TrangThai"].Value);
                    if (TrangThai == "Đã thanh toán")
                    {
                        DialogResult r;
                        r = MessageBox.Show("Bạn có chắc muốn xóa hóa đơn?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (r == DialogResult.Yes)
                        {
                            int result = HoaDonDAO.Instance.XoaHoaDon(maHD);
                            if (result > 0)
                            {
                                loadHoaDon();
                                MessageBox.Show("Xóa thành công!");
                            }
                            else
                            {
                                MessageBox.Show("Xóa không thành công!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không thể xóa do hóa đơn chưa thanh toán!");
                    }

                }
                else
                {
                    MessageBox.Show("Vui lòng chọn một dòng để xóa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        private void dgv_HoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                List<KhachHang> listKH = KhachHangDAO.Instance.LoadListKH();

                cbo_MAKH.DataSource = listKH;
                cbo_MAKH.DisplayMember = "TenKH";
                cbo_MAKH.ValueMember = "MaKH";
                DataGridViewRow row = dgv_HoaDon.Rows[e.RowIndex];
                txt_MaHD.Text = Convert.ToString(row.Cells["MaHD"].Value);
                dateTimePicker_NgayBD.Text = Convert.ToString(row.Cells["NgayTao"].Value);
                txt_Tongtien.Text = Convert.ToString(row.Cells["TongTien"].Value);
                int maKHFromDGV = Convert.ToInt32(row.Cells["MaKH"].Value);
                txt_PhutDa.Text = (row.Cells["TongPhut"].Value).ToString();
                txt_MaSan.Text = row.Cells["MaSan"].Value.ToString();
                txt_Dongia.Text = row.Cells["DonGia"].Value.ToString();
                // Tìm tên khách hàng tương ứng trong danh sách listKH
                KhachHang khachHang = listKH.Find(kh => kh.MaKH == maKHFromDGV);

                if (khachHang != null)
                {
                    cbo_MAKH.SelectedValue = khachHang.MaKH;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }

        }
        private void btn_SuaHD_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_HoaDon.SelectedCells.Count > 0)
                {
                    int selectedRowIndex = dgv_HoaDon.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dgv_HoaDon.Rows[selectedRowIndex];

                    int maHD = Convert.ToInt32(selectedRow.Cells["MaHD"].Value);

                    bool canEdit = HoaDonDAO.Instance.CanEditMaHD(maHD);

                    if (canEdit == true)
                    {
                        DateTime ngayTao = dateTimePicker_NgayBD.Value;
                        decimal tongTien;

                        if (!decimal.TryParse(txt_Tongtien.Text, out tongTien))
                        {
                            MessageBox.Show("Tổng tiền không hợp lệ!");
                            return;
                        }

                        int maKH = Convert.ToInt32(cbo_MAKH.SelectedValue);
                        int maSan = Convert.ToInt32(txt_MaSan.Text);
                        int tongPhut = Convert.ToInt32(txt_PhutDa.Text);

                        // Thực hiện sửa thông tin
                        int result = HoaDonDAO.Instance.SuaHoaDon(maHD, ngayTao, tongTien, maKH, maSan, tongPhut);

                        // Kiểm tra và hiển thị kết quả
                        if (result > 0)
                        {
                            MessageBox.Show("Sửa thông tin hóa đơn thành công!");
                            loadHoaDon();
                        }
                        else
                        {
                            MessageBox.Show("Không thể sửa thông tin hóa đơn. Vui lòng thử lại!");
                        }
                    }
                    if (canEdit == false)
                    {
                        MessageBox.Show("Không thể sửa mã hóa đơn!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn muốn sửa.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        private void loadCTHD()
        {
            List<ChiTietHoaDon> listCTHD = ChiTietHDDAO.Instance.LoadListLoadCTHD();
            dgv_CTHD.DataSource = listCTHD;


            List<DichVu> listDV = DichVuDAO.Instance.LoadListDichVu();
            cbo_MaDV.DataSource = listDV;
            cbo_MaDV.DisplayMember = "TenDV";
            cbo_MaDV.ValueMember = "MaDV";

            List<HoaDon> listHD = HoaDonDAO.Instance.LoadListHoaDon();
            cbo_MaHD.DataSource = listHD;
            cbo_MaHD.DisplayMember = "MaHD";
            cbo_MaHD.ValueMember = "MaHD";

        }
        private void dgv_CTHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow row = dgv_CTHD.Rows[e.RowIndex];
                txt_SoLuong.Text = Convert.ToString(row.Cells["SoLuong"].Value);
                int maDV = Convert.ToInt32(row.Cells["MaDV"].Value);
                int maHD = Convert.ToInt32(row.Cells["MaHD"].Value);
                cbo_MaDV.SelectedValue = maDV;
                cbo_MaHD.SelectedValue = maHD;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}");
            }

        }
        private void btn_ThemCTHD_Click(object sender, EventArgs e)
        {
            if (dgv_CTHD.SelectedCells.Count > 0)
            {
                int maHD = Convert.ToInt32(cbo_MaHD.SelectedValue);
                int maDV = Convert.ToInt32(cbo_MaDV.SelectedValue);
                try
                {
                    int soluong = Convert.ToInt32(txt_SoLuong.Text);
                    if (soluong <= 0)
                    {
                        MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0!");
                    }
                    else
                    {
                        HoaDon hd = HoaDonDAO.Instance.getHoaDon(maHD);
                        if (hd.TrangThai == "Đã thanh toán")
                        {
                            MessageBox.Show("Không thể thêm dịch vụ do hóa đơn đã thanh toán!");
                        }
                        else
                        {
                            int result = ChiTietHDDAO.Instance.ThemCTHD(maHD, maDV, soluong);
                            if (result > 0)
                            {
                                MessageBox.Show("Thêm chi tiết hóa đơn thành công!");
                                loadCTHD();
                            }
                            else
                            {
                                MessageBox.Show("Không thể thêm chi tiết hóa đơn. Vui lòng thử lại!");
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Số lượng không hợp lệ!");
                }

            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn và dịch vụ muốn thêm chi tiết.");
            }
            loadHoaDon();
        }

        private void btn_XoaCTHD_Click(object sender, EventArgs e)
        {
            int maDV = int.Parse(cbo_MaDV.SelectedValue.ToString());
            int maHD = int.Parse(cbo_MaHD.SelectedValue.ToString());
            DialogResult r;
            r = MessageBox.Show("Bạn có chắc muốn xóa chi tiết hóa đơn?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (r == DialogResult.Yes)
            {
                int result = ChiTietHDDAO.Instance.XoaCTHD(maHD, maDV);
                if (result > 0)
                {
                    MessageBox.Show("Xóa chi tiết hóa đơn thành công!");
                    loadCTHD();
                }
                loadHoaDon();
            }
        }

        private void btn_SuaCTHD_Click(object sender, EventArgs e)
        {
            int maHD = 0;
            try
            {
                maHD = int.Parse(cbo_MaHD.SelectedValue.ToString());
            }
            catch
            {
                MessageBox.Show("Vui lòng chọn hóa đơn");
            }

            if (maHD != 0)
            {
                try
                {
                    int madv = int.Parse(cbo_MaDV.SelectedValue.ToString());
                    try
                    {
                        int soluong = int.Parse(txt_SoLuong.Text);
                        if (soluong <= 0)
                        {
                            MessageBox.Show("Vui lòng nhập số lượng lớn hơn 0!");
                        }
                        else
                        {
                            HoaDon hd = HoaDonDAO.Instance.getHoaDon(maHD);
                            if (hd.TrangThai == "Đã thanh toán")
                            {
                                MessageBox.Show("Không thể thay đổi dịch vụ do hóa đơn đã thanh toán!");
                            }
                            else
                            {
                                int result = ChiTietHDDAO.Instance.SuaCTHD(maHD, madv, soluong);
                                if (result > 0)
                                {
                                    MessageBox.Show("Sửa chi tiết hóa đơn thành công!");
                                    loadCTHD();
                                }
                                else if (result == 0)
                                {
                                    MessageBox.Show("Hóa đơn không tồn tại hoặc không thể sửa!");
                                }
                                else
                                {
                                    MessageBox.Show("Lỗi khi sửa chi tiết hóa đơn!");
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Số lượng không hợp lệ.");
                    }
                }
                catch
                {
                    MessageBox.Show("Vui lòng chọn Mã Dịch vụ.");
                }
            }
            loadHoaDon();
        }

        private void loadKhachHang()
        {
            List<KhachHang> listKH = KhachHangDAO.Instance.LoadListKH();
            dataGridView_DSKH.DataSource = listKH;
        }

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            try
            {
                string tenKhachHang = txt_TenKH.Text;
                int makh = int.Parse(txtMaKH.Text);
                if (tenKhachHang == "")
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng");
                }
                else
                {
                    string diachi = txt_DiaChi.Text;
                    string sdt = txt_SDT.Text;
                    if (sdt == "")
                    {
                        MessageBox.Show("Vui lòng nhập số điện thoại!");
                    }
                    else if (!IsValidPhoneNumber(sdt))
                    {
                        MessageBox.Show("Số điện thoại không hợp lệ!");
                        txt_SDT.Clear();
                    }
                    else
                    {
                        List<KhachHang> listKH = KhachHangDAO.Instance.LoadListKH();
                        int flag = 0;
                        foreach (var item in listKH)
                        {
                            if (item.SDT == sdt && item.MaKH == makh)
                            {
                                flag = 1;
                            }
                        }
                        if (flag == 1)
                        {
                            MessageBox.Show("Số điện thoại đã tồn tại!");
                        }
                        else
                        {
                            KhachHangDAO.Instance.ThemKhachHang(tenKhachHang, diachi, sdt);
                            txt_TenKH.Clear();
                            txt_DiaChi.Clear();
                            txt_SDT.Clear();
                        }
                    }
                }
                loadKhachHang();
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^0\d{3}\d{3}\d{3}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(phoneNumber);
        }

        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            try
            {
                string tenKhachHang = txt_TenKH.Text;
                int makh = int.Parse(txtMaKH.Text);
                if (tenKhachHang == "")
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng");
                }

                else
                {
                    string diachi = txt_DiaChi.Text;
                    string sdt = txt_SDT.Text;
                    if (sdt == "")
                    {
                        MessageBox.Show("Vui lòng nhập số điện thoại!");
                    }
                    else if (!IsValidPhoneNumber(sdt))
                    {
                        MessageBox.Show("Số điện thoại không hợp lệ!");
                        txt_SDT.Clear();
                    }
                    else
                    {
                        List<KhachHang> listKH = KhachHangDAO.Instance.LoadListKH();
                        int flag = 0;
                        foreach (var item in listKH)
                        {
                            if (item.SDT == sdt && item.MaKH != makh)
                            {
                                flag = 1;
                            }
                        }
                        if (flag == 1)
                        {
                            MessageBox.Show("Số điện thoại đã tồn tại!");
                        }
                        else
                        {
                            KhachHangDAO.Instance.SuaDanhSach(tenKhachHang, diachi, sdt, makh);
                            MessageBox.Show("Sửa thành công!");
                        }
                    }
                }
                loadKhachHang();
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
            }

        }
        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            string tenKhachHang = txtTimKiemTenKH.Text;
            List<KhachHang> ListKhachHang = KhachHangDAO.Instance.timKiemKhachHang(tenKhachHang);
            dataGridView_DSKH.DataSource = ListKhachHang;
        }

        private void dataGridView_DSKH_Click(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = dataGridView_DSKH.SelectedCells[0].RowIndex;
            DataGridViewRow row = dataGridView_DSKH.Rows[rowIndex];
            txt_TenKH.Text = row.Cells["TenKH"].Value.ToString();
            txt_DiaChi.Text = row.Cells["DiaChi"].Value.ToString();
            txt_SDT.Text = row.Cells["SDT"].Value.ToString();
            txtMaKH.Text = row.Cells["MaKH"].Value.ToString();
        }

        private void FormQLKDNhanVien_Load(object sender, EventArgs e)
        {

        }

    }
}
