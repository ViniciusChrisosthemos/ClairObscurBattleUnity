using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BattleController : MonoBehaviour
{
    [SerializeField] private List<CharacterSpot> _characters;
    [SerializeField] private BattleSkillAnimationController m_battleSkillAnimationController;

    [Header("Events")]
    public UnityEvent<CharacterSpot> OnCharacterTurnChanged;

    private CharacterSpot m_currentCharacterTurn;
    private TimelineController<CharacterSpot> _timelieController;

    private void Awake()
    {
        _timelieController = new TimelineController<CharacterSpot>(_characters);
    }

    private void Start()
    {
        PassTurn();
    }

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            PassTurn();
        }
    }

    public void PassTurn()
    {
        if (_timelieController.IsEmpty())
        {
            _timelieController.UpdateTimeLine();
        }

        m_currentCharacterTurn = _timelieController.Dequeue();

        OnCharacterTurnChanged?.Invoke(m_currentCharacterTurn);
    }

    public List<CharacterSpot> GetCharacterSpotsOrder() => _timelieController.GetTimeline();

    public List<CharacterRuntime> GetPlayerCharacterRuntime() => _characters.FindAll(c => c.IsPlayerCharacter).ConvertAll(c => c.CharacterRuntime);

    public void PlayAction(SkillSO skill, List<CharacterSpot> targets)
    {
        m_battleSkillAnimationController.PlaySkill(m_currentCharacterTurn, skill, targets, () =>
        {
            PassTurn();
        });
    }
}
