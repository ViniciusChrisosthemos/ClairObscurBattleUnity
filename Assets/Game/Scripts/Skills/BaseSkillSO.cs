using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillSO : ScriptableObject
{
    public enum SkillTargetType
    {
        Self,
        SingleEnemy,
        AllEnemies,
        SingleAlly,
        AllAllies
    }

    [SerializeField] private string m_name;
    [SerializeField] private Sprite m_icon;
    [SerializeField] private string m_description;
    [SerializeField] private List<Sprite> m_dicesRequired;
    [SerializeField] private SkillTargetType m_targetType;
    [SerializeField] private string m_animationTrigger;

    public abstract SkillResult Execute(BattleCharacter source, List<BattleCharacter> targets, QuickTimeEventResult qteResult);

    public string SkillName => m_name;
    public Sprite Icon => m_icon;
    public string Description => m_description;
    public List<Sprite> DicesRequired => m_dicesRequired;
    public SkillTargetType TargetType => m_targetType;
    public string AnimationTrigger => m_animationTrigger;
}
