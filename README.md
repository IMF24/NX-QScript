<div align="center">

![](./Assets/th_nx_qscript.png)

</div>

# Neversoft QScript - QBC Syntax
*Written by [IMF24](https://github.com/IMF24) & [donnaken15](https://github.com/donnaken15)*

Very lightweight C# library for working with Neversoft QScript (`*.q`, `*.qb`) files in a straightforward, no-nonsense way. Adapted from [the Node.js version written by Zedek The Plague Doctor](https://gitgud.io/fretworks/nodeqbc); ported to C# as a standalone library that other C# programs can use.

[*If you want to jump right into using the library, follow the Usage section!*](#usage---quick-start)

*Made in C# 7.3 on .NET Framework 4.6.2*

------------------

## This library can:
- Decompile Neversoft TH engine QScript files (`*.qb`) into human-readable source code in [QBC format](https://gitgud.io/fretworks/nodeqbc) (`*.q`)
- Compile QBC source code files (`*.q`) back into serialized QB bytecode
- Support for detailed error logging, concisely describing problems in compilation or decompilation
- All public types included are IntelliSense documented for your convenience

------------------

## Games Supported
The following games are supported for either compilation, decompilation, or both:
| Game                             | Compile | Decompile |
| -------------------------------- | ------- | --------- |
| Guitar Hero III: Legends of Rock |    ✔    |     ✔    |
| Guitar Hero: World Tour          |    ✔    |     ✔    |
| Guitar Hero: Warriors of Rock    |    ✔    |     ✔    |
| Tony Hawk's American Wasteland   |    ✔    |     ✔    |
| Tony Hawk's Underground 1        |    ❌   |     ✔    |
| Tony Hawk's Underground 2        |    ❌   |     ✔    |
| Tony Hawk's Pro Skater 4         |    ❌   |     ✔    |

------------------

## Usage - Quick Start
Just grab the latest release of the DLL [from Releases](https://github.com/IMF24/NX-QScript/releases/latest) and add it as a reference into your C# project.

Using the library is as simple as importing the `NX_QScript` namespace, and using the static [`QBC`](./QBC.cs) class to work the magic. At its simplest, the `Compile()` and `Decompile()` methods will produce a `byte[]` and a `string`, respectively:
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

Alternatively, if you would like the QB file to be decompiled or compiled into a sequence of QB type objects that you can work with further, use the `CompileAsObject()` and `DecompileAsObject()` methods, which will (de)compile the file to a single type called [`QBFile`](./Types/QBFile.cs):
```cs
// The QBFile object lives in the NX_QScript.Types namespace
using NX_QScript;
using NX_QScript.Types;

QBFile sourceFile = QBC.DecompileAsObject("test_script.qb.xen");
QBFile compiledFile = QBC.CompileAsObject("test_script.q");
```

------------------

If you'd like to compile a script for a specific game, you can give that in the `QBC.Compile()` function's parameters:
```cs
using NX_QScript;

byte[] worCompiled = QBC.Compile("test_script.q", QBGameTarget.GH6);
File.WriteAllBytes("test_script.qb.xen", qbBytecode);
```
**Make sure you check the [Games Supported](#games-supported) table to know which games support compilation!** The game target by default, if not specified, is for Guitar Hero: World Tour.
