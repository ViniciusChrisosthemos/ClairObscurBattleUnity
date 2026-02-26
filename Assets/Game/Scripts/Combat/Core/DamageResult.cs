using UnityEngine;

public class DamageResult
{
    public int DamageDone { get; private set; }

    public bool HasParryIt { get; private set; }

    public DamageResult(int damageDone, bool hasParryIt)
    {
        DamageDone = damageDone;
        HasParryIt = hasParryIt;
    }
}
