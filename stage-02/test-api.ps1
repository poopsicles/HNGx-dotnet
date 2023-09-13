param (
    [string]$Url
)

if (-not $Url) {
    Write-Host "Usage: test-api.ps1 <url>"
    exit 1
}

$Url = $Url.TrimEnd("/")

Write-Host "Using $Url..."
Write-Host "Generating data..."

$ColourList = "Red", "Green", "Blue", "Yellow", "Orange", "Purple", "Black"

$Data = @{
    # a random string of letters
    # name = "FWoje"
    name            = -join ((65..90) + (97..122) | Get-Random -Count 5 | ForEach-Object { [char]$_ })
    age             = (Get-Random -Minimum 1 -Maximum 100)
    favouriteColour = (Get-Random -InputObject $ColourList)
}

Function Compare-Response([ref]$Passed, $Line) {
    if ($Line[1] -eq $Line[2]) {
        # Write-Host "$($Line[0]) matches, expected $($Line[1]), got $($Line[2])"
        Write-Host "$($Line[0]) matches..."
    }
    else {
        $Passed.Value = $false
        Write-Host "FAIL " -ForegroundColor Red -NoNewLine
        Write-Host "Expected $($Line[0]): $($Line[1]), got $($Line[2])"
    }
}

Write-Host "Name: $($data.name)"
Write-Host "Age: $($data.age)"
Write-Host "Favourite Colour: $($data.favouriteColour)"
Write-Host ""

# Test POST /api for 201
Write-Host "Testing " -NoNewLine
Write-Host "POST $Url" -ForegroundColor Blue -NoNewLine
Write-Host ":"

$PostPassed = $true
$PostJsonData = ConvertTo-Json -InputObject $Data

try {
    $PostResponse = Invoke-WebRequest -Uri $Url -Method Post -ContentType "application/json" -Body $PostJsonData
    if ($PostResponse.StatusCode -ne 201) {
        Write-Host "FAIL " -ForegroundColor Red -NoNewLine
        Write-Host "Expected Status: 201, got $($PostResponse.StatusCode)"
        exit 1
    }

    $PostResponse = ConvertFrom-Json -InputObject $PostResponse
}
catch {
    Write-Host "FAIL " -ForegroundColor Red -NoNewLine
    Write-Host "Expected Status: 201, got $([int]$_.Exception.Response.StatusCode)"
    Write-Host "Error: $($_.Exception.Message)"
    exit 1
}

# ensure response is what we put in

$ops = @(
    @("Name", $Data.name, $PostResponse.name),
    @("Age", $Data.age, $PostResponse.age),
    @("Favourite Colour", $Data.favouriteColour, $PostResponse.favouriteColour)
)

foreach ($line in $ops) {
    Compare-Response -Passed ([ref]$PostPassed) -Line $line
}

if ($PostPassed) {
    $id = $PostResponse.id
    Write-Host "Storing id=$id for future requests..."
    Write-Host "PASS " -ForegroundColor Green
}
else {
    Write-Host "FAIL " -ForegroundColor Red
}

Write-Host ""

# Test GET /api/:id for 200
Write-Host "Testing " -NoNewLine
Write-Host "GET $Url/$id" -ForegroundColor Blue -NoNewLine
Write-Host ":"

$GetPassed = $true

try {
    $GetResponse = Invoke-WebRequest -Uri $($Url + "/" + $id) -Method Get -ContentType "application/json"
    if ($GetResponse.StatusCode -ne 200) {
        Write-Host "FAIL " -ForegroundColor Red -NoNewLine
        Write-Host "Expected Status: 200, got $($GetResponse.StatusCode)"
        exit 1
    }

    $GetResponse = ConvertFrom-Json -InputObject $GetResponse
}
catch {
    Write-Host "FAIL " -ForegroundColor Red -NoNewLine
    Write-Host "Expected Status: 200, got $([int]$_.Exception.Response.StatusCode)"
    Write-Host "Error: $($_.Exception.Message)"
    exit 1
}

# validate response
$ops = @(
    @("Id", $id, $GetResponse.Id),
    @("Name", $Data.name, $GetResponse.name),
    @("Age", $Data.age, $GetResponse.age),
    @("Favourite Colour", $Data.favouriteColour, $GetResponse.favouriteColour)
)

foreach ($line in $ops) {
    Compare-Response -Passed ([ref]$GetPassed) -Line $line
}

if ($GetPassed) {
    Write-Host "PASS " -ForegroundColor Green
}
else {
    Write-Host "FAIL " -ForegroundColor Red
}

Write-Host ""

# Test PATCH /api/:id for 200

# values to change to
$Changed = @{
    name            = -join ((65..90) + (97..122) | Get-Random -Count 5 | ForEach-Object { [char]$_ })
    age             = (Get-Random -Minimum 1 -Maximum 100)
    favouriteColour = (Get-Random -InputObject $ColourList)
}

# PATCH just name, age and favouriteColour individually
@(
    @("name", $Changed.name),
    @("age", $Changed.age),
    @("favouriteColour", $Changed.favouriteColour)
) | Foreach-Object {
    Write-Host "Testing " -NoNewLine
    Write-Host "PATCH $Url/$id " -ForegroundColor Blue -NoNewLine
    Write-Host "with $($_[0])=`"$($_[1])`""

    $PatchData = @{
        id    = $id
        $_[0] = $_[1]
    }

    $PatchJsonData = ConvertTo-Json -InputObject $PatchData
    $PatchPassed = $true

    try {
        $PatchResponse = Invoke-WebRequest -Uri $($Url + "/" + $id) -Method Patch -ContentType "application/json" -Body $PatchJsonData
        if ($PatchResponse.StatusCode -ne 200) {
            Write-Host "FAIL " -ForegroundColor Red -NoNewLine
            Write-Host "Expected Status: 200, got $($GetResponse.StatusCode)"
            exit 1
        }
        
        $PatchResponse = ConvertFrom-Json -InputObject $PatchResponse
    }
    catch {
        Write-Host "FAIL " -ForegroundColor Red -NoNewLine
        Write-Host "Expected Status: 200, got $([int]$_.Exception.Response.StatusCode)"
        Write-Host "Error: $($_.Exception.Message)"
        exit 1
    } 

    $ops = @(
        @("Id", $id, $PatchResponse.Id),
        @($_[0], $_[1], $PatchResponse.$($_[0]))
    )

    foreach ($line in $ops) {
        Compare-Response -Passed ([ref]$PatchPassed) -Line $line
    }

    if ($PatchPassed) {
        Write-Host "PASS " -ForegroundColor Green
    }
    else {
        Write-Host "FAIL " -ForegroundColor Red
    }

    Write-Host ""
}

# Test GET /api/:id for 200 with new values

Write-Host "Testing " -NoNewLine
Write-Host "GET $Url/$id " -ForegroundColor Blue -NoNewLine
Write-Host "with new values:"

$SecondGetPassed = $true

try {
    $SecondGetResponse = Invoke-WebRequest -Uri $($Url + "/" + $id) -Method Get -ContentType "application/json"
    if ($SecondGetResponse.StatusCode -ne 200) {
        Write-Host "FAIL " -ForegroundColor Red -NoNewLine
        Write-Host "Expected Status: 200, got $($SecondGetResponse.StatusCode)"
        exit 1
    }

    $SecondGetResponse = ConvertFrom-Json -InputObject $SecondGetResponse
}
catch {
    Write-Host "FAIL " -ForegroundColor Red -NoNewLine
    Write-Host "Expected Status: 200, got $([int]$_.Exception.Response.StatusCode)"
    Write-Host "Error: $($_.Exception.Message)"
    exit 1
}

# validate response
$ops = @(
    @("Id", $id, $SecondGetResponse.Id),
    @("Name", $Changed.name, $SecondGetResponse.name),
    @("Age", $Changed.age, $SecondGetResponse.age),
    @("Favourite Colour", $Changed.favouriteColour, $SecondGetResponse.favouriteColour)
)

foreach ($line in $ops) {
    Compare-Response -Passed ([ref]$SecondGetPassed) -Line $line
}

if ($SecondGetPassed) {
    Write-Host "PASS " -ForegroundColor Green
}
else {
    Write-Host "FAIL " -ForegroundColor Red
}

Write-Host ""

# Test DELETE /api/:id for 204

Write-Host "Testing " -NoNewLine
Write-Host "DELETE $Url/$id" -ForegroundColor Blue -NoNewline
Write-Host ":"

try {
    $DeleteResponse = Invoke-WebRequest -Uri $($Url + "/" + $id) -Method Delete -ContentType "application/json"
    if ($DeleteResponse.StatusCode -ne 204) {
        Write-Host "FAIL " -ForegroundColor Red -NoNewLine
        Write-Host "Expected Status: 204, got $($DeleteResponse.StatusCode)"
        exit 1
    }

    Write-Host "Expected Status: 204, got $($DeleteResponse.StatusCode)"
}
catch {
    Write-Host "FAIL " -ForegroundColor Red -NoNewLine
    Write-Host "Expected Status: 204, got $([int]$_.Exception.Response.StatusCode)"
    Write-Host "Error: $($_.Exception.Message)"
    exit 1
}

Write-Host "PASS " -ForegroundColor Green
Write-Host ""

# Test GET /api/:id for 404

Write-Host "Testing " -NoNewLine
Write-Host "GET $Url/$id" -ForegroundColor Blue -NoNewline
Write-Host ":"

try {
    $ThirdGetResponse = Invoke-WebRequest -Uri $($Url + "/" + $id) -Method Get -ContentType "application/json"
    if ($ThirdGetResponse.StatusCode -ne 200) {
        Write-Host "FAIL " -ForegroundColor Red -NoNewLine
        Write-Host "Expected Status: 404, got $($ThirdGetResponse.StatusCode)"
        exit 1
    }

    Write-Host "FAIL " -ForegroundColor Red -NoNewLine
    Write-Host "Expected Status: 404, got $($ThirdGetResponse.StatusCode)"
    Write-Host "Error: $($ThirdGetResponse.Content)"
    exit 1

    $ThirdGetResponse = ConvertFrom-Json -InputObject $ThirdGetResponse
}
catch {
    if ($_.Exception.Response.StatusCode -ne 404) {
        Write-Host "FAIL " -ForegroundColor Red -NoNewLine
        Write-Host "Expected Status: 404, got $([int]$_.Exception.Response.StatusCode)"
        Write-Host "Error: $($_.Exception.Message)"
    }

    Write-Host "Expected Status: 404, got $([int]$_.Exception.Response.StatusCode)"
    Write-Host "PASS " -ForegroundColor Green
}