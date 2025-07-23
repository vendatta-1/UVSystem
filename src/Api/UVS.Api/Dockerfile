FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Api/UVS.Api/UVS.Api.csproj", "src/Api/UVS.Api/"]
COPY ["src/Modules/Authentication/UVS.Modules.Authentication.IntgerationEvents/UVS.Modules.Authentication.IntgerationEvents.csproj", "src/Modules/Authentication/UVS.Modules.Authentication.IntgerationEvents/"]
COPY ["src/Common/UVS.Common.Application/UVS.Common.Application.csproj", "src/Common/UVS.Common.Application/"]
COPY ["src/Common/UVS.Common.Domain/UVS.Common.Domain.csproj", "src/Common/UVS.Common.Domain/"]
COPY ["src/Modules/System/UVS.Modules.System.Infrastructure/UVS.Modules.System.Infrastructure.csproj", "src/Modules/System/UVS.Modules.System.Infrastructure/"]
COPY ["src/Common/UVS.Common.Infrastructure/UVS.Common.Infrastructure.csproj", "src/Common/UVS.Common.Infrastructure/"]
COPY ["src/Modules/System/UVS.Modules.System.Application/UVS.Modules.System.Application.csproj", "src/Modules/System/UVS.Modules.System.Application/"]
COPY ["src/Modules/System/UVS.Modules.System.Integration/UVS.Modules.System.Integration.csproj", "src/Modules/System/UVS.Modules.System.Integration/"]
COPY ["src/Modules/System/UVS.Modules.System.Domain/UVS.Modules.System.Domain.csproj", "src/Modules/System/UVS.Modules.System.Domain/"]
COPY ["src/Modules/System/UVS.Modules.System.Presentation/UVS.Modules.System.Presentation.csproj", "src/Modules/System/UVS.Modules.System.Presentation/"]
COPY ["src/Common/UVS.Common.Presentation/UVS.Common.Presentation.csproj", "src/Common/UVS.Common.Presentation/"]
COPY ["src/Modules/Authentication/UVS.Modules.Authentication.Presentation/UVS.Modules.Authentication.Presentation.csproj", "src/Modules/Authentication/UVS.Modules.Authentication.Presentation/"]
COPY ["src/Modules/Authentication/UVS.Moduels.Authentication.Application/UVS.Moduels.Authentication.Application.csproj", "src/Modules/Authentication/UVS.Moduels.Authentication.Application/"]
COPY ["src/Modules/Authentication/UVS.Modules.Authentication.Domain/UVS.Modules.Authentication.Domain.csproj", "src/Modules/Authentication/UVS.Modules.Authentication.Domain/"]
COPY ["src/Modules/Authentication/UVS.Modules.Authentication.Infrastrcture/UVS.Modules.Authentication.Infrastrcture.csproj", "src/Modules/Authentication/UVS.Modules.Authentication.Infrastrcture/"]
RUN dotnet restore "src/Api/UVS.Api/UVS.Api.csproj"
COPY . .
WORKDIR "/src/src/Api/UVS.Api"
RUN dotnet build "./UVS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UVS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UVS.Api.dll"]
