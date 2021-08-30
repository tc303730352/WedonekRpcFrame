#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AutoTaskService/ConsoleApp/ConsoleApp.csproj", "AutoTaskService/ConsoleApp/"]
COPY ["AutoTaskService/AutoTaskService/AutoTaskService.csproj", "AutoTaskService/AutoTaskService/"]
COPY ["SqlExecHelper/SqlExecHelper.csproj", "SqlExecHelper/"]
COPY ["RpcHelper/RpcHelper.csproj", "RpcHelper/"]
COPY ["RpcClient/RpcClient.csproj", "RpcClient/"]
COPY ["RpcModel/RpcModel.csproj", "RpcModel/"]
COPY ["SocketTcpServer/SocketTcpServer.csproj", "SocketTcpServer/"]
COPY ["SocketBuffer/SocketBuffer.csproj", "SocketBuffer/"]
COPY ["RpcCacheClient/RpcCacheClient.csproj", "RpcCacheClient/"]
COPY ["SocketTcpClient/SocketTcpClient.csproj", "SocketTcpClient/"]
RUN dotnet restore "AutoTaskService/ConsoleApp/ConsoleApp.csproj"
COPY . .
WORKDIR "/src/AutoTaskService/ConsoleApp"
RUN dotnet build "ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 839
ENTRYPOINT ["dotnet", "ConsoleApp.dll"]