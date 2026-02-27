using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBattleHUDView : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private BattleCameraManager m_battleCameraManager;

    [Header("HUD")]
    [SerializeField] private UIListDisplay m_playerCharacterListDisplay;
    [SerializeField] private UIListDisplay m_timelineListDisplay;

    [Header("Player Actions References")]
    [SerializeField] private GameObject m_actionView;
    [SerializeField] private Transform m_worldCanvasView;
    [SerializeField] private Button m_btnRollDices;
    [SerializeField] private Button m_passTurn;

    [Header("Skill References")]
    [SerializeField] private GameObject m_skillView;
    [SerializeField] private UIListDisplay m_skillListDisplay;

    private BattleCharacterView m_battleCharacterView;
    private Action m_onPassTurnCallback;
    private Action<BaseSkillSO> m_onSkillSelectedCallback;

    private CombatManager m_combatManager;

    private void Awake()
    {
        m_btnRollDices.onClick.AddListener(HandleRollDiceEvent);
        m_passTurn.onClick.AddListener(HandlePassTurn);
    }

    public void Setup(CombatManager combatManager)
    {
        m_combatManager = combatManager;

        m_playerCharacterListDisplay.SetItems(combatManager.PlayerViews, null);

        combatManager.OnTurnChanged.AddListener(HandleTurnChanged);
    }

    private void HandleTurnChanged(BattleCharacter character)
    {
        var queue = m_combatManager.TurnManager.GetCharacterQueue();
        queue.Insert(0, character);

        m_timelineListDisplay.SetItems(queue, null);
    }

    private void HandleRollDiceEvent()
    {
        //OnRollDiceEvent?.Invoke();
        
        m_actionView.SetActive(false);
        m_skillView.SetActive(true);

        m_battleCameraManager.MoveCameraTo(m_battleCharacterView.SkillSelectionCameraSpot);
        SetLocation(m_battleCharacterView.SkillSelectionCanvasSpot);

        InputManager.Instance.OnEscapePressed.AddListener(HandleEscapeInputPressed);
    }

    private void HandlePassTurn()
    {
        m_onPassTurnCallback?.Invoke();
    }

    public void SetCharacter(BattleCharacterView characterView, Action<BaseSkillSO> skillSelectedCallback, Action passTurnCallback)
    {
        m_battleCharacterView = characterView;

        m_onPassTurnCallback = passTurnCallback;
        m_onSkillSelectedCallback = skillSelectedCallback;

        SetLocation(characterView.ActionSelectionCanvasSpot);

        m_skillListDisplay.SetItems(characterView.BattleCharacter.Skills, HandleSkillSelected);

        m_actionView.SetActive(true);
        m_skillView.SetActive(false);
    }

    private void HandleSkillSelected(UIItemController controller)
    {
        var skill = controller.GetItem<BaseSkillSO>();

        m_onSkillSelectedCallback?.Invoke(skill);
    }

    private void SetLocation(Transform location)
    {
        m_worldCanvasView.position = location.position;
        m_worldCanvasView.rotation = location.rotation;
    }

    private void HandleEscapeInputPressed()
    {
        m_actionView.SetActive(false);
        m_skillView.SetActive(true);

        InputManager.Instance.OnEscapePressed.RemoveListener(HandleEscapeInputPressed);
    }

    public void Deactivate()
    {
        m_actionView.SetActive(false);
        m_skillView.SetActive(false);
    }
}
