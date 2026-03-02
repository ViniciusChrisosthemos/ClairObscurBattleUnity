using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacter : IBattleCharacter, ITimelineElement
{
    public event Action<int, int> OnTakeDamageEvent;
    public event Action OnDieEvent;

    public BattleCharacter(CharacterRuntime character, bool isPlayer)
    {
        IsPlayer = isPlayer;
        CharacterRuntime = character;
        CurrentHP = character.MaxHP;

        Skills = character.BaseCharacterData.Skills;
    }

    public bool IsAlive()
    {
        return CurrentHP > 0;
    }

    public void TakeDamage(int damage)
    {
        CurrentHP = Mathf.Max(0, CurrentHP - damage);

        OnTakeDamageEvent?.Invoke(damage, CurrentHP);

        if (CurrentHP == 0)
        {
            OnDieEvent?.Invoke();
        }
    }

    public int GetPriority()
    {
        return CharacterRuntime.Agility;
    }

    public bool IsActive()
    {
        return CurrentHP > 0;
    }

    public List<BaseSkillSO> Skills { get; private set; }

    public int CurrentHP { get; private set; }

    public int MaxpHP => CharacterRuntime.MaxHP;

    public bool IsPlayer { get; private set; }

    public CharacterRuntime CharacterRuntime { get; private set; }
}
