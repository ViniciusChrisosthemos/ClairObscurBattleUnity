using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIHUDView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BattleController _battleController;

    [Header("UI References")]
    [SerializeField] private UIListDisplay _characterListDisplay;
    [SerializeField] private UIListDisplay _timelineListDisplay;
    [SerializeField] private UIBattleCanvasView _uiBattleCanvasView;

    private void Start()
    {
        _battleController.OnCharacterTurnChanged.AddListener(HandleCharacterTurnChanged);

        var playerCharacters = _battleController.GetPlayerCharacterRuntime();
        
        Init(playerCharacters);
    }

    private void HandleCharacterTurnChanged(CharacterSpot currentCharacter)
    {
        var characters = _battleController.GetCharacterSpotsOrder();
        characters.Insert(0, currentCharacter);

        _timelineListDisplay.SetItems(characters, null);
    }

    public void Init(List<CharacterRuntime> playerCharacters)
    {
        _characterListDisplay.SetItems(playerCharacters, null);
    }
}
