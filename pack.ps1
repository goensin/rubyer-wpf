# Rebuild all solutions
#dotnet restore

$currentPath=Get-Location
Write-Output $currentPath
# Create publish folder
$publishFolder= Join-Path $currentPath "Publish"
Write-Output "publish folder" $publishFolder
if(!(Test-Path $publishFolder)){
	mkdir $publishFolder
    Write-Output "Create publish folder"
}

# Delete old packages
Write-Output "Delete old packages"
cd $publishFolder
del *.nupkg
cd ..

# move rubyer folder
$projectName=Join-Path $currentPath "Rubyer"
cd $projectName

# pack
dotnet pack -o $publishFolder -c Release
cd ..
Write-Output "Publish success"
	
