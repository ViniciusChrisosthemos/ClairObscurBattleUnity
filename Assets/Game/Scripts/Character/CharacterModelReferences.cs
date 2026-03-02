using UnityEngine;

public class CharacterModelReferences : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private Transform m_animationCameraPivot;
    [SerializeField] private Transform m_hitPivot;
    [SerializeField] private SkillAnimationTriggers m_skillAnimationTriggers;

    public Transform HitPivot => m_hitPivot;
    public Transform AnimationCameraPivot => m_animationCameraPivot;
    public SkillAnimationTriggers SkillAnimationTriggers => m_skillAnimationTriggers;
    public Animator Animator => m_animator;
}
