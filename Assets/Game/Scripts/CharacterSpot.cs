using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSpot : MonoBehaviour, ITimelineElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("References")]
    public CharacterRuntime CharacterRuntime;
    public Transform ActionSelectionCameraSpot;
    public Transform SkillSelectionCameraSpot;
    public Transform ActionSelectionCanvasSpot;
    public Transform SkillSelectionCanvasSpot;
    public Transform TargetPosition;
    public Slider SliderHPBar;

    [Header("Animation")]
    [SerializeField] private Animator _characterAnimator;
    [SerializeField] private string _takeDamageTriggerName = "TakeDamage";
    [SerializeField] private string _dieTriggerName = "Die";

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
}
