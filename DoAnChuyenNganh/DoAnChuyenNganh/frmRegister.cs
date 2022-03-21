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
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace DoAnChuyenNganh
{
    public partial class frmRegister : Form
    {

        Library library = new Library();

        IFirebaseClient client;

        public bool ConnectFireBase()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                // Project settings -> Database secrets
                AuthSecret = "8TeUixsbsKGGZJ6lVUh6K8wXG6DpZl4WOyCjl0Mj",
                // Realtime Database
                BasePath = "https://esp8266project-8ea11-default-rtdb.firebaseio.com/",
            };
            client = new FireSharp.FirebaseClient(config);
            try
            {
                if (client != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public frmRegister()
        {
            InitializeComponent();
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                errorProvider.SetError(txtName, "Vui lòng nhập họ và tên");
            }
            else if (txtName.Text.Length < 6 || txtName.Text.Length >= 64)
            {
                errorProvider.SetError(txtName, "Độ dài tối thiểu của họ và tên là 6 và tối đa là 64 ký tự");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtName, null);
            }
        }

        private void txtAge_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtAge.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(txtAge, "Vui lòng nhập tuổi");
            }
            else if (Regex.IsMatch(txtAge.Text.Trim(), "^[a-zA-Z]"))
            {
                errorProvider.SetError(txtAge, "Vui lòng chỉ nhập số. Không nhập chữ");
            }
            else if(Convert.ToInt32(txtAge.Text.Trim()) < 15)
            {
                errorProvider.SetError(txtAge, "Xin lỗi bạn chưa đủ 15 tuổi để đăng ký");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtAge, null);
            }
        }

        private void cboGender_Validating(object sender, CancelEventArgs e)
        {
            if (cboGender.SelectedIndex == -1)
            {
                errorProvider.SetError(cboGender, "Vui lòng chọn giới tính");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(cboGender, null);
            }
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtAddress.Text.Trim()))
            {
                errorProvider.SetError(txtAddress, "Vui lòng nhập địa chỉ");
            }
            else if (txtAddress.Text.Length < 3 || txtAddress.Text.Length >= 255)
            {
                errorProvider.SetError(txtAddress, "Độ dài tối thiểu của địa chỉ là 3 và tối đa là 255 ký tự");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtAddress, null);
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider.SetError(txtPhone, "Vui lòng nhập số điện thoại");
            }
            else if (Regex.IsMatch(txtPhone.Text.Trim(), "^[a-zA-Z]"))
            {
                errorProvider.SetError(txtPhone, "Vui lòng chỉ nhập số. Không nhập chữ");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtPhone, null);
            }
        }

        private void txtUser_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                errorProvider.SetError(txtUser, "Vui lòng nhập tài khoản");
            }
            else if (txtUser.Text.Length < 6 || txtUser.Text.Length >= 64)
            {
                errorProvider.SetError(txtUser, "Độ dài tối thiểu của tài khoản là 6 và tối đa là 64 ký tự");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtUser, null);
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                errorProvider.SetError(txtPassword, "Vui lòng nhập mật khẩu");
            }
            else if (txtPassword.Text.Length < 6 || txtPassword.Text.Length >= 64)
            {
                errorProvider.SetError(txtPassword, "Độ dài tối thiểu của mật khẩu là 6 và tối đa là 64 ký tự");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtPassword, null);
            }
        }

        private void txtPassword_Confirm_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtPassword_Confirm.Text.Trim()))
            {
                errorProvider.SetError(txtPassword_Confirm, "Vui lòng nhập lại mật khẩu để xác nhận");
            }
            else if (txtPassword.Text.Trim() != txtPassword_Confirm.Text.Trim())
            {
                errorProvider.SetError(txtPassword_Confirm, "Mật khẩu không trùng khớp");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtPassword_Confirm, null);
            }
        }

        private void ResetData()
        {
            txtName.Clear();
            txtAge.Clear();
            cboGender.SelectedIndex = 0;
            txtAddress.Clear();
            txtPhone.Clear();
            txtUser.Clear();
            txtPassword.Clear();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            if (ConnectFireBase())
            {
                if (ValidateChildren(ValidationConstraints.Enabled))
                {
                    string Name = txtName.Text.Trim();
                    string Age = txtAge.Text.Trim();
                    string Gender = cboGender.Text.Trim();
                    string Address = txtAddress.Text.Trim();
                    string Phone = txtPhone.Text.Trim();
                    string Username = txtUser.Text.Trim();
                    string Password = library.MD5Hash(txtPassword.Text.Trim());

                    User user = new User()
                    {
                        Name = Name,
                        Age = Convert.ToInt32(Age),
                        Gender = Gender,
                        Address = Address,
                        Phone = Convert.ToInt32(Phone),
                        Username = Username,
                        Password = Password,
                        Image = "",
                        Rule = "",
                    };
                    try
                    {

                        if (user != null)
                        {
                            SetResponse response = await client.SetAsync("Users/" + user.Username, user);
                            ResetData();
                            MessageBox.Show("Đăng ký thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GoToLoginForm();
                        }
                        else
                        {
                            MessageBox.Show("Đăng ký thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lỗi kết nối với hệ thống. Vui lòng kiểm tra lại!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng form không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void GoToLoginForm()
        {
            this.Hide();
            frmLogin frm = new frmLogin();
            frm.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GoToLoginForm();
        }
    }
}
