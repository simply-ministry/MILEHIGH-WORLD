// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using MilehighWorld.Data;

namespace MilehighWorld.Characters
{
    public class CyrusAIController : CharacterControllerBase
    {
        public override void ExecuteBehavior()
        {
            ExecuteInvasion();
        }

        public void ExecuteInvasion()
        {
            string target = "Onalym Nexus";
            Debug.Log($"Cyrus: Invading {target}!");
            UnleashVoidShockwave();
            OverwriteCoreLogic();
        }

        private void UnleashVoidShockwave()
        {
            Debug.Log("Cyrus: Unleashing Void Shockwave!");
        }

        private void OverwriteCoreLogic()
        {
            Debug.Log("Cyrus: Overwriting Core Logic...");
        }
    }
}
