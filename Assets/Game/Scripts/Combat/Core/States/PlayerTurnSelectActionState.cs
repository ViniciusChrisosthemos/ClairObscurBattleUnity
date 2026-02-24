using UnityEngine;

public class PlayerTurnSelectActionState : BaseBattleState
{
    [Header("References")]
    [SerializeField] private UIBattleHUDView m_uiBattleHUDView;
    [SerializeField] private BattleCameraManager m_battleCameraManager;

    public override void Enter()
    {
        var battleCharacterView = CombatManager.CurrentCharacterTurn;

        m_uiBattleHUDView.SetPosition(battleCharacterView.ActionSelectionCanvasSpot);
        m_battleCameraManager.MoveCameraTo(battleCharacterView.ActionSelectionCameraSpot);
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    protected override void HandleInternalAwake()
    {

    }

    protected override void HandleInternalStart()
    {

    }
}
