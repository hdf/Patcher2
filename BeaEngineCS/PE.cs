using System;
using System.IO;
using System.Runtime.InteropServices;

#pragma warning disable 649

namespace BeaEngineCS
{
  public static class PeHeader
  {
    public static IMAGE_SECTION_HEADER[] section;

    public struct IMAGE_DOS_HEADER
    { // DOS .EXE header
      public UInt16 e_magic;              // Magic number
      public UInt16 e_cblp;               // Bytes on last page of file
      public UInt16 e_cp;                 // Pages in file
      public UInt16 e_crlc;               // Relocations
      public UInt16 e_cparhdr;            // Size of header in paragraphs
      public UInt16 e_minalloc;           // Minimum extra paragraphs needed
      public UInt16 e_maxalloc;           // Maximum extra paragraphs needed
      public UInt16 e_ss;                 // Initial (relative) SS value
      public UInt16 e_sp;                 // Initial SP value
      public UInt16 e_csum;               // Checksum
      public UInt16 e_ip;                 // Initial IP value
      public UInt16 e_cs;                 // Initial (relative) CS value
      public UInt16 e_lfarlc;             // File address of relocation table
      public UInt16 e_ovno;               // Overlay number
      public UInt16 e_res_0;              // Reserved words
      public UInt16 e_res_1;              // Reserved words
      public UInt16 e_res_2;              // Reserved words
      public UInt16 e_res_3;              // Reserved words
      public UInt16 e_oemid;              // OEM identifier (for e_oeminfo)
      public UInt16 e_oeminfo;            // OEM information; e_oemid specific
      public UInt16 e_res2_0;             // Reserved words
      public UInt16 e_res2_1;             // Reserved words
      public UInt16 e_res2_2;             // Reserved words
      public UInt16 e_res2_3;             // Reserved words
      public UInt16 e_res2_4;             // Reserved words
      public UInt16 e_res2_5;             // Reserved words
      public UInt16 e_res2_6;             // Reserved words
      public UInt16 e_res2_7;             // Reserved words
      public UInt16 e_res2_8;             // Reserved words
      public UInt16 e_res2_9;             // Reserved words
      public UInt32 e_lfanew;             // File address of new exe header
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_DATA_DIRECTORY
    {
      public UInt32 VirtualAddress;
      public UInt32 size;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_OPTIONAL_HEADER
    {
      public UInt16 Magic;
      public Byte MajorLinkerVersion;
      public Byte MinorLinkerVersion;
      public UInt32 SizeOfCode;
      public UInt32 SizeOfInitializedData;
      public UInt32 SizeOfUninitializedData;
      public UInt32 AddressOfEntryPoint;
      public UInt32 BaseOfCode;
      public UInt32 BaseOfData;
      public UInt32 ImageBase;
      public UInt32 SectionAlignment;
      public UInt32 FileAlignment;
      public UInt16 MajorOperatingSystemVersion;
      public UInt16 MinorOperatingSystemVersion;
      public UInt16 MajorImageVersion;
      public UInt16 MinorImageVersion;
      public UInt16 MajorSubsystemVersion;
      public UInt16 MinorSubsystemVersion;
      public UInt32 Win32VersionValue;
      public UInt32 SizeOfImage;
      public UInt32 SizeOfHeaders;
      public UInt32 CheckSum;
      public UInt16 Subsystem;
      public UInt16 DllCharacteristics;
      public UInt32 SizeOfStackReserve;
      public UInt32 SizeOfStackCommit;
      public UInt32 SizeOfHeapReserve;
      public UInt32 SizeOfHeapCommit;
      public UInt32 LoaderFlags;
      public UInt32 NumberOfRvaAndSizes;
      public IMAGE_DATA_DIRECTORY ExportTable;
      public IMAGE_DATA_DIRECTORY ImportTable;
      public IMAGE_DATA_DIRECTORY ResTable;
      public IMAGE_DATA_DIRECTORY ExceptTable;
      public IMAGE_DATA_DIRECTORY CertTable;
      public IMAGE_DATA_DIRECTORY RelocTable;
      public IMAGE_DATA_DIRECTORY DebugTable;
      public IMAGE_DATA_DIRECTORY ArchTable;
      public IMAGE_DATA_DIRECTORY MachineTable;
      public IMAGE_DATA_DIRECTORY TLSTable;
      public IMAGE_DATA_DIRECTORY LoadConfigTable;
      public IMAGE_DATA_DIRECTORY BoundImportTable;
      public IMAGE_DATA_DIRECTORY ImportAddressTable;
      public IMAGE_DATA_DIRECTORY DelayImportTable;
      public IMAGE_DATA_DIRECTORY COMRuntimeTable;
      public IMAGE_DATA_DIRECTORY ReservedTable;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_FILE_HEADER
    {
      public UInt16 Machine;
      public UInt16 NumberOfSections;
      public UInt32 TimeDateStamp;
      public UInt32 PointerToSymbolTable;
      public UInt32 NumberOfSymbols;
      public UInt16 SizeOfOptionalHeader;
      public UInt16 Characteristics;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_NT_HEADERS
    {
      public char Signature0;
      public char Signature1;
      public char Signature2;
      public char Signature3;
      public IMAGE_FILE_HEADER FileHeader;
      public IMAGE_OPTIONAL_HEADER OptionalHeader;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_SECTION_HEADER
    {
      public char Name0;
      public char Name1;
      public char Name2;
      public char Name3;
      public char Name4;
      public char Name5;
      public UInt16 PhisicalAddress;
      public UInt32 VirtualSize;
      public UInt32 VirtualAddress;
      public UInt32 SizeOfRawData;
      public UInt32 PointerToRawData;
      public UInt32 PointerToRelocations;
      public UInt32 PointerToLinenumbers;
      public UInt16 NumberOfRelocations;
      public UInt16 NumberOfLinenumbers;
      public UInt32 Characteristics;
    }

    public static T ReadStruct<T>(Stream s)
    {
      byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
      s.Read(buffer, 0, Marshal.SizeOf(typeof(T)));
      GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
      T temp = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
      handle.Free();
      return temp;
    }

    private static string GetSectName(IMAGE_SECTION_HEADER s)
    {
      return s.Name0.ToString() + s.Name1 + s.Name2 + s.Name3 + s.Name4 + s.Name5; // stooopid
    }

    public static uint[] RvaToOffset(uint rva)
    {
      string s = "";
      return RvaToOffset(rva, ref s);
    }

    public static uint[] RvaToOffset(uint rva, ref string sectionName)
    {
      for (int i = 0; i < section.Length; i++)
      {
        IMAGE_SECTION_HEADER c = section[i];
        uint vBase = c.VirtualAddress;
        uint vSize = c.VirtualSize;
        uint vMax = vBase + vSize;

        if (rva >= vBase && rva < vMax)
        {
          sectionName = GetSectName(c);
          return new uint[] { rva - vBase + c.PointerToRawData, c.PointerToRawData, c.SizeOfRawData };
        }
      }
      return new uint[] { };
    }

    public static uint OffsetToRVA(uint foffset)
    {
      string s = "";
      return OffsetToRVA(foffset, ref s);
    }

    public static uint OffsetToRVA(uint foffset, ref string sectName)
    {
      IMAGE_SECTION_HEADER c;
      uint rBase, rSize, rMax, i;

      for (i = 0; i < section.Length; i++)
      {
        c = section[i];
        rBase = c.PointerToRawData;
        rSize = c.SizeOfRawData;
        rMax = rBase + rSize;

        if (foffset >= rBase && foffset < rMax)
        {
          sectName = GetSectName(c);
          return c.VirtualAddress + foffset - rBase;
        }
      }
      return 0;
    }
  }
}