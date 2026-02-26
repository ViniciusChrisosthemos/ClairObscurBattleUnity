using UnityEngine;

public class EndBattleState : BaseBattleState
{

    public EndBattleState(CombatManager combatManager) : base(combatManager)
    {
    }

    public override void Enter()
    {
        CombatManager.UIEndBattleView.Setup(CombatManager.GetBattleResult(), HandlePlayAgain);
    }

    public override void Exit()
    {
        CombatManager.UIEndBattleView.Close();
    }

    public override void UpdateState()
    {

    }

    private void HandlePlayAgain()
    {
        CombatManager.StartCombat();
    }
}
