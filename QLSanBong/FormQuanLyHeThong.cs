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
    public partial class FormQuanLyHeThong : Form
    {
        public FormQuanLyHeThong()
        {
            InitializeComponent();
            loadAccount();
        }

        private void loadAccount()
        {
            List<Account> listKH = AccountDAO.Instance.LoadListTK();
            dataGridView4.DataSource = listKH;
        }

        private void btn_Them_Click(object sender, EventArgs e)
        {
            string UserName = txt_TK.Text;
            string Password = txt_MK.Text;
            string DisplayName = txt_tenHT.Text;
            if (UserName == "")
            {
                MessageBox.Show("Vui lòng nhập tài khoản");
            }
            else if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu");
            }
            else if (string.IsNullOrEmpty(DisplayName))
            {
                MessageBox.Show("Vui lòng nhập tên hiển thị");
            }
            else
            {
                if (AccountDAO.Instance.KiemTraTrungTaiKhoan(UserName))
                    MessageBox.Show("Tên tài khoản đã tồn tại!");
                else
                {

                    string Role = cb_Quyen.Text;
                    AccountDAO.Instance.ThemTaiKhoan(UserName, Password, DisplayName, Role);
                }
            }
            loadAccount();
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            string UserName = "";
            try
            {
                UserName = txt_TK.Text;
            }
            catch
            {
                MessageBox.Show("Vui lòng chọn tài khoản muốn xóa!");
            }
            if (!string.IsNullOrEmpty(UserName))
            {
                AccountDAO.Instance.XoaTaiKhoan(UserName);
                txt_MK.Clear();
                txt_tenHT.Clear();
                cb_Quyen.SelectedIndex = -1;
                loadAccount();
            }
        }

        private void btn__Sua_Click(object sender, EventArgs e)
        {
            string UserName = txt_TK.Text;
            string Password = txt_MK.Text;
            string DisplayName = txt_tenHT.Text;
            if (UserName == "")
            {
                MessageBox.Show("Vui lòng nhập tài khoản");
            }
            else if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu");
            }
            else if (string.IsNullOrEmpty(DisplayName))
            {
                MessageBox.Show("Vui lòng nhập tên hiển thị");
            }
            else
            {
                string Role = cb_Quyen.Text;
                AccountDAO.Instance.SuaTaiKhoan(UserName, Password, DisplayName, Role);
                MessageBox.Show("Sửa thông tin tài khoản thành công!");
            }
            loadAccount();
        }

        private void dataGridView4_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView4.SelectedCells[0].RowIndex;
            DataGridViewRow row = dataGridView4.Rows[rowIndex];
            txt_TK.Text = row.Cells["UserName"].Value.ToString();
            txt_MK.Text = row.Cells["Password"].Value.ToString();
            txt_tenHT.Text = row.Cells["DisplayName"].Value.ToString();
            cb_Quyen.Text = row.Cells["Role"].Value.ToString();
        }

        private void btn_TimKiem_Click(object sender, EventArgs e)
        {
            string tenTaiKhoan = txt_TimkiemTK.Text;
            List<Account> ListTaiKhoan = AccountDAO.Instance.timkiemTaiKhoan(tenTaiKhoan);
            dataGridView4.DataSource = ListTaiKhoan;
        }

        private void btnXoaTK_Click(object sender, EventArgs e)
        {
            try
            {
                string UserName = txt_TK.Text;
                if (string.IsNullOrWhiteSpace(UserName))
                {
                    MessageBox.Show("Vui lòng chọn tài khoản muốn xóa!");
                    return;
                }
                DialogResult r;
                r = MessageBox.Show("Bạn có chắc muốn xóa tài khoản?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (r == DialogResult.Yes)
                {
                    AccountDAO.Instance.XoaTaiKhoan(UserName);
                    MessageBox.Show("Xóa thông tin tài khoản thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa thông tin tài khoản: " + ex.Message);
            }
            loadAccount();
        }
    }
}
