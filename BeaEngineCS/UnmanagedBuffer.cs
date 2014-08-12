using System;
using System.Runtime.InteropServices;

namespace BeaEngineCS
{
  public class UnmanagedBuffer
  {
    public readonly IntPtr Ptr = IntPtr.Zero;
    public readonly int Length = 0;

    public UnmanagedBuffer(ref byte[] data)
    {
      Ptr = Marshal.AllocHGlobal(data.Length);
      Marshal.Copy(data, 0, Ptr, data.Length);
      Length = data.Length;
    }

    ~UnmanagedBuffer()
    {
      if (Ptr != IntPtr.Zero)
        Marshal.FreeHGlobal(Ptr);
    }
  }
}