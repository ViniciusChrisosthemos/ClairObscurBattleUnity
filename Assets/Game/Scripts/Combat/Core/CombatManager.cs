using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("States")]
    [SerializeField] private IdleState m_idleState;
    [SerializeField] private PlayerTurnState m_playerTurnState;

    private StateMachineController m_stateMachineController;
    private TimelineController<BattleCharacterView> m_timelineController;

    private List<BattleCharacter> m_playerBattleCharacters;
    private List<BattleCharacter> m_enemiesBattleCharacters;

    public void StartCombat(List<CharacterRuntime> playerCharacters, List<CharacterRuntime> enemiesCharacters)
    {
        m_playerBattleCharacters = playerCharacters.Select(c => new BattleCharacter(c, true)).ToList();
        m_enemiesBattleCharacters = enemiesCharacters.Select(c => new BattleCharacter(c, false)).ToList();
        

    }

    public List<BattleCharacterView> PlayerCharacters { get; private set; }
    public List<BattleCharacterView> EnemiesCharacters { get; private set; }
    public BattleCharacterView CurrentCharacterTurn { get; private set; }
}
