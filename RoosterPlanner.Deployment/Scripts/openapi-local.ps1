Set-Location -Path ./RoosterPlanner.Api
dotnet restore
dotnet build
Set-Location -Path ../

$env:NODE_TLS_REJECT_UNAUTHORIZED=0
ng new openapi --create-application=false --package-manager=npm --skip-git --skip-tests
Copy-Item ./RoosterPlanner.api/bin/debug/net5.0/win-x64/open-api-spec.json ./openapi
Set-Location -Path ./openapi
ng g library @RoosterPlanner/openapi
Write-Output "export * from './lib'" > ./projects/RoosterPlanner/openapi/src/public-api.ts
Remove-Item ./projects/RoosterPlanner/openapi/src/lib -Force -Recurse
openapi-generator-cli generate -g typescript-angular -i ./open-api-spec.json -o ./projects/RoosterPlanner/openapi/src/lib
ng build --project=@RoosterPlanner/openapi --configuration=production
Copy-Item -Path ./dist/RoosterPlanner/* -Destination ../RoosterPlanner.UI/node_modules/@RoosterPlanner -Recurse -Force
Set-Location -Path ../
Remove-Item -Path ./openapi -Force -Recurse
$env:NODE_TLS_REJECT_UNAUTHORIZED=1