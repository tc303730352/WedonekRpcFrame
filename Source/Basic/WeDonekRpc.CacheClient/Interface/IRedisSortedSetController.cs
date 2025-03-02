using System;
using System.Collections.Generic;
using CSRedis;

namespace WeDonekRpc.CacheClient.Interface
{
    public interface IRedisSortedSetController
    {
        long Add (string key, object name, decimal val);
        long Add<T> (string key, Dictionary<T, decimal> dic);
        long Count (string key, decimal start, decimal end);
        long Count (string key, string start, string end);
        long CountByLex (string key, string min, string max);
        bool Exists (string key);
        string[] GetRanks (string key, long start, long end);
        T[] GetRanks<T> (string key, long start, long end);
        string[] Gets (string key, long start, long end);
        RedisScan<(T member, decimal score)> Gets<T> (string key, long index, string pattern = null, long? count = null);
        T[] Gets<T> (string key, long start, long end);
        long Length (string key);
        long? Rank<T> (string key, T val);
        string[] RankByScore (string key, decimal max, decimal min, long? count = null, long offset = 0);
        long Remove<T> (string key, params T[] vals);
        long RemoveByLex (string key, string min, string max);
        long RemoveByRank<T> (string key, long start, int end);
        long RemoveByScore (string key, decimal min, decimal max);
        T[] RnankByScore<T> (string key, decimal max, decimal min, long? count = null, long offset = 0);
        decimal? Score<T> (string key, T value);
        bool SetExpire (string key, TimeSpan time);
        long Union<T> (string destination, decimal[] weight, RedisAggregate aggregate, params string[] key);
    }
}