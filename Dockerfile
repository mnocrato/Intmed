FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebApi/Intmed.csproj", "WebApi/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Test/Test.csproj", "Test/"]

RUN dotnet restore "WebApi/Intmed.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "Intmed.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Intmed.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Intmed.dll"]
