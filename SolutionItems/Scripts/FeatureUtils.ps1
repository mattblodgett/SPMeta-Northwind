. .\LoggingUtils.ps1

function UpgradeFeatures ($webApp)
{
	UpgradeSiteScopedFeatures $webApp
}

function UpgradeSiteScopedFeatures ($webApp)
{
	$featureOrder = @(
		"Northwind.Common"
	)
	
	foreach ($site in (Get-SPSite -WebApplication $webApp -Limit ALL))
	{
		#Log "Checking for upgrades on $($site.Url)..."
		
		$cultureInfo = New-Object -TypeName System.Globalization.CultureInfo -ArgumentList 1033
	
		foreach ($featureName in $featureOrder)
		{
			 $feature = $site.Features | ?{($_.Definition -ne $null) -and ($_.Definition.GetTitle($cultureInfo) -eq $featureName)}
			 
			 if ($feature)
			 {
				#Write-Host $featureName
				#Write-Host "Feature version: " + $feature.Version
				#Write-Host "Feature definition version: " + $feature.Definition.Version
				
				if ($feature.Version.CompareTo($feature.Definition.Version) -lt 0)
				{
					Log "Upgrading $featureName on $($site.Url)..."
					
					$haveNotifiedOfExceptions = $false
			
					$exceptions = $feature.Upgrade($false)
					
					if ($exceptions.Count -gt 0)
					{
						foreach ($exception in $exceptions)
						{
							if (!$haveNotifiedOfExceptions)
							{
								Log "Feature upgrade failed with exception(s):"
								$haveNotifiedOfExceptions = $true
							}
						
							Log "$exception" $true "DarkYellow"
						}
					}
								
					if (!$haveNotifiedOfExceptions)
					{
						Log "$featureName successfully upgraded on $($site.Url)"
					}
				}
				else
				{
					#Log "Do not need to upgrade $featureName"
				}
			 }
		}
	}
}