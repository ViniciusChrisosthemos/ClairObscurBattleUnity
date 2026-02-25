using NUnit.Framework;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    [Header("Rerefences")]
    [SerializeField] private BattleEnvironmentManager m_battleEnvironmentManager;
    [SerializeField] private BattleCameraManager m_battleCameraManager;
    [SerializeField] private UIBattleHUDView m_battleHUDView;
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
            var playerCharacters = m_playerCharacters.Select(c => new CharacterRuntime(c)).ToList();
            var enemyCharacters = m_enemyCharacters.Select(c => new CharacterRuntime(c)).ToList();

            StartCombat(playerCharacters, enemyCharacters);
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

        NextTurn();
    }

    public void NextTurn()
    {
        var character = TurnManager.Next();

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

    public void ExecuteSkill(SkillSO skillSO, List<BattleCharacterView> views)
    {
        m_stateMachineController.ChangeState(new SkillExecutionState(this, CurrentCharacterTurn, skillSO, views, HandleSkillFinished));
    }

    private void HandleSkillFinished()
    {
        NextTurn();
    }

    public BattleCharacterView GetCharacterView(BattleCharacter character) => m_battleEnvironmentManager.GetCharacterView(character);

    public BattleCharacterView CurrentCharacterTurn => GetCharacterView(TurnManager.Current);

    public BattleCameraManager BattleCameraManager => m_battleCameraManager;

    public UIBattleHUDView UIBattleHUDView => m_battleHUDView;

    public TargetSeletionManager TargetSelectionManager => m_targetSelectionManager;

    public BattleSkillAnimationManager BattleSkillAnimationManager => m_battleSkillAnimationManager;

    public CombatContext Context { get; private set; }
    
    public TurnManager TurnManager { get; private set; }
}
