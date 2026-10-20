<#
.SYNOPSIS
    Local verification of PEA.NET before committing or pushing.

.DESCRIPTION
    Runs the same checks as CI plus the repository conventions:
      1. Clean Release build of src/PEA/PEA.sln
      2. Unit tests (PEA.Tests) in Release, and in Debug so that Debug.Assert checks run too
      3. PEA.ObserverApp build (outside the solution, Windows only)
      4. TSP reference run (bit-identical best distance and entity count)
      5. Folder == namespace for every file of the PEA library
      6. Solution/project file changes against -Base
      7. Diff filter: lines other than using/namespace changed (enforced with -MoveOnly)
    Exit code is 1 if any check fails.

.EXAMPLE
    ./tools/verify.ps1
    ./tools/verify.ps1 -Quick
    ./tools/verify.ps1 -Base origin/master -MoveOnly
#>
[CmdletBinding()]
param(
    [string] $Base = 'HEAD',
    [switch] $MoveOnly,
    [switch] $Quick,
    [switch] $SkipTsp,
    [string] $TspExpectedDistance = '7849.395176892104',
    [string] $TspExpectedEntities = '9006240'
)

$ErrorActionPreference = 'Continue'
$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot '..')).Path
$solution = Join-Path $repoRoot 'src/PEA/PEA.sln'
$testProject = Join-Path $repoRoot 'src/PEA/PEA.Tests/PEA.Tests.csproj'
$observerProject = Join-Path $repoRoot 'src/PEA/PEA.ObserverApp/PEA.ObserverApp.csproj'
$tspProject = Join-Path $repoRoot 'src/PEA/Examples/PEA_TSP_Example/PEA_TSP_Example.csproj'
$libraryRoot = Join-Path $repoRoot 'src/PEA/PEA'
$isWindowsHost = $env:OS -eq 'Windows_NT'

$results = New-Object System.Collections.Generic.List[object]

function Add-Result([string] $check, [string] $status, [string] $detail) {
    $results.Add([pscustomobject]@{ Check = $check; Status = $status; Detail = $detail })
    $color = switch ($status) { 'PASS' { 'Green' } 'FAIL' { 'Red' } 'WARN' { 'Yellow' } default { 'Gray' } }
    Write-Host ("[{0}] {1} {2}" -f $status, $check, $detail) -ForegroundColor $color
}

function Invoke-Native([string] $exe, [string[]] $arguments) {
    $output = & $exe @arguments 2>&1 | ForEach-Object { "$_" }
    [pscustomobject]@{ ExitCode = $LASTEXITCODE; Output = $output }
}

function Get-ErrorSummary([string[]] $output) {
    $errors = $output | Where-Object { $_ -cmatch ': error [A-Z]+[0-9]+:' } | Select-Object -Unique
    ($errors | Select-Object -First 5) -join [Environment]::NewLine
}

function Test-Build {
    $run = Invoke-Native 'dotnet' @('build', $solution, '-c', 'Release', '--no-incremental', '-nologo')
    if ($run.ExitCode -eq 0) {
        $warnings = ($run.Output | Where-Object { $_ -match '^\s+\d+ Warning\(s\)' } | Select-Object -Last 1)
        Add-Result 'Build PEA.sln' 'PASS' ("clean Release build; " + "$warnings".Trim())
        return $true
    }
    Add-Result 'Build PEA.sln' 'FAIL' (Get-ErrorSummary $run.Output)
    return $false
}

function Test-UnitTests([string] $configuration) {
    $arguments = @('test', $testProject, '-c', $configuration, '-nologo')
    if ($configuration -eq 'Release') { $arguments += '--no-build' }
    $run = Invoke-Native 'dotnet' $arguments
    $summary = "$($run.Output | Where-Object { $_ -match '(Passed|Failed)!' } | Select-Object -Last 1)".Trim()
    if (-not $summary) { $summary = Get-ErrorSummary $run.Output }
    $status = if ($run.ExitCode -eq 0) { 'PASS' } else { 'FAIL' }
    Add-Result "Unit tests ($configuration)" $status $summary
}

function Test-ObserverApp {
    if (-not $isWindowsHost) {
        Add-Result 'Build PEA.ObserverApp' 'SKIP' 'Windows-only project'
        return
    }
    $run = Invoke-Native 'dotnet' @('build', $observerProject, '-c', 'Release', '--no-incremental', '-nologo')
    if ($run.ExitCode -eq 0) { Add-Result 'Build PEA.ObserverApp' 'PASS' 'outside PEA.sln' }
    else { Add-Result 'Build PEA.ObserverApp' 'FAIL' (Get-ErrorSummary $run.Output) }
}

