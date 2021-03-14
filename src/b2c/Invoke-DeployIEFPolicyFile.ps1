[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$TenantId,
    [Parameter(Mandatory = $true)][string]$ClientId,
    [Parameter(Mandatory = $true)][string]$ClientSecret
)

$Policies = @{
    B2C_1A_TrustFrameworkBase = "TrustFrameworkBase.xml";
    B2C_1A_TrustFrameworkExtensions = "TrustFrameworkExtensions.xml";
    B2C_1A_Invitation = "Invitation.xml";
    B2C_1A_SU_Invitation = "SignUpWithInvitation.xml";
    B2C_1A_SI = "SignIn.xml";
}

try {
    $Body = @{grant_type = "client_credentials"; scope = "https://graph.microsoft.com/.default"; client_id = $ClientId; client_secret = $ClientSecret }

    $Response = Invoke-RestMethod -Uri https://login.microsoftonline.com/$TenantId/oauth2/v2.0/token -Method Post -Body $Body
    $Token = $Response.access_token

    $Headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
    $Headers.Add("Content-Type", 'application/xml')
    $Headers.Add("Authorization", 'Bearer ' + $token)

    foreach ($Policy in $Policies.GetEnumerator()) {
        $PolicyId = $Policy.Key
        $FileName = $Policy.Value

        $GraphUri = 'https://graph.microsoft.com/beta/trustframework/policies/' + $PolicyId + '/$value'
        $PolicyContent = Get-Content $FileName
        $Response = Invoke-RestMethod -Uri $GraphUri -Method Put -Body $PolicyContent -Headers $Headers

        Write-Host "Policy" $PolicyId "uploaded successfully."
    }
}
catch {
    Write-Host "StatusCode:" $_.Exception.Response.StatusCode.value__

    $_

    $StreamReader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
    $StreamReader.BaseStream.Position = 0
    $StreamReader.DiscardBufferedData()
    $SrrResp = $streamReader.ReadToEnd()
    $StreamReader.Close()

    $ErrResp

    exit 1
}

exit 0
