param(
    [Parameter(Mandatory=$true, ValueFromPipeline=$true)]
    [string[]] $InputPath
)

foreach ($path in $InputPath) {
    if (-not (Test-Path $path)) {
        Write-Error "File not found: $path"
        continue
    }
    $output = [System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($path), 
        [System.IO.Path]::GetFileNameWithoutExtension($path) + "_compressed.pdf")
    $gsPath = "C:\Program Files\gs\gs9.55.0\bin\gswin64c.exe" # Adjust if your Ghostscript version/path differs
    if (-not (Test-Path $gsPath)) {
        Write-Error "Ghostscript not found at $gsPath. Install Ghostscript and update the path."
        continue
    }
    & "$gsPath" -sDEVICE=pdfwrite -dCompatibilityLevel=1.4 -dPDFSETTINGS=/ebook -dNOPAUSE -dQUIET -dBATCH -sOutputFile="$output" "$path"
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Compressed $path -> $output"
    } else {
        Write-Error "Ghostscript failed to compress $path"
    }
}
