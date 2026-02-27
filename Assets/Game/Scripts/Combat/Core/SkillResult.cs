using UnityEngine;

public class SkillResult
{
    public int DamageDone { get; private set; }

    public bool HasParryIt { get; private set; }
    public bool HasMissed { get; private set; }

    public SkillResult(int damageDone, bool hasParryIt, bool hasMissed)
    {
        DamageDone = damageDone;
        HasParryIt = hasParryIt;
        HasMissed = hasMissed;
    }
}
