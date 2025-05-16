using System.Security.Cryptography;
using System.Windows.Forms;
using static EncryptionManager;

namespace diplom
{
    public partial class Form1 : Form
    {
        private readonly EncryptionManager _manager;
        public Form1()
        {
            InitializeComponent();




            this.Controls.Add(txtLog);
            _manager = new EncryptionManager(txtLog);
            cmb.DataSource = Enum.GetValues(typeof(EncryptionAlgorithm));
            cmb.SelectedIndex = 0;
            radioButton1.Checked = true;
            radioButton1.Text = "����������";
            radioButton2.Text = "������������";
            checkBox1.Text = "������������ �������� ����";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = dialog.SelectedPath; // ���������� ��������� ����
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog.FileName;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    textBox3.Text = GenerateRandomKey(16); // 16 �������� ��� AES-128
                    MessageBox.Show($"������������ ����: {textBox3.Text}", "����������",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                var algorithm = (EncryptionManager.EncryptionAlgorithm)cmb.SelectedItem;
                bool encrypt = radioButton1.Checked;

                txtLog.Text = algorithm.ToString();

                _manager.Process(
                textBox2.Text,
                textBox3.Text,
                algorithm,
                encrypt,
                checkBox1.Checked

                   );

                MessageBox.Show("�������� ��������� �������!", "������",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������: {ex.Message}", "������",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private string GenerateRandomKey(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}