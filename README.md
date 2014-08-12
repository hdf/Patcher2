Patcher2
========

Small C# binary file patcher utility.<br>
The interesting bit is the byte pattern based search and replace.<br>
Should handle file sizes up to 2GB, and is really fast.<br>
(A lot less fast, while making patch patterns, and only handles files of a few hundred megabytes.)<br>
(Created for educational porpoises.)

---

**Notes:**

Pattern format is like in 010 Editor, but Cheat Engine AOBScan format is also accepted.<br>
Replace is also a pattern, like search.<br>
Offset means the offset compared to the beginning location, of where the pattern is found.<br>
Offset can be negative, and is in base 10, while the patterns are in base 16.

**Extra (hidden) features:**

Has experimental, memory based pattern replace feature.<br>
Compile for 64 bits, and it will work with 64 bit processes.<br>
(Can also patch itself to 64 bit. Just run self_to_64.bat in the Release directory. :) )<br>
Writing "proc:" in to the file field, will bring up the process list window.<br>
Leaving the replace field empty, will allow you to use the Locate feature.

---

### BeaEngineCS optional extension project is:

**Based on:**

PE file header stuff: http://sandsprite.com/CodeStuff/PEOFFSETCALC.cs<br>
BeaEngineCS: https://github.com/carterjones/beaengine-cs

**Using:**

BeaEngine: disassembler library for x86 x86-64 (IA32 and Intel64)<br>
http://www.beaengine.org/

**Notes:**

Have BeaEngineCS(64).dll (and BeaEngineCheetah(64).dll as well of course) in the folder, to use it's disassembly based address masking features,
when creating a difference based patch with the "m" command line option. It will make the process MUCH slower.

---

**License:**<br>
LGPL (<http://wikipedia.org/wiki/GNU_Lesser_General_Public_License>)
