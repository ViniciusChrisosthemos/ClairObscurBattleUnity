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
        var characterSpot = (CharacterSpot)obj;

        _imgCharacterIcon.sprite = characterSpot.CharacterRuntime.CharacterSO.CharacterIcon;

        _imgBackground.color = characterSpot.IsPlayerCharacter ? _playerBackgroundColor : _enemyBackgroundColor;
    }
}
