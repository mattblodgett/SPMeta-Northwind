function RestartService ($serviceName)
{
	$servers = Get-SPServer
	
	foreach ($server in $servers)
	{
		if ($server.Role -ne "Application")
		{
			continue
		}
		
		$computerName = $server.Address
	
		$service = Get-Service -DisplayName $serviceName -ComputerName $computerName
	
		Log "Stopping the $serviceName service on $computerName..."
		
		$service.Stop()
		$service.WaitForStatus("Stopped")
		
		Log "Stopped"
		
		Log "Starting the $serviceName service on $computerName..."
		
		$service.Start()
		$service.WaitForStatus("Running")
		
		Log "Started"
	}
}

function RestartTimerService
{
	RestartService "SharePoint 2010 Timer"
}

function RestartAdminService
{
	RestartService "SharePoint 2010 Administration"
}

function ResetIIS
{
	$servers = Get-SPServer
	
	foreach ($server in $servers)
	{
		if ($server.Role -ne "Application")
		{
			continue
		}
		
		$computerName = $server.Address
	
		Log "Resetting IIS on $computerName..."
		
		iisreset $computerName | Out-Null
		
		Log "Finished resetting IIS on $computerName"
	}
}