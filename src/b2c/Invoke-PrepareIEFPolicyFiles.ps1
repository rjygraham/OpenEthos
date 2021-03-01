param (
	[Parameter(Mandatory=$true)]
	[string] $EnvironmentName
)

# Create file path variables.
$BasePath = Join-Path $PSScriptRoot "Base"
$SuSiPath = Join-Path $PSScriptRoot "SuSi"
$InvitationPath = Join-Path $PSScriptRoot "Invitation"
$DeployPath = Join-Path $PSScriptRoot "Deploy"

$TrustFrameworkBaseSourcePath = Join-Path $BasePath "TrustFrameworkBase.xml"
$TrustFrameworkBaseToDeployPath = Join-Path $DeployPath "TrustFrameworkBase.xml"

$TrustFrameworkExtensionsSourcePath = Join-Path $BasePath "TrustFrameworkExtensions.xml"
$TrustFrameworkExtensionsToDeployPath = Join-Path $DeployPath "TrustFrameworkExtensions.xml"

$SignUpWithInvitationSourcePath = Join-Path $SuSiPath "SignUpWithInvitation.xml"
$SignUpWithInvitationDeployPath = Join-Path $DeployPath "SignUpWithInvitation.xml"

$SignInSourcePath = Join-Path $SuSiPath "Signin.xml"
$SignInToDeployPath = Join-Path $DeployPath "Signin.xml"

$InvitationSourcePath = Join-Path $InvitationPath "Invitation.xml"
$InvitationDeployPath = Join-Path $DeployPath "Invitation.xml"

# Create variables from environment variables.
$B2C_TENANTID = "B2C_TENANTID"
$B2C_TENANTID_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_TENANTID + "_" + $EnvironmentName)

$B2C_DEPLOYMENTMODE = "B2C_DEPLOYMENTMODE"
$B2C_DEPLOYMENTMODE_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_DEPLOYMENTMODE + "_" + $EnvironmentName)

$B2C_EXTENSIONAPP_CLIENTID = "B2C_EXTENSIONAPP_CLIENTID"
$B2C_EXTENSIONAPP_CLIENTID_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_EXTENSIONAPP_CLIENTID + "_" + $EnvironmentName)
$B2C_EXTENSIONAPP_OBJECTID = "B2C_EXTENSIONAPP_OBJECTID"
$B2C_EXTENSIONAPP_OBJECTID_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_EXTENSIONAPP_OBJECTID + "_" + $EnvironmentName)

$B2C_VALIDATEINVITATIONCODE_URL = "B2C_VALIDATEINVITATIONCODE_URL"
$B2C_VALIDATEINVITATIONCODE_URL_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_VALIDATEINVITATIONCODE_URL + "_" + $EnvironmentName)
$B2C_CREATEPROFILE_URL = "B2C_CREATEPROFILE_URL"
$B2C_CREATEPROFILE_URL_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_CREATEPROFILE_URL + "_" + $EnvironmentName)
$B2C_GETPROFILE_URL = "B2C_GETPROFILE_URL"
$B2C_GETPROFILE_URL_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_GETPROFILE_URL + "_" + $EnvironmentName)

$B2C_APPLE_CLIENTID = "B2C_APPLE_CLIENTID"
$B2C_APPLE_CLIENTID_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_APPLE_CLIENTID + "_" + $EnvironmentName)
$B2C_APPLE_CLIENTSECRET = "B2C_APPLE_CLIENTSECRET"
$B2C_APPLE_CLIENTSECRET_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_APPLE_CLIENTSECRET + "_" + $EnvironmentName)

$B2C_GOOGLE_CLIENTID = "B2C_GOOGLE_CLIENTID"
$B2C_GOOGLE_CLIENTID_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_GOOGLE_CLIENTID + "_" + $EnvironmentName)
$B2C_GOOGLE_CLIENTSECRET = "B2C_GOOGLE_CLIENTSECRET"
$B2C_GOOGLE_CLIENTSECRET_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_GOOGLE_CLIENTSECRET + "_" + $EnvironmentName)

