FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MechanicalWorkshop.API/MechanicalWorkshop.API.csproj", "MechanicalWorkshop.API/"]
COPY ["MechanicalWorkshop.Application/MechanicalWorkshop.Application.csproj", "MechanicalWorkshop.Application/"]
COPY ["MechanicalWorkshop.Domain/MechanicalWorkshop.Domain.csproj", "MechanicalWorkshop.Domain/"]
COPY ["MechanicalWorkshop.Infrastructure/MechanicalWorkshop.Infrastructure.csproj", "MechanicalWorkshop.Infrastructure/"]

RUN dotnet restore "MechanicalWorkshop.API/MechanicalWorkshop.API.csproj"

COPY . .

WORKDIR "/src/MechanicalWorkshop.API"
RUN dotnet build "MechanicalWorkshop.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MechanicalWorkshop.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MechanicalWorkshop.API.dll"]