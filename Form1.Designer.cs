namespace diplom
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            методыToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            button1 = new Button();
            button2 = new Button();
            checkBox1 = new CheckBox();
            cmb = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            button4 = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            radioButton1 = new RadioButton();
            textBox3 = new TextBox();
            label7 = new Label();
            txtLog = new RichTextBox();
            radioButton2 = new RadioButton();
            button5 = new Button();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { методыToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(119, 26);
            // 
            // методыToolStripMenuItem
            // 
            методыToolStripMenuItem.Name = "методыToolStripMenuItem";
            методыToolStripMenuItem.Size = new Size(118, 22);
            методыToolStripMenuItem.Text = "Методы";
            // 
            // menuStrip1
            // 
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // button1
            // 
            button1.Location = new Point(102, 20);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(23, 228);
            button2.Name = "button2";
            button2.Size = new Size(116, 23);
            button2.TabIndex = 5;
            button2.Text = "Шифрование";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(19, 116);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(158, 19);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "Нужно ли перезапиcать";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // cmb
            // 
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.FormattingEnabled = true;
            cmb.Items.AddRange(new object[] { "AES", "DES", "XOR" });
            cmb.Location = new Point(102, 87);
            cmb.Name = "cmb";
            cmb.Size = new Size(121, 23);
            cmb.TabIndex = 7;
            cmb.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 50);
            label1.Name = "label1";
            label1.Size = new Size(80, 15);
            label1.TabIndex = 9;
            label1.Text = "Путь к файлу";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 24);
            label2.Name = "label2";
            label2.Size = new Size(77, 15);
            label2.TabIndex = 10;
            label2.Text = "Путь к папке";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 87);
            label3.Name = "label3";
            label3.Size = new Size(62, 15);
            label3.TabIndex = 11;
            label3.Text = "Алгоритм";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 124);
            label4.Name = "label4";
            label4.Size = new Size(0, 15);
            label4.TabIndex = 12;
            // 
            // button4
            // 
            button4.Location = new Point(102, 50);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 15;
            button4.Text = "button4";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(784, 435);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 17;
            textBox1.Visible = false;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(784, 435);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 18;
            textBox2.Visible = false;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(23, 163);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(94, 19);
            radioButton1.TabIndex = 19;
            radioButton1.TabStop = true;
            radioButton1.Text = "radioButton1";
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(155, 181);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 20;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(155, 163);
            label7.Name = "label7";
            label7.Size = new Size(37, 15);
            label7.TabIndex = 21;
            label7.Text = "ключ";
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 279);
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(264, 143);
            txtLog.TabIndex = 22;
            txtLog.Text = "";
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(23, 188);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(94, 19);
            radioButton2.TabIndex = 23;
            radioButton2.TabStop = true;
            radioButton2.Text = "radioButton2";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            // 
            // button5
            // 
            button5.Location = new Point(155, 228);
            button5.Name = "button5";
            button5.Size = new Size(109, 23);
            button5.TabIndex = 24;
            button5.Text = "Дешифрование";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button5);
            Controls.Add(radioButton2);
            Controls.Add(txtLog);
            Controls.Add(label7);
            Controls.Add(textBox3);
            Controls.Add(radioButton1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button4);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cmb);
            Controls.Add(checkBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "T";
            Load += Form1_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem методыToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ProgressBar progressBar1;
        private Button button1;
        private Button button2;
        private CheckBox checkBox1;
        private ComboBox cmb;
        private Button button3;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label6;
        private Button button4;
        private ListBox listBox1;
        private TextBox textBox1;
        private TextBox textBox2;
        private RadioButton radioButton1;
        private TextBox textBox3;
        private Label label7;
        private RichTextBox txtLog;
        private RadioButton radioButton2;
        private Button button5;
    }
}