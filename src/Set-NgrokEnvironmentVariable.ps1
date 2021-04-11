$ErrorActionPreference= 'silentlycontinue'

do {
    $Response = Invoke-WebRequest -Uri http://localhost:4040/api/tunnels | ConvertFrom-Json -Depth 100
    if (!$Response) {
        Start-Sleep -Seconds 5
    }
} until ($Response)

$NgrokUrl = $Response.tunnels[0].public_url
[Environment]::SetEnvironmentVariable("API_HOST_URL", $NgrokUrl, "User")
Write-Host $NgrokUrl

while ($true) {
    Start-Sleep -Seconds 10
}