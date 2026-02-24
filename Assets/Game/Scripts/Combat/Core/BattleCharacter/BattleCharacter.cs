using System;
using UnityEngine;

public class BattleCharacter : IBattleCharacter
{
    public event Action<int, int> OnTakeDamageEvent;
    public event Action OnDieEvent;

    public BattleCharacter(CharacterRuntime character, bool isPlayer)
    {
        IsPlayer = isPlayer;
        BaseCharacter = character;
        CurrentHP = character.MaxHP;
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

    public int CurrentHP { get; private set; }

    public bool IsPlayer { get; private set; }

    public CharacterRuntime BaseCharacter { get; private set; }
}
