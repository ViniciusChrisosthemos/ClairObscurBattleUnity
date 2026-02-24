using UnityEngine;

public class PlayerTurnState : BaseBattleState
{
    [Header("States")]
    [SerializeField] private IdleState m_idleState;
    [SerializeField] private PlayerTurnSelectActionState m_selectActionState;

    private StateMachineController m_stateMachinecontroller;

    protected override void HandleInternalAwake()
    {
        m_stateMachinecontroller = new StateMachineController();

        m_stateMachinecontroller.Setup(m_idleState);
    }

    protected override void HandleInternalStart()
    {
        m_selectActionState.Setup(CombatManager);
    }

    public override void Enter()
    {
        m_stateMachinecontroller.ChangeState(m_selectActionState);
    }

    public override void Exit()
    {

    }

    public override void UpdateState()
    {

    }
}
