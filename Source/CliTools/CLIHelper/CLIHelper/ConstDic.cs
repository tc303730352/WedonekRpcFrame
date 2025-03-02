using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace GatewayBuildCli
{
    internal class ConstDic
    {
        public static readonly Type _AppIdAttr = typeof(AppIdentityAttr);
        public static readonly Type _BasicPage = typeof(BasicPage);
        public static readonly Type _UpFileType = typeof(RpcUpFile);
        public static readonly Type _VoidType = typeof(void);
        public static readonly Type _RpcRemoteType = typeof(RpcRemote);
        public static readonly Type _RpcBroadcastType = typeof(RpcBroadcast);
        public static readonly Type _BasicPageType = typeof(BasicPage<>);
        public static readonly Type _objectType = typeof(object);
        public static readonly Type _IConfigType = typeof(IRemoteConfig);

        public static readonly Type _IBroadcastType = typeof(IRemoteBroadcast);
        public static string CsprojTemplate = "<Project Sdk=\"Microsoft.NET.Sdk\">" +
            "<PropertyGroup><TargetFramework>net8.0</TargetFramework><EnablePreviewFeatures>True</EnablePreviewFeatures><ImplicitUsings>enable</ImplicitUsings><Nullable>enable</Nullable></PropertyGroup>" +
                "<PropertyGroup Condition=\"'$(Configuration)|$(Platform)'=='Debug|AnyCPU'\"><DocumentationFile>{1}.xml</DocumentationFile></PropertyGroup>" +
                "<PropertyGroup Condition=\"'$(Configuration)|$(Platform)'=='Release|AnyCPU'\"><DocumentationFile>{1}.xml</DocumentationFile></PropertyGroup>" +
                "<ItemGroup>" +
                "<PackageReference Include=\"WeDonekRpc.Helper\" Version=\"1.0.7\" />" +
               "<PackageReference Include=\"WeDonekRpc.CacheClient\" Version=\"1.0.7\" />" +
               "<PackageReference Include=\"WeDonekRpc.Client\" Version=\"1.0.7\" />" +
               "<PackageReference Include=\"WeDonekRpc.Helper\" Version=\"1.0.7\" />" +
               "<PackageReference Include=\"WeDonekRpc.Model\" Version=\"1.0.7\" />" +
              "<PackageReference Include=\"WeDonekRpc.Modular\" Version=\"1.0.7\" />" +
              "<PackageReference Include=\"WeDonekRpc.ApiGateway\" Version=\"1.0.7\" />" +
              "<PackageReference Include=\"WeDonekRpc.HttpApiGateway\" Version=\"1.0.7\" />" +
              "<PackageReference Include=\"WeDonekRpc.HttpApiDoc\" Version=\"1.0.7\" />" +
              "</ItemGroup ></Project>";
    }
}
