function GetSystemInfo
{
  param(
    [string] $computer
  )
  
  $systemInfo = New-Object PSObject
  if ($wmi = Get-WmiObject -Class Win32_ComputerSystem -ErrorAction SilentlyContinue) 
  {
      $systemInfo | Add-Member 'HardwareManufacturer' $wmi.Manufacturer
      $systemInfo | Add-Member 'HardwareModel' $wmi.Model
      $systemInfo | Add-Member 'PhysicalMemoryMb' ($wmi.TotalPhysicalMemory/1MB).ToString('N')
      $systemInfo | Add-Member 'LoggedOnUser' $wmi.Username
  }
  return $systemInfo
}

function GetDiskInfo
{
  param(
    [string] $computer
  )
  
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
  return $diskInfo
}

function GetIpInfo
{
  param(
    [string] $computer
  )
  
  $ipInfo = New-Object System.Collections.ArrayList
  $wmi = $null
  if ($wmi = Get-WmiObject -Class Win32_NetworkAdapterConfiguration -ErrorAction SilentlyContinue) 
  {
    $Ips = @{}
    $wmi | Where { $_.IPAddress -match '\S+' } | Foreach { $Ips.$($_.IPAddress -join ', ') = $_.MACAddress }

    $Ips.GetEnumerator() | Foreach {
      $ip = New-Object PSObject
      $ip | Add-Member Name $_.Name
      $ip | Add-Member Mac $_.Value
      $ipInfo.Add($ip) | Out-Null
    }
  }
  return $ipInfo
}

function GetCpuInfo
{
  param(
    [string] $computer
  )
  
  $cpuInfo = New-Object PSObject
  $wmi = $null
  if ($wmi = Get-WmiObject -Class Win32_Processor -ErrorAction SilentlyContinue) 
  {
    $wmi | Foreach {
      $maxClockSpeed     =  $_.MaxClockSpeed
      $numberOfCores     += $_.NumberOfCores
      $description       =  $_.Description
      $numberOfLogProc   += $_.NumberOfLogicalProcessors
      $socketDesignation =  $_.SocketDesignation
      $status            =  $_.Status
      $manufacturer      =  $_.Manufacturer
      $name              =  $_.Name
    }
    
    $cpuInfo | Add-Member ClockSpeed $maxClockSpeed
    $cpuInfo | Add-Member Cores $numberOfCores
    $cpuInfo | Add-Member Description $description
    $cpuInfo | Add-Member LogicProcessors $numberOfLogProc
    $cpuInfo | Add-Member Socket $socketDesignation
    $cpuInfo | Add-Member Status $status
    $cpuInfo | Add-Member Manufacturer $manufacturer
    $cpuInfo | Add-Member Name ($name -replace '\s+', ' ')
	
	$cpuInfo | Add-Member Usage (Get-WmiObject win32_processor | Measure-Object -property LoadPercentage -Average | Foreach {$_.Average})
  }
  return $cpuInfo
}

function GetRamInfo
{
  param(
    [string] $computer
  )
  
  $ramInfo = New-Object PSObject
  $wmi = $null
  if ($wmi = Get-WmiObject -Class Win32_OperatingSystem -computername $Computer -ErrorAction SilentlyContinue| Select-Object Name, TotalVisibleMemorySize, FreePhysicalMemory,TotalVirtualMemorySize,FreeVirtualMemory,FreeSpaceInPagingFiles,NumberofProcesses,NumberOfUsers ) {
    $wmi | Foreach {
      
      $TotalRAM     =  $_.TotalVisibleMemorySize/1MB
      $FreeRAM     = $_.FreePhysicalMemory/1MB
      $UsedRAM       =  $_.TotalVisibleMemorySize/1MB - $_.FreePhysicalMemory/1MB
      $TotalRAM = [Math]::Round($TotalRAM, 2)
      $FreeRAM = [Math]::Round($FreeRAM, 2)
      $UsedRAM = [Math]::Round($UsedRAM, 2)
      $RAMPercentFree = ($FreeRAM / $TotalRAM) * 100
      $RAMPercentFree = [Math]::Round($RAMPercentFree, 2)
      $TotalVirtualMemorySize  = [Math]::Round($_.TotalVirtualMemorySize/1MB, 3)
      $FreeVirtualMemory =  [Math]::Round($_.FreeVirtualMemory/1MB, 3)
      $FreeSpaceInPagingFiles            =  [Math]::Round($_.FreeSpaceInPagingFiles/1MB, 3)
      $NumberofProcesses      =  $_.NumberofProcesses
      $NumberOfUsers              =  $_.NumberOfUsers
    }
    $ramInfo | Add-Member TotalGb $TotalRAM
    $ramInfo | Add-Member FreeGb $FreeRAM
    $ramInfo | Add-Member UsedGb $UsedRAM
    $ramInfo | Add-Member PercentageFree $RAMPercentFree
    $ramInfo | Add-Member TotalVirtual $TotalVirtualMemorySize
    $ramInfo | Add-Member FreeVirtual $FreeVirtualMemory
    $ramInfo | Add-Member FreeSpaceInPagingFiles $FreeSpaceInPagingFiles
    $ramInfo | Add-Member NumberOfProcesses $NumberofProcesses
    $ramInfo | Add-Member NumberOfUsers ($NumberOfUsers -replace '\s+', ' ')
  }
  return $ramInfo
}

function GetBiosInfo
{
  param(
    [string] $computer
  )
  
  $biosInfo = New-Object PSObject
  $wmi = $null
  if ($wmi = Get-WmiObject -Class Win32_Bios -ErrorAction SilentlyContinue) 
  {
    $biosInfo | Add-Member Manufacturer $wmi.Manufacturer
    $biosInfo | Add-Member Name $wmi.Name
    $biosInfo | Add-Member Version $wmi.Version
  }
  return $biosInfo
}

function GetOsInfo
{
  param(
    [string] $computer
  )

  $osInfo = New-Object PSObject    
  $wmi = $null
  if ($wmi = Get-WmiObject -Class Win32_OperatingSystem -ErrorAction SilentlyContinue) 
  {
    $osInfo | Add-Member BootTime $wmi.ConvertToDateTime($wmi.LastBootUpTime)
    $osInfo | Add-Member SystemDrive $wmi.SystemDrive
    $osInfo | Add-Member SystemDevice $wmi.SystemDevice
    $osInfo | Add-Member Language $wmi.OSLanguage
    $osInfo | Add-Member Version $wmi.Version
    $osInfo | Add-Member WindowsDirectory $wmi.WindowsDirectory
    $osInfo | Add-Member Name $wmi.Caption
    $osInfo | Add-Member InstallDate $wmi.ConvertToDateTime($wmi.InstallDate)
    $osInfo | Add-Member ServicePack ([string]$wmi.ServicePackMajorVersion + '.' + $wmi.ServicePackMinorVersion)
  }
  return $osInfo
}

$computer = $env:computername
$data = New-Object PSObject
$data | Add-Member ComputerName $computer
$data | Add-Member System (GetSystemInfo $computer)
$data | Add-Member Disk @(GetDiskInfo $computer)
$data | Add-Member Ip @(GetIpInfo $computer)
$data | Add-Member Cpu (GetCpuInfo $computer)
$data | Add-Member RAM (GetRamInfo $computer)
$data | Add-Member Bios (GetBiosInfo $computer)
$data | Add-Member OS (GetOsInfo $computer)
#$data | Add-Member Events (Get-EventLog -Newest 20 -EntryType Error -LogName Application)

return ($data | ConvertTo-Json)