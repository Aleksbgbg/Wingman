param (
    [Parameter(Mandatory=$true)][string]$oldVersion,
    [Parameter(Mandatory=$true)][string]$newVersion
)

Write-Host "Publishing new Wingman version: $($newVersion)"

Write-Host "Updating new version in all projects..."

cd ..

Get-ChildItem -Directory -Filter Wingman.* -Exclude Wingman.Tests, Wingman.WpfAppExample  | foreach {
    cd $_.FullName
    
    $projectFile = "$($_.Name).csproj"
    (Get-Content -Path $projectFile) -replace "$($oldVersion).0", "$($newVersion).0" | Out-File $projectFile
    
    dotnet build -c=Release
}

cd ..\NuGet

Write-Host "Updating new version in all packages..."

Get-ChildItem *.nuspec | foreach {
    (Get-Content -Path $_.FullName) -replace $oldVersion, $newVersion | Out-File ($_.FullName -replace $oldVersion, $newVersion)
    Remove-Item $_.FullName
}

Write-Host "Deleting old packages..."

$nugetKey = Get-Content -Raw nuget-key.txt 
$nugetUrl = "https://api.nuget.org/v3/index.json"

Get-ChildItem *.nupkg | foreach {
    Remove-Item -Path $_.FullName
    nuget delete ($_.Name -replace ".$($oldVersion).nupkg", "") $oldVersion -ApiKey $nugetKey -Source $nugetUrl -NoPrompt
}

Write-Host "Packing new packages..."

Get-ChildItem *.nuspec | foreach {
    nuget pack $_.FullName
}

Write-Host "Publishing new packages..."

Get-ChildItem *.nupkg | foreach {
    dotnet nuget push $_.FullName -k $nugetKey -s $nugetUrl
}