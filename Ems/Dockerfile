FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Ems/Ems.csproj", "Ems/"]
RUN dotnet restore "Ems/Ems.csproj"
COPY . .
WORKDIR "/src/Ems"
RUN dotnet build "Ems.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ems.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ems.dll"]
