FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG NUGET_USER
ARG NUGET_PASSWORD

WORKDIR /src
COPY ["/src/.", "."]

WORKDIR "/src/Meteor.Controller.Api"
RUN dotnet nuget add source "https://nuget.pkg.github.com/GermonenkoBy/index.json" -u $NUGET_USER -p $NUGET_PASSWORD --store-password-in-clear-text
RUN dotnet build "Meteor.Controller.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meteor.Controller.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meteor.Controller.Api.dll"]
