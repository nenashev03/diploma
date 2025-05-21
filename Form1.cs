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
            _manager = new EncryptionManager(message =>
            {
                if (txtLog.InvokeRequired)
                {
                    txtLog.Invoke(new Action(() => AppendLog(message)));
                }
                else
                {
                    AppendLog(message);
                }
            });
            textBox2.Clear();
            textBox3.Clear();
            textBox1.Visible=false;
            textBox2.Visible = false;
            cmb.DataSource = Enum.GetValues(typeof(EncryptionAlgorithm));
            cmb.SelectedIndex = 0;
            checkBox1.Text = "Перезаписать исходный файл";
            button4.Enabled = false;
            richTextBox1.ReadOnly = true;
            richTextBox2.ReadOnly = true;
        }
        private void AppendLog(string message)
        {
            if (message.Contains("ОШИБКА"))
            {
                txtLog.SelectionColor = Color.Red;
            }
            else if (message.Contains("завершена"))
            {
                txtLog.SelectionColor = Color.Green;
            }
            else
            {
                txtLog.SelectionColor = Color.Black;
            }

            txtLog.AppendText(message + Environment.NewLine);
            txtLog.ScrollToCaret();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
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
                    textBox1.Text = dialog.SelectedPath;
                }
            }
            button4.Enabled = true;
            textBox1.Visible = true;
            button1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string folderPath = textBox1.Text;

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show("Сначала выберите корректную папку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fullFilePath = openFileDialog.FileName;
                    textBox2.Text = fullFilePath;
                    string pathu = fullFilePath;
                    textBox2.Text = Path.GetDirectoryName(pathu); // путь к папке
                    SetFileNameOnly(pathu); // только имя файла с расширением

                    DisplayFileContent(fullFilePath, richTextBox1);
                    textBox4.Text = fullFilePath;
                }
                textBox2.Visible = true;
                button4.Visible = false;
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
                    textBox3.Text = GenerateRandomKey(16);
                    AppendLog($"Сгенерирован ключ: {textBox3.Text}");
                }

                var algorithm = (EncryptionAlgorithm)cmb.SelectedItem;
                string outputPath= _manager.Process(
                    textBox4.Text,
                    textBox3.Text,
                    algorithm,
                    encrypt: true,
                    overwrite: checkBox1.Checked
                );
            }
            catch (Exception ex)
            {
                AppendLog($"Критическая ошибка: {ex.Message}");
            }
            button1.Visible = true;
            button4.Visible = true;
            textBox1 .Visible = false;
            textBox2.Visible = false;
        }

        private string GenerateRandomKey(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    AppendLog("ОШИБКА: Введите ключ для дешифровки!");
                    return;
                }

                var algorithm = (EncryptionAlgorithm)cmb.SelectedItem;
                string outputPath=_manager.Process(
                    textBox4.Text,
                    textBox3.Text,
                    algorithm,
                    encrypt: false,
                    overwrite: checkBox1.Checked
                );
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button1.Visible = true;
            button4.Visible = true;
            textBox1.Visible = false;
            textBox2.Visible = false;

        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void DisplayFileContent(string filePath, RichTextBox richTextBox)
        {
            try
            {
                if (File.Exists(filePath))
                {
                     string content = File.ReadAllText(filePath);
                        richTextBox.Invoke((MethodInvoker)(() =>
                        {
                            richTextBox.Text = content;
                        }));
                    
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Ошибка при чтении файла: {ex.Message}");
            }
        }
        private void SetFileNameOnly(string fullFilePath)
        {
            if (!string.IsNullOrEmpty(fullFilePath))
            {
                string fileName = "\\" + Path.GetFileName(fullFilePath); // добавляем ведущий \
                textBox2.Text = fileName;
            }
        }
    }
}