using UnityEngine;

public class BattleResult
{
    public bool PlayerWin {  get; private set; }

    public BattleResult(bool playerWin)
    {
        PlayerWin = playerWin;
    }
}
