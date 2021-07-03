#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LiLog.Server/LiLog.Server.csproj", "LiLog.Server/"]
RUN dotnet restore "LiLog.Server/LiLog.Server.csproj"
COPY . .
WORKDIR "/src/LiLog.Server"
RUN dotnet build "LiLog.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LiLog.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LiLog.Server.dll"]