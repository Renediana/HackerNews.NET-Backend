FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster
WORKDIR /src
COPY ["HackerNewsApi/HackerNewsApi.csproj", "HackerNewsApi/"]
RUN dotnet restore "HackerNewsApi/HackerNewsApi.csproj"
COPY . .
WORKDIR "/src/HackerNewsApi"
RUN dotnet build "HackerNewsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HackerNewsApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackerNewsApi.dll"]