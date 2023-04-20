# Meteor Controller Service

This service responsible for managing customers.

## Image Build

TODO: Add build steps

## Migrations

To add migration run the following command:
```shell
dotnet ef migrations add {MIGRATION_NAME_PLACEHOLDER} --project src/Meteor.Controller.Migrations --startup-project src/Meteor.Controller.Api -o ./
```

## Docker Build/Push

To build the image run the following command
```shell
docker build -f src/Meteor.Controller.Api/Dockerfile -t sgermonenko/meteor-controller:{version} --build-arg NUGET_USER={USERNAME} --build-arg NUGET_PASSWORD={PASSWORD} .
```
where:
- {version} is microservice release version
- {USERNAME} is private nuget feed username
- {PASSWORD} is private nuget feed password
