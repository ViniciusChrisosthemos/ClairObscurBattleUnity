using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillExecutionState : BaseBattleState
{
    private BattleCharacterView m_characterView;
    private BaseSkillSO m_skillSO;
    private List<BattleCharacterView> m_targets;
    private Action m_callback;

    public SkillExecutionState(CombatManager combatManager, BattleCharacterView character, BaseSkillSO skill, List<BattleCharacterView> targets, Action callback) : base(combatManager)
    {
        m_characterView = character;
        m_skillSO = skill;
        m_targets = targets;
        m_callback = callback;
    }

    public override void Enter()
    {
        CombatManager.BattleSkillAnimationManager.PlaySkill(CombatManager, m_characterView, m_skillSO, m_targets, m_callback);
    }

    public override void Exit()
    {

    }

    public override void UpdateState()
    {

    }
}
