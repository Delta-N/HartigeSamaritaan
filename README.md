# Tearfund Roosterplanner

Roosterplannersysteem voor de stichting Tearfund NL (Originally Charity Hartige Samaritaan).
Bezoek: https://delta-n.github.io/HartigeSamaritaan/ voor de documentatie.

## Gebruikte technologieën:

- Angular 11.1
- .NET 5
- Entity Framework Core 5.0.2
- Azure Storage
- Azure Keyvault
- Azure B2C
- Azure SQL
- Azure App Service (Plan)
- Azure Application Insights

# Local development

Helpful database commands:

```bash
dotnet ef migration add "<MIGRATION_NAME>" --project RoosterPlanner.Data --startup-project RoosterPlanner.Api
```

```bash
dotnet ef database update --project RoosterPlanner.Data --startup-project RoosterPlanner.Api
```

# CI / CD

![Build the project](https://github.com/Delta-N/HartigeSamaritaan/workflows/Build%20the%20project/badge.svg)  
![Deploy Master](https://github.com/Delta-N/HartigeSamaritaan/workflows/Deploy%20Master/badge.svg)

# Deployment

The app is deployed in an Azure Subscription managed by Delta-N. The repository includes IaC in order to easily set up new environments in Azure, see `/.infra/` for Bicep files.

## Manual steps

### Azure B2C

- Create a new B2C tenant and configure a SUSI policy
- Create two app registrations in said tenant, one for the API and one for the frontend
- The API app registration should have a client secret, which is stored in Azure Keyvault `AzureAuthentication--ClientSecret`
- The front-end registration should have rights to access the API registration and should have a redirect URI to the front-end.

### SQL Server

SQL server & database are created from Bicep files. The API requires access to the database, which requires a manual query. Currently the API applies migrations on startup and thus requires the owner role. Ideally both the migrations and the role assignment would be executed from the pipeline, but this is currently not implemented.

Therefore execute the following query in the database:

```sql
IF NOT EXISTS(select * from sys.database_principals where name = 'roosterplanner-env-api')
BEGIN
    CREATE USER [roosterplanner-env-api] FROM EXTERNAL PROVIDER WITH DEFAULT_SCHEMA=[dbo];
    ALTER ROLE db_datareader ADD MEMBER [roosterplanner-env-api];
    ALTER ROLE db_datawriter ADD MEMBER [roosterplanner-env-api];
END
```
