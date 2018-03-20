$lib = Join-Path $PSScriptRoot "get-libvlc.ps1"
. $lib
Get-Libvlc "http://download.videolan.org/pub/videolan/vlc/last/win64/" "libvlc/x64"
Get-Libvlc "http://download.videolan.org/pub/videolan/vlc/last/win32/" "libvlc/x86"