function Test-TspReference {
    $run = Invoke-Native 'dotnet' @('run', '--project', $tspProject, '-c', 'Release', '--no-build')
    $text = $run.Output -join "`n"
    $distanceOk = $text.Contains("Best distance: $TspExpectedDistance")
    $entitiesOk = $text.Contains("Entities: $TspExpectedEntities ")
    $found = ($run.Output | Where-Object { $_ -match 'Best distance|Entities' }) -join ' | '
    if ($run.ExitCode -eq 0 -and $distanceOk -and $entitiesOk) { Add-Result 'TSP reference run' 'PASS' 'bit-identical' }
    else { Add-Result 'TSP reference run' 'FAIL' ("expected $TspExpectedDistance / $TspExpectedEntities, got: $found") }
}

function Get-ExpectedNamespace([string] $file) {
    $directory = Split-Path $file -Parent
    $relative = $directory.Substring($libraryRoot.Length).Trim('\', '/')
    if ($relative -eq '') { return 'Pea' }
    return 'Pea.' + ($relative -replace '[\\/]', '.')
}

function Test-FolderNamespace {
    $mismatches = New-Object System.Collections.Generic.List[string]
    $files = Get-ChildItem $libraryRoot -Recurse -Filter '*.cs' |
        Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' }
    foreach ($file in $files) {
        $match = [regex]::Match((Get-Content $file.FullName -Raw), '(?m)^\s*namespace\s+([A-Za-z0-9_.]+)')
        if (-not $match.Success) { continue }
        $expected = Get-ExpectedNamespace $file.FullName
        if ($match.Groups[1].Value -ne $expected) {
            $mismatches.Add(("{0}: {1} (expected {2})" -f $file.FullName.Substring($repoRoot.Length + 1), $match.Groups[1].Value, $expected))
        }
    }
    if ($mismatches.Count -eq 0) { Add-Result 'Folder == namespace' 'PASS' ("{0} files" -f @($files).Count) }
    else { Add-Result 'Folder == namespace' 'FAIL' ($mismatches -join [Environment]::NewLine) }
}

function Get-WorkingTreeDiff([string[]] $diffArguments) {
    $temporaryIndex = [System.IO.Path]::GetTempFileName()
    Remove-Item $temporaryIndex
    $previousIndex = $env:GIT_INDEX_FILE
    try {
        $env:GIT_INDEX_FILE = $temporaryIndex
        & git -C $repoRoot read-tree HEAD 2>&1 | Out-Null
        & git -C $repoRoot add -A 2>&1 | Out-Null
        return (& git -C $repoRoot diff --cached @diffArguments $Base 2>&1 | ForEach-Object { "$_" })
    }
    finally {
        $env:GIT_INDEX_FILE = $previousIndex
        Remove-Item $temporaryIndex -ErrorAction SilentlyContinue
    }
}

function Test-ProjectFiles {
    $changed = Get-WorkingTreeDiff @('--name-only', '--', '*.sln', '*.csproj')
    if (-not $changed) { Add-Result 'Solution/project files' 'PASS' "unchanged vs $Base" ; return }
    $status = if ($MoveOnly) { 'FAIL' } else { 'WARN' }
    Add-Result 'Solution/project files' $status ("changed vs ${Base} (review): " + ($changed -join ', '))
}

function Test-DiffFilter {
    $diff = Get-WorkingTreeDiff @('-M', '--unified=0', '--', '*.cs')
    $bom = [string][char]0xFEFF
    $other = $diff | Where-Object { $_ -match '^[+-]' -and $_ -notmatch '^(\+\+\+|---)' } |
        ForEach-Object { $_.Replace($bom, '') } |
        Where-Object { $_ -notmatch '^[+-]\s*$' -and $_ -notmatch '^[+-]\s*(using\s+[A-Za-z0-9_.=\s]+;|namespace\s+[A-Za-z0-9_.]+;?)\s*$' }
    $count = @($other).Count
    if ($count -eq 0) { Add-Result 'Diff filter' 'PASS' "only using/namespace lines changed vs $Base"; return }
    $status = if ($MoveOnly) { 'FAIL' } else { 'INFO' }
    $sample = ($other | Select-Object -First 10) -join [Environment]::NewLine
    Add-Result 'Diff filter' $status ("{0} other changed lines vs {1}:{2}{3}" -f $count, $Base, [Environment]::NewLine, $sample)
}

Write-Host "PEA.NET verification (base: $Base$(if ($MoveOnly) { ', move-only' })$(if ($Quick) { ', quick' }))" -ForegroundColor Cyan

if (-not $Quick) {
    $built = Test-Build
    if ($built) {
        Test-UnitTests 'Release'
        if (-not $SkipTsp) { Test-TspReference } else { Add-Result 'TSP reference run' 'SKIP' '-SkipTsp' }
    }
    Test-UnitTests 'Debug'
    Test-ObserverApp
}
Test-FolderNamespace
Test-ProjectFiles
Test-DiffFilter

Write-Host ''
$results | Format-Table -AutoSize -Wrap Check, Status
if ($results | Where-Object { $_.Status -eq 'FAIL' }) { exit 1 }
exit 0
