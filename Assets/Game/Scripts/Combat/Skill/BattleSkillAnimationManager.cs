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
    [SerializeField] private VFXAnimationHelper m_characterDieVFX;
    [SerializeField] private VFXAnimationHelper m_hitVFX;
    [SerializeField] private VFXAnimationHelper m_hitParryVFX;

    private CombatManager m_combatManager;
    private BaseSkillSO m_skill;
    private BattleCharacterView m_characterView;
    private BattleCharacterView m_enemyCharacterView;

    private Action m_callback;
    private QuickTimeEventResult m_quickTimeEventResult;

    public async void PlaySkill(CombatManager manager, BattleCharacterView character, BaseSkillSO skill, List<BattleCharacterView> targets, Action callback)
    {
        m_callback = callback;

        m_combatManager = manager;
        m_skill = skill;
        m_characterView = character;
        m_enemyCharacterView = targets[0];
        m_quickTimeEventResult = new QuickTimeEventResult();

        BindAnimationTriggers();

        var enemySpotPosition = m_enemyCharacterView.AttackerSpot;
        
        await m_characterView.MoveTo(enemySpotPosition, m_runSpeed);

        m_characterView.PlaySkillAnimation(skill.AnimationTrigger);
    }

    private void HandleStartQTE()
    {
        m_characterView.SetAnimationSpeed(m_speedInQTE);

        m_quickTimeEventManager.StartEvents(m_qteDuration, m_qteAmount, m_qteInterval, HandleQTEResult);
    }

    private void HandleEndQTE()
    {
        m_characterView.SetAnimationSpeed(1f);
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
        VFXAnimationHelper hitInstance = null;

        if (m_enemyCharacterView.HasParryIt)
        {
            var instance = Instantiate(m_parryVFX, m_enemyCharacterView.VFXSpot.position, m_enemyCharacterView.VFXSpot.rotation, m_vfxParent);
            hitInstance = Instantiate(m_hitParryVFX, m_characterView.HitSpot.position, m_characterView.HitSpot.rotation, m_vfxParent);
        }
        else
        {
            var targets = new List<BattleCharacter>() { m_enemyCharacterView.BattleCharacter };
            var skillResult = m_skill.Execute(m_characterView.BattleCharacter, targets, m_quickTimeEventResult);

            if (skillResult.HasMissed)
            {
                m_enemyCharacterView.Dodge();

                Instantiate(m_missVFX, m_enemyCharacterView.VFXSpot.position, m_enemyCharacterView.VFXSpot.rotation, m_vfxParent);
            }
            else
            {
                var instance = Instantiate(m_damageVFX, m_enemyCharacterView.VFXSpot.position, m_enemyCharacterView.VFXSpot.rotation, m_vfxParent);
                instance.SetContent(skillResult.DamageDone);

                hitInstance = Instantiate(m_hitVFX, m_characterView.HitSpot.position, m_characterView.HitSpot.rotation, m_vfxParent);
            }

            if (!m_enemyCharacterView.BattleCharacter.IsAlive())
            {
                var dieVfx = Instantiate(m_characterDieVFX, m_enemyCharacterView.VFXSpot.position, m_enemyCharacterView.VFXSpot.rotation, m_vfxParent);

                if (m_combatManager.HasEnd)
                {
                    UnbindAnimationTriggers();

                    m_battleCameraManager.StopFollow();

                    var actors = new List<IMutableAnimationSpeed>();

                    actors.Add(m_characterView);
                    actors.Add(m_enemyCharacterView);
                    actors.Add(dieVfx);
                    dieVfx.SetAutoPlay(false);

                    if (hitInstance != null)
                    {
                        hitInstance.SetAutoPlay(false);
                        actors.Add(hitInstance);
                    }

                    StartCoroutine(SlowAnimationCoroutine(actors, m_timeSlowDurantionOnBattleEnd, null));
                    m_combatManager.EndBattle();
                }
            }
        }
    }

    private IEnumerator SlowAnimationCoroutine(List<IMutableAnimationSpeed> actors, float duration, Action callback)
    {
        var currentSpeedAnimation = m_startTimeScale;
        var accumTime = 0f;

        yield return null;

        while (accumTime  < duration)
        {
            actors.ForEach(c => c.SetAnimationSpeed(currentSpeedAnimation));
            TimeManager.Instance.SetTimeScale(currentSpeedAnimation);

            currentSpeedAnimation = (1 - (accumTime / duration)) * m_startTimeScale;

            accumTime += Time.deltaTime;

            yield return null;
        }

        callback?.Invoke();

        TimeManager.Instance.SetTimeScale(1);
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
