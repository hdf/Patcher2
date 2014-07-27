using System;
using System.Collections;
using System.Windows.Forms;

namespace Patcher2
{
  public class ListViewColumnSorter : IComparer
  {
    private int ColumnToSort;
    private SortOrder OrderOfSort;
    private CaseInsensitiveComparer ObjectCompare;
    private bool numCompare;

    public ListViewColumnSorter()
    {
      ColumnToSort = 0;
      OrderOfSort = SortOrder.None;
      numCompare = false;
      ObjectCompare = new CaseInsensitiveComparer();
    }

    /// <summary>
    /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
    /// </summary>
    /// <param name="x">First object to be compared</param>
    /// <param name="y">Second object to be compared</param>
    /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
    public int Compare(object x, object y)
    {
      int compareResult;
      ListViewItem listviewX, listviewY;
      // Cast the objects to be compared to ListViewItem objects
      listviewX = (ListViewItem)x;
      listviewY = (ListViewItem)y;
      // Compare the two items
      if (numCompare)
        compareResult = Math.Sign(Int32.Parse(listviewX.SubItems[ColumnToSort].Text).CompareTo(Int32.Parse(listviewY.SubItems[ColumnToSort].Text)));
      else
        compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
      // Calculate correct return value based on object comparison
      if (OrderOfSort == SortOrder.Ascending)
        return compareResult; // Ascending sort is selected, return normal result of compare operation
      else if (OrderOfSort == SortOrder.Descending)
        return (-compareResult); // Descending sort is selected, return negative result of compare operation
      else
        return 0; // Return '0' to indicate they are equal
    }

    public int SortColumn
    {
      set
      {
        ColumnToSort = value;
      }
      get
      {
        return ColumnToSort;
      }
    }

    public SortOrder Order
    {
      set
      {
        OrderOfSort = value;
      }
      get
      {
        return OrderOfSort;
      }
    }

    public bool NumSort
    {
      set
      {
        numCompare = value;
      }
      get
      {
        return numCompare;
      }
    }
  }
}