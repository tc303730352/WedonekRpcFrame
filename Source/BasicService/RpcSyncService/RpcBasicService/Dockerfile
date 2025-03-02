#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 835

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BasicService/RpcSyncService/RpcBasicService/RpcBasicService.csproj", "BasicService/RpcSyncService/RpcBasicService/"]
COPY ["BasicService/RpcSyncService/RpcSync.Service/RpcSync.Service.csproj", "BasicService/RpcSyncService/RpcSync.Service/"]
COPY ["Basic/WeDonekRpc.Client/WeDonekRpc.Client.csproj", "Basic/WeDonekRpc.Client/"]
COPY ["BasicService/ExtendService/WeDonekRpc.ExtendModel/WeDonekRpc.ExtendModel.csproj", "BasicService/ExtendService/WeDonekRpc.ExtendModel/"]
COPY ["Basic/WeDonekRpc.Helper/WeDonekRpc.Helper.csproj", "Basic/WeDonekRpc.Helper/"]
COPY ["Basic/WeDonekRpc.Model/WeDonekRpc.Model.csproj", "Basic/WeDonekRpc.Model/"]
COPY ["Socket/WeDonekRpc.IOSendInterface/WeDonekRpc.IOSendInterface.csproj", "Socket/WeDonekRpc.IOSendInterface/"]
COPY ["Socket/WeDonekRpc.TcpClient/WeDonekRpc.TcpClient.csproj", "Socket/WeDonekRpc.TcpClient/"]
COPY ["Socket/WeDonekRpc.IOBuffer/WeDonekRpc.IOBuffer.csproj", "Socket/WeDonekRpc.IOBuffer/"]
COPY ["Socket/WeDonekRpc.TcpServer/WeDonekRpc.TcpServer.csproj", "Socket/WeDonekRpc.TcpServer/"]
COPY ["Gateway/WeDonekRpc.ModularModel/WeDonekRpc.ModularModel.csproj", "Gateway/WeDonekRpc.ModularModel/"]
COPY ["Gateway/WeDonekRpc.Modular/WeDonekRpc.Modular.csproj", "Gateway/WeDonekRpc.Modular/"]
COPY ["Basic/WeDonekRpc.CacheModular/WeDonekRpc.CacheModular.csproj", "Basic/WeDonekRpc.CacheModular/"]
COPY ["Basic/WeDonekRpc.CacheClient/WeDonekRpc.CacheClient.csproj", "Basic/WeDonekRpc.CacheClient/"]
COPY ["BasicService/RpcSyncService/RpcSync.Collect/RpcSync.Collect.csproj", "BasicService/RpcSyncService/RpcSync.Collect/"]
COPY ["BasicService/RpcSyncService/RpcSync.DAL/RpcSync.DAL.csproj", "BasicService/RpcSyncService/RpcSync.DAL/"]
COPY ["Tools/WeDonekRpc.SqlSugar/WeDonekRpc.SqlSugar.csproj", "Tools/WeDonekRpc.SqlSugar/"]
COPY ["BasicService/RpcSyncService/RpcSync.Model/RpcSync.Model.csproj", "BasicService/RpcSyncService/RpcSync.Model/"]
RUN dotnet restore "./BasicService/RpcSyncService/RpcBasicService/./RpcBasicService.csproj"
COPY . .
WORKDIR "/src/BasicService/RpcSyncService/RpcBasicService"
RUN dotnet build "./RpcBasicService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RpcBasicService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RpcBasicService.dll"]