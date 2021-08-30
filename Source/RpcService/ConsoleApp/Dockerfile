#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["RpcService/ConsoleApp/ConsoleApp.csproj", "RpcService/ConsoleApp/"]
COPY ["RpcService/RpcService/RpcService.csproj", "RpcService/RpcService/"]
COPY ["SocketTcpClient/SocketTcpClient.csproj", "SocketTcpClient/"]
COPY ["SocketBuffer/SocketBuffer.csproj", "SocketBuffer/"]
COPY ["RpcCacheClient/RpcCacheClient.csproj", "RpcCacheClient/"]
COPY ["SqlExecHelper/SqlExecHelper.csproj", "SqlExecHelper/"]
COPY ["SocketTcpServer/SocketTcpServer.csproj", "SocketTcpServer/"]
COPY ["RpcModel/RpcModel.csproj", "RpcModel/"]
RUN dotnet restore "RpcService/ConsoleApp/ConsoleApp.csproj"
COPY . .
WORKDIR "/src/RpcService/ConsoleApp"
RUN dotnet build "ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 983
ENTRYPOINT ["dotnet", "ConsoleApp.dll"]
