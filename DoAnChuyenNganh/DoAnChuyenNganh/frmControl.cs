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
using System.IO;

namespace DoAnChuyenNganh
{
    public partial class frmControl : Form
    {
        public string Rule = "";

        Library library = new Library();

        IFirebaseClient client;

        public frmControl()
        {
            InitializeComponent();
            ConnectFireBase();
        }

        public frmControl(string rule)
        {
            InitializeComponent();
            ConnectFireBase();
            this.Rule = rule;
        }

        private void ConnectFireBase()
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

                }
                else
                {
                    MessageBox.Show("Không thể kết nối đến cơ sỡ dữ liệu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void frmControl_Load(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetAsync("Devices/");
            Home data = response.ResultAs<Home>();
            if(data != null)
            {
                txtDen.Text = data.Lamp;
                txtQuat.Text = data.Fan;
                txtDieuHoa.Text = data.AirConditioner;

                if(data.Lamp == "On")
                {
                    cboDen.SelectedIndex = 0;
                    txtDen.BackColor = Color.Yellow;
                }
                else
                {
                    cboDen.SelectedIndex = 1;
                    txtDen.BackColor = Color.Red;
                }

                if (data.Fan == "On")
                {
                    cboQuat.SelectedIndex = 0;
                    txtQuat.BackColor = Color.Yellow;
                }
                else
                {
                    cboQuat.SelectedIndex = 1;
                    txtQuat.BackColor = Color.Red;
                }

                if (data.AirConditioner == "On")
                {
                    cboDieuHoa.SelectedIndex = 0;
                    txtDieuHoa.BackColor = Color.Yellow;
                }
                else
                {
                    cboDieuHoa.SelectedIndex = 1;
                    txtDieuHoa.BackColor = Color.Red;
                }
            }
            else
            {
                MessageBox.Show("Hệ thống đang gặp lỗi. Vui lòng thử lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if(Rule == "Admin")
            {
                string statusLamp = "";
                string statusFan = "";
                string statusAirConditioner = "";
                if (cboDen.SelectedIndex == 0)
                {
                    statusLamp = "On";
                }
                else if (cboDen.SelectedIndex == 1)
                {
                    statusLamp = "Off";
                }
                if (cboQuat.SelectedIndex == 0)
                {
                    statusFan = "On";
                }
                else if (cboQuat.SelectedIndex == 1)
                {
                    statusFan = "Off";
                }
                if (cboDieuHoa.SelectedIndex == 0)
                {
                    statusAirConditioner = "On";
                }
                else if (cboDieuHoa.SelectedIndex == 1)
                {
                    statusAirConditioner = "Off";
                }

                Home data = new Home(statusLamp, statusFan, statusAirConditioner);

                try
                {
                    if (data != null)
                    {
                        SetResponse response = await client.SetAsync("Devices/", data);
                        MessageBox.Show("Cập nhật trạng thái nhà thông minh thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật trạng thái nhà thông minh thất bại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    frmControl_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng form không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void ToolStripColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panel1.BackColor;
            colorDialog1.AllowFullOpen = true;
            colorDialog1.FullOpen = true;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
            }
        }

        private void ToolStripFont_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = panel1.Font;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.Font = fontDialog1.Font;
            }
        }

        private void ToolStripExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng form không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
