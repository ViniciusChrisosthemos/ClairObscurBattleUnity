using System.Collections.Generic;

public class PlayerTurnState : BaseBattleState
{
    private BaseSkillSO m_selectedSkill;

    public PlayerTurnState(CombatManager combatManager) : base(combatManager) { }

    public override void Enter()
    {
        var battleCharacterView = CombatManager.CurrentCharacterTurn;

        CombatManager.UIBattleHUDView.SetCharacter(battleCharacterView, HandleSkillSelected, HandlePassTurnSelected);
        CombatManager.BattleCameraManager.MoveCameraTo(battleCharacterView.ActionSelectionCameraSpot);
        CombatManager.TargetSelectionManager.OnTargetSelected.AddListener(HandleTargetSelected);
    }

    public override void Exit()
    {
        CombatManager.TargetSelectionManager.OnTargetSelected.RemoveListener(HandleTargetSelected);
        CombatManager.UIBattleHUDView.Deactivate();
    }

    public override void UpdateState()
    {

    }

    private void HandlePassTurnSelected()
    {
        CombatManager.NextTurn();
    }

    private void HandleTargetSelected(List<BattleCharacterView> targets)
    {
        CombatManager.ExecuteSkill(m_selectedSkill, targets);
    }

    private void HandleSkillSelected(BaseSkillSO skill)
    {
        m_selectedSkill = skill;

        if (skill.TargetType == BaseSkillSO.SkillTargetType.AllEnemies)
        {
            CombatManager.TargetSelectionManager.SetAlltargetSelection();
        }
        else
        {
            CombatManager.TargetSelectionManager.SetSingleTargetSelection();
        }

        CombatManager.BattleCameraManager.MoveCameraTo(CombatManager.TargetSelectionManager.TargetSelectionCameraPivot);
    }
}
