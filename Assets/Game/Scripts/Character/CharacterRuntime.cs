using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CharacterRuntime
{
    public CharacterSO BaseCharacterData {  get; private set; }
    public int MaxHP {  get; private set; }
    public int Agility {  get; private set; }

    public CharacterRuntime(CharacterSO characterSO)
    {
        BaseCharacterData = characterSO;
        MaxHP = characterSO.MaxHealth;
        Agility = characterSO.Agility;
    }

}
