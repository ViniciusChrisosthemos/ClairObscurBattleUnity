using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using static QuickTimeEventElementController;

public class BattleSkillAnimationController : MonoBehaviour
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
    private CharacterSpot m_characterSpot;
    private CharacterSpot m_enemyCharacterSpot;

    private Action m_callback;
    private List<QuickTimeEventResultType> m_quickTimeEventResult;

    public void PlaySkill(CharacterSpot character, SkillSO skill, List<CharacterSpot> targets, Action callback)
    {
        m_callback = callback;

        m_skill = skill;
        m_characterSpot = character;
        m_enemyCharacterSpot = targets[0];

        m_characterSpot.SkillAnimationTriggers.OnQTEStart.AddListener(HandleStartQTE);
        m_characterSpot.SkillAnimationTriggers.OnQTEEnd.AddListener(HandleEndQTE);
        m_characterSpot.SkillAnimationTriggers.OnAnimationStart.AddListener(HandleAnimationStart);
        m_characterSpot.SkillAnimationTriggers.OnAnimationEnd.AddListener(HandleAnimationEnd);
        m_characterSpot.SkillAnimationTriggers.OnDamageEvent.AddListener(HandleDamageEvent);

        var enemySpotPosition = m_enemyCharacterSpot.AttackerSpot;
        m_characterSpot.RunToPosition(enemySpotPosition, m_runSpeed, () =>
        {
            m_characterSpot.Animator.SetTrigger(skill.AnimationTrigger);
        });
    }

    private void HandleStartQTE()
    {
        m_characterSpot.Animator.speed = m_speedInQTE;

        m_quickTimeEventManager.StartEvents(m_qteDuration, m_qteAmount, m_qteInterval, (result) => m_quickTimeEventResult = result);
    }

    private void HandleEndQTE()
    {
        m_characterSpot.Animator.speed = 1;
    }

    private void HandleAnimationStart()
    {
        m_camera.SetParent(m_characterSpot.AnimatorCameraPivot, false);
        m_camera.localPosition = Vector3.zero;
        m_camera.localRotation = Quaternion.identity;
    }

    private async void HandleAnimationEnd()
    {
        m_camera.SetParent(m_mainCameraPivot, false);

        await Task.Delay(1);

        m_characterSpot.ResetPosition();

        m_characterSpot.SkillAnimationTriggers.OnQTEStart.RemoveListener(HandleStartQTE);
        m_characterSpot.SkillAnimationTriggers.OnQTEEnd.RemoveListener(HandleEndQTE);
        m_characterSpot.SkillAnimationTriggers.OnAnimationStart.RemoveListener(HandleAnimationStart);
        m_characterSpot.SkillAnimationTriggers.OnAnimationEnd.RemoveListener(HandleAnimationEnd);
        m_characterSpot.SkillAnimationTriggers.OnDamageEvent.RemoveListener(HandleDamageEvent);

        m_callback?.Invoke();
    }

    private void HandleDamageEvent()
    {
        if (m_quickTimeEventResult.All(result => result == QuickTimeEventResultType.Miss))
        {
            m_enemyCharacterSpot.DodgeAttack();

            Instantiate(m_missVFX, m_enemyCharacterSpot.VFXSpot.position, m_enemyCharacterSpot.VFXSpot.rotation, m_vfxParent);
        }
        else
        {
            var damange = 50;

            m_enemyCharacterSpot.ApplyDamage(damange);

            var instance = Instantiate(m_damageVFX, m_enemyCharacterSpot.VFXSpot.position, m_enemyCharacterSpot.VFXSpot.rotation, m_vfxParent);
            instance.SetContent(damange);
        }
    }
}
