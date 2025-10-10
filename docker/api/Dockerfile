# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY pj-server/ ./pj-server/
RUN dotnet restore pj-server/Api/Api.csproj \
 && dotnet publish pj-server/Api/Api.csproj -c Release -o /app

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet","pj-server.Api.dll"]