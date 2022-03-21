using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace DoAnChuyenNganh
{
    public partial class frmMain : Form
    {
        public string Username = "";
        public string Rule = "";

        Library library = new Library();

        IFirebaseClient client;

        Dictionary<string, Data> dataDic;
        List<Data> listData = new List<Data>();
        List<Data> listFilterActive = new List<Data>();
        List<Data> listFilterInActive = new List<Data>();
        List<Data> listSearch = new List<Data>();

        public frmMain()
        {
            InitializeComponent();
            ConnectFireBase();
        }

        public frmMain(string username, string rule)
        {
            InitializeComponent();
            ConnectFireBase();
            this.Username = username;
            this.Rule = rule;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbTime.Text = "Bây giờ là: " + DateTime.Now.Hour.ToString() + " : " + DateTime.Now.Minute.ToString() + " : " + DateTime.Now.Second.ToString();
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

        private void Form1_Load(object sender, EventArgs e)
        {
            lbDate.Text = library.changeDate(DateTime.Now.DayOfWeek.ToString()) + " " + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            if (Username != null && Username != "")
            {
                mnuTaiKhoan.Text = "Xin chào " + Username;
            }
            StartForm();
            GetAllData();
        }

        private string RandomID()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string a = new string(Enumerable.Repeat(chars, 4).Select(x => x[random.Next(x.Length)]).ToArray());
            string numbers = "123456789";
            string b = new string(Enumerable.Repeat(numbers, 4).Select(y => y[random.Next(y.Length)]).ToArray());
            return "-" + b + a + b + a + b;
        }

        private void StartForm()
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnRefresh.Enabled = false;
            cboStatus.SelectedIndex = 0;
            rdActive.Checked = false;
            rdInactive.Checked = false;
            cboSortASC.SelectedIndex = 0;
            cboSortDesc.SelectedIndex = 0;
            cboMax.SelectedIndex = 0;
            cboMin.SelectedIndex = 0;
        }

        private async void GetAllData()
        {
            dgvDatas.Rows.Clear();
            FirebaseResponse response = await client.GetAsync("FirebaseIOT");
            dataDic = response.ResultAs<Dictionary<string, Data>>();
            if (dataDic != null)
            {
                foreach (var item in dataDic)
                {
                    listData.Add(item.Value);
                    dgvDatas.Rows.Add(item.Key, item.Value.Temperature, item.Value.Humidity, item.Value.Status, item.Value.Day, item.Value.Time);
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu trống.Vui lòng thêm dữ liệu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshData()
        {
            txtTemperature.Focus();
            txtTemperature.Clear();
            txtHumidity.Clear();
            cboStatus.SelectedIndex = 0;
            rdActive.Checked = false;
            rdInactive.Checked = false;
            cboSortASC.SelectedIndex = 0;
            cboSortDesc.SelectedIndex = 0;
            cboMax.SelectedIndex = 0;
            cboMin.SelectedIndex = 0;
            cboMinMax.SelectedIndex = -1;
            txtMin.Text = "Min";
            txtMax.Text = "Max";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng form không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private async void dgvDatas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDatas.SelectedRows.Count > 0)
            {
                var id = dgvDatas.SelectedRows[0].Cells["Id"].Value.ToString();

                FirebaseResponse result = await client.GetAsync("FirebaseIOT/" + id);

                Data data = result.ResultAs<Data>();
                if (data != null)
                {
                    txtTemperature.Text = data.Temperature.ToString();
                    txtHumidity.Text = data.Humidity.ToString();
                    cboStatus.Text = data.Status;
                }
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnRefresh.Enabled = true;
                btnAdd.Enabled = false;
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
           if(Rule == "Admin")
            {
                btnRefresh.Enabled = true;

                float temperature = 0;
                if (txtTemperature.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập nhiệt độ !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTemperature.Focus();
                    return;
                }
                else if (Regex.IsMatch(txtTemperature.Text.Trim(), "^[a-zA-Z]"))
                {
                    MessageBox.Show("Vui lòng chỉ nhập số cho nhiệt độ. Không nhập chữ !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTemperature.Clear();
                    txtTemperature.Focus();
                    return;
                }
                else
                {
                    temperature = float.Parse(txtTemperature.Text.Trim());
                }

                float humidity = 0;
                if (txtHumidity.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập độ ẩm !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHumidity.Focus();
                    return;
                }
                else if (Regex.IsMatch(txtHumidity.Text.Trim(), "^[a-zA-Z]"))
                {
                    MessageBox.Show("Vui lòng chỉ nhập số cho độ ẩm. Không nhập chữ !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHumidity.Clear();
                    txtHumidity.Focus();
                    return;
                }
                else
                {
                    humidity = float.Parse(txtHumidity.Text.Trim());
                }

                string status = "";
                if (cboStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboStatus.Focus();
                    return;
                }
                else
                {
                    status = cboStatus.Text.Trim();
                }

                //string day = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                //string dayFormat = String.Format("{0:yyyy-MM-dd}", day);
                //string time = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                //string timeFormat = String.Format("{0:HH:mm:ss}", time);

                DateTime now = DateTime.Now;
                string day = now.ToString("yyyy-MM-dd");
                string time = now.ToString("HH:mm:ss");

                Data data = new Data();
                data.Humidity = humidity;
                data.Temperature = temperature;
                data.Status = status;
                data.Day = day;
                data.Time = time;

                try
                {
                    if (data != null)
                    {
                        SetResponse response = await client.SetAsync("FirebaseIOT/" + RandomID(), data);
                        MessageBox.Show("Thêm dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Thêm dữ liệu thất bại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    RefreshData();
                    Form1_Load(sender, e);
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
    
        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if(Rule == "Admin")
            {
                float temperature = 0;
                if (txtTemperature.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập nhiệt độ !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTemperature.Focus();
                    return;
                }
                else if (Regex.IsMatch(txtTemperature.Text.Trim(), "^[a-zA-Z]"))
                {
                    MessageBox.Show("Vui lòng chỉ nhập số cho nhiệt độ. Không nhập chữ !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTemperature.Clear();
                    txtTemperature.Focus();
                    return;
                }
                else
                {
                    temperature = float.Parse(txtTemperature.Text.Trim());
                }

                float humidity = 0;
                if (txtHumidity.Text.Trim() == "")
                {
                    MessageBox.Show("Vui lòng nhập độ ẩm !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHumidity.Focus();
                    return;
                }
                else if (Regex.IsMatch(txtHumidity.Text.Trim(), "^[a-zA-Z]"))
                {
                    MessageBox.Show("Vui lòng chỉ nhập số cho độ ẩm. Không nhập chữ !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHumidity.Clear();
                    txtHumidity.Focus();
                    return;
                }
                else
                {
                    humidity = float.Parse(txtHumidity.Text.Trim());
                }

                string status = "";
                if (cboStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn trạng thái !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cboStatus.Focus();
                    return;
                }
                else
                {
                    status = cboStatus.Text.Trim();
                }

                //string day = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
                //string dayFormat = String.Format("{0:yyyy-MM-dd}", day);
                //string time = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
                //string timeFormat = String.Format("{0:HH:mm:ss}", time);

                DateTime now = DateTime.Now;
                string day = now.ToString("yyyy-MM-dd");
                string time = now.ToString("HH:mm:ss");

                var id = dgvDatas.SelectedRows[0].Cells["Id"].Value.ToString();

                FirebaseResponse result = await client.GetAsync("FirebaseIOT/" + id);

                Data data = result.ResultAs<Data>();

                if (data != null)
                {
                    data.Temperature = temperature;
                    data.Humidity = humidity;
                    data.Status = status;
                    data.Day = day;
                    data.Time = time;
                }

                try
                {
                    if (data != null)
                    {
                        SetResponse response = await client.SetAsync("FirebaseIOT/" + id, data);
                        MessageBox.Show("Cập nhật dữ liệu thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật dữ liệu thất bại !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    RefreshData();
                    Form1_Load(sender, e);
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

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if(Rule == "Admin")
            {
                if (dgvDatas.SelectedRows.Count > 0)
                {
                    DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xoá dữ liệu này ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (confirm == DialogResult.Yes)
                    {
                        var id = dgvDatas.SelectedRows[0].Cells["Id"].Value.ToString();
                        FirebaseResponse result = await client.DeleteAsync("FirebaseIOT/" + id);
                        try
                        {
                            if (result != null)
                            {
                                MessageBox.Show("Xoá dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Xoá dữ liệu thất bại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            RefreshData();
                            Form1_Load(sender, e);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn dữ liệu muốn xoá !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Bạn không có quyền thực hiện thao tác này !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if(dgvDatas.Rows.Count > 0)
            {
                TextWriter writer = new StreamWriter(@"F:\TongHop\DoAnChuyenNganh\DoAnChuyenNganh\DoAnChuyenNganh\Files\Data.txt");
                for (int i = 0; i < dgvDatas.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dgvDatas.Columns.Count; j++)
                    {
                        if (j == dgvDatas.Columns.Count - 1)
                        {
                            writer.Write("\t" + dgvDatas.Rows[i].Cells[j].Value.ToString());
                        }
                        else
                        {
                            writer.Write("\t" + dgvDatas.Rows[i].Cells[j].Value.ToString() + "\t" + "|");
                        }
                    }
                    writer.WriteLine("");
                }
                writer.Close();
                MessageBox.Show("Xuất dữ liệu ra file txt thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Dữ liệu trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Bỏ tick Enable Adding trong DataGridView
        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            if (dgvDatas.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Data.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (Exception ex)
                        {
                            fileError = true;
                            MessageBox.Show(ex.Message,"Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable = new PdfPTable(dgvDatas.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn column in dgvDatas.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                pdfTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dgvDatas.Rows)
                            {
                                try
                                {
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        pdfTable.AddCell(cell.Value.ToString());
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Xuất File Pdf thành công", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Dữ liệu trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FilterBy(string name)
        {
            switch (name)
            {
                case "Active":
                    listFilterActive = listData.Where(x => x.Status == "Active").ToList();
                    break;
                case "Inactive":
                    listFilterInActive = listData.Where(x => x.Status == "Inactive").ToList();
                    break;
            }
        }

        private void SortByASC(string name)
        {
            switch (name)
            {
                case "Temperature":
                    listData = listData.OrderBy(x => x.Temperature).ToList();
                    break;
                case "Humidity":
                    listData = listData.OrderBy(x => x.Humidity).ToList();
                    break;
                case "Status":
                    listData = listData.OrderBy(x => x.Status).ToList();
                    break;
                case "Day":
                    listData = listData.OrderBy(x => x.Day).ToList();
                    break;
                case "Time":
                    listData = listData.OrderBy(x => x.Time).ToList();
                    break;
            }
        }

        private void SortByDESC(string name)
        {
            switch (name)
            {
                case "Temperature":
                    listData = listData.OrderByDescending(x => x.Temperature).ToList();
                    break;
                case "Humidity":
                    listData = listData.OrderByDescending(x => x.Humidity).ToList();
                    break;
                case "Status":
                    listData = listData.OrderByDescending(x => x.Status).ToList();
                    break;
                case "Day":
                    listData = listData.OrderByDescending(x => x.Day).ToList();
                    break;
                case "Time":
                    listData = listData.OrderByDescending(x => x.Time).ToList();
                    break;
            }
        }

        private void ShowList(List<Data> list)
        {
            dgvDatas.Rows.Clear();
            var count = 0;
            foreach (var item in list)
            {
                dgvDatas.Rows.Add(count++, item.Temperature, item.Humidity, item.Status, item.Day, item.Time);
            }
        }

        private void rdActive_CheckedChanged(object sender, EventArgs e)
        {
            if (rdActive.Checked)
            {
                FilterBy("Active");
            }
            ShowList(listFilterActive);
        }

        private void rdInactive_CheckedChanged(object sender, EventArgs e)
        {
            if (rdInactive.Checked)
            {
                FilterBy("Inactive");
            }
            ShowList(listFilterInActive);
        }

        private void cboSortASC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboSortASC.Text.Trim() == "Temperature" || cboSortASC.SelectedIndex == 1)
            {
                SortByASC("Temperature");
            }
            else if(cboSortASC.Text.Trim() == "Humidity" || cboSortASC.SelectedIndex == 2)
            {
                SortByASC("Humidity");
            }
            else if(cboSortASC.Text.Trim() == "Status" || cboSortASC.SelectedIndex == 3)
            {
                SortByASC("Status");
            }
            else if(cboSortASC.Text.Trim() == "Day" || cboSortASC.SelectedIndex == 4)
            {
                SortByASC("Day");
            }
            else if(cboSortASC.Text.Trim() == "Time" || cboSortASC.SelectedIndex == 5)
            {
                SortByASC("Time");
            }
            ShowList(listData);
        }

        private void cboSortDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSortDesc.Text.Trim() == "Temperature" || cboSortDesc.SelectedIndex == 1)
            {
                SortByDESC("Temperature");
            }
            else if (cboSortDesc.Text.Trim() == "Humidity" || cboSortDesc.SelectedIndex == 2)
            {
                SortByDESC("Humidity");
            }
            else if (cboSortDesc.Text.Trim() == "Status" || cboSortDesc.SelectedIndex == 3)
            {
                SortByDESC("Status");
            }
            else if(cboSortDesc.Text.Trim() == "Day" || cboSortDesc.SelectedIndex == 4)
            {
                SortByDESC("Day");
            }
            else if(cboSortDesc.Text.Trim() == "Time" || cboSortDesc.SelectedIndex == 5)
            {
                SortByDESC("Time");
            }
            ShowList(listData);
        }

        private void cboMax_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboMax.Text.Trim() == "Nhiệt độ" || cboMax.SelectedIndex == 1)
            {
                float maxTem = listData.Max(x => x.Temperature);
                MessageBox.Show("Nhiệt độ lớn nhất là: " + maxTem);
            }
            else if(cboMax.Text.Trim() == "Độ ẩm" || cboMax.SelectedIndex == 2)
            {
                float maxHum= listData.Max(x => x.Humidity);
                MessageBox.Show("Độ ẩm lớn nhất là: " + maxHum);
            }
        }

        private void cboMin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMin.Text.Trim() == "Nhiệt độ" || cboMin.SelectedIndex == 1)
            {
                float minTem = listData.Min(x => x.Temperature);
                MessageBox.Show("Nhiệt độ nhỏ nhất là: " + minTem);
            }
            else if (cboMin.Text.Trim() == "Độ ẩm" || cboMin.SelectedIndex == 2)
            {
                float minHum = listData.Min(x => x.Humidity);
                MessageBox.Show("Độ ẩm nhỏ nhất là: " + minHum);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            float min = float.Parse(txtMin.Text.Trim());
            float max = float.Parse(txtMax.Text.Trim());
            if (cboMinMax.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn dữ liệu bạn muốn tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if(cboMinMax.SelectedIndex == 0)
            {
                listSearch = listData.Where(x => x.Temperature >= min && x.Temperature <= max).ToList();
            }
            else if (cboMinMax.SelectedIndex == 1)
            {
                listSearch = listData.Where(x => x.Humidity >= min && x.Humidity <= max).ToList();
            }
            ShowList(listSearch);
        }

        private void mnuCapNhat_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmAccount frm = new frmAccount(Username, Rule);
            frm.Show();
        }

        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin frm = new frmLogin();
            frm.Show();
        }

        private void mnuEmail_Click(object sender, EventArgs e)
        {
            frmEmail frm = new frmEmail();
            frm.Show();
        }

        private void mnuDieuKhien_Click(object sender, EventArgs e)
        {
            frmControl frm = new frmControl(Rule);
            frm.Show();
        }

        private void mnuMayAnh_Click(object sender, EventArgs e)
        {
            frmCamera frm = new frmCamera();
            frm.Show();
        }

        private void mnuMayTinh_Click(object sender, EventArgs e)
        {
            frmCaculator frm = new frmCaculator();
            frm.Show();
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
                Application.Exit() ;
            }
        }
    }
}
