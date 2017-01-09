function Get-Server-Utilization
{
	$sleep = 1000
    $info = Get-WmiObject -Class Win32_OperatingSystem -ErrorAction SilentlyContinue| Select-Object Name, TotalVisibleMemorySize, FreePhysicalMemory,TotalVirtualMemorySize,FreeVirtualMemory,FreeSpaceInPagingFiles,NumberofProcesses,NumberOfUsers
	
	$obj = New-Object PSObject
    $obj | Add-Member RamUsage ($info.TotalVisibleMemorySize/1MB - $info.FreePhysicalMemory/1MB)
    $obj | Add-Member RamTotal ($info.TotalVisibleMemorySize/1MB)

    $obj | Add-Member CpuUsage (Get-WmiObject win32_processor | Measure-Object -property LoadPercentage -Average | Foreach {$_.Average})

    $diskInfo = New-Object System.Collections.ArrayList
    $wmi = $null
    if ($wmi = Get-WmiObject -Class Win32_LogicalDisk -ErrorAction SilentlyContinue) 
    {
        $wmi | Select 'DeviceID', 'Size', 'FreeSpace' | Foreach {
            $disk = New-Object PSObject
            $disk | Add-Member Letter $_.DeviceID
            $disk | Add-Member FreeSpaceMb ($_.FreeSpace/1MB).ToString('N')
            $disk | Add-Member TotalSpaceMb ($_.Size/1MB).ToString('N')
            $disk | Add-Member UsedSpaceMb ($_.Size/1MB - $_.FreeSpace/1MB).ToString('N')
            $diskInfo.Add($disk) | Out-Null
        }
    }

	$networkInfo = New-Object PSObject
	$wmi = $null
	if($wmi = Get-WmiObject -Class Win32_PerfFormattedData_Tcpip_NetworkInterface -ErrorAction SilentlyContinue)
	{
		$sentBytesPerSec = 0
		$receivedBytesPerSec = 0
		$wmi | Select 'BytesReceivedPersec', 'BytesSentPersec' | Foreach {
			$sentBytesPerSec += $_.BytesReceivedPersec
			$receivedBytesPerSec += $_.BytesSentPersec
		}
		$networkInfo | Add-Member SentKiloBytesPerSec ($sentBytesPerSec / 1KB)
		$networkInfo | Add-Member ReceivedKiloBytesPerSec ($receivedBytesPerSec / 1KB)

		$obj | Add-Member NetworkUtilization $networkInfo
	}

    $obj | Add-Member Disks @($diskInfo)

    return $obj
}

return (Get-Server-Utilization | ConvertTo-Json)