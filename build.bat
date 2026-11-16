@echo off
:: Build the PDF compressor single‑file exe

:: Ensure we are in the repo root
pushd "%~dp0"

:: If needed, create a temporary project folder
if not exist PdfCompressor (dotnet new console -n PdfCompressor -o . -f net8.0 --no-restore)

:: Overwrite the default Program.cs with our source (already present as PdfCompressor.cs)
copy /Y PdfCompressor.cs . >nul

:: Publish a single‑file exe (framework‑dependent). Change --self-contained true for a fully self‑contained binary.
dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true

popd
