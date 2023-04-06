FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Meteor.Controller.Api/Meteor.Controller.Api.csproj", "src/Meteor.Controller.Api/"]
RUN dotnet restore "src/Meteor.Controller.Api/Meteor.Controller.Api.csproj"
COPY . .
WORKDIR "/src/src/Meteor.Controller.Api"
RUN dotnet build "Meteor.Controller.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meteor.Controller.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meteor.Controller.Api.dll"]
