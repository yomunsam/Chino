
Chino 

``` powershell
dotnet ef migrations add Init --context ChinoApplicationDbContext --project ../database/Chino.EntityFramework.Mysql --output-dir Migrations/Chino/Application -- --OverrideDbProvider mysql
```



Identity Server

``` powershell
Add-Migration InitConfigurations -Context ConfigurationDbContext -OutputDir Data\Migrations\IdentityServer\Configuration

Add-Migration InitPersisted -Context PersistedGrantDbContext -OutputDir Data\Migrations\IdentityServer\PersistedGrant
```