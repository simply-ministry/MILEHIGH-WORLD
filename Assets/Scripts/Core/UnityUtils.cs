using UnityEngine;
using System.Collections.Generic;

namespace Milehigh.Core
{
    /// <summary>
    /// Centralized utility for Unity-specific optimizations.
    /// </summary>
    public static class UnityUtils
    {
        private static readonly Dictionary<float, WaitForSeconds> _waitCache = new Dictionary<float, WaitForSeconds>();

        /// <summary>
        /// Returns a cached WaitForSeconds object for the specified duration.
        /// This eliminates GC pressure from repeated 'new WaitForSeconds' allocations.
        /// </summary>
        public static WaitForSeconds GetWait(float seconds)
        {
            if (!_waitCache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitCache[seconds] = wait;
            }
            return wait;
        }
    }
}
