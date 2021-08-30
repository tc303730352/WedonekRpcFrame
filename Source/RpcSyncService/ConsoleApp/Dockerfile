#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["RpcSyncService/ConsoleApp/ConsoleApp.csproj", "RpcSyncService/ConsoleApp/"]
COPY ["RpcSyncService/RpcSyncService/RpcSyncService.csproj", "RpcSyncService/RpcSyncService/"]
COPY ["RpcModularModel/RpcModularModel.csproj", "RpcModularModel/"]
COPY ["RpcModel/RpcModel.csproj", "RpcModel/"]
COPY ["RpcHelper/RpcHelper.csproj", "RpcHelper/"]
COPY ["RpcClient/RpcClient.csproj", "RpcClient/"]
COPY ["SocketTcpServer/SocketTcpServer.csproj", "SocketTcpServer/"]
COPY ["SocketBuffer/SocketBuffer.csproj", "SocketBuffer/"]
COPY ["RpcCacheClient/RpcCacheClient.csproj", "RpcCacheClient/"]
COPY ["SocketTcpClient/SocketTcpClient.csproj", "SocketTcpClient/"]
COPY ["SqlExecHelper/SqlExecHelper.csproj", "SqlExecHelper/"]
COPY ["RpcModular/RpcModular.csproj", "RpcModular/"]
RUN dotnet restore "RpcSyncService/ConsoleApp/ConsoleApp.csproj"
COPY . .
WORKDIR "/src/RpcSyncService/ConsoleApp"
RUN dotnet build "ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 835
ENTRYPOINT ["dotnet", "ConsoleApp.dll"]