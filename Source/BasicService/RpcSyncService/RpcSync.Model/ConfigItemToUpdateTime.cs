using WeDonekRpc.Helper;

namespace RpcSync.Model
{
    public class ConfigItemToUpdateTime : IEquatable<ConfigItemToUpdateTime>
    {
        public long RpcMerId { get; set; }

        public long ServerId { get; set; }

        public int RegionId { get; set; }

        public long ContainerGroup { get; set; }

        public int VerNum { get; set; }

        public string SystemType { get; set; }

        public DateTime ToUpdateTime { get; set; }

        public bool IsSystemType ()
        {
            return this.SystemType.IsNotNull() && this.RegionId == 0 &&
                this.ContainerGroup == 0 &&
                this.RegionId == 0 &&
                this.ServerId == 0 &&
                this.VerNum == 0;
        }
        public override bool Equals ( object? obj )
        {
            if ( obj is ConfigItemToUpdateTime i )
            {
                return this.Equals(i);
            }
            return false;
        }
        public override int GetHashCode ()
        {
            return string.Join(',', this.ServerId, this.RpcMerId, this.RegionId, this.ContainerGroup, this.VerNum, this.SystemType ?? string.Empty).GetHashCode();
        }
        public bool Equals ( ConfigItemToUpdateTime? other )
        {
            if ( other == null )
            {
                return false;
            }
            if ( other.SystemType == null )
            {
                other.SystemType = string.Empty;
            }
            if ( this.SystemType == null )
            {
                this.SystemType = string.Empty;
            }
            return other.RpcMerId == this.RpcMerId &&
                other.RegionId == this.RegionId &&
                other.ServerId == this.ServerId &&
                other.ContainerGroup == this.ContainerGroup &&
                other.VerNum == this.VerNum &&
                this.SystemType == other.SystemType;
        }
    }
}
