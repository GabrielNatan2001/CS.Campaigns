FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CS.Campaigns.API/CS.Campaigns.API.csproj", "src/CS.Campaigns.API/"]
COPY ["src/CS.Campaigns.Application/CS.Campaigns.Application.csproj", "src/CS.Campaigns.Application/"]
COPY ["src/CS.Campaigns.Domain/CS.Campaigns.Domain.csproj", "src/CS.Campaigns.Domain/"]
COPY ["src/CS.Campaigns.Infrastructure/CS.Campaigns.Infrastructure.csproj", "src/CS.Campaigns.Infrastructure/"]
RUN dotnet restore "src/CS.Campaigns.API/CS.Campaigns.API.csproj"
COPY . .
RUN dotnet publish "src/CS.Campaigns.API/CS.Campaigns.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CS.Campaigns.API.dll"]
