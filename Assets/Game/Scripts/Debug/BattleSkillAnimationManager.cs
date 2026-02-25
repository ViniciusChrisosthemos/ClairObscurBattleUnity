using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BattleSkillAnimationManager : MonoBehaviour
{
    public float m_speedInQTE = 0.1f;
    public float m_runSpeed = 0.5f;

    public QuickTimeEventManager m_quickTimeEventManager;
    public float m_qteDuration = 1.0f;
    public float m_qteInterval = 0.45f;
    public int m_qteAmount = 2;

    public Transform m_camera;
    public Transform m_mainCameraPivot;

    [Header("VFX")]
    [SerializeField] private Transform m_vfxParent;
    [SerializeField] private GameObject m_missVFX;
    [SerializeField] private BattleDamageNotificationController m_damageVFX;

    private SkillSO m_skill;
    private BattleCharacterView m_characterView;
    private BattleCharacterView m_enemyCharacterView;

    private Action m_callback;
    private QuickTimeEventResult m_quickTimeEventResult;

    public async void PlaySkill(BattleCharacterView character, SkillSO skill, List<BattleCharacterView> targets, Action callback)
    {
        m_callback = callback;

        m_skill = skill;
        m_characterView = character;
        m_enemyCharacterView = targets[0];

        m_characterView.SkillAnimationTriggers.OnQTEStart.AddListener(HandleStartQTE);
        m_characterView.SkillAnimationTriggers.OnQTEEnd.AddListener(HandleEndQTE);
        m_characterView.SkillAnimationTriggers.OnAnimationStart.AddListener(HandleAnimationStart);
        m_characterView.SkillAnimationTriggers.OnAnimationEnd.AddListener(HandleAnimationEnd);
        m_characterView.SkillAnimationTriggers.OnDamageEvent.AddListener(HandleDamageEvent);

        var enemySpotPosition = m_enemyCharacterView.AttackerSpot;
        
        await m_characterView.MoveTo(enemySpotPosition, m_runSpeed);

        m_characterView.PlaySkillAnimation(skill.AnimationTrigger);
    }

    private void HandleStartQTE()
    {
        m_characterView.SetAnimatorSpeed(m_speedInQTE);

        m_quickTimeEventManager.StartEvents(m_qteDuration, m_qteAmount, m_qteInterval, HandleQTEResult);
    }

    private void HandleEndQTE()
    {
        m_characterView.SetAnimatorSpeed(1f);
    }

    private void HandleQTEResult(QuickTimeEventResult result)
    {
        m_quickTimeEventResult = result;
    }

    private void HandleAnimationStart()
    {
        m_camera.SetParent(m_characterView.AnimationCameraPivot, false);
        m_camera.localPosition = Vector3.zero;
        m_camera.localRotation = Quaternion.identity;
    }

    private async void HandleAnimationEnd()
    {
        m_camera.SetParent(m_mainCameraPivot, false);

        await Task.Delay(1);

        m_characterView.ResetPosition();

        m_characterView.SkillAnimationTriggers.OnQTEStart.RemoveListener(HandleStartQTE);
        m_characterView.SkillAnimationTriggers.OnQTEEnd.RemoveListener(HandleEndQTE);
        m_characterView.SkillAnimationTriggers.OnAnimationStart.RemoveListener(HandleAnimationStart);
        m_characterView.SkillAnimationTriggers.OnAnimationEnd.RemoveListener(HandleAnimationEnd);
        m_characterView.SkillAnimationTriggers.OnDamageEvent.RemoveListener(HandleDamageEvent);

        m_callback?.Invoke();
    }

    private void HandleDamageEvent()
    {
        if (m_quickTimeEventResult.Misses == m_quickTimeEventResult.EventAmount)
        {
            m_enemyCharacterView.Dodge();

            Instantiate(m_missVFX, m_enemyCharacterView.VFXSpot.position, m_enemyCharacterView.VFXSpot.rotation, m_vfxParent);
        }
        else
        {
            var damange = 50;

            m_enemyCharacterView.TakeDamage(damange);

            var instance = Instantiate(m_damageVFX, m_enemyCharacterView.VFXSpot.position, m_enemyCharacterView.VFXSpot.rotation, m_vfxParent);
            instance.SetContent(damange);
        }
    }
}
