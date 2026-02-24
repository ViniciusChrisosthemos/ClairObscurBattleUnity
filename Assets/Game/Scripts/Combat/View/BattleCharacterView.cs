using System;
using System.Threading.Tasks;
using UnityEngine;

public class BattleCharacterView : MonoBehaviour, ITimelineElement
{
    [Header("View References")]
    public Transform ModelTransform;
    public Transform ActionSelectionCameraSpot;
    public Transform SkillSelectionCameraSpot;
    public Transform ActionSelectionCanvasSpot;
    public Transform SkillSelectionCanvasSpot;
    public Transform AnimatorCameraPivot;
    public Transform AttackerSpot;
    public Transform VFXSpot;


    [Header("Animation")]
    [SerializeField] private Animator m_characterAnimator;
    [SerializeField] private SkillAnimationTriggers m_skillAnimationTriggers;
    [SerializeField] private string m_takeDamageTriggerName = "TakeDamage";
    [SerializeField] private string m_dieTriggerName = "Die";
    [SerializeField] private string m_dodgeTriggerName = "Dodge";
    [SerializeField] private string m_parryTriggerName = "Parry";
    [SerializeField] private string m_runBoolName = "Run";

    public void Setup(BattleCharacter battleCharacter)
    {
        BattleCharacter = battleCharacter;
    }

    public async Task MoveTo(Transform location, float speed = 0.5f)
    {
        var t = 0f;

        var startPosition = ModelTransform.position;
        var endPosition = location.position;

        while (t < 1)
        {
            var newPosition = Vector3.Lerp(startPosition, endPosition, t);

            t += Time.deltaTime / speed;

            await Task.Yield();
        }

        ModelTransform.position = endPosition;
    }

    public void SetAnimatorSpeed(float speed)
    {
        m_characterAnimator.speed = speed;
    }

    public void SetTriggerAnimator(string animationTrigger)
    {
        m_characterAnimator.SetTrigger(animationTrigger);
    }

    public void ResetPosition()
    {
        ModelTransform.localPosition = Vector3.zero;
        ModelTransform.localRotation = Quaternion.identity;
    }

    public void Dodge()
    {
        m_characterAnimator.SetTrigger(m_dodgeTriggerName);
    }

    public void TakeDamage(int damage)
    {
        BattleCharacter.TakeDamage(damage);

        if (!BattleCharacter.IsAlive())
        {
            m_characterAnimator.SetTrigger(m_dieTriggerName);
        }
        else
        {
            m_characterAnimator.SetTrigger(m_takeDamageTriggerName);
        }
    }

    public int GetPriority()
    {
        return BattleCharacter.BaseCharacter.GetPriority();
    }

    public bool IsActive()
    {
        return BattleCharacter.IsAlive();
    }

    public BattleCharacter BattleCharacter { get; private set; }

    public SkillAnimationTriggers SkillAnimationTriggers => m_skillAnimationTriggers;
}
