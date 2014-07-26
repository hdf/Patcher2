using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Patcher2
{
  public partial class Form1 : Form
  {
    internal static class NativeMethods
    {
      [DllImport("kernel32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool AttachConsole(int dwProcessId);

      [DllImport("kernel32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool FreeConsole();

      [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
      internal static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

      /*[DllImport("uxtheme.dll")]
      private static extern int SetWindowThemeAttribute(
          IntPtr hWnd,
          WINDOWTHEMEATTRIBUTETYPE wtype,
          ref WTA_OPTIONS attributes,
          uint size);

      [StructLayout(LayoutKind.Sequential)]
      private struct WTA_OPTIONS
      {
          public WTNCA dwFlags;
          public WTNCA dwMask;
      }

      [Flags]
      private enum WTNCA : uint
      {
          NODRAWCAPTION = 1,
          NODRAWICON = 2,
          NOSYSMENU = 4,
          NOMIRRORHELP = 8,
          VALIDBITS = NODRAWCAPTION | NODRAWICON | NOSYSMENU | NOMIRRORHELP
      }

      private enum WINDOWTHEMEATTRIBUTETYPE : uint
      {
          WTA_NONCLIENT = 1,
      }*/
    }

    private const string _q = "?";
    private const string _qq = "??";
    private const string _ss = "**";
    private const string _b = ".bak";
    private static readonly string iam = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

    private static List<string[]> settings = new List<string[]>();
    private static string[] args;
    private static int isconsole = 0;
    private static List<string> seen = new List<string>();

    public Form1()
    {
      InitializeComponent();
    }

    /*protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            //cp.Style &= ~unchecked((int)0x00000080);
            return cp;
        }
    }*/

    private void Form1_Load(object sender, EventArgs e)
    {
      args = Environment.GetCommandLineArgs();
      if (args.Length < 3)
        GetSettings(AppDomain.CurrentDomain.BaseDirectory + "patcher.ini");
      else if (args.Length == 3)
        GetSettings(args[2]);
      if (args.Length > 1)
        manageConsole();

      //Icon = Icon.FromHandle(ExtractIcon(IntPtr.Zero, "shell32.dll", 50));
      Icon = Icon.FromHandle(NativeMethods.ExtractIcon(IntPtr.Zero, "imageres.dll", 142));
      ShowInTaskbar = true;
      //ShowIcon = false;
      /*WTA_OPTIONS options = new WTA_OPTIONS()
      {
          dwFlags = WTNCA.NODRAWICON,
          dwMask = WTNCA.NODRAWICON
      };
      SetWindowThemeAttribute(Handle, WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT, ref options, (uint)Marshal.SizeOf(typeof(WTA_OPTIONS)));*/

      openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
      listBox1.Items.Clear();
      for (int i = 0; i < settings.Count; i++)
        listBox1.Items.Add(settings[i][0]);
      if (listBox1.Items.Count > 0)
        listBox1.SelectedIndex = 0;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (openFileDialog1.ShowDialog() == DialogResult.OK)
        textBox1.Text = openFileDialog1.FileName;
    }

    private void button2_Click(object sender, EventArgs e)
    {
      textBox1.Text = textBox1.Text.Trim();
      if (textBox1.Text.EndsWith(_b, StringComparison.OrdinalIgnoreCase))
      {
        string file = textBox1.Text.Substring(0, textBox1.Text.Length - _b.Length);
        if (restore(file, false) == 1)
          textBox1.Text = file;
      }
      else if (doPatch(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text))
        MessageBox.Show("All done.");
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (listBox1.SelectedIndex < 0 || listBox1.SelectedIndex >= settings.Count)
        return;
      textBox1.Text = settings[listBox1.SelectedIndex][1];
      textBox2.Text = settings[listBox1.SelectedIndex][2];
      textBox3.Text = settings[listBox1.SelectedIndex][3];
      textBox4.Text = settings[listBox1.SelectedIndex][4];
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      if (textBox1.Text.EndsWith(_b, StringComparison.OrdinalIgnoreCase))
        button2.Text = "Restore";
      else
        button2.Text = "Patch";
    }

    private void Form1_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
    {
      MessageBox.Show("Any questions? Ask me!\n(Can be run with command line parameters. See \"" + iam + " --help\" for more on that.)");
      e.Cancel = true;
    }

    private void manageConsole()
    {
      try
      {
        NativeMethods.AttachConsole(-1);
        isconsole = 2;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new String(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, Console.CursorTop - 2);
      }
      catch (IOException)
      {
        isconsole = 1;
      }

      string ret = processArgs();

      if (isconsole == 2)
      {
        Console.WriteLine(ret);
        Console.SetCursorPosition(0, Console.CursorTop - 1);
        SendKeys.Send("{ENTER}");
        NativeMethods.FreeConsole();
      }
      Close();
    }

    private static string processArgs()
    {
      int patched = 0;
      string ret = " ";
      switch (args[1])
      {
        case "e":
        case "-e":
        case "/e":
          for (int i = 0; i < settings.Count; i++)
            if (doPatch(settings[i][1], settings[i][2], settings[i][3], settings[i][4]))
              patched++;
            else if (isconsole == 2)
              Console.WriteLine(settings[i][0] + " patch failed. (Nothing written.)");
          break;

        case "x":
        case "-x":
        case "/x":
          if (args.Length != 6)
            goto default;
          if (doPatch(args[2], args[3], args[4], args[5]))
            return "All done. " + args[2] + " successfully patched.\n";
          break;

        case "r":
        case "-r":
        case "/r":
          for (int i = 0; i < settings.Count; i++)
            restore(settings[i][1]);
          break;

        default:
          return "Command line options:\n\n" +
                iam + " [e [ini_file]]\n" +
                "\tRun all patches specified in the inifile.\n\n" +
                iam + " [x file search_pattern offset replace_pattern]\n" +
                "\tRun patch given by arguments.\n\n" +
                iam + " [r [ini_file]]\n" +
                "\tRestore all files mentioned in inifile from their backups.\n";
        //break;
      }
      if (patched > 0)
        ret = "All done. Patched " + patched.ToString() + " files.\n";
      return ret;
    }

    private static void GetSettings(string file)
    {
      if (!File.Exists(file))
        return;
      string[] comments = new string[] { "#", "//" };
      string[] setting = new string[5];
      string[] lines = File.ReadAllLines(file);

      int c = 0;
      for (int i = 0; i < lines.Length; i++)
      {
        lines[i] = lines[i].Trim();
        if (lines[i].Length < 1)
          continue;
        for (int j = 0; j < comments.Length; j++)
          if (lines[i].Length >= comments[j].Length && comments[j] == lines[i].Substring(0, comments[j].Length))
            goto nope;
        setting[c] = lines[i];
        if (c == 4)
        {
          settings.Add(setting);
          setting = new string[5];
          c = 0;
        }
        else
          c++;
      nope: ;
      }
    }

    private static bool doPatch(string file, string search, string offset, string replace)
    {
      // Variable setup
      int off;
      if (!Int32.TryParse(offset, out off))
        return false;
      search = search.Trim();
      replace = replace.Trim();
      string[] svals = search.Replace(_qq, _q).Replace(_ss, _q).Split(' ');
      string[] rvals = replace.Replace(_qq, _q).Replace(_ss, _q).Split(' ');

      if (!File.Exists(file))
      {
        if (isconsole == 0)
          MessageBox.Show("File not found.");
        else if (isconsole == 2)
          Console.WriteLine(file + " not found.");
        return false;
      }

      // Get file contents
      byte[] bytes = File.ReadAllBytes(file);
      // Search binary data for pattern
      int[] locs = Patcher.BinaryPatternSearch(ref bytes, svals);
      // Make sure we only have 1 match
      if (!onlyOne(locs.Length, file, search))
        return false;
      // Replace
      int replaced = Patcher.BinaryPatternReplace(ref bytes, locs[0], rvals, off);
      if (replaced < 1)
        return false;

      // Write new file
      if (!File.Exists(file + _b))
        File.Move(file, file + _b);
      File.WriteAllBytes(file, bytes);
      return true;
    }

    private static bool onlyOne(int locs, string file, string search)
    {
      if (locs == 1)
        return true;
      if (locs < 1)
      {
        if (isconsole == 0)
          MessageBox.Show("Pattern not found.");
        else if (isconsole == 2)
          Console.WriteLine("No match for pattern: \"" + search + "\" in: " + file);
      }
      else
        if (isconsole == 0)
          MessageBox.Show("More than one occurance of pattern found. This is not OK.");
        else if (isconsole == 2)
          Console.WriteLine("More than one occurance of pattern (" + search + ") found, aborting patching of " + file);
      return false;
    }

    private static int restore(string file, bool unique = true)
    {
      if (unique)
      {
        for (int i = 0; i < seen.Count; i++)
          if (file == seen[i])
            return 0;
        seen.Add(file);
      }
      if (File.Exists(file + _b))
      {
        File.Delete(file);
        File.Move(file + _b, file);
        if (isconsole == 2)
          Console.WriteLine(file + " restored.");
        else if (isconsole == 0)
          MessageBox.Show("Restored from backup.");
        return 1;
      }
      else if (isconsole == 2)
        Console.WriteLine(file + ".bak not found.");
      else if (isconsole == 0)
        MessageBox.Show("File not found.");
      return -1;
    }
  }
}