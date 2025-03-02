using System;
using WeDonekRpc.Model.Server;

namespace WeDonekRpc.Client.Helper
{
    internal class GCHelper
    {
        public static GCBody GetGC ()
        {
            int[] recycle = new int[GC.MaxGeneration];
            for (int i = 0; i < GC.MaxGeneration; i++)
            {
                recycle[i] = GC.CollectionCount(i);
            }
            GCMemoryInfo memory = GC.GetGCMemoryInfo();
            return new GCBody
            {
                TotalAvailableMemoryBytes = memory.TotalAvailableMemoryBytes,
                Compacted = memory.Compacted,
                Concurrent = memory.Concurrent,
                FinalizationPendingCount = memory.FinalizationPendingCount,
                FragmentedBytes = memory.FragmentedBytes,
                Generation = memory.Generation,
                GenerationInfo = memory.GenerationInfo.ToArray(),
                HeapSizeBytes = memory.HeapSizeBytes,
                HighMemoryLoadThresholdBytes = memory.HighMemoryLoadThresholdBytes,
                Index = memory.Index,
                MemoryLoadBytes = memory.MemoryLoadBytes,
                PauseDurations = memory.PauseDurations.ToArray(),
                PauseTimePercentage = memory.PauseTimePercentage,
                PinnedObjectsCount = memory.PinnedObjectsCount,
                PromotedBytes = memory.PromotedBytes,
                Recycle = recycle,
                TotalCommittedBytes = memory.TotalCommittedBytes
            };
        }
    }
}
