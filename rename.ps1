param(
    [Parameter(Mandatory=$true)]
    [string]$NewName
)

$OldName = "Generic"
$RootPath = Get-Location
$ExcludeDirs = @(".git", ".vs", "bin", "obj", ".idea")
$TextExtensions = @(".cs", ".csproj", ".sln", ".json", ".md", ".http", ".xml", ".config", ".ps1", ".cmd")

Write-Host "Renaming project from '$OldName' to '$NewName'..." -ForegroundColor Cyan

# 1. Replace Content in Files
Write-Host "Step 1: Replacing content in files..." -ForegroundColor Yellow
$files = Get-ChildItem -Path $RootPath -Recurse -File | Where-Object { 
    $pathParts = $_.FullName.Split([IO.Path]::DirectorySeparatorChar)
    $isExcluded = $pathParts | Where-Object { $ExcludeDirs -contains $_ }
    $isTextFile = $TextExtensions -contains $_.Extension
    return ($null -eq $isExcluded) -and $isTextFile -and ($_.Name -ne "rename.ps1")
}

foreach ($file in $files) {
    $content = Get-Content -Path $file.FullName -Raw
    if ($content -match $OldName) {
        Write-Host "Updating content: $($file.Name)" -ForegroundColor Gray
        $newContent = $content -replace $OldName, $NewName
        Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
    }
}

# 2. Rename Directories and Files
Write-Host "Step 2: Renaming files and directories..." -ForegroundColor Yellow
$items = Get-ChildItem -Path $RootPath -Recurse | Where-Object { 
    $pathParts = $_.FullName.Split([IO.Path]::DirectorySeparatorChar)
    $isExcluded = $pathParts | Where-Object { $ExcludeDirs -contains $_ }
    return ($null -eq $isExcluded) -and ($_.Name -like "*$OldName*")
}

# Sort by length descending to rename deepest paths first
$itemsToRename = $items | Sort-Object { $_.FullName.Length } -Descending

foreach ($item in $itemsToRename) {
    $newNameItem = $item.Name -replace $OldName, $NewName
    Write-Host "Renaming: $($item.Name) -> $newNameItem" -ForegroundColor Gray
    Rename-Item -Path $item.FullName -NewName $newNameItem
}

Write-Host "Rename complete! You can now open '$NewName.sln'." -ForegroundColor Green
