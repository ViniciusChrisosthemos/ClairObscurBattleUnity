using UnityEngine;

public abstract class BaseBattleState : IState
{
    public CombatManager CombatManager { get; private set; }

    public BaseBattleState(CombatManager combatManager)
    {
        CombatManager = combatManager;
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void UpdateState();
}
