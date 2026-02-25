using System.Threading.Tasks;
using UnityEngine;

public class EnemyTurnState : BaseBattleState
{
    public EnemyTurnState(CombatManager combatManager) : base(combatManager) {}

    public override void Enter()
    {
        var battleCharacterView = CombatManager.CurrentCharacterTurn;

        CombatManager.BattleCameraManager.MoveCameraTo(battleCharacterView.ActionSelectionCameraSpot);

        HandleTurn();
    }

    public override void Exit()
    {
        Debug.Log("EnemyTurnState Exit");
    }

    public override void UpdateState()
    {
        Debug.Log("EnemyTurnState UpdateState");
    }

    private async Task HandleTurn()
    {
        await Task.Delay(1000);

        CombatManager.NextTurn();
    }
}
