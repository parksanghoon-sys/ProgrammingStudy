Get-ChildItem .\src -Recurse -Filter *.csproj | ForEach-Object {
    dotnet sln add $_.FullName
}
