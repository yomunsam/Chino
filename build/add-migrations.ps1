param([string] $migration = 'DbInit', [string] $migrationProviderName = 'All')
$projectName = "Chino.IdentityServer";
$currentPath = Get-Location
Set-Location "../src/$projectName"
Copy-Item appsettings.json -Destination appsettings-backup.json
# $settings = Get-Content appsettings.json -raw

#Initialze db context and define the target directory
$targetContexts = @{ 
    ChinoApplicationDbContext               = "Migrations\Chino\Application"
    ConfigurationDbContext                  = "Migrations\IdentityServer\Configuration";
    PersistedGrantDbContext                 = "Migrations\IdentityServer\PersistedGrant";
}

#Initialize the db providers and it's respective projects
$dpProviders = @{
    SqlServer  = "..\..\src\database\Chino.EntityFramework.SqlServer\Chino.EntityFramework.SqlServer.csproj";
    # PostgreSQL = "..\..\src\$projectName.Admin.EntityFramework.PostgreSQL\$projectName.Admin.EntityFramework.PostgreSQL.csproj";
    MySql      = "..\..\src\database\Chino.EntityFramework.Mysql\Chino.EntityFramework.Mysql.csproj";
    Sqlite     = "..\..\src\database\Chino.EntityFramework.Sqlite\Chino.EntityFramework.Sqlite.csproj"
}

#Fix issue when the tools is not installed and the nuget package does not work see https://github.com/MicrosoftDocs/azure-docs/issues/40048
# Write-Host "Updating donet ef tools"
$env:Path += "	% USERPROFILE % \.dotnet\tools";
# dotnet tool update --global dotnet-ef

Write-Host "[Chino]Start migrate projects`n";
foreach ($provider in $dpProviders.Keys) {

    if ($migrationProviderName -eq 'All' -or $migrationProviderName -eq $provider) {
    
        $projectPath = (Get-Item -Path $dpProviders[$provider] -Verbose).FullName;
        Write-Host "[Chino]Generate migration for db provider:" $provider ", for project path - " $projectPath "`n";

        $providerName = '"ProviderType": "' + $provider + '"'

        # $settings = $settings -replace '"ProviderType".*', $providerName
        # $settings | set-content appsettings.json
        if ((Test-Path $projectPath) -eq $true) {
            foreach ($context in $targetContexts.Keys) {
                $migrationPath = $targetContexts[$context];

                Write-Host "Migrating context " $context
                # Write-Host "执行 dotnet ef migrations add $migration -c $context -o $migrationPath -p $projectPath -- --OverrideDbProvider $provider ";
                dotnet ef migrations add $migration -c $context -o $migrationPath -p $projectPath -- --OverrideDbProvider $provider 
                Write-Host "`n"
            } 
        }
        
    }
}

# Remove-Item appsettings.json
# Copy-Item appsettings-backup.json -Destination appsettings.json
# Remove-Item appsettings-backup.json
Set-Location $currentPath