using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Patcher2
{
  public partial class Form2 : Form
  {
    private ListViewColumnSorter ColumnSorter;
    private string _p;

    public string Prefix
    {
      set
      {
        _p = value;
      }
      get
      {
        return _p;
      }
    }

    public Form2()
    {
      InitializeComponent();
      ColumnSorter = new ListViewColumnSorter();
      this.listView1.ListViewItemSorter = ColumnSorter;
    }

    private void Form2_Load(object sender, EventArgs e)
    {
      listView1.Items.Clear();
      Process[] procs = Process.GetProcesses();
      listView1.SmallImageList = new ImageList();
      listView1.SmallImageList.ImageSize = new Size(22, 22);
      listView1.SmallImageList.ColorDepth = ColorDepth.Depth32Bit;
      for (int i = 0; i < procs.Length; i++)
      {
        try
        {
          listView1.SmallImageList.Images.Add(procs[i].ProcessName, Icon.ExtractAssociatedIcon(procs[i].MainModule.FileName));
          listView1.Items.Add(new ListViewItem(new string[] { "", procs[i].Id.ToString(), procs[i].ProcessName }, procs[i].ProcessName));
        }
        catch (System.ComponentModel.Win32Exception) { }
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (listView1.SelectedIndices[0] >= 0 && listView1.SelectedIndices[0] < listView1.Items.Count)
        ((Form1)Owner).textBox1.Text = _p + listView1.SelectedItems[0].SubItems[2].Text + ":" + listView1.SelectedItems[0].SubItems[1].Text;
      Close();
    }

    private void listView1_DoubleClick(object sender, EventArgs e)
    {
      button1_Click(sender, e);
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column == 0)
        return;
      else if (e.Column == 1)
        ColumnSorter.NumSort = true;
      else
        ColumnSorter.NumSort = false;
      if (e.Column == ColumnSorter.SortColumn)
      {
        // Reverse the current sort direction for this column.
        if (ColumnSorter.Order == SortOrder.Ascending)
          ColumnSorter.Order = SortOrder.Descending;
        else
          ColumnSorter.Order = SortOrder.Ascending;
      }
      else
      {
        // Set the column number that is to be sorted; default to ascending.
        ColumnSorter.SortColumn = e.Column;
        ColumnSorter.Order = SortOrder.Ascending;
      }
      // Perform the sort with these new sort options.
      listView1.Sort();
    }
  }
}