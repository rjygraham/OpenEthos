param (
	[Parameter(Mandatory=$true)]
	[string] $EnvironmentName
)

# Create file path variables.
$TrustFrameworkBaseSourcePath = Join-Path $PSScriptRoot "SuSi" "TrustFrameworkBase.xml"
$TrustFrameworkBaseToDeployPath = Join-Path $PSScriptRoot "SuSi" "Deploy" "TrustFrameworkBase.xml"

$TrustFrameworkExtensionsSourcePath = Join-Path $PSScriptRoot "SuSi" "TrustFrameworkExtensions.xml"
$TrustFrameworkExtensionsToDeployPath = Join-Path $PSScriptRoot "SuSi" "Deploy" "TrustFrameworkExtensions.xml"

$SignInSourcePath = Join-Path $PSScriptRoot "SuSi" "Signin.xml"
$SignInToDeployPath = Join-Path $PSScriptRoot "SuSi" "Deploy" "Signin.xml"

$SignUpWithInvitationSourcePath = Join-Path $PSScriptRoot "SuSi" "SignUpWithInvitation.xml"
$SignUpWithInvitationDeployPath = Join-Path $PSScriptRoot "SuSi" "Deploy" "SignUpWithInvitation.xml"

$InvitationTrustFrameworkBaseSourcePath = Join-Path $PSScriptRoot "Invitation" "TrustFrameworkBase.xml"
$InvitationTrustFrameworkBaseDeployPath = Join-Path $PSScriptRoot "Invitation" "Deploy" "TrustFrameworkBase.xml"

$InvitationTrustFrameworkExtensionsSourcePath = Join-Path $PSScriptRoot "Invitation" "TrustFrameworkExtensions.xml"
$InvitationTrustFrameworkExtensionsDeployPath = Join-Path $PSScriptRoot "Invitation" "Deploy" "TrustFrameworkExtensions.xml"

$InvitationSourcePath = Join-Path $PSScriptRoot "Invitation" "Invitation.xml"
$InvitationDeployPath = Join-Path $PSScriptRoot "Invitation" "Deploy" "Invitation.xml"

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

New-Item -Path (Join-Path $PSScriptRoot "SuSi" "Deploy") -ItemType Directory -Force

# Create TrustFrameworkBase deployment file.
Get-Content $TrustFrameworkBaseSourcePath -Raw
	| % { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| % { $_.replace("{{$B2C_DEPLOYMENTMODE}}", $B2C_DEPLOYMENTMODE_VALUE) }
	| % { $_.replace("{{$B2C_EXTENSIONAPP_CLIENTID}}", $B2C_EXTENSIONAPP_CLIENTID_VALUE) }
	| % { $_.replace("{{$B2C_EXTENSIONAPP_OBJECTID}}", $B2C_EXTENSIONAPP_OBJECTID_VALUE) }
	| Out-File $TrustFrameworkBaseToDeployPath

# Create TrustFrameworkExtensions deployment file.
Get-Content $TrustFrameworkExtensionsSourcePath -Raw
	| % { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| % { $_.replace("{{$B2C_DEPLOYMENTMODE}}", $B2C_DEPLOYMENTMODE_VALUE) }
	| % { $_.replace("{{$B2C_APPLE_CLIENTID}}", $B2C_APPLE_CLIENTID_VALUE) }
	| % { $_.replace("{{$B2C_APPLE_CLIENTSECRET}}", $B2C_APPLE_CLIENTSECRET_VALUE) }
	| % { $_.replace("{{$B2C_GOOGLE_CLIENTID}}", $B2C_GOOGLE_CLIENTID_VALUE) }
	| % { $_.replace("{{$B2C_GOOGLE_CLIENTSECRET}}", $B2C_GOOGLE_CLIENTSECRET_VALUE) }
	| % { $_.replace("{{$B2C_MICROSOFT_CLIENTID}}", $B2C_MICROSOFT_CLIENTID_VALUE) }
	| % { $_.replace("{{$B2C_MICROSOFT_CLIENTSECRET}}", $B2C_MICROSOFT_CLIENTSECRET_VALUE) }
	| Out-File $TrustFrameworkExtensionsToDeployPath

# Create SignUpWithInvitation deployment file.
Get-Content $SignUpWithInvitationSourcePath -Raw
| % { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
| % { $_.replace("{{$B2C_DEPLOYMENTMODE}}", $B2C_DEPLOYMENTMODE_VALUE) }
| % { $_.replace("{{$B2C_ID_TOKEN_HINT_METADATA}}", $B2C_ID_TOKEN_HINT_METADATA_VALUE) }
| % { $_.replace("{{$B2C_ID_TOKEN_HINT_AUDIENCE}}", $B2C_ID_TOKEN_HINT_AUDIENCE_VALUE) }
| % { $_.replace("{{$B2C_ID_TOKEN_HINT_ISSUER}}", $B2C_ID_TOKEN_HINT_ISSUER_VALUE) }
| % { $_.replace("{{$B2C_ID_TOKEN_HINT_CERT}}", $B2C_ID_TOKEN_HINT_CERT_VALUE) }
| % { $_.replace("{{$B2C_APPINSIGHTS_INSTRUMENTATIONKEY}}", $B2C_APPINSIGHTS_INSTRUMENTATIONKEY_VALUE) }
| % { $_.replace("{{$B2C_APPINSIGHTS_DEVELOPERMODE}}", $B2C_APPINSIGHTS_DEVELOPERMODE_VALUE) }
| Out-File $SignUpWithInvitationDeployPath

# Create Signin deployment file.
Get-Content $SignInSourcePath -Raw
	| % { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| % { $_.replace("{{$B2C_DEPLOYMENTMODE}}", $B2C_DEPLOYMENTMODE_VALUE) }
	| % { $_.replace("{{$B2C_EXTENSIONAPP_CLIENTID}}", $B2C_EXTENSIONAPP_CLIENTID_VALUE) }
	| % { $_.replace("{{$B2C_EXTENSIONAPP_OBJECTID}}", $B2C_EXTENSIONAPP_OBJECTID_VALUE) }
	| % { $_.replace("{{$B2C_VALIDATEINVITATIONCODE_URL}}", $B2C_VALIDATEINVITATIONCODE_URL_VALUE) }
	| % { $_.replace("{{$B2C_CREATEPROFILE_URL}}", $B2C_CREATEPROFILE_URL_VALUE) }
	| % { $_.replace("{{$B2C_GETPROFILE_URL}}", $B2C_GETPROFILE_URL_VALUE) }
	| % { $_.replace("{{$B2C_APPLE_CLIENTID}}", $B2C_APPLE_CLIENTID_VALUE) }
	| % { $_.replace("{{$B2C_APPLE_CLIENTSECRET}}", $B2C_APPLE_CLIENTSECRET_VALUE) }
	| % { $_.replace("{{$B2C_GOOGLE_CLIENTID}}", $B2C_GOOGLE_CLIENTID_VALUE) }
	| % { $_.replace("{{$B2C_GOOGLE_CLIENTSECRET}}", $B2C_GOOGLE_CLIENTSECRET_VALUE) }
	| % { $_.replace("{{$B2C_MICROSOFT_CLIENTID}}", $B2C_MICROSOFT_CLIENTID_VALUE) }
	| % { $_.replace("{{$B2C_MICROSOFT_CLIENTSECRET}}", $B2C_MICROSOFT_CLIENTSECRET_VALUE) }
	| % { $_.replace("{{$B2C_APPINSIGHTS_INSTRUMENTATIONKEY}}", $B2C_APPINSIGHTS_INSTRUMENTATIONKEY_VALUE) }
	| % { $_.replace("{{$B2C_APPINSIGHTS_DEVELOPERMODE}}", $B2C_APPINSIGHTS_DEVELOPERMODE_VALUE) }
	| Out-File $SignInToDeployPath

New-Item -Path (Join-Path $PSScriptRoot "Invitation" "Deploy") -ItemType Directory -Force

Get-Content $InvitationTrustFrameworkBaseSourcePath -Raw
	| % { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| Out-File $InvitationTrustFrameworkBaseDeployPath

Get-Content $InvitationTrustFrameworkExtensionsSourcePath -Raw
	| % { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| % { $_.replace("{{$B2C_PROXY_IEF_CLIENTID}}", $B2C_PROXY_IEF_CLIENTID_VALUE) }
	| % { $_.replace("{{$B2C_IEF_CLIENTID}}", $B2C_IEF_CLIENTID_VALUE) }
	| Out-File $InvitationTrustFrameworkExtensionsDeployPath

Get-Content $InvitationSourcePath -Raw
	| % { $_.replace("{{$B2C_TENANTID}}", $B2C_TENANTID_VALUE) }
	| % { $_.replace("{{$B2C_TENANTOBJECTID}}", $B2C_TENANTOBJECTID_VALUE) }
	| Out-File $InvitationDeployPath