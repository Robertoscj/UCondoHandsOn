FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/uCondoHandsOn.API/uCondoHandsOn.API.csproj", "src/uCondoHandsOn.API/"]
COPY ["src/uCondoHandsOn.Application/uCondoHandsOn.Application.csproj", "src/uCondoHandsOn.Application/"]
COPY ["src/uCondoHandsOn.Domain/uCondo.Planos.Domain.csproj", "src/uCondoHandsOn.Domain/"]
COPY ["src/uCondoHandsOn.Infrastructure/uCondoHandsOn.Infrastructure.csproj", "src/uCondoHandsOn.Infrastructure/"]
RUN dotnet restore "src/uCondoHandsOnh.API/uCondo.Planos.API.csproj"
COPY . .
WORKDIR "/src/src/uCondoHandsOn.API"
RUN dotnet build "uCondoHandsOn.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "uCondoHandsOn.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "uCondoHandsOn.API.dll"]