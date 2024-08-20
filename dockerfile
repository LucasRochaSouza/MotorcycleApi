FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish

ARG TARGETARCH

COPY . .

RUN dotnet publish "Motorcycle.Api\Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

ENV DOTNET_ENVIRONMENT=Docker
ENV DOTNET_RUNNING_IN_CONTAINER=true
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
