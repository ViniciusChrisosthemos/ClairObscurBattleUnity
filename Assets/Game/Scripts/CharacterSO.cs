using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character_", menuName = "ScriptableObjects/Characters/Character")]
public class CharacterSO : ScriptableObject
{
    public string CharacterName;
    public Sprite CharacterIcon;
    public int MaxHealth;
    public int Agility;

    public List<SkillSO> Skills;
}
