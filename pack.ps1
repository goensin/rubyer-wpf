# Rebuild all solutions
dotnet restore

$currentPath=Get-Location
# Create publish folder
$publishFolder="Publish"
if(!(Test-Path $publishFolder)){
	mkdir $publishFolder
    Write-Output "Create publish folder"
}

# Delete old packages
Write-Output "Delete old packages"
cd $publishFolder
del *.nupkg
cd ..

# Create nuget pack
$projectName="Rubyer"
$publishPath=Join-Path $currentPath $publishFolder

dotnet clean
dotnet pack -o $publishFolder -c Release

Write-Output "Publish success"
	
