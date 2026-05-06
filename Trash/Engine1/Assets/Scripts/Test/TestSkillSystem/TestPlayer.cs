using GGMLib.ModuleSystem;
using Players;
using UnityEngine;

namespace Test.TestSkillSystem
{
    public class TestPlayer : ModuleOwner
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }    
    }
}
