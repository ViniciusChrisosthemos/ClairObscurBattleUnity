using UnityEngine;

public static class SkillResultFactory
{
    public static SkillResult MissSkillResult() => new SkillResult(0, false, true);
    public static SkillResult ParrySkillResult() => new SkillResult(0, true, false);
    public static SkillResult DamageTakenSkillResult(int damageTaken) => new SkillResult(damageTaken, false, false);
}
