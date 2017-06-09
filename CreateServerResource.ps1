Param (
    [Parameter(Mandatory=$True)]
    [string]$SolutionDir
)

"Bundling server and generating manifest files..."

$xml = @"
<meta>
    <info name="ProjetoRP Gamemode" author="Manajjdisc" type="script" />
    <script src="ProjetoRP.dll" type="server" lang="compiled" />
	<export class="Ui" event="onUiEventTrigger" />
</meta>
"@

$ResourceName = "projetorp"
$SolutionDir = $SolutionDir -replace '"', ""
$BasePath = "$(Resolve-Path $SolutionDir)\ProjetoRP\Modules"

$ServerResourcePath = "$($SolutionDir)\server\resources\$($ResourceName)"
$ModulesPath = "$BasePath\*\Client\*"
$arr = Get-ChildItem $ModulesPath -rec | where { ! $_.PSIsContainer }

$xml = [xml]$xml

foreach ($item in $arr) 
{
	$RelativePath = $item.fullname -split "Modules\\"
	$RelativePath = $RelativePath[1]
	$Folders = $RelativePath -split "\\"
	$Module = $Folders[0]
	$File = $Folders[2]
	
	Robocopy "$($BasePath)\$($Module)\Client" "$($ServerResourcePath)\$($Module)" /MIR /FFT /Z /XA:H /W:5 > $null
	
	# New-Item -ItemType Directory -Force -Path "$($ServerResourcePath)\$($Module)"
	# xcopy $item.fullname "$($ServerResourcePath)\$($Module)" /y
	
	if ($item.fullname -like "*.client.js") 
	{
		$Script = $xml.CreateElement("script");
		
		#<script src="player_local.js" type="client" lang="javascript" />
		$ScriptPath = $item.fullname -split [RegEx]::Escape("$($BasePath)")
		$ScriptPath = $ScriptPath[1] -replace "\\Client", ""
		
		$Script.SetAttribute("src", $ScriptPath);
		$Script.SetAttribute("type", "client");
		$Script.SetAttribute("lang", "javascript");
		
		$xml.meta.AppendChild($Script) > $null
	} 
	else 
	{
		$Script = $xml.CreateElement("file");
		
		#<script src="player_local.js" type="client" lang="javascript" />
		$ScriptPath = $item.fullname -split [RegEx]::Escape("$($BasePath)")
		$ScriptPath = $ScriptPath[1] -replace "\\Client", ""
		
		$Script.SetAttribute("src", $ScriptPath);
		
		$xml.meta.AppendChild($Script) > $null
	}
} 

$xml.Save("$($ServerResourcePath)\meta.xml");