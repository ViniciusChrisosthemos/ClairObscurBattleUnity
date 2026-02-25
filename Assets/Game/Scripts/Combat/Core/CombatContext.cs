using System.Collections.Generic;

public class CombatContext
{
    public List<BattleCharacter> PlayerBattleCharacters { get; private set; }
    
    public List<BattleCharacter> EnemyBattleCharacters { get; private set; }

    public CombatContext(List<BattleCharacter> playerBattleCharacters, List<BattleCharacter> enemyBattleCharacters)
    {
        PlayerBattleCharacters = playerBattleCharacters;
        EnemyBattleCharacters = enemyBattleCharacters;
    }
}
