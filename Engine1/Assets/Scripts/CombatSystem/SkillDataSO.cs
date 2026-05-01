using UnityEngine;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = "Skill data", menuName = "Agent/Skill data")]
    public class SkillDataSO : ScriptableObject
    {
        public int skillIndex;
        public string skillName;
        public float cooldown;
    }
}
