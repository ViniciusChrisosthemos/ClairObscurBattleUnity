using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIBattleCanvasView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BattleController _battleController;
    [SerializeField] private BattleCameraController _battleCameraController;

    [Header("UI References")]
    [SerializeField] private GameObject _actionSelectionView;
    [SerializeField] private GameObject _skillSelectionView;
    [SerializeField] private Transform _defaultTargetCameraPosition;

    [Header("UI References / Actions Selection View")]
    [SerializeField] private Button _btnPassTurn;
    [SerializeField] private Button _btnRollDices;

    [Header("UI References / Skill Selection View")]
    [SerializeField] private UIListDisplay _skillsListDisplay;

    [Header("UI References / target Selection")]
    [SerializeField] private TargetSelectionController _targetSelectionController;

    private SelectionState _currentState;
    private CharacterSpot _currentCharacter;
    private SkillSO _currentSkill;

    private List<CharacterSpot> _enemiesSpots;

    private enum SelectionState
    {
        ActionSelection,
        SkillSelection,
        TargetSelection,
    }

    private void Awake()
    {
        _btnPassTurn.onClick.AddListener(PassTurn);
        _btnRollDices.onClick.AddListener(HandleSkillSelection);

        _battleController.OnCharacterTurnChanged.AddListener(HandleCharacterTurnChanged);
    }

    private void Start()
    {
        var characterSpots = _battleController.GetCharacterSpotsOrder();

        _targetSelectionController.Init(characterSpots);
        _enemiesSpots = characterSpots.FindAll(c => !c.IsPlayerCharacter);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleEscPressed();
        }
    }

    private void HandleEscPressed()
    {
        switch (_currentState)
        {
            case SelectionState.ActionSelection: break;
            case SelectionState.SkillSelection: HandleCharacterTurnChanged(_currentCharacter); break;
            case SelectionState.TargetSelection: HandleSkillSelection(); break;
        }
    }

    private void HandleCharacterTurnChanged(CharacterSpot characterSpot)
    {
        _currentCharacter = characterSpot;

        ShowActionSelectionView();

        _battleCameraController.MoveCameraTo(_currentCharacter.ActionSelectionCameraSpot);

        if (characterSpot.IsPlayerCharacter)
        {
            _actionSelectionView.SetActive(true);

            transform.position = _currentCharacter.ActionSelectionCanvasSpot.position;
            transform.rotation = _currentCharacter.ActionSelectionCanvasSpot.rotation;

            _currentState = SelectionState.ActionSelection;
        }
        else
        {
            DisableUI();

            HandleEnemyTurn();
        }
    }

    private async void HandleEnemyTurn()
    {
        await Task.Delay(1000);

        _battleController.PassTurn();
    }

    private void HandleSkillSelection()
    {
        ShowSkillSelectionView();

        _battleCameraController.MoveCameraTo(_currentCharacter.SkillSelectionCameraSpot);

        transform.position = _currentCharacter.SkillSelectionCanvasSpot.position;
        transform.rotation = _currentCharacter.SkillSelectionCanvasSpot.rotation;

        _skillsListDisplay.SetItems(_currentCharacter.CharacterRuntime.CharacterSO.Skills, HandleTargetSelection);

        _currentState = SelectionState.SkillSelection;
        _targetSelectionController.DisableSelection();
    }

    private void HandleTargetSelection(UIItemController itemController)
    {
        _currentSkill = itemController.GetItem<SkillSO>();

        ShowTargetSelectionView();

        _battleCameraController.MoveCameraTo(_defaultTargetCameraPosition);

        if (_currentSkill.TargetType == SkillSO.SkillTargetType.SingleEnemy)
        {
            _targetSelectionController.SetSingleTargetSelection(HandleTargetSelected);
        }
        else
        {
            _targetSelectionController.SetAlltargetSelection(HandleTargetSelected);
        }

        _currentState = SelectionState.TargetSelection;
    }

    private async void HandleTargetSelected(List<CharacterSpot> characterSeleced)
    {
        _battleController.PlayAction(_currentSkill, characterSeleced);
        
        _currentState = SelectionState.ActionSelection;

        await HandleSkillApplied();
    }

    private async Task HandleSkillApplied()
    {
        _enemiesSpots.ForEach(character => character.UpdateHP());

        await Task.Delay(1000);

        _battleController.PassTurn();
    }

    public void ShowActionSelectionView()
    {
        _actionSelectionView.SetActive(true);
        _skillSelectionView.SetActive(false);
    }

    public void ShowSkillSelectionView()
    {
        _actionSelectionView.SetActive(false);
        _skillSelectionView.SetActive(true);
    }

    public void ShowTargetSelectionView()
    {
        _actionSelectionView.SetActive(false);
        _skillSelectionView.SetActive(false);
    }

    private void DisableUI()
    {
        _actionSelectionView.SetActive(false);
        _skillSelectionView.SetActive(false);
    }

    public void PassTurn()
    {
        _battleController.PassTurn();
    }
}
