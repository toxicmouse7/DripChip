FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DripChip.csproj", "./"]
RUN dotnet restore "DripChip.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "DripChip.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DripChip.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DripChip.dll"]
