using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyTurnState : BaseBattleState
{
    public EnemyTurnState(CombatManager combatManager) : base(combatManager) {}

    public override void Enter()
    {
        var battleCharacterView = CombatManager.CurrentCharacterTurn;

        CombatManager.BattleCameraManager.MoveCameraTo(battleCharacterView.ActionSelectionCameraSpot);

        CombatManager.EnableParry();

        HandleTurn();
    }

    public override void Exit()
    {
        CombatManager.DisableParry();
    }

    public override void UpdateState()
    {
        Debug.Log("EnemyTurnState UpdateState");
    }

    private async Task HandleTurn()
    {
        await Task.Delay(500);

        var character = CombatManager.CurrentCharacterTurn;
        var skill = character.BattleCharacter.Skills[0];
        var targets = new List<BattleCharacterView>(); 

        foreach (var battleCharacter in CombatManager.Context.PlayerBattleCharacters)
        {
            var view = CombatManager.GetCharacterView(battleCharacter);

            if (view.IsActive())
            {
                targets.Add(view);
                break;
            }
        }

        CombatManager.BattleSkillAnimationManager.PlaySkill(CombatManager, character, skill, targets, HandleSkillFinished);

    }

    private void HandleSkillFinished()
    {
        CombatManager.NextTurn();
    }
}
