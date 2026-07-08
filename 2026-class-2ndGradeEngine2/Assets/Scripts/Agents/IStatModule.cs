using Agents.StatSystem;
using CombatSystem;

namespace Agents
{
    public interface IStatModule
    {
        StatSO GetStat(int statIndex);
        bool TryGetStat(int statIndex, out StatSO stat);
        void AddModifier(int statIndex, object key, float value);
        void RemoveModifier(int statIndex, object key);
        float SubscribeStat(int statIndex, StatSO.ValueChangeHandler handler, float defaultValue);
        void UnSubscribeStat(int statIndex, StatSO.ValueChangeHandler handler);
        
        DamageData CalculateDamage(SkillDataSO skillData);
    }
}