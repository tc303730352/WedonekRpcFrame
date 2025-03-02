using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WeDonekRpc.ApiGateway.IpBlack.Model
{

    internal class RemoteIpBlackTo:IEqualityComparer<RemoteIpBlackTo>,IEquatable<RemoteIpBlackTo>
    {
        private readonly long _BeginIp;
        private readonly long _EndIp;
        private string _Id;
        public long BeginIp => _BeginIp;

        public long EndIp => _EndIp;

        public RemoteIpBlackTo(long begin, long end)
        {
            this._Id = begin + "_" + end;
            this._BeginIp = begin;
            this._EndIp = end;
        }

        public bool IsLimit(long ip)
        {
            return this._BeginIp <= ip && ip <= this._EndIp;
        }


        public bool Equals(RemoteIpBlackTo x, RemoteIpBlackTo y)
        {
            return x._Id == y._Id;
        }
        public override bool Equals(object obj)
        {
            if(obj is RemoteIpBlackTo i)
            {
                return i._Id == this._Id;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return this._Id.GetHashCode();
        }
        public int GetHashCode([DisallowNull] RemoteIpBlackTo obj)
        {
            return obj._Id.GetHashCode();
        }

        public bool Equals(RemoteIpBlackTo other)
        {
            return other._Id == this._Id;
        }
    }
}