FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble AS base
RUN apt-get update -y && apt-get upgrade -y
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build
WORKDIR /app/src
COPY "Src/Application/DDD.Application.csproj" "Application/"
COPY "Src/Domain/DDD.Domain.csproj" "Domain/"
COPY "Src/Infrastructure/DDD.Infrastructure.csproj" "Infrastructure/"
COPY "Src/Presentation/DDD.Presentation.csproj" "Presentation/"
COPY "Src/Api/DDD.Api.csproj" "Api/"
RUN dotnet restore "Api/DDD.Api.csproj"
COPY "Src/Application/." "Application/"
COPY "Src/Domain/." "Domain/"
COPY "Src/Infrastructure/." "Infrastructure/"
COPY "Src/Presentation/." "Presentation/"
COPY "Src/Api/." "Api/"
WORKDIR /app/src/Api
RUN dotnet build "DDD.Api.csproj" -c $ASPNETCORE_ENVIRONMENT -o /app/build

FROM build AS publish
RUN dotnet publish "DDD.Api.csproj" -c $ASPNETCORE_ENVIRONMENT -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "DDD.Api.dll" ]