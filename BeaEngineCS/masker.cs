using System;
using System.Collections.Generic;
using System.IO;

namespace BeaEngineCS
{
  public static class masker
  {
    public static uint[] GetRVA(ref byte[] bytes)
    {
      Stream s = new MemoryStream(bytes);
      PeHeader.IMAGE_DOS_HEADER idh = PeHeader.ReadStruct<PeHeader.IMAGE_DOS_HEADER>(s);
      s.Seek(idh.e_lfanew, SeekOrigin.Begin);
      PeHeader.IMAGE_NT_HEADERS ntHeader = PeHeader.ReadStruct<PeHeader.IMAGE_NT_HEADERS>(s);

      if (ntHeader.Signature0 != 'P' && ntHeader.Signature1 != 'E') // Not a PE file
        return new uint[] { };

      PeHeader.section = new PeHeader.IMAGE_SECTION_HEADER[ntHeader.FileHeader.NumberOfSections];
      while (bytes[s.Position] != 46) // 64 bit buisness?
        s.Seek(s.Position + 1, SeekOrigin.Begin);
      for (int i = 0; i < PeHeader.section.Length; i++)
        PeHeader.section[i] = PeHeader.ReadStruct<PeHeader.IMAGE_SECTION_HEADER>(s);
      return PeHeader.RvaToOffset(ntHeader.OptionalHeader.AddressOfEntryPoint);
    }

    public static int[] GetAddressMaskLocs(ref byte[] bytes)
    {
      return GetAddressMaskLocs(ref bytes, GetRVA(ref bytes));
    }

    public static int[] GetAddressMaskLocs(ref byte[] bytes, uint[] rva)
    {
      if (rva.Length < 3)
        return new int[] { };
      UnmanagedBuffer buffer = new UnmanagedBuffer(ref bytes);

      BeaEngine._Disasm disasm = new BeaEngine._Disasm();
      ulong begin = (ulong)buffer.Ptr.ToInt64();
      ulong end = begin + (ulong)(rva[1] + rva[2]);
      disasm.InstructionPointer = (UIntPtr)(begin + rva[1]);
      int result, off, n;

      List<int> locs = new List<int>();
      while (disasm.InstructionPointer.ToUInt64() < end)
      {
        result = BeaEngine.Disassemble(ref disasm, true);
        if (result == BeaEngine.UnknownOpcode) // This is not good practice, but not sure, what else I can do
        {
          Console.WriteLine("UnknownOpcode (" + bytes[(int)(disasm.InstructionPointer.ToUInt64() - begin)].ToString("X2") + ") at: " + (disasm.InstructionPointer.ToUInt64() - begin).ToString() + " skipping 1 byte.");
          result = 1;
        }
        if (result == BeaEngine.OutOfBlock)
          break;
        //if (result < 5) // Does not contain address
        if (result > 4) // Mask addresses
        {
          off = result % 4;
          n = result - off;
          off += (int)(disasm.InstructionPointer.ToUInt64() - begin);
          for (int i = 0; i < n; i++)
            locs.Add(off + i);
        }
        disasm.InstructionPointer = (UIntPtr)(disasm.InstructionPointer.ToUInt64() + (ulong)result);
      }
      return locs.ToArray();
    }
  }
}