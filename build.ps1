function Get-ScriptDirectory
{
	$Invocation = (Get-Variable MyInvocation -Scope 1).Value
	Split-Path $Invocation.MyCommand.Path
}

$version = "v4.0.30319"

# build the solution from scratch
. $env:windir\Microsoft.NET\Framework\$version\MSBuild.exe GlimpseDependencies.sln /t:Rebuild /ToolsVersion:4.0 /p:configuration=Release /m /p:BUILD_NUMBER=$build_number /m /v:M /nr:false

$mvc3nuspec = Join-Path (Get-ScriptDirectory) GlimpseDependencies.nuspec
$mvc4nuspec = Join-Path (Get-ScriptDirectory) GlimpseDependencies.MVC4.nuspec
$basePath = (Get-ScriptDirectory) 

$nuget = Join-Path (Get-ScriptDirectory) .nuget\NuGet.exe

$pkg_folder = "artifacts"

if(!(test-path $pkg_folder -pathtype container)){
	New-Item -Type Directory $pkg_folder | Out-Null
} 

. $nuget pack $mvc3nuspec -BasePath $basePath -OutputDir $pkg_folder
. $nuget pack $mvc4nuspec -BasePath $basePath -OutputDir $pkg_folder
