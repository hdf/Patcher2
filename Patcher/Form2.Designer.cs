namespace Patcher2
{
  partial class Form2
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
      this.button1 = new System.Windows.Forms.Button();
      this.listView1 = new System.Windows.Forms.ListView();
      this.Img = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.PID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.PN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.button1.Location = new System.Drawing.Point(102, 228);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(82, 26);
      this.button1.TabIndex = 1;
      this.button1.Text = "Select";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // listView1
      // 
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Img,
            this.PID,
            this.PN});
      this.listView1.FullRowSelect = true;
      this.listView1.GridLines = true;
      this.listView1.LabelWrap = false;
      this.listView1.Location = new System.Drawing.Point(12, 12);
      this.listView1.MultiSelect = false;
      this.listView1.Name = "listView1";
      this.listView1.ShowGroups = false;
      this.listView1.Size = new System.Drawing.Size(260, 206);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
      this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
      // 
      // Img
      // 
      this.Img.Text = "";
      this.Img.Width = 28;
      // 
      // PID
      // 
      this.PID.Text = "PID";
      this.PID.Width = 40;
      // 
      // PN
      // 
      this.PN.Text = "ProcessName";
      this.PN.Width = 170;
      // 
      // Form2
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.listView1);
      this.Controls.Add(this.button1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form2";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Process List";
      this.Load += new System.EventHandler(this.Form2_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ColumnHeader PID;
    private System.Windows.Forms.ColumnHeader PN;
    private System.Windows.Forms.ColumnHeader Img;
  }
}