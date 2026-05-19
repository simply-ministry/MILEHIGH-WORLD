// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System.Threading.Tasks;

namespace MilehighWorld.Core
{
    public interface IMemoryBridgeClient
    {
        /// <summary>
        /// Pulls the historical narrative context for an entity from the Vector DB.
        /// </summary>
        Task<string> FetchSemanticMemoryAsync(string entityId);

        /// <summary>
        /// Pushes a new event to the Vector DB so the LLM remembers it later.
        /// </summary>
        Task SubmitMemoryFragmentAsync(string entityId, string memoryData);
    }
}
