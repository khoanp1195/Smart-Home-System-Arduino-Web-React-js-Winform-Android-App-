using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace DoAnChuyenNganh
{
    public partial class frmLogin : Form
    {
        IFirebaseClient client;

        Library library = new Library();

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

        public frmLogin()
        {
            InitializeComponent();
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtUsername.Text.Trim()))
            {
                errorProvider.SetError(txtUsername, "Vui lòng nhập tài khoản");
            }
            else if (txtUsername.Text.Length < 6 || txtUsername.Text.Length >= 64)
            {
                errorProvider.SetError(txtUsername, "Độ dài tối thiểu của tài khoản là 6 và tối đa là 64 ký tự");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtUsername, null);
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

        private void ResetData()
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }
        
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (ConnectFireBase())
            {
                if (ValidateChildren(ValidationConstraints.Enabled))
                {
                    string Username = txtUsername.Text.Trim();
                    string Password = library.MD5Hash(txtPassword.Text.Trim());
                    try
                    {
                        FirebaseResponse response = await client.GetAsync("Users/" + Username);
                        User responseUser = response.ResultAs<User>();
                        User currentUser = new User()
                        {
                            Username = Username,
                            Password = Password
                        };
                        if (User.checkLogin(responseUser, currentUser))
                        {
                            ResetData();
                            this.Hide();
                            frmMain frm = new frmMain(responseUser.Username, responseUser.Rule);
                            frm.Show();
                        }
                        else
                        {
                            MessageBox.Show(User.error, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lỗi kết nối với hệ thống. Vui lòng kiểm tra lại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void GoToRegisterForm()
        {
            this.Hide();
            frmRegister frm = new frmRegister();
            frm.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GoToRegisterForm();
        }
    }
}