$B2C_MICROSOFT_CLIENTID = "B2C_MICROSOFT_CLIENTID"
$B2C_MICROSOFT_CLIENTID_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_MICROSOFT_CLIENTID + "_" + $EnvironmentName)
$B2C_MICROSOFT_CLIENTSECRET = "B2C_MICROSOFT_CLIENTSECRET"
$B2C_MICROSOFT_CLIENTSECRET_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_MICROSOFT_CLIENTSECRET + "_" + $EnvironmentName)

$B2C_APPINSIGHTS_INSTRUMENTATIONKEY = "B2C_APPINSIGHTS_INSTRUMENTATIONKEY"
$B2C_APPINSIGHTS_INSTRUMENTATIONKEY_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_APPINSIGHTS_INSTRUMENTATIONKEY + "_" + $EnvironmentName)
$B2C_APPINSIGHTS_DEVELOPERMODE = "B2C_APPINSIGHTS_DEVELOPERMODE"
$B2C_APPINSIGHTS_DEVELOPERMODE_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_APPINSIGHTS_DEVELOPERMODE + "_" + $EnvironmentName)

$B2C_ID_TOKEN_HINT_METADATA = "B2C_ID_TOKEN_HINT_METADATA"
$B2C_ID_TOKEN_HINT_METADATA_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_ID_TOKEN_HINT_METADATA + "_" + $EnvironmentName)
$B2C_ID_TOKEN_HINT_AUDIENCE = "B2C_ID_TOKEN_HINT_AUDIENCE"
$B2C_ID_TOKEN_HINT_AUDIENCE_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_ID_TOKEN_HINT_AUDIENCE + "_" + $EnvironmentName)
$B2C_ID_TOKEN_HINT_ISSUER = "B2C_ID_TOKEN_HINT_ISSUER"
$B2C_ID_TOKEN_HINT_ISSUER_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_ID_TOKEN_HINT_ISSUER + "_" + $EnvironmentName)
$B2C_ID_TOKEN_HINT_CERT = "B2C_ID_TOKEN_HINT_CERT"
$B2C_ID_TOKEN_HINT_CERT_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_ID_TOKEN_HINT_CERT + "_" + $EnvironmentName)

$B2C_TENANTOBJECTID = "B2C_TENANTOBJECTID"
$B2C_TENANTOBJECTID_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_TENANTOBJECTID + "_" + $EnvironmentName)

$B2C_PROXY_IEF_CLIENTID = "B2C_PROXY_IEF_CLIENTID"
$B2C_PROXY_IEF_CLIENTID_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_PROXY_IEF_CLIENTID + "_" + $EnvironmentName)
$B2C_IEF_CLIENTID = "B2C_IEF_CLIENTID"
$B2C_IEF_CLIENTID_VALUE = [System.Environment]::GetEnvironmentVariable($B2C_IEF_CLIENTID + "_" + $EnvironmentName)

New-Item -Path $DeployPath -ItemType Directory -Force

# Create TrustFrameworkBase deployment file.
Get-Content $TrustFrameworkBaseSourcePath -Raw
	| ForEach-Object { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| ForEach-Object { $_.replace("{{$B2C_DEPLOYMENTMODE}}", $B2C_DEPLOYMENTMODE_VALUE) }
	| ForEach-Object { $_.replace("{{$B2C_EXTENSIONAPP_CLIENTID}}", $B2C_EXTENSIONAPP_CLIENTID_VALUE) }
	| ForEach-Object { $_.replace("{{$B2C_EXTENSIONAPP_OBJECTID}}", $B2C_EXTENSIONAPP_OBJECTID_VALUE) }
	| Out-File $TrustFrameworkBaseToDeployPath

# Create TrustFrameworkExtensions deployment file.
Get-Content $TrustFrameworkExtensionsSourcePath -Raw
	| ForEach-Object  { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_DEPLOYMENTMODE}}", $B2C_DEPLOYMENTMODE_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPLE_CLIENTID}}", $B2C_APPLE_CLIENTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPLE_CLIENTSECRET}}", $B2C_APPLE_CLIENTSECRET_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_GOOGLE_CLIENTID}}", $B2C_GOOGLE_CLIENTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_GOOGLE_CLIENTSECRET}}", $B2C_GOOGLE_CLIENTSECRET_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_MICROSOFT_CLIENTID}}", $B2C_MICROSOFT_CLIENTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_MICROSOFT_CLIENTSECRET}}", $B2C_MICROSOFT_CLIENTSECRET_VALUE) }
	| Out-File $TrustFrameworkExtensionsToDeployPath

