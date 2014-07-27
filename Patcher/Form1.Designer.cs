namespace Patcher2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
      this.label1 = new System.Windows.Forms.Label();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.button1 = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.textBox4 = new System.Windows.Forms.TextBox();
      this.button2 = new System.Windows.Forms.Button();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(8, 112);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(56, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select file:";
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(8, 128);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(182, 20);
      this.textBox1.TabIndex = 1;
      this.textBox1.Text = "a.txt";
      this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(196, 127);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(64, 20);
      this.button1.TabIndex = 2;
      this.button1.Text = "Browse";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(8, 151);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(80, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Search pattern:";
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(8, 167);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(182, 20);
      this.textBox2.TabIndex = 4;
      this.textBox2.Text = "? 63 64 65 ?";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(191, 151);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(38, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Offset:";
      // 
      // textBox3
      // 
      this.textBox3.Location = new System.Drawing.Point(194, 167);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(66, 20);
      this.textBox3.TabIndex = 6;
      this.textBox3.Text = "-1";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(8, 190);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(72, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Replace with:";
      // 
      // textBox4
      // 
      this.textBox4.Location = new System.Drawing.Point(8, 206);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new System.Drawing.Size(252, 20);
      this.textBox4.TabIndex = 8;
      this.textBox4.Text = "41";
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(86, 232);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(104, 24);
      this.button2.TabIndex = 9;
      this.button2.Text = "Patch";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // listBox1
      // 
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7"});
      this.listBox1.Location = new System.Drawing.Point(8, 8);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(252, 95);
      this.listBox1.TabIndex = 10;
      this.listBox1.TabStop = false;
      this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(268, 262);
      this.Controls.Add(this.listBox1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.textBox4);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.textBox3);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.HelpButton = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Patcher";
      this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.Form1_HelpButtonClicked);
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBox1;
        internal System.Windows.Forms.TextBox textBox1;
    }
}

