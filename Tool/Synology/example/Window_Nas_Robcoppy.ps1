$source = "D:\Temp\Nas_Test"
$destination = "\\192.168.20.52\hoon\98.Temp"
$logFile = "C:\Temp\backup_to_nas.log"

robocopy $source $destination /MIR /MT:16 /R:2 /W:5 /LOG:$logFile /Z
