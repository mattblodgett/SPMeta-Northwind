$logFileName = "DeploymentLog.txt"

function Log ($message, $suppressTimestamp, $color="Gray")
{	
	if (!$suppressTimestamp)
	{
		$timestamp = Get-Date -Format g
		$message = $timestamp + " - " + $message
	}
	
	$message += "`n"
	
	Write-Host $message -ForegroundColor $color
	$message >> $logFileName
}

function ClearLog
{
	if (Test-Path $logFileName)
	{
		Clear-Content $logFileName
	}
}