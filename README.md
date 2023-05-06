# database

See https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
See https://learn.microsoft.com/en-us/ef/core/cli/dotnet

## drop existing database

> dotnet ef database drop

## migrate database

> rm -r Migrations
> dotnet ef migrations add InitialCreate
> dotnet ef database update

