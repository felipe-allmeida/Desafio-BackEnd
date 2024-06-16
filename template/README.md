## Add migration

dotnet ef migrations add Initial_Create --context BikeRentalContext --project ./src/Services/BikeRental/BikeRental.Data/BikeRental.Data.csproj --startup-project ./src/Services/BikeRental/BikeRental.API/BikeRental.API.csproj -o Migrations

## Remove Migration
dotnet ef migrations remove --context BikeRentalContext --project ./src/Services/BikeRental/BikeRental.Data/BikeRental.Data.csproj --startup-project ./src/Services/BikeRental/BikeRental.API/BikeRental.API.csproj

## Publish added migrations

dotnet ef database update --context BikeRentalContext --project ./src/BikeRental.API/BikeRental.API.csproj
