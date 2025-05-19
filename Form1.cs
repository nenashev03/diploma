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
            cmb.DataSource = Enum.GetValues(typeof(EncryptionAlgorithm));
            cmb.SelectedIndex = 0;
            radioButton1.Text = "Шифрование";
            radioButton2.Text = "Дешифрование";
            checkBox1.Text = "Перезаписать исходный файл";
            //radioButton1.Checked = true;
            button4.Enabled = false;
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
                    textBox3.Text = GenerateRandomKey(16);
                    AppendLog($"Сгенерирован ключ: {textBox3.Text}");
                }

                var algorithm = (EncryptionAlgorithm)cmb.SelectedItem;
                _manager.Process(
                    textBox2.Text,
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
            ResetRadioButtons();

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
            // Если выбрано "Дешифрование"
            if (radioButton2.Checked)
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = true;
            }
           
        }

        private bool IsKeyValidForDecryption(string key, EncryptionAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case EncryptionAlgorithm.AES:
                    return key.Length >= 16; // AES-128 требует минимум 16 байт
                case EncryptionAlgorithm.DES:
                    return key.Length >= 8; // DES требует 8 байт
                case EncryptionAlgorithm.XOR:
                    return !string.IsNullOrEmpty(key); // XOR работает с любым ключом
                default:
                    return false;
            }
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
                _manager.Process(
                    textBox2.Text,
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
            ResetRadioButtons();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // Если выбрано "Шифрование"
            if (radioButton1.Checked)
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void ResetRadioButtons()
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;

            // Блокируем кнопки пока операция не выбрана
           button2.Enabled = true;
            button5.Enabled = true;
        }
        
    }
}