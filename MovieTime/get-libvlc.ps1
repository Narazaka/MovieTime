function Get-ZipUri-From-Index([string] $indexUri) {
    $zipFile = Invoke-WebRequest -Verbose $indexUri |
        select -expand Links |
        select -expand href |
        where { $_ -match "\.zip$" }

    $joinedUri = [System.IO.Path]::Combine($indexUri, $zipFile)
    New-Object uri $joinedUri.Replace('\', '/')
}

function Get-Zip-Content($zipUri, $dir) {
    Invoke-WebRequest -Verbose $zipUri -OutFile tmp.zip
    Expand-Archive -Verbose tmp.zip -DestinationPath $dir
    Remove-Item tmp.zip
}

$libvlcItems = @("libvlc", "libvlccore", "plugins", "COPYING")

function Move-Libvlc-Items($dir) {
    $contentDir = Get-ChildItem $dir | select -First 1 | select -expand FullName
    $items = Get-ChildItem $contentDir |
        where { $_.BaseName -in $libvlcItems } |
        select -expand FullName
    Move-Item $items $dir
    Remove-Item -Recurse $contentDir
}

function Get-Libvlc([string] $indexUri, [string] $dir, [bool] $force) {
    if (Test-Path $dir) {
        if ($force) {
            Remove-Item -Recurse $dir
        } else {
            return
        }
    }
    $zipUri = Get-ZipUri-From-Index $indexUri
    New-Item -ItemType Directory $dir
    Get-Zip-Content $zipUri $dir
    Move-Libvlc-Items $dir
}
