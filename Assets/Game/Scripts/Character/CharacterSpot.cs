using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSpot : MonoBehaviour, ITimelineElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    public CharacterRuntime CharacterRuntime;
    public Transform ModelTransform;
    public Transform ActionSelectionCameraSpot;
    public Transform SkillSelectionCameraSpot;
    public Transform ActionSelectionCanvasSpot;
    public Transform SkillSelectionCanvasSpot;
    public Transform AnimatorCameraPivot;
    public Transform AttackerSpot;
    public Transform TargetPosition;
    public Slider SliderHPBar;

    [Header("Animation")]
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private SkillAnimationTriggers m_skillAnimationTriggers;
    [SerializeField] private string _takeDamageTriggerName = "TakeDamage";
    [SerializeField] private string _dieTriggerName = "Die";
    [SerializeField] private string _dodgeTriggerName = "Dodge";
    [SerializeField] private string _runBoolName = "Run";

    [Header("Parameters")]
    public bool IsPlayerCharacter = false;

    [Header("Events")]
    public UnityEvent<CharacterSpot> OnCharacterHoverEnter;
    public UnityEvent<CharacterSpot> OnCharacterHoverExit;
    public UnityEvent<CharacterSpot> OnCharacterSelected;

    private void Start()
    {
        if (CharacterRuntime != null && SliderHPBar != null)
        {
            SliderHPBar.maxValue = CharacterRuntime.MaxHP;
            SliderHPBar.value = CharacterRuntime.CurrentHP;
        }
    }

    public int GetPriority()
    {
        return CharacterRuntime.GetPriority();
    }

    public bool IsActive()
    {
        return CharacterRuntime.IsActive();
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

    public void UpdateHP()
    {
        if (SliderHPBar.value <= 0) return;

        if (SliderHPBar.value != CharacterRuntime.CurrentHP)
        {
            if (CharacterRuntime.CurrentHP <= 0)
            {
                _characterAnimator.SetTrigger(_dieTriggerName);
            }
            else
            {
                _characterAnimator.SetTrigger(_takeDamageTriggerName);
            }
        }

        SliderHPBar.maxValue = CharacterRuntime.MaxHP;
        SliderHPBar.value = CharacterRuntime.CurrentHP;
    }

    public void DodgeAttack()
    {
        _characterAnimator.SetTrigger(_dodgeTriggerName);
    }

    public void ApplyDamage(int damage)
    {
        CharacterRuntime.TakeDamage(damage);
        UpdateHP();
    }

    public void ResetPosition()
    {
        ModelTransform.localPosition = Vector3.zero;
    }

    public void RunToPosition(Transform targetSpot, float speed, Action callback)
    {
        StartCoroutine(AnimateRunCoroutine(targetSpot.position, targetSpot.rotation, speed, callback));
    }

    private IEnumerator AnimateRunCoroutine(Vector3 targetPosition, Quaternion targetRotation, float speed, Action callback)
    {
        _characterAnimator.SetBool(_runBoolName, true);

        var t = 0f;
        var startPosition = ModelTransform.position;

        while (t < 1f)
        {
            ModelTransform.position = Vector3.Lerp(startPosition, targetPosition, t);

            t += Time.deltaTime / speed;

            yield return null;
        }

        ModelTransform.position = targetPosition;

        _characterAnimator.SetBool(_runBoolName, false);

        callback?.Invoke();
    }

    public Animator Animator => _characterAnimator;
    public SkillAnimationTriggers SkillAnimationTriggers => m_skillAnimationTriggers;
}
