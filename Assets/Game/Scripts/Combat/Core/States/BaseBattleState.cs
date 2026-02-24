using UnityEngine;

public abstract class BaseBattleState : MonoBehaviour, IState
{
    protected CombatManager CombatManager { get; private set; }

    private void Awake()
    {
        HandleInternalAwake();
    }

    private void Start()
    {
        HandleInternalStart();
    }

    public void Setup(CombatManager manager)
    {
        CombatManager = manager;
    }

    protected abstract void HandleInternalAwake();

    protected abstract void HandleInternalStart();

    public abstract void Enter();

    public abstract void Exit();

    public abstract void UpdateState();
}
