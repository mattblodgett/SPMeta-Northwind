Param(
	[parameter(Mandatory=$false)]
    $WebAppName
)

Add-PsSnapin Microsoft.SharePoint.PowerShell
Set-location "C:\Code\Side\spmetanorthwind\SolutionItems\Scripts"

. .\LoggingUtils.ps1
. .\SolutionUtils.ps1
. .\WebAppUtils.ps1
. .\ServiceUtils.ps1
. .\FeatureUtils.ps1

echo ""
ClearLog

$targetWebApp = GetTargetWebApp $WebAppName
if (!$targetWebApp)
{
	Log "Could not determine the target web app"
	Log "Please run the script again supplying a valid web app name with the -WebAppName parameter"
	return
}

Log "Going up."

foreach ($wsp in (ls *.wsp))
{
	$solutionName = $wsp.Name.ToLower()
		
	EnsureAdded $solutionName
}

RestartAdminService

foreach ($wsp in (ls *.wsp))
{
	$solutionName = $wsp.Name.ToLower()
		
	EnsureDeployed $solutionName $targetWebApp.Name
}

ResetIIS

RestartTimerService

UpgradeFeatures $targetWebApp

Log "We're back up."