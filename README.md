<div align="center">

![](./Assets/th_nx_qscript.png)

</div>

# Neversoft QScript - QBC Syntax
*Written by IMF24 & donnaken15*

Very lightweight C# library for working with Neversoft QScript (`*.q`, `*.qb`) files in a straightforward, no-nonsense way. Adapted from [the Node.js version written by Zedek The Plague Doctor](https://gitgud.io/fretworks/nodeqbc); ported to C# as a standalone library that other C# programs can use.

[*If you want to jump right into using the library, follow the Usage section!*](#usage---quick-start)

*Made in C# 7.3 on .NET Framework 4.6.2*

------------------

### This library can:
- Decompile Neversoft TH engine QScript files (`*.qb`) into human-readable source code in [QBC format](https://gitgud.io/fretworks/nodeqbc) (`*.q`)
- Compile QBC source code files (`*.q`) back into serialized QB bytecode
- Support for detailed error logging, concisely describing problems in compilation or decompilation

------------------

### Games Supported
The following games are supported for either compilation, decompilation, or both:
| Game                             | Compile | Decompile |
| -------------------------------- | ------- | --------- |
| Guitar Hero III: Legends of Rock |    ✔    |     ✔    |
| Guitar Hero: World Tour          |    ✔    |     ✔    |
| Guitar Hero: Warriors of Rock    |    ✔    |     ✔    |
| Tony Hawk's American Wasteland   |    ✔    |     ✔    |
| Tony Hawk's Underground 1        |    ✘    |     ✔    |
| Tony Hawk's Underground 2        |    ✘    |     ✔    |
| Tony Hawk's Pro Skater 4         |    ✘    |     ✔    |

------------------

### Usage - Quick Start
Just grab the latest release of the DLL [from Releases](https://github.com/IMF24/NX-QScript/releases/latest) and add it as a reference into your C# project.

Using the library is as simple as importing the `NX_QScript` namespace, and using the static [`QBC`](./QBC.cs) class to work the magic:
```cs
// All functionality lives in the NX_QScript namespace
using NX_QScript;

// Using for decompilation:
string qbcSource = QBC.Decompile("test_script.qb.xen");
File.WriteAllText("test_script.q", qbcSource);

// Or, use for compilation:
byte[] qbBytecode = QBC.Compile("test_script.q");
File.WriteAllBytes("test_script.qb.xen", qbBytecode);
```

------------------

If you'd like to compile for a specific game, you can give that in the `QBC.Compile()` function's parameters:
```cs
byte[] worCompiled = QBC.Compile("test_script.q", JobGameTarget.GH6);
File.WriteAllBytes("test_script.qb.xen", qbBytecode);
```
**Make sure you check the [Games Supported](#games-supported) table to know which games support compilation!**

You can further customize how the compilation or decompilation process should go by using the [`QBCJobOptions`](./Internal/JobOptions.cs) type to give you full control, and pass it into either `QBC.Compile()` or `QBC.Decompile()`:
```cs
// For example, a type of decompile job that enables debug logging 
QBCJobOptions options = new QBCJobOptions();
options.CanDebug = true;
string qbcSource = QBC.Decompile("test_script.qb.xen", options);
```
