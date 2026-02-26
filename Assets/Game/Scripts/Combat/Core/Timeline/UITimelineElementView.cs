using UnityEngine;
using UnityEngine.UI;

public class UITimelineElementView : UIItemController
{
    [SerializeField] private Image _imgCharacterIcon;
    [SerializeField] private Image _imgBackground;

    [Header("Parameters")]
    [SerializeField] private Color _playerBackgroundColor;
    [SerializeField] private Color _enemyBackgroundColor;

    protected override void HandleInit(object obj)
    {
        var characterView = (BattleCharacterView)obj;

        _imgCharacterIcon.sprite = characterView.BattleCharacter.CharacterRuntime.BaseCharacterData.CharacterIcon;

        _imgBackground.color = characterView.BattleCharacter.IsPlayer ? _playerBackgroundColor : _enemyBackgroundColor;
    }
}
