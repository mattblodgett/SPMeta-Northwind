function IsSolutionAdded ($solutionName)
{
	$solution = Get-SPSolution | ?{$_.Name -eq $solutionName}
	
	if ($solution)
	{
		return $true
	}
	else
	{
		return $false
	}
}

function IsSolutionDeployedToWebApp ($solutionName, $webAppName)
{
	$solution = Get-SPSolution $solutionName

	$webApp = $solution.DeployedWebApplications | ?{$_.Name -eq $webAppName}
	
	if ($webApp)
	{
		return $true
	}
	else
	{
		return $false
	}
}

function WaitForSolutionJob ($solution)
{
	while ($solution.JobExists)
	{
		$solutionName = $solution.Name
	
		Log "$solutionName has pending job..."

		Start-Sleep -Seconds 5
	}
}

function EnsureRetracted ($solutionName)
{
	if (IsSolutionAdded $solutionName)
	{
		#Log "$solutionName has been added"
	
		$solution = Get-SPSolution $solutionName
		
		if ($solution.Deployed)
		{
			#Log "$solutionName has been deployed"
			
			if ($solution.ContainsWebApplicationResource)
			{
				#Log "$solutionName does contain web application resources"
				Log "Retracting $solutionName..."
				
				Uninstall-SPSolution $solutionName -Confirm:$false -AllWebApplications
			}
			else
			{
				#Log "$solutionName does NOT contain web application resources"
				Log "Retracting $solutionName..."
				
				Uninstall-SPSolution $solutionName -Confirm:$false
			}
			
			WaitForSolutionJob $solution
			
			Log "$solutionName successfully retracted"
		}
		else
		{
			#Log "$solutionName has NOT been deployed"
		}
	}
	else
	{
		#Log "$solutionName has NOT been added"
	}
}

function EnsureDeleted ($solutionName)
{
	if (IsSolutionAdded $solutionName)
	{
		#Log "$solutionName has been added"
	
		$solution = Get-SPSolution $solutionName
		
		if ($solution.Deployed)
		{
			#Log "$solutionName has been deployed"
		}
		else
		{
			#Log "$solutionName has NOT been deployed"
			Log "Deleting $solutionName..."
			
			Remove-SPSolution $solutionName -Confirm:$false
			
			Log "$solutionName successfully deleted"
		}
	}
	else
	{
		#Log "$solutionName has NOT been added"
	}
}

function EnsureAdded ($solutionName)
{
	if (IsSolutionAdded $solutionName)
	{
		#Log "$solutionName has been added"
	}
	else
	{
		#Log "$solutionName has NOT been added"
		Log "Adding $solutionName..."
		
		$wspPath = Resolve-Path $solutionName
		
		Add-SPSolution $wspPath | Out-Null
		
		Log "$solutionName successfully added"
	}
}

function ConfirmDeployed ($solution, $webAppName)
{
	if ($solution.Deployed)
	{
		Log "$($solution.Name) successfully deployed"
	}
	else
	{
		Log "Deployment of $($solution.Name) failed:"		
		Log $solution.LastOperationDetails $true "DarkYellow"
		
		if ($solution.LastOperationDetails -match "feature with ID ([a-z0-9-]*) has already been installed")
		{
			$stuckFeatureId = $matches[1]
			
			Log "Attempting to unstick feature $stuckFeatureId..."
			
			Uninstall-SPFeature $stuckFeatureId -Confirm:$false
			
			EnsureDeployed $solution.Name $webAppName
		}
	}
}

function EnsureDeployed ($solutionName, $webAppName)
{
	if (IsSolutionAdded $solutionName)
	{
		#Log "$solutionName has been added"
		
		$solution = Get-SPSolution $solutionName
		
		if ($solution.ContainsWebApplicationResource)
		{
			if (IsSolutionDeployedToWebApp $solutionName $webAppName)
			{
				#Log "$solutionName has been deployed to $webAppName"
			}
			else
			{
				#Log "$solutionName has NOT been deployed to $webAppName"
				Log "Deploying $solutionName to $webAppName..."
				
				Install-SPSolution $solutionName -GACDeployment -WebApplication $webAppName -Force
				
				WaitForSolutionJob $solution
				
				ConfirmDeployed $solution $webAppName
			}
		}
		else
		{
			if ($solution.Deployed)
			{
				#Log "$solutionName has been deployed globally"
			}
			else
			{
				#Log "$solutionName has NOT been deployed globally"
				Log "Deploying $solutionName globally..."
				
				Install-SPSolution $solutionName -GACDeployment
				
				WaitForSolutionJob $solution
				
				ConfirmDeployed $solution $webAppName
			}
		}
	}
	else
	{
		#Log "$solutionName has NOT been added"
	}
}