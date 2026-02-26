using System;
using System.Collections;
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

    [Header("References")]
    [SerializeField] private BattleCameraManager m_battleCameraManager;

    [Header("Paramters")]
    [SerializeField] private float m_timeSlowDurantionOnBattleEnd = 2f;
    [SerializeField] private float m_startTimeScale = 0.5f;

    [Header("VFX")]
    [SerializeField] private Transform m_vfxParent;
    [SerializeField] private GameObject m_missVFX;
    [SerializeField] private GameObject m_parryVFX;
    [SerializeField] private BattleDamageNotificationController m_damageVFX;

    private CombatManager m_combatManager;
    private SkillSO m_skill;
    private BattleCharacterView m_characterView;
    private BattleCharacterView m_enemyCharacterView;

    private Action m_callback;
    private QuickTimeEventResult m_quickTimeEventResult;

    public async void PlaySkill(CombatManager manager, BattleCharacterView character, SkillSO skill, List<BattleCharacterView> targets, Action callback)
    {
        m_callback = callback;

        m_combatManager = manager;
        m_skill = skill;
        m_characterView = character;
        m_enemyCharacterView = targets[0];

        BindAnimationTriggers();

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
        m_battleCameraManager.FollowTarget(m_characterView.AnimationCameraPivot);
    }

    private async void HandleAnimationEnd()
    {
        m_battleCameraManager.StopFollow();

        await Task.Delay(1);

        m_characterView.ResetPosition();

        UnbindAnimationTriggers();

        m_callback?.Invoke();
    }

    private void HandleDamageEvent()
    {
        if (m_quickTimeEventResult != null && m_quickTimeEventResult.Misses == m_quickTimeEventResult.EventAmount)
        {
            m_enemyCharacterView.Dodge();

            Instantiate(m_missVFX, m_enemyCharacterView.VFXSpot.position, m_enemyCharacterView.VFXSpot.rotation, m_vfxParent);
        }
        else
        {
            var damange = 50;

            var damageResult = m_enemyCharacterView.TakeDamage(damange);

            if (damageResult.HasParryIt)
            {
                var instance = Instantiate(m_parryVFX, m_enemyCharacterView.VFXSpot.position, m_enemyCharacterView.VFXSpot.rotation, m_vfxParent);
            }
            else
            {
                var instance = Instantiate(m_damageVFX, m_enemyCharacterView.VFXSpot.position, m_enemyCharacterView.VFXSpot.rotation, m_vfxParent);
                instance.SetContent(damange);
            }

            Debug.Log($"Executa Skill | Enemy is Alive? {!m_enemyCharacterView.BattleCharacter.IsAlive()}");
            if (!m_enemyCharacterView.BattleCharacter.IsAlive())
            {
                Debug.Log($"   Is Battle Over? {m_combatManager.HasEnd}");
                if (m_combatManager.HasEnd)
                {
                    UnbindAnimationTriggers();

                    m_battleCameraManager.StopFollow();

                    var actors = new List<BattleCharacterView>();

                    actors.Add(m_characterView);
                    actors.Add(m_enemyCharacterView);

                    StartCoroutine(SlowAnimationCoroutine(actors, m_timeSlowDurantionOnBattleEnd, null));
                    m_combatManager.EndBattle();
                }
            }
        }
    }

    private IEnumerator SlowAnimationCoroutine(List<BattleCharacterView> actors, float duration, Action callback)
    {
        var currentSpeedAnimation = m_startTimeScale;
        var accumTime = 0f;

        while (accumTime  < duration)
        {
            actors.ForEach(c => c.SetAnimatorSpeed(currentSpeedAnimation));

            currentSpeedAnimation = (1 - (accumTime / duration)) * m_startTimeScale;

            Debug.Log($"   {currentSpeedAnimation}");
            accumTime += Time.deltaTime;

            yield return null;
        }

        callback?.Invoke();
    }

    private void BindAnimationTriggers()
    {
        m_characterView.SkillAnimationTriggers.OnQTEStart.AddListener(HandleStartQTE);
        m_characterView.SkillAnimationTriggers.OnQTEEnd.AddListener(HandleEndQTE);
        m_characterView.SkillAnimationTriggers.OnAnimationStart.AddListener(HandleAnimationStart);
        m_characterView.SkillAnimationTriggers.OnAnimationEnd.AddListener(HandleAnimationEnd);
        m_characterView.SkillAnimationTriggers.OnDamageEvent.AddListener(HandleDamageEvent);
    }

    private void UnbindAnimationTriggers()
    {
        m_characterView.SkillAnimationTriggers.OnQTEStart.RemoveListener(HandleStartQTE);
        m_characterView.SkillAnimationTriggers.OnQTEEnd.RemoveListener(HandleEndQTE);
        m_characterView.SkillAnimationTriggers.OnAnimationStart.RemoveListener(HandleAnimationStart);
        m_characterView.SkillAnimationTriggers.OnAnimationEnd.RemoveListener(HandleAnimationEnd);
        m_characterView.SkillAnimationTriggers.OnDamageEvent.RemoveListener(HandleDamageEvent);
    }
}
