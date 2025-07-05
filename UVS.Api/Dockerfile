FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UVS.Api/UVS.Api.csproj", "UVS.Api/"]
COPY ["UVS.Modules.Authentication.Infrastrcture/UVS.Modules.Authentication.Infrastrcture.csproj", "UVS.Modules.Authentication.Infrastrcture/"]
COPY ["UVS.Moduels.System.Infrastructure/UVS.Moduels.System.Infrastructure.csproj", "UVS.Moduels.System.Infrastructure/"]
COPY ["UVS.Modules.System.Application/UVS.Modules.System.Application.csproj", "UVS.Modules.System.Application/"]
COPY ["UVS.Common.Application/UVS.Common.Application.csproj", "UVS.Common.Application/"]
COPY ["UVS.Common.Domain/UVS.Common.Domain.csproj", "UVS.Common.Domain/"]
COPY ["UVS.Module.System.Domain/UVS.Module.System.Domain.csproj", "UVS.Module.System.Domain/"]
COPY ["UVS.Common.Infrastructure/UVS.Common.Infrastructure.csproj", "UVS.Common.Infrastructure/"]
RUN dotnet restore "UVS.Api/UVS.Api.csproj"
COPY . .
WORKDIR "/src/UVS.Api"
RUN dotnet build "./UVS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UVS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UVS.Api.dll"]
