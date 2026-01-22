using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName = "ScriptableObjects/Skills/Skill")]
public class SkillSO : ScriptableObject
{
    public enum SkillTargetType
    {
        Self,
        SingleEnemy,
        AllEnemies,
        SingleAlly,
        AllAllies
    }

    public string SkillName;
    public Sprite Icon;
    public string Description;
    public List<Sprite> DicesRequired;
    public SkillTargetType TargetType;
}
