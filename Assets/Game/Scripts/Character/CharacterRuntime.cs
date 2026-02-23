using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CharacterRuntime: ITimelineElement
{
    public CharacterSO CharacterSO;
    public int MaxHP;
    public int CurrentHP;
    public int CurrentAgility;

    public CharacterRuntime(CharacterSO characterSO)
    {
        CharacterSO = characterSO;
        MaxHP = characterSO.MaxHealth;
        CurrentHP = MaxHP;
        CurrentAgility = characterSO.Agility;
    }

    public bool IsAlive() => CurrentHP > 0;

    public int GetPriority()
    {
        return CurrentAgility;
    }

    public bool IsActive()
    {
        return IsAlive();
    }

    public void TakeDamage(int damage)
    {
        CurrentHP -= damage;
    }
}
