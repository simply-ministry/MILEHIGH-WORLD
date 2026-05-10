using UnityEngine;

namespace Milehigh.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Milehigh/Character Data")]
    public class CharacterData : ScriptableObject
    {
        public string characterName = null!;
        public string role = null!;
        [TextArea(3, 10)]
        public string[] traits = null!;
        [TextArea(10, 20)]
        public string behaviorScript = null!;
        public string behaviorScript;

        public float health = 100f;
        public float resonance = 1.0f;
        public float integrity = 1.0f;
        public float vanguardMultiplier = 1.0f;
    }
}
