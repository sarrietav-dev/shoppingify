FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["shoppingify/shoppingify.csproj", "shoppingify/"]
RUN dotnet restore "shoppingify/shoppingify.csproj"
COPY . .
WORKDIR "/src/shoppingify"
RUN dotnet build "shoppingify.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "shoppingify.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "shoppingify.dll"]
