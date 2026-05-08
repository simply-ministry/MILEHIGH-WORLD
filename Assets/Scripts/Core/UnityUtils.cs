using System.Collections.Generic;
using UnityEngine;

namespace Milehigh.Core
{
    /// <summary>
    /// Centralized utility for Unity-specific optimizations.
    /// </summary>
    public static class UnityUtils
    {
        private static readonly Dictionary<float, WaitForSeconds> _waitCache = new Dictionary<float, WaitForSeconds>();

        /// <summary>
        /// Retrieves a cached WaitForSeconds object for the given duration.
        /// ⚡ Bolt: Eliminates GC pressure by avoiding redundant 'new WaitForSeconds()' allocations.
        /// </summary>
        /// <param name="seconds">The duration to wait.</param>
        /// <returns>A cached WaitForSeconds instance.</returns>
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
