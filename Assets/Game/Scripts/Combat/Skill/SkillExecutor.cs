using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuickTimeEventManager m_quickTimeEventManager;
    [SerializeField] private BattleCameraController m_battleCameraController;

    [Header("Parameters")]
    [SerializeField] private float m_timeAfterMoveToLocation = 0.5f;
    [SerializeField] private float m_qteAnimationSpeed = 0.04f;
    [SerializeField] private float m_qteInterval = 0.7f;
    [SerializeField] private float m_qteDuration = 1.2f;
    [SerializeField] private int m_qteAmount = 2;

    private BattleCharacterView m_battleCharacterView;
    private SkillSO m_skill;
    private List<BattleCharacterView> m_targets;

    private bool m_animationEnded;
    private QuickTimeEventResult m_qteResult;

    public async void ExecuteSkill(BattleCharacterView battleCharacter, SkillSO skill, List<BattleCharacterView> targets)
    {
        m_battleCharacterView = battleCharacter;
        m_skill = skill;
        m_targets = targets;

        m_animationEnded = false;

        BindTriggerEvents(battleCharacter);

        var location = targets[0].AttackerSpot;

        await battleCharacter.MoveTo(location);

        await Task.Delay((int)(m_timeAfterMoveToLocation * 1000));

        m_battleCharacterView.SetTriggerAnimator(skill.AnimationTrigger);

        await UniTask.WaitUntil(() => m_animationEnded);

        UnbindTriggerEvents(battleCharacter);

        m_battleCameraController.Reset();
        m_battleCharacterView.ResetPosition();
    }

    private void HandleAnimationStart()
    {
        m_battleCameraController.SetParent(m_battleCharacterView.AnimatorCameraPivot);
    }

    private void HandleAnimationEnd()
    {
        m_animationEnded = true;
    }

    private void HandleQTEStart()
    {
        m_battleCharacterView.SetAnimatorSpeed(m_qteAnimationSpeed);

        m_quickTimeEventManager.StartEvents(m_qteDuration, m_qteAmount, m_qteInterval, HandleQTEResult);
    }

    private void HandleQTEEnd()
    {
        m_battleCharacterView.SetAnimatorSpeed(1f);
    }

    private void HandleDamageEvent()
    {
        if (m_qteResult.Misses == 0)
        {
            m_targets.ForEach(bcv => bcv.Dodge());
        }
        else
        {
            m_targets.ForEach(bcv => bcv.TakeDamage(50));
        }
    }

    private void HandleQTEResult(QuickTimeEventResult qteResult)
    {
        m_qteResult = qteResult;
    }

    private void BindTriggerEvents(BattleCharacterView battleCharacterView)
    {
        battleCharacterView.SkillAnimationTriggers.OnAnimationStart.AddListener(HandleAnimationStart);
        battleCharacterView.SkillAnimationTriggers.OnAnimationEnd.AddListener(HandleAnimationEnd);
        battleCharacterView.SkillAnimationTriggers.OnQTEStart.AddListener(HandleQTEStart);
        battleCharacterView.SkillAnimationTriggers.OnQTEEnd.AddListener(HandleQTEEnd);
        battleCharacterView.SkillAnimationTriggers.OnDamageEvent.AddListener(HandleDamageEvent);
    }

    private void UnbindTriggerEvents(BattleCharacterView battleCharacterView)
    {
        battleCharacterView.SkillAnimationTriggers.OnAnimationStart.RemoveListener(HandleAnimationStart);
        battleCharacterView.SkillAnimationTriggers.OnAnimationEnd.RemoveListener(HandleAnimationEnd);
        battleCharacterView.SkillAnimationTriggers.OnQTEStart.RemoveListener(HandleQTEStart);
        battleCharacterView.SkillAnimationTriggers.OnQTEEnd.RemoveListener(HandleQTEEnd);
        battleCharacterView.SkillAnimationTriggers.OnDamageEvent.RemoveListener(HandleDamageEvent);
    }
}
