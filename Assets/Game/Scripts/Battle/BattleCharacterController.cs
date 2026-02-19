using UnityEngine;
using UnityEngine.InputSystem;

public class BattleCharacterController : BattleCharacterUnitController
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private string m_blockingString = "IsBlocking";
    [SerializeField] private string m_takeHitString = "TakeHit";
    [SerializeField] private float m_timeToParry = 0.3f;

    private bool m_isBlocking = false;
    private float m_lastBlockingTime;


    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            SetBlockUp();
        }
        else if (Keyboard.current.qKey.wasReleasedThisFrame)
        {
            SetBlockDown();
        }
    }

    public void SetBlockUp()
    {
        m_isBlocking = true;
        m_lastBlockingTime = Time.time;

        m_animator.SetBool(m_blockingString, m_isBlocking);
    }

    public void SetBlockDown()
    {
        m_isBlocking = false;

        m_animator.SetBool(m_blockingString, m_isBlocking);
    }

    public override void TakeDamage(int damage)
    {
        if (Time.time - m_lastBlockingTime < m_timeToParry)
        {
            Debug.Log("Parry!");
        }
        else
        {
            if (m_isBlocking)
            {
                Debug.Log("Damage Blocked!");
            }
            else
            {
                Debug.Log("Damage Taken");
            }
        }

        m_animator.SetTrigger(m_takeHitString);
    }
}
