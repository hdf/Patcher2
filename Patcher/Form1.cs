using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text;
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

      [DllImport("user32.dll")]
      internal static extern IntPtr GetForegroundWindow();

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool SetForegroundWindow(IntPtr hWnd);

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool AllowSetForegroundWindow(int dwProcessId);

      [DllImport("user32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool BringWindowToTop(IntPtr hWnd);

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

      internal const uint SW_SHOW = 5;

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

      [Flags]
      internal enum ProcessAccessFlags : uint
      {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DupHandle = 0x00000040,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        Synchronize = 0x00100000
      }

      [Flags]
      internal enum Protection : uint
      {
        PAGE_NOACCESS = 0x01,
        PAGE_READONLY = 0x02,
        PAGE_READWRITE = 0x04,
        PAGE_WRITECOPY = 0x08,
        PAGE_EXECUTE = 0x10,
        PAGE_EXECUTE_READ = 0x20,
        PAGE_EXECUTE_READWRITE = 0x40,
        PAGE_EXECUTE_WRITECOPY = 0x80,
        PAGE_GUARD = 0x100,
        PAGE_NOCACHE = 0x200,
        PAGE_WRITECOMBINE = 0x400
      }

      [DllImport("kernel32.dll")]
      internal static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

      [DllImport("kernel32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool CloseHandle(IntPtr hProcess);

      [DllImport("kernel32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool ReadProcessMemory(IntPtr handle, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr nSize, ref int lpNumberOfBytesRead);

      [DllImport("kernel32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr nSize, ref int lpNumberOfBytesWritten);

      [DllImport("kernel32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      internal static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flNewProtect, ref uint lpflOldProtect);
    }

    private const string _q = "?";
    private const string _qq = "??";
    private const string _ss = "**";
    private const string _b = ".bak";
    private const string _p = "proc:";
    private const string gz = ".gz";
    private static readonly string iam = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
    private static IntPtr consoleWindow;

    private static List<string[]> settings = new List<string[]>();
    private static string[] args;
    private static int isconsole = 0;
    private static List<string> seen = new List<string>();
    private static string forTxtBox4;

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
      forTxtBox4 = "";
      textBox1.Text = textBox1.Text.Trim();
      if (textBox1.Text.EndsWith(_b, StringComparison.OrdinalIgnoreCase))
      {
        string file = textBox1.Text.Substring(0, textBox1.Text.Length - _b.Length);
        if (restore(file, false) == 1)
          textBox1.Text = file;
      }
      else if (doPatch(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text))
        MessageBox.Show("All done.");
      if (forTxtBox4.Length > 0)
        textBox4.Text = forTxtBox4;
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

      if (textBox1.Text.ToLower() != _p)
        return;
      Form2 form2 = new Form2();
      form2.Prefix = _p;
      form2.Show(this);
    }

    private void textBox4_TextChanged(object sender, EventArgs e)
    {
      if (textBox4.Text.Length > 2 && textBox4.Text.Substring(0, 1) != "?" && textBox4.Text.Substring(2, 1) != " ")
        button2.Enabled = false;
      else
        button2.Enabled = true;
      if (textBox4.Text.Length == 0)
        button2.Text = "Locate";
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
        consoleWindow = NativeMethods.GetForegroundWindow();
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
        // All useless on Win7
        NativeMethods.AllowSetForegroundWindow(-1);
        NativeMethods.ShowWindow(consoleWindow, NativeMethods.SW_SHOW);
        NativeMethods.BringWindowToTop(consoleWindow);
        NativeMethods.SetForegroundWindow(consoleWindow);
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

        case "m":
        case "-m":
        case "/m":
          if (args.Length != 5)
            goto default;
          if (!File.Exists(args[3]) || !File.Exists(args[4]))
            break;
          if ((new FileInfo(args[3])).Length != (new FileInfo(args[4])).Length)
          {
            if (isconsole == 2)
              Console.WriteLine("Not equal size!");
            break;
          }
          Spinner.Start();
          byte[] bytes1 = File.ReadAllBytes(args[3]);
          byte[] bytes2 = File.ReadAllBytes(args[4]);
          if (args[2].Length > gz.Length && args[2].Substring(args[2].Length - gz.Length) == gz)
            CompressINI(args[2], Patcher.Format4Ini(Patcher.FindDiffs(ref bytes1, ref bytes2), args[3], "Patch"));
          else
            File.WriteAllText(args[2], Patcher.Format4Ini(Patcher.FindDiffs(ref bytes1, ref bytes2), args[3], "Patch"));
          Spinner.Stop();
          ret = args[2] + " written.\n";
          break;

        default:
          return "Command line options:\n\n" +
                iam + " [e [ini_file]]\n" +
                "\tRun all patches specified in the inifile.\n\n" +
                iam + " [x file search_pattern offset replace_pattern]\n" +
                "\tRun patch given by arguments.\n\n" +
                iam + " [r [ini_file]]\n" +
                "\tRestore all files mentioned in inifile from their backups.\n\n" +
                iam + " [m out_ini_file[" + gz + "] original_file changed_file]\n" +
                "\tTry to create a pattern based patch for original_file. (Don't get your hopes up.)\n";
      }
      if (patched > 0)
        ret = "All done. " + patched.ToString() + " patches executed.\n";
      return ret;
    }

    private static void GetSettings(string file)
    {
      if (!File.Exists(file))
        if (File.Exists(file + gz))
          file = file + gz;
        else
          return;
      string[] comments = new string[] { "#", "//" };
      string[] setting = new string[5];
      string[] lines;
      if (file.Substring(file.Length - gz.Length) == gz)
        lines = ExtractINI(file);
      else
        lines = File.ReadAllLines(file);

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

    private static void CompressINI(string path, string iniContents)
    {
      byte[] bytes = Encoding.Default.GetBytes(iniContents);
      FileStream dest = File.Create(path);
      GZipStream output = new GZipStream(dest, CompressionMode.Compress);
      output.Write(bytes, 0, bytes.Length);
      output.Close();
      dest.Close();
    }

    private static string[] ExtractINI(string path)
    {
      MemoryStream input = new MemoryStream(File.ReadAllBytes(path));
      MemoryStream dest = new MemoryStream();
      (new GZipStream(input, CompressionMode.Decompress)).CopyTo(dest);
      return Encoding.Default.GetString(dest.ToArray()).Split(new string[] { "\r\n" }, StringSplitOptions.None);
    }

    private static bool findOnly(string file, ref string[] svals)
    {
      // I know, an ugly clone, but refactoring would not actually be very efficient here.
      byte[] bytes;
      if (file.Length > 5 && file.Substring(0, 5).ToLower() == _p)
      {
        string[] processName = file.Split(':');
        Process proc;
        if (processName.Length == 3)
          proc = Process.GetProcessById(int.Parse(processName[2]));
        else
          proc = Process.GetProcessesByName(processName[1])[0];
        file = proc.ProcessName;
        IntPtr hProc = NativeMethods.OpenProcess(NativeMethods.ProcessAccessFlags.All, false, proc.Id);
        // Read bytes
        int bytesRead = 0;
        bytes = new byte[proc.MainModule.ModuleMemorySize];
        if (!NativeMethods.ReadProcessMemory(hProc, proc.MainModule.BaseAddress, bytes, (IntPtr)proc.MainModule.ModuleMemorySize, ref bytesRead) || bytes == null)
        {
          NativeMethods.CloseHandle(hProc);
          return false;
        }
        NativeMethods.CloseHandle(hProc);
      }
      else if (!File.Exists(file))
      {
        forTxtBox4 = "File not found.";
        return false;
      }
      else
        bytes = File.ReadAllBytes(file);

      int[] locs = Patcher.BinaryPatternSearch(ref bytes, svals);

      if (locs.Length == 1)
        forTxtBox4 = string.Format("Pattern found at: {0}+{1:X8}", file, locs[0]);
      else if (locs.Length < 1)
        forTxtBox4 = "Pattern not found.";
      else
        forTxtBox4 = "Pattern not unique.";
      return false;
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

      if (replace.Length == 0)
        return findOnly(file, ref svals);

      // MemPatch! :) (Highly experimental!)
      if (file.Length > 5 && file.Substring(0, 5).ToLower() == _p)
        return doMemPatch(file.Split(':'), svals, off, rvals);

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

    private static bool doMemPatch(string[] processName, string[] svals, int offset, string[] rvals)
    {
      // Get process
      Process proc;
      if (processName.Length == 3)
        proc = Process.GetProcessById(int.Parse(processName[2]));
      else
        proc = Process.GetProcessesByName(processName[1])[0];
      IntPtr baseAddress = proc.MainModule.BaseAddress;
      IntPtr moduleSize = (IntPtr)proc.MainModule.ModuleMemorySize;

      IntPtr hProc = NativeMethods.OpenProcess(NativeMethods.ProcessAccessFlags.All, false, proc.Id);

      // Read bytes
      int bytesRead = 0;
      byte[] bytes = new byte[moduleSize.ToInt32()];
      if (!NativeMethods.ReadProcessMemory(hProc, baseAddress, bytes, moduleSize, ref bytesRead) || bytes == null)
        goto badEnding;

      // Search binary data for pattern
      int[] locs = Patcher.BinaryPatternSearch(ref bytes, svals);
      // Make sure we only have 1 match
      if (!onlyOne(locs.Length, processName[1], String.Join(" ", svals)))
        goto badEnding;
      // Replace
      int replaced = Patcher.BinaryPatternReplace(ref bytes, locs[0], rvals, offset);
      if (replaced < 1)
        goto badEnding;

      // Write bytes
      int bytesWritten = 0;
      uint oldp = 0;
      uint oldp2 = 0;
      byte[] newbytes = new byte[rvals.Length];
      IntPtr off = baseAddress + locs[0] + offset;
      // Get write privileges
      if (!NativeMethods.VirtualProtectEx(proc.Handle, off, (IntPtr)rvals.Length, (uint)NativeMethods.Protection.PAGE_EXECUTE_WRITECOPY, ref oldp))
        goto badEnding;
      // Do actual write
      Array.Copy(bytes, locs[0] + offset, newbytes, 0, rvals.Length);
      NativeMethods.WriteProcessMemory(hProc, off, newbytes, (IntPtr)rvals.Length, ref bytesWritten);
      // Set original access rights back (we have no buisness getting errors here)
      NativeMethods.VirtualProtectEx(proc.Handle, off, (IntPtr)rvals.Length, oldp, ref oldp2);
      // Were we successful?
      if (bytesWritten != rvals.Length)
        goto badEnding;

      // Happy ending. :)
      NativeMethods.CloseHandle(hProc);
      return true;
    badEnding:
      NativeMethods.CloseHandle(hProc);
      return false;
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