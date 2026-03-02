using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BattleCharacterView : MonoBehaviour, ITimelineElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IMutableAnimationSpeed
{
    [Header("View References")]
    [SerializeField] private Transform m_modelTransform;
    [SerializeField] private Transform m_actionSelectionCameraSpot;
    [SerializeField] private Transform m_skillSelectionCameraSpot;
    [SerializeField] private Transform m_actionSelectionCanvasSpot;
    [SerializeField] private Transform m_skillSelectionCanvasSpot;
    [SerializeField] private Transform m_SelectionSpot;
    [SerializeField] private Transform m_attackerSpot;
    [SerializeField] private Transform m_vfxSpot;

    [Header("Animation")]
    [SerializeField] private string m_takeDamageTriggerName = "TakeDamage";
    [SerializeField] private string m_dieTriggerName = "Die";
    [SerializeField] private string m_dodgeTriggerName = "Dodge";
    [SerializeField] private string m_parryTriggerName = "Parry";
    [SerializeField] private string m_runBoolName = "Run";

    [Header("Parameters")]
    [SerializeField] private float m_parryTime = 0.3f;
    [SerializeField] private float m_parryInterval = 1.5f;

    [Header("Events")]
    public UnityEvent<BattleCharacterView> OnCharacterHoverEnter;
    public UnityEvent<BattleCharacterView> OnCharacterHoverExit;
    public UnityEvent<BattleCharacterView> OnCharacterSelected;
    public UnityEvent<int> OnTakeDamage;


    private Animator m_characterAnimator;
    private SkillAnimationTriggers m_skillAnimationTriggers;
    private Transform m_animationCameraPivot;
    private Transform m_hitPivot;

    private float m_lastParryTime;

    public void Setup(BattleCharacter battleCharacter)
    {
        BattleCharacter = battleCharacter;

        var modelReference = Instantiate(battleCharacter.CharacterRuntime.BaseCharacterData.ModelReferences, m_modelTransform);
        modelReference.transform.localPosition = Vector3.zero;
        modelReference.transform.localRotation = Quaternion.identity;

        m_characterAnimator = modelReference.Animator;
        m_skillAnimationTriggers = modelReference.SkillAnimationTriggers;
        m_animationCameraPivot = modelReference.AnimationCameraPivot;
        m_hitPivot = modelReference.HitPivot;

        BattleCharacter.OnDieEvent += HandleCharacterDie;
        BattleCharacter.OnTakeDamageEvent += HandleCharacterTakeDamage;
    }

    public async Task MoveTo(Transform location, float speed = 0.5f)
    {
        m_characterAnimator.SetBool(m_runBoolName, true);

        var t = 0f;

        var startPosition = m_modelTransform.position;
        var endPosition = location.position;

        m_modelTransform.LookAt(location, Vector3.up);

        while (t < 1)
        {
            var newPosition = Vector3.Lerp(startPosition, endPosition, t);
            m_modelTransform.position = newPosition;  

            t += Time.deltaTime / speed;

            await Task.Yield();
        }

        m_modelTransform.position = endPosition;
        m_modelTransform.rotation = location.rotation;

        m_characterAnimator.SetBool(m_runBoolName, false);
    }

    public void SetTriggerAnimator(string animationTrigger)
    {
        m_characterAnimator.SetTrigger(animationTrigger);
    }

    public void ResetPosition()
    {
        m_modelTransform.localPosition = Vector3.zero;
        m_modelTransform.localRotation = Quaternion.identity;
    }

    public void Dodge()
    {
        m_characterAnimator.SetTrigger(m_dodgeTriggerName);
    }

    public void PlaySkillAnimation(string skillAnimationTrigger)
    {
        m_characterAnimator.SetTrigger(skillAnimationTrigger);
    }

    public int GetPriority()
    {
        return BattleCharacter.CharacterRuntime.Agility;
    }

    public bool IsActive()
    {
        return BattleCharacter.IsAlive();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnCharacterSelected?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCharacterHoverEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnCharacterHoverExit?.Invoke(this);
    }

    private void HandleParryEvent()
    {
        if (Time.time - m_lastParryTime >= m_parryInterval)
        {
            m_lastParryTime = Time.time;

            m_characterAnimator.SetTrigger(m_parryTriggerName);
        }
    }

    private void HandleCharacterDie()
    {
        m_characterAnimator.SetTrigger(m_dieTriggerName);
    }

    private void HandleCharacterTakeDamage(int damageTaken, int currentHP)
    {
        if (BattleCharacter.IsAlive())
        {
            m_characterAnimator.SetTrigger(m_takeDamageTriggerName);
        }

        OnTakeDamage?.Invoke(damageTaken);
    }

    public void EnableParry()
    {
        InputManager.Instance.OnParryEvent.AddListener(HandleParryEvent);
    }

    public void DisableParry()
    {
        InputManager.Instance.OnParryEvent.RemoveListener(HandleParryEvent);
    }

    public void SetAnimationSpeed(float speed)
    {
        m_characterAnimator.speed = speed;
    }

    public bool HasParryIt => (Time.time - m_lastParryTime) < m_parryTime;
    public Transform ModelTransform => m_modelTransform;
    public Transform ActionSelectionCameraSpot => m_actionSelectionCameraSpot;
    public Transform SkillSelectionCameraSpot => m_skillSelectionCameraSpot;
    public Transform ActionSelectionCanvasSpot => m_actionSelectionCanvasSpot;
    public Transform SkillSelectionCanvasSpot => m_skillSelectionCanvasSpot;
    public Transform AnimationCameraPivot => m_animationCameraPivot;
    public Transform SelectionSpot => m_SelectionSpot;
    public Transform AttackerSpot => m_attackerSpot;
    public Transform VFXSpot => m_vfxSpot;
    public Transform HitSpot => m_hitPivot;

    public BattleCharacter BattleCharacter { get; private set; }

    public SkillAnimationTriggers SkillAnimationTriggers => m_skillAnimationTriggers;
}
