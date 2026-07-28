$source = "D:\Project\01.Program\2025\mil-gen-doc-main"
$destination = "\\192.168.20.52\hoon\01.Study\99.TestProject\02.WPF\91.soletop"
$logFile = "C:\Temp\backup_to_nas.log"
Write-Host "Coppy Start..."
robocopy $source $destination /MIR /MT:16 /R:2 /W:5 /LOG:$logFile /Z
Write-Host "Copy End!"
