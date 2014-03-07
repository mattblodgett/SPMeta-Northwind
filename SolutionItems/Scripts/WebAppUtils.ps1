function WebAppExists ($webAppName)
{
	$webApp = Get-SPWebApplication | ?{$_.Name -eq $webAppName}
	
	if ($webApp)
	{
		return $true
	}
	else
	{
		return $false
	}
}

function GetTargetWebApp ($suppliedWebAppName)
{
	$targetWebApp = $null

	if (!$suppliedWebAppName)
	{
		Log "No web app supplied"
		
		$defaultWebAppName = "Northwind"

		if (WebAppExists $defaultWebAppName)
		{
			Log "Defaulting to the '$defaultWebAppName' web app"
			
			$targetWebApp = Get-SPWebApplication $defaultWebAppName
		}
		else
		{
			Log "Default web app '$defaultWebAppName' not found" 
			Log "Please run the script again with the -WebAppName parameter"
		}
	}
	else
	{		
		if (WebAppExists $suppliedWebAppName)
		{
			$targetWebApp = Get-SPWebApplication $suppliedWebAppName
		}
		else
		{
			Log "Web app '$suppliedWebAppName' not found"
		}
	}
	
	return $targetWebApp
}