FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# copy csproj and restore as distinct layers
COPY *.sln .
COPY src/NLayeredApi.1Template.WebApi/*.csproj ./src/NLayeredApi.1Template.WebApi/
COPY src/NLayeredApi.1Template.Tests/*.csproj ./src/NLayeredApi.1Template.Tests/

ARG PAT
ARG FEED_URL
ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED true
RUN dotnet nuget add source "${FEED_URL}" -u AldabaFeed -p "${PAT}" --store-password-in-clear-text --valid-authentication-types basic
RUN dotnet restore

# copy everything else and build app
COPY src/. ./src/
WORKDIR /src/NLayeredApi.1Template.WebApi/
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "NLayeredApi.1Template.WebApi.dll"]