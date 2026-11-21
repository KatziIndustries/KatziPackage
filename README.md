# KatziPackage

A simple library for parsing and creating .kpg (KatziPackage) files.

# Usage
```
using Katzi.Package;

// To extract information out of a package
KatziPackage katziPackage = KatziPackager.Parse(".kpg file path");

// To create a package
KatziPackager.Create("source directory", "output path");

// Verifying package integrity
// Get all unrecognized files
KatziPackager.GetUnknownFiles("directory");

// Get missing required files lol
KatziPackager.GetMissingRequiredFiles("directory");

// Get missing optional files lol
KatziPackager.GetMissingOptionalFiles("directory");
```

# Including in your project
```
dotnet add package KatziPackage
```
