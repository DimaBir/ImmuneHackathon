param(
    [string]$KustoUri,
    [string]$BearerToken,
    [string]$UserId,
    [string]$Query = "GetTvmLogs | where pod_name has ""ruleengine"" | where env_time > ago(1h) | take 1000 | project service_name, Message, Level, TIMESTAMP, DC, pod_name"
)

$bodyContent = @{
    db = "Geneva"
    csl = $Query
    properties = @{
        Options = @{
            servertimeout = "00:04:00"
            queryconsistency = "strongconsistency"
            query_language = "kql"
            request_readonly = $false
            request_readonly_hardline = $false
        }
    }
}

$body = $bodyContent | ConvertTo-Json -Compress

$response = Invoke-WebRequest -UseBasicParsing -Uri $KustoUri `
   -Method 'POST' `
   -Headers @{
       "Accept"                  = "application/json"
       "Accept-Encoding"         = "gzip, deflate, br, zstd"
       "Accept-Language"         = "en-US,en;q=0.9"
       "Authorization"           = "$BearerToken"
       "Origin"                  = "https://dataexplorer.azure.com"
       "Referer"                 = "https://dataexplorer.azure.com/"
       "Sec-Fetch-Dest"          = "empty"
       "Sec-Fetch-Mode"          = "cors"
       "Sec-Fetch-Site"          = "cross-site"
       "sec-ch-ua"               = "`"Not(A:Brand`";v=`"99`", `"Microsoft Edge`";v=`"133`", `"Chromium`";v=`"133`""
       "sec-ch-ua-mobile"        = "?0"
       "sec-ch-ua-platform"      = "`"Windows`""
       "x-ms-app"                = "Kusto.Web.KWE:2.188.0-1|embeddedIn:dataexplorer.azure.com"
       "x-ms-client-request-id"  = "Kusto.Web.KWE.Query;unique-request-id"
       "x-ms-user-id"            = $UserId
   } `
   -ContentType "application/json; charset=UTF-8" `
   -Body $body

$logData = $response.Content | ConvertFrom-Json;
$primaryResult = $logData | Where-Object { $_.FrameType -eq 'DataTable' -and $_.TableKind -eq 'PrimaryResult' };
$logs = $primaryResult | ForEach-Object { $_.Rows } | Select-Object -First 1000;
$logs | ConvertTo-Json -Depth 3
