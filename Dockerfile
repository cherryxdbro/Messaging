FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS publish

WORKDIR /source/Messaging

COPY "Messaging.sln" .
COPY "Messaging/Messaging.csproj" "Messaging/"

RUN dotnet restore

COPY . .

WORKDIR /source/Messaging/Messaging

RUN dotnet publish "Messaging.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS runtime

WORKDIR /source/Messaging/application

COPY --from=publish /publish .
ENTRYPOINT "dotnet" "Messaging.dll"