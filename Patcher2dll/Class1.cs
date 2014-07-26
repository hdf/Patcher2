using System;

namespace Patcher2
{
  public static class Patcher
  {
    private const string _q = "?";

    public static int[] BinaryPatternSearch(ref byte[] bytes, string[] searchBytes, string wildcard, bool onlyOne)
    {
      int sl = searchBytes.Length;
      if (sl < 1 || bytes.Length < sl)
        return new int[] { };
      byte[] sbytes = new byte[sl];
      for (int i = 0; i < sl; i++)
        if (searchBytes[i] != wildcard)
          if (!Byte.TryParse(searchBytes[i], System.Globalization.NumberStyles.HexNumber, null, out sbytes[i]))
            return new int[] { };

      System.Collections.Generic.List<int> locs = new System.Collections.Generic.List<int>();
      int n = bytes.Length - sl;
      int found = 0;
      for (var i = 0; i <= n; i++)
      {
        for (int j = 0; j < sl; j++)
          if (searchBytes[j] != wildcard)
            if (bytes[i + j] != sbytes[j])
              goto nope;
        locs.Add(i);
        found++;
        if (onlyOne && found > 1)
          break;
      nope: ;
      }
      return locs.ToArray();
    }

    // (without wildcard)
    public static int[] BinaryPatternSearch(ref byte[] bytes, string[] searchBytes, bool onlyOne)
    {
      return BinaryPatternSearch(ref bytes, searchBytes, _q, onlyOne);
    }

    // (return only 1)
    public static int[] BinaryPatternSearch(ref byte[] bytes, string[] searchBytes, string wildcard)
    {
      return BinaryPatternSearch(ref bytes, searchBytes, wildcard, true);
    }

    // (return only 1 and no wildcard)
    public static int[] BinaryPatternSearch(ref byte[] bytes, string[] searchBytes)
    {
      return BinaryPatternSearch(ref bytes, searchBytes, _q, true);
    }

    private static int largest(ref string[][] list)
    {
      int size = 0;
      for (int i = 0; i < list.Length; i++)
        size = (list[i].Length > size) ? list[i].Length : size;
      return size;
    }

    public static int BinaryPatternReplace(ref byte[] bytes, int[] locs, string[][] replaceBytes, int[] offsets, string wildcard)
    {
      int ls = locs.Length;
      if (ls < 1 || ls != replaceBytes.Length || ls != offsets.Length || bytes.Length < largest(ref replaceBytes))
        return -1;
      int replaced = 0;
      for (int i2 = 0; i2 < ls; i2++)
        for (var i = 0; i < replaceBytes[i2].Length; i++)
          if (replaceBytes[i2][i] != wildcard)
            if (Byte.TryParse(replaceBytes[i2][i], System.Globalization.NumberStyles.HexNumber, null, out bytes[locs[i2] + i + offsets[i2]]))
              replaced++;
            else
              return -1;
      // To get hex back from bytes:
      // string hex = BitConverter.ToString(bytes).Replace("-", " ");
      return replaced;
    }

    // Multiple replaces (without wildcard)
    public static int BinaryPatternReplace(ref byte[] bytes, int[] locs, string[][] replaceBytes, int[] offsets)
    {
      return BinaryPatternReplace(ref bytes, locs, replaceBytes, offsets, _q);
    }

    // Single replace
    public static int BinaryPatternReplace(ref byte[] bytes, int loc, string[] replaceBytes, int offset, string wildcard)
    {
      return BinaryPatternReplace(ref bytes, new int[] { loc }, new string[][] { replaceBytes }, new int[] { offset }, wildcard);
    }

    // (without wildcard)
    public static int BinaryPatternReplace(ref byte[] bytes, int loc, string[] replaceBytes, int offset)
    {
      return BinaryPatternReplace(ref bytes, new int[] { loc }, new string[][] { replaceBytes }, new int[] { offset }, _q);
    }

    // (without offset)
    public static int BinaryPatternReplace(ref byte[] bytes, int loc, string[] replaceBytes, string wildcard)
    {
      return BinaryPatternReplace(ref bytes, new int[] { loc }, new string[][] { replaceBytes }, new int[] { 0 }, wildcard);
    }

    // (without offset or wildcard)
    public static int BinaryPatternReplace(ref byte[] bytes, int loc, string[] replaceBytes)
    {
      return BinaryPatternReplace(ref bytes, new int[] { loc }, new string[][] { replaceBytes }, new int[] { 0 }, _q);
    }
  }
}