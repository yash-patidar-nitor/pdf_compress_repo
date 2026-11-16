# PDF Compressor Utility

This repository contains a tiny Windows utility that compresses PDF files with a single right‑click.

## Contents
- `pdf_compress.ps1` – PowerShell script that invokes Ghostscript to compress a PDF.
- `add_pdf_compress_context_menu.reg` – Registry file that adds **Compress PDF** to the Windows context menu.
- `PdfCompressor.cs` – Minimal C# console app that runs Ghostscript and can be compiled to a single‑file executable.
- `build.bat` – Batch script that builds `PdfCompressor.exe` (single‑file, framework‑dependent; add `--self-contained true` for a fully self‑contained binary).
- `README.md` – This document.

## How to build the executable (requires .NET 8 SDK and Ghostscript)
```bat
pushd "%~dp0"
if not exist PdfCompressor (dotnet new console -n PdfCompressor -o . -f net8.0 --no-restore)
copy /Y PdfCompressor.cs . >nul
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true
popd
```
The resulting binary will be in `bin\Release\net8.0\win-x64\publish\PdfCompressor.exe`.

## Deploy on a laptop
1. Install **Ghostscript** (default path `C:\Program Files\gs\gs9.55.0\bin\gswin64c.exe`).
2. Copy `PdfCompressor.exe` to a folder, e.g. `C:\Tools\PdfCompressor\`.
3. Run the following batch file **as Administrator** to register the right‑click menu:
```bat
@echo off
set "EXE_PATH=C:\Tools\PdfCompressor\PdfCompressor.exe"
reg add "HKCR\SystemFileAssociations\.pdf\shell\Compress PDF" /ve /d "Compress PDF" /f
reg add "HKCR\SystemFileAssociations\.pdf\shell\Compress PDF" /v "Icon" /d "%EXE_PATH%" /f
reg add "HKCR\SystemFileAssociations\.pdf\shell\Compress PDF\command" /ve /d "\"%EXE_PATH%\" \"%%1\"" /f

echo Context‑menu entry added.
```
Now right‑click any PDF → **Compress PDF** → a `_compressed.pdf` file appears next to the original.
