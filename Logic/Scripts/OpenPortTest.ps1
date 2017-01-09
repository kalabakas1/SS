function OpenPortTest
{
	param(
		[string] $hostAddress,
		[int] $port
	)

	try
	{
		$client = New-Object Net.Sockets.TcpClient $hostAddress, $port 
		if($client.Connected)
		{
			return $true
		}
		return $false
	}
	catch
	{
		return $false
	}
}