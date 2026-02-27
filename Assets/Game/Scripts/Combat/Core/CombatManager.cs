using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    [Header("Rerefences")]
    [SerializeField] private BattleEnvironmentManager m_battleEnvironmentManager;
    [SerializeField] private BattleCameraManager m_battleCameraManager;
    [SerializeField] private UIBattleHUDView m_battleHUDView;
    [SerializeField] private UIEndBattleView m_uiEndBattleView;
    [SerializeField] private TargetSeletionManager m_targetSelectionManager;
    [SerializeField] private BattleSkillAnimationManager m_battleSkillAnimationManager;

    [Header("Events")]
    public UnityEvent<BattleCharacter> OnTurnChanged;

    [Header("Debug")]
    [SerializeField] private bool m_debug;
    [SerializeField] private List<CharacterSO> m_playerCharacters;
    [SerializeField] private List<CharacterSO> m_enemyCharacters;

    private StateMachineController m_stateMachineController;
    private PlayerTurnState m_playerTurnState;
    private EnemyTurnState m_enemyTurnState;

    private void Awake()
    {
        m_stateMachineController = new StateMachineController();

        m_stateMachineController.Setup(new IdleState());

        m_playerTurnState = new PlayerTurnState(this);
        m_enemyTurnState = new EnemyTurnState(this);
    }

    private void Start()
    {
        if (m_debug)
        {
            StartCombat();
        }
    }

    public void StartCombat(List<CharacterRuntime> playerCharacters, List<CharacterRuntime> enemiesCharacters)
    {
        // Init Context
        var playerBattleCharacters = playerCharacters.Select(c => new BattleCharacter(c, true)).ToList();
        var enemiesBattleCharacters = enemiesCharacters.Select(c => new BattleCharacter(c, false)).ToList();
        
        Context = new CombatContext(playerBattleCharacters, enemiesBattleCharacters);

        // Init Turn Manager
        var allBattleCharacters = new List<BattleCharacter>(playerBattleCharacters);
        allBattleCharacters.AddRange(enemiesBattleCharacters);

        TurnManager = new TurnManager(allBattleCharacters);

        // Init BattleCharacterView
        m_battleEnvironmentManager.Setup(playerBattleCharacters, enemiesBattleCharacters);

        m_targetSelectionManager.Setup(m_battleEnvironmentManager.PlayerBattleViews, m_battleEnvironmentManager.EnemyBattleViews);

        m_battleHUDView.Setup(this);

        NextTurn();
    }

    public void NextTurn()
    {
        BattleCharacter character;

        do
        {
            character = TurnManager.Next();
        } while (!character.IsActive());

        if (character.IsPlayer)
        {
            m_stateMachineController.ChangeState(m_playerTurnState);
        }
        else
        {
            m_stateMachineController.ChangeState(m_enemyTurnState);
        }

        OnTurnChanged?.Invoke(character);
    }

    public void ExecuteSkill(BaseSkillSO skillSO, List<BattleCharacterView> views)
    {
        m_stateMachineController.ChangeState(new SkillExecutionState(this, CurrentCharacterTurn, skillSO, views, HandleSkillFinished));
    }

    private void HandleSkillFinished()
    {
        NextTurn();
    }

    public BattleCharacterView GetCharacterView(BattleCharacter character) => m_battleEnvironmentManager.GetCharacterView(character);

    public void EnableParry()
    {
        m_battleEnvironmentManager.PlayerBattleViews.ForEach(view => view.EnableParry());
    }

    public void DisableParry()
    {
        m_battleEnvironmentManager.PlayerBattleViews.ForEach ((view) => view.DisableParry());
    }

    public void EndBattle()
    {
        Debug.Log("CombatManager Endbattle");
        m_stateMachineController.ChangeState(new EndBattleState(this));
    }

    public BattleResult GetBattleResult()
    {
        var playerWin = Context.PlayerBattleCharacters.Any(c => c.IsAlive());

        return new BattleResult(playerWin);
    }

    public void StartCombat()
    {
        var playerCharacters = m_playerCharacters.Select(c => new CharacterRuntime(c)).ToList();
        var enemyCharacters = m_enemyCharacters.Select(c => new CharacterRuntime(c)).ToList();

        StartCombat(playerCharacters, enemyCharacters);
    }

    public BattleCharacterView CurrentCharacterTurn => GetCharacterView(TurnManager.Current);
    public BattleCameraManager BattleCameraManager => m_battleCameraManager;
    public UIBattleHUDView UIBattleHUDView => m_battleHUDView;
    public UIEndBattleView UIEndBattleView => m_uiEndBattleView;
    public TargetSeletionManager TargetSelectionManager => m_targetSelectionManager;
    public BattleSkillAnimationManager BattleSkillAnimationManager => m_battleSkillAnimationManager;
    public List<BattleCharacterView> PlayerViews => m_battleEnvironmentManager.PlayerBattleViews;

    public CombatContext Context { get; private set; }
    public TurnManager TurnManager { get; private set; }
    public bool HasEnd => !(Context.PlayerBattleCharacters.Any(c => c.IsAlive()) && Context.EnemyBattleCharacters.Any(c => c.IsAlive()));
}
