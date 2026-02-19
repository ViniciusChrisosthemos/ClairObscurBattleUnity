using UnityEngine;

public class EnemyBattleCharacterController : BattleCharacterUnitController
{
    [SerializeField] private BattleCharacterUnitController m_target;
    [SerializeField] private float m_attackInterval = 1.5f;
    [SerializeField] private Animator m_animator;
    [SerializeField] private string m_attackString = "Attack";

    private void Start()
    {
        InvokeRepeating("Attack", m_attackInterval, m_attackInterval);
    }

    private void Attack()
    {
        m_animator.SetTrigger(m_attackString);
    }

    public void TriggerAttackDamage()
    {
        m_target.TakeDamage(100);
    }
}
