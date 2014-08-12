using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UInt8 = System.Byte;

namespace BeaEngineCS
{
  internal static class NativeMethods
  {
    [DllImport("BeaEngine64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Disasm")]
    internal static extern int Disassemble64(ref BeaEngine._Disasm instruction);

    [DllImport("BeaEngine.dll", EntryPoint = "Disasm")]
    internal static extern int Disassemble32(ref BeaEngine._Disasm instruction);

    [DllImport("BeaEngine64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "BeaEngineVersion")]
    internal static extern IntPtr BeaEngineVersion64();

    [DllImport("BeaEngine.dll", EntryPoint = "BeaEngineVersion")]
    internal static extern IntPtr BeaEngineVersion32();

    [DllImport("BeaEngine64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "BeaEngineRevision")]
    internal static extern IntPtr BeaEngineRevision64();

    [DllImport("BeaEngine.dll", EntryPoint = "BeaEngineRevision")]
    internal static extern IntPtr BeaEngineRevision32();

    [DllImport("BeaEngineCheetah64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Disasm")]
    internal static extern int Disassemble64Cheetah(ref BeaEngine._Disasm instruction);

    [DllImport("BeaEngineCheetah.dll", EntryPoint = "Disasm")]
    internal static extern int Disassemble32Cheetah(ref BeaEngine._Disasm instruction);

    [DllImport("BeaEngine64Cheetah.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "BeaEngineVersion")]
    internal static extern IntPtr BeaEngineVersion64Cheetah();

    [DllImport("BeaEngineCheetah.dll", EntryPoint = "BeaEngineVersion")]
    internal static extern IntPtr BeaEngineVersion32Cheetah();

    [DllImport("BeaEngine64Cheetah.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "BeaEngineRevision")]
    internal static extern IntPtr BeaEngineRevision64Cheetah();

    [DllImport("BeaEngineCheetah.dll", EntryPoint = "BeaEngineRevision")]
    internal static extern IntPtr BeaEngineRevision32Cheetah();
  }

  public static class BeaEngine
  {
    #region Constants

    public const byte ESReg = 1;
    public const byte DSReg = 2;
    public const byte FSReg = 3;
    public const byte GSReg = 4;
    public const byte CSReg = 5;
    public const byte SSReg = 6;

    public const byte InvalidPrefix = 4;
    public const byte SuperfluousPrefix = 2;
    public const byte NotUsedPrefix = 0;
    public const byte MandatoryPrefix = 8;
    public const byte InUsePrefix = 1;

    public const byte LowPosition = 0;
    public const byte HighPosition = 1;

    public const int UnknownOpcode = -1;
    public const int OutOfBlock = 0;
    public const int InstructionLength = 64;

    #endregion Constants

    #region Enumerations

    [Flags]
    public enum InstructionSet
    {
      GENERAL_PURPOSE_INSTRUCTION = 0x10000,
      FPU_INSTRUCTION = 0x20000,
      MMX_INSTRUCTION = 0x40000,
      SSE_INSTRUCTION = 0x80000,
      SSE2_INSTRUCTION = 0x100000,
      SSE3_INSTRUCTION = 0x200000,
      SSSE3_INSTRUCTION = 0x400000,
      SSE41_INSTRUCTION = 0x800000,
      SSE42_INSTRUCTION = 0x1000000,
      SYSTEM_INSTRUCTION = 0x2000000,
      VM_INSTRUCTION = 0x4000000,
      UNDOCUMENTED_INSTRUCTION = 0x8000000,
      AMD_INSTRUCTION = 0x10000000,
      ILLEGAL_INSTRUCTION = 0x20000000,
      AES_INSTRUCTION = 0x40000000,
      CLMUL_INSTRUCTION = unchecked((int)0x80000000),
    }

    public enum InstructionType
    {
      DATA_TRANSFER = 0x1,
      ARITHMETIC_INSTRUCTION,
      LOGICAL_INSTRUCTION,
      SHIFT_ROTATE,
      BIT_UInt8,
      CONTROL_TRANSFER,
      STRING_INSTRUCTION,
      InOutINSTRUCTION,
      ENTER_LEAVE_INSTRUCTION,
      FLAG_CONTROL_INSTRUCTION,
      SEGMENT_REGISTER,
      MISCELLANEOUS_INSTRUCTION,
      COMPARISON_INSTRUCTION,
      LOGARITHMIC_INSTRUCTION,
      TRIGONOMETRIC_INSTRUCTION,
      UNSUPPORTED_INSTRUCTION,
      LOAD_CONSTANTS,
      FPUCONTROL,
      STATE_MANAGEMENT,
      CONVERSION_INSTRUCTION,
      SHUFFLE_UNPACK,
      PACKED_SINGLE_PRECISION,
      SIMD128bits,
      SIMD64bits,
      CACHEABILITY_CONTROL,
      FP_INTEGER_CONVERSION,
      SPECIALIZED_128bits,
      SIMD_FP_PACKED,
      SIMD_FP_HORIZONTAL,
      AGENT_SYNCHRONISATION,
      PACKED_ALIGN_RIGHT,
      PACKED_SIGN,
      PACKED_BLENDING_INSTRUCTION,
      PACKED_TEST,
      PACKED_MINMAX,
      HORIZONTAL_SEARCH,
      PACKED_EQUALITY,
      STREAMING_LOAD,
      INSERTION_EXTRACTION,
      DOT_PRODUCT,
      SAD_INSTRUCTION,
      ACCELERATOR_INSTRUCTION,    /* crc32, popcnt (sse4.2) */
      ROUND_INSTRUCTION
    }

    [Flags]
    public enum EFlagsStates
    {
      TE_ = 1,
      MO_ = 2,
      RE_ = 4,
      SE_ = 8,
      UN_ = 0x10,
      PR_ = 0x20
    }

    public enum BranchType
    {
      JO = 1,
      JC,
      JE,
      JA,
      JS,
      JP,
      JL,
      JG,
      JB,
      JECXZ,
      JmpType,
      CallType,
      RetType,
      JNO = -1,
      JNC = -2,
      JNE = -3,
      JNA = -4,
      JNS = -5,
      JNP = -6,
      JNL = -7,
      JNG = -8,
      JNB = -9
    }

    [Flags]
    public enum ArgumentDetails : int
    {
      NO_ARGUMENT = 0x10000000,
      REGISTER_TYPE = 0x20000000,
      MEMORY_TYPE = 0x40000000,
      CONSTANT_TYPE = unchecked((int)0x80000000),

      MMX_REG = 0x10000,
      GENERAL_REG = 0x20000,
      FPU_REG = 0x40000,
      SSE_REG = 0x80000,
      CR_REG = 0x100000,
      DR_REG = 0x200000,
      SPECIAL_REG = 0x400000,
      MEMORY_MANAGEMENT_REG = 0x800000,
      SEGMENT_REG = 0x1000000,

      RELATIVE_ = 0x4000000,
      ABSOLUTE_ = 0x8000000,
    }

    [Flags]
    public enum RegisterId : short
    {
      REG0 = 0x1,
      REG1 = 0x2,
      REG2 = 0x4,
      REG3 = 0x8,
      REG4 = 0x10,
      REG5 = 0x20,
      REG6 = 0x40,
      REG7 = 0x80,
      REG8 = 0x100,
      REG9 = 0x200,
      REG10 = 0x400,
      REG11 = 0x800,
      REG12 = 0x1000,
      REG13 = 0x2000,
      REG14 = 0x4000,
      REG15 = unchecked((short)0x8000)
    }

    public enum AccessMode
    {
      READ = 0x1,
      WRITE = 0x2,
    }

    [Flags]
    public enum SpecialInfo : ulong
    {
      /* === mask = 0xff */
      NoTabulation = 0x00000000,
      Tabulation = 0x00000001,

      /* === mask = 0xff00 */
      MasmSyntax = 0x00000000,
      GoAsmSyntax = 0x00000100,
      NasmSyntax = 0x00000200,
      ATSyntax = 0x00000400,

      /* === mask = 0xff0000 */
      PrefixedNumeral = 0x00010000,
      SuffixedNumeral = 0x00000000,

      /* === mask = 0xff000000 */
      ShowSegmentRegs = 0x01000000
    }

    public enum Architecture : uint
    {
      x86_32 = 0,
      x86_64 = 64
    }

    #endregion Enumerations

    #region Methods

    public static string Version()
    {
      return Version(false);
    }

    public static string Version(bool Cheetah)
    {
      if (Cheetah)
        return (IntPtr.Size == 8) ? Marshal.PtrToStringAnsi(NativeMethods.BeaEngineVersion64Cheetah()) : Marshal.PtrToStringAnsi(NativeMethods.BeaEngineVersion32Cheetah());
      else
        return (IntPtr.Size == 8) ? Marshal.PtrToStringAnsi(NativeMethods.BeaEngineVersion64()) : Marshal.PtrToStringAnsi(NativeMethods.BeaEngineVersion32());
    }

    public static string Revision()
    {
      return Revision(false);
    }

    public static string Revision(bool Cheetah)
    {
      if (Cheetah)
        return (IntPtr.Size == 8) ? Marshal.PtrToStringAnsi(NativeMethods.BeaEngineRevision64Cheetah()) : Marshal.PtrToStringAnsi(NativeMethods.BeaEngineRevision32Cheetah());
      else
        return (IntPtr.Size == 8) ? Marshal.PtrToStringAnsi(NativeMethods.BeaEngineRevision64()) : Marshal.PtrToStringAnsi(NativeMethods.BeaEngineRevision32());
    }

    public static int Disassemble(ref _Disasm instruction)
    {
      return Disassemble(ref instruction, false);
    }

    public static int Disassemble(ref _Disasm instruction, bool Cheetah)
    {
      if (Cheetah)
        return (IntPtr.Size == 8) ? NativeMethods.Disassemble64Cheetah(ref instruction) : NativeMethods.Disassemble32Cheetah(ref instruction);
      else
        return (IntPtr.Size == 8) ? NativeMethods.Disassemble64(ref instruction) : NativeMethods.Disassemble32(ref instruction);
    }

    public static IEnumerable<_Disasm> Disassemble(byte[] data, long address, Architecture architecture)
    {
      return BeaEngine.Disassemble(data, new UIntPtr((ulong)address), architecture);
    }

    public static IEnumerable<_Disasm> Disassemble(byte[] data, ulong address, Architecture architecture)
    {
      return BeaEngine.Disassemble(data, new UIntPtr(address), architecture);
    }

    public static IEnumerable<_Disasm> Disassemble(byte[] data, IntPtr address, Architecture architecture)
    {
      return BeaEngine.Disassemble(data, new UIntPtr((ulong)address.ToInt64()), architecture);
    }

    public static IEnumerable<_Disasm> Disassemble(byte[] data, UIntPtr address, Architecture architecture)
    {
      GCHandle h = GCHandle.Alloc(data, GCHandleType.Pinned);
      UInt64 EndCodeSection = (UInt64)h.AddrOfPinnedObject().ToInt64() + (ulong)data.Length;

      _Disasm d = new _Disasm();
      d.InstructionPointer = (UIntPtr)h.AddrOfPinnedObject().ToInt64();
      d.VirtualAddr = address.ToUInt64();
      d.Architecture = architecture;
      bool error = false;
      while (!error)
      {
        d.SecurityBlock = (uint)(EndCodeSection - d.InstructionPointer.ToUInt64());

        d.Length = BeaEngine.Disassemble(ref d);
        if (d.Length == BeaEngine.OutOfBlock)
          error = true;
        else if (d.Length == BeaEngine.UnknownOpcode)
        {
          _Disasm yieldedInst = d;
          d.InstructionPointer = d.InstructionPointer + 1;
          d.VirtualAddr = d.VirtualAddr + 1;
          yield return yieldedInst;
        }
        else
        {
          _Disasm yieldedInst = d;
          d.InstructionPointer = d.InstructionPointer + d.Length;
          d.VirtualAddr = d.VirtualAddr + (ulong)d.Length;
          if (d.InstructionPointer.ToUInt64() >= EndCodeSection)
            error = true;
          yield return yieldedInst;
        }
      }
      yield break;
    }

    #endregion Methods

    #region Structures

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct _Disasm
    {
      public UIntPtr InstructionPointer;
      public UInt64 VirtualAddr;
      public UInt32 SecurityBlock;

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = InstructionLength)]
      public string CompleteInstr;

      public Architecture Architecture;
      public SpecialInfo Options;
      public INSTRTYPE Instruction;
      public ARGTYPE Argument1;
      public ARGTYPE Argument2;
      public ARGTYPE Argument3;
      public PREFIXINFO Prefix;
      private InternalDatas reserved;

      // A place to optionally store the length of an instruction.
      public int Length { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct INSTRTYPE
    {
      public Int32 Category;
      public Int32 Opcode;

      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
      public string Mnemonic;

      public Int32 BranchType;
      public EFLStruct Flags;
      public UInt64 AddrValue;
      public Int64 Immediat;
      public UInt32 ImplicitModifiedRegs;

      public InstructionSet InstructionSet
      {
        get { return (InstructionSet)(0xffff0000 & this.Category); }
      }

      public InstructionType InstructionType
      {
        get { return (InstructionType)(0x0000ffff & this.Category); }
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct EFLStruct
    {
      public UInt8 OF_;
      public UInt8 SF_;
      public UInt8 ZF_;
      public UInt8 AF_;
      public UInt8 PF_;
      public UInt8 CF_;
      public UInt8 TF_;
      public UInt8 IF_;
      public UInt8 DF_;
      public UInt8 NT_;
      public UInt8 RF_;
      public UInt8 Alignment;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PREFIXINFO
    {
      public int Number;
      public int NbUndefined;
      public UInt8 LockPrefix;
      public UInt8 OperandSize;
      public UInt8 AddressSize;
      public UInt8 RepnePrefix;
      public UInt8 RepPrefix;
      public UInt8 FSPrefix;
      public UInt8 SSPrefix;
      public UInt8 GSPrefix;
      public UInt8 ESPrefix;
      public UInt8 CSPrefix;
      public UInt8 DSPrefix;
      public UInt8 BranchTaken;
      public UInt8 BranchNotTaken;
      public REX_Struct REX;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct REX_Struct
    {
      public UInt8 W_;
      public UInt8 R_;
      public UInt8 X_;
      public UInt8 B_;
      public UInt8 State;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ARGTYPE
    {
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
      public string ArgMnemonic;

      public Int32 ArgType;
      public Int32 ArgSize;
      public Int32 ArgPosition;
      public AccessMode AccessMode;
      public MEMORYTYPE Memory;
      public UInt32 SegmentReg;

      public ArgumentDetails Details
      {
        get { return (ArgumentDetails)(0xffff0000 & this.ArgType); }
      }

      public RegisterId RegisterId
      {
        get { return (RegisterId)(0x0000ffff & this.ArgType); }
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct MEMORYTYPE
    {
      public Int32 BaseRegister;
      public Int32 IndexRegister;
      public Int32 Scale;
      public Int64 Displacement;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct InternalDatas
    {
      private UIntPtr EIP_;
      private UInt64 EIP_VA;
      private UIntPtr EIP_REAL;
      private Int32 OriginalOperandSize;
      private Int32 OperandSize;
      private Int32 MemDecoration;
      private Int32 AddressSize;
      private Int32 MOD_;
      private Int32 RM_;
      private Int32 INDEX_;
      private Int32 SCALE_;
      private Int32 BASE_;
      private Int32 MMX_;
      private Int32 SSE_;
      private Int32 CR_;
      private Int32 DR_;
      private Int32 SEG_;
      private Int32 REGOPCODE;
      private UInt32 DECALAGE_EIP;
      private Int32 FORMATNUMBER;
      private Int32 SYNTAX_;
      private UInt64 EndOfBlock;
      private Int32 RelativeAddress;
      private UInt32 Architecture;
      private Int32 ImmediatSize;
      private Int32 NB_PREFIX;
      private Int32 PrefRepe;
      private Int32 PrefRepne;
      private UInt32 SEGMENTREGS;
      private UInt32 SEGMENTFS;
      private Int32 third_arg;
      private Int32 TAB_;
      private Int32 ERROR_OPCODE;
      private REX_Struct REX;
      private Int32 OutOfBlock;
    }

    #endregion Structures
  }
}