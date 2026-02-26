using UnityEngine;

public class EndBattleState : BaseBattleState
{

    public EndBattleState(CombatManager combatManager) : base(combatManager)
    {
    }

    public override void Enter()
    {
        CombatManager.UIEndBattleView.Setup(CombatManager.GetBattleResult());
    }

    public override void Exit()
    {

    }

    public override void UpdateState()
    {

    }
}
