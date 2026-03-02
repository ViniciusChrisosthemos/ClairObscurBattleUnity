using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_Damage_", menuName = "ScriptableObjects/Skills/Damage")]
public class DamageSkillSO : BaseSkillSO
{
    [SerializeField] private int m_damage;
    [SerializeField] private float m_bonusForPerfectQTE = 1.5f;

    public override SkillResult Execute(BattleCharacter source, List<BattleCharacter> targets, QuickTimeEventResult qteResult)
    {
        if (qteResult.EventAmount != 0 && qteResult.Misses == qteResult.EventAmount)
        {
            return SkillResultFactory.MissSkillResult();
        }
        else
        {
            var finalDamage = m_damage;

            finalDamage = qteResult.Perfects == qteResult.EventAmount && qteResult.EventAmount != 0 ? (int)(finalDamage * m_bonusForPerfectQTE) : finalDamage;

            Debug.Log($"{finalDamage} {qteResult.EventAmount} {qteResult.Perfects} {qteResult.Regulars} {qteResult.Misses}");

            targets.ForEach(t => t.TakeDamage(finalDamage));

            return SkillResultFactory.DamageTakenSkillResult(finalDamage);
        }
    }
}
