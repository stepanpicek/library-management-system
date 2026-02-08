FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/LibraryManagementSystem.Web/LibraryManagementSystem.Web.csproj", "src/LibraryManagementSystem.Web/"]
COPY ["src/LibraryManagementSystem.Application/LibraryManagementSystem.Application.csproj", "src/LibraryManagementSystem.Application/"]
COPY ["src/LibraryManagementSystem.Domain/LibraryManagementSystem.Domain.csproj", "src/LibraryManagementSystem.Domain/"]
COPY ["src/LibraryManagementSystem.Infrastructure/LibraryManagementSystem.Infrastructure.csproj", "src/LibraryManagementSystem.Infrastructure/"]
RUN dotnet restore "src/LibraryManagementSystem.Web/LibraryManagementSystem.Web.csproj"
COPY . .
WORKDIR "/src/src/LibraryManagementSystem.Web"
RUN dotnet build "LibraryManagementSystem.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LibraryManagementSystem.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
USER root
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Docker
COPY --from=publish /app/publish .
COPY --from=build /src/storage ./storage
RUN chown -R $APP_UID:$APP_UID /app/storage
USER $APP_UID
ENTRYPOINT ["dotnet", "LibraryManagementSystem.Web.dll"]