// PdfCompressor.cs
// .NET 8 console app that compresses a PDF using Ghostscript.
// Build with: dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
// The resulting single exe can be copied to any Windows machine with Ghostscript installed.

using System;
using System.Diagnostics;
using System.IO;

class PdfCompressor
{
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Usage: PdfCompressor.exe <pdf-path>");
            return 1;
        }

        string inputPath = args[0];
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"File not found: {inputPath}");
            return 1;
        }

        // Locate Ghostscript executable. Adjust if installed elsewhere.
        string gsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "gs", "gs9.55.0", "bin", "gswin64c.exe");
        if (!File.Exists(gsPath))
        {
            Console.Error.WriteLine($"Ghostscript not found at {gsPath}. Install Ghostscript or update the path in the code.");
            return 1;
        }

        string outputPath = Path.Combine(Path.GetDirectoryName(inputPath) ?? "", Path.GetFileNameWithoutExtension(inputPath) + "_compressed.pdf");

        var psi = new ProcessStartInfo
        {
            FileName = gsPath,
            Arguments = $"-sDEVICE=pdfwrite -dCompatibilityLevel=1.4 -dPDFSETTINGS=/ebook -dNOPAUSE -dQUIET -dBATCH -sOutputFile=\"{outputPath}\" \"{inputPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var proc = Process.Start(psi);
        if (proc == null)
        {
            Console.Error.WriteLine("Failed to start Ghostscript process.");
            return 1;
        }
        proc.WaitForExit();
        string stdOut = proc.StandardOutput.ReadToEnd();
        string stdErr = proc.StandardError.ReadToEnd();
        if (proc.ExitCode != 0)
        {
            Console.Error.WriteLine($"Ghostscript error (exit {proc.ExitCode}):\n{stdErr}");
            return 1;
        }
        Console.WriteLine($"Compressed PDF saved to: {outputPath}");
        return 0;
    }
}