# Create SignUpWithInvitation deployment file.
Get-Content $SignUpWithInvitationSourcePath -Raw
	| ForEach-Object  { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_DEPLOYMENTMODE}}", $B2C_DEPLOYMENTMODE_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_ID_TOKEN_HINT_METADATA}}", $B2C_ID_TOKEN_HINT_METADATA_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_ID_TOKEN_HINT_AUDIENCE}}", $B2C_ID_TOKEN_HINT_AUDIENCE_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_ID_TOKEN_HINT_ISSUER}}", $B2C_ID_TOKEN_HINT_ISSUER_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_ID_TOKEN_HINT_CERT}}", $B2C_ID_TOKEN_HINT_CERT_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPINSIGHTS_INSTRUMENTATIONKEY}}", $B2C_APPINSIGHTS_INSTRUMENTATIONKEY_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPINSIGHTS_DEVELOPERMODE}}", $B2C_APPINSIGHTS_DEVELOPERMODE_VALUE) }
	| Out-File $SignUpWithInvitationDeployPath

# Create Signin deployment file.
Get-Content $SignInSourcePath -Raw
	| ForEach-Object  { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_DEPLOYMENTMODE}}", $B2C_DEPLOYMENTMODE_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_EXTENSIONAPP_CLIENTID}}", $B2C_EXTENSIONAPP_CLIENTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_EXTENSIONAPP_OBJECTID}}", $B2C_EXTENSIONAPP_OBJECTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_VALIDATEINVITATIONCODE_URL}}", $B2C_VALIDATEINVITATIONCODE_URL_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_CREATEPROFILE_URL}}", $B2C_CREATEPROFILE_URL_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_GETPROFILE_URL}}", $B2C_GETPROFILE_URL_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPLE_CLIENTID}}", $B2C_APPLE_CLIENTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPLE_CLIENTSECRET}}", $B2C_APPLE_CLIENTSECRET_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_GOOGLE_CLIENTID}}", $B2C_GOOGLE_CLIENTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_GOOGLE_CLIENTSECRET}}", $B2C_GOOGLE_CLIENTSECRET_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_MICROSOFT_CLIENTID}}", $B2C_MICROSOFT_CLIENTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_MICROSOFT_CLIENTSECRET}}", $B2C_MICROSOFT_CLIENTSECRET_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPINSIGHTS_INSTRUMENTATIONKEY}}", $B2C_APPINSIGHTS_INSTRUMENTATIONKEY_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPINSIGHTS_DEVELOPERMODE}}", $B2C_APPINSIGHTS_DEVELOPERMODE_VALUE) }
	| Out-File $SignInToDeployPath

Get-Content $InvitationSourcePath -Raw
	| ForEach-Object  { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_TENANTOBJECTID}}", $B2C_TENANTOBJECTID_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_DEPLOYMENTMODE}}", $B2C_DEPLOYMENTMODE_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPINSIGHTS_INSTRUMENTATIONKEY}}", $B2C_APPINSIGHTS_INSTRUMENTATIONKEY_VALUE) }
	| ForEach-Object  { $_.replace("{{$B2C_APPINSIGHTS_DEVELOPERMODE}}", $B2C_APPINSIGHTS_DEVELOPERMODE_VALUE) }
	| Out-File $InvitationDeployPath