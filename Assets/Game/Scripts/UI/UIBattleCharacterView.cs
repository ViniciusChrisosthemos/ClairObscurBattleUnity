using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleCharacterView : UIItemController
{
    [SerializeField] private Image _imgCharacterFace;
    [SerializeField] private Slider _sliderHPBar;
    [SerializeField] private TextMeshProUGUI _txtHP;

    private CharacterRuntime _chraracterRuntime;

    protected override void HandleInit(object obj)
    {
        _chraracterRuntime = obj as CharacterRuntime;

        _imgCharacterFace.sprite = _chraracterRuntime.CharacterSO.CharacterIcon;
        _sliderHPBar.maxValue = _chraracterRuntime.MaxHP;

        UpdateHP();
    }

    public void UpdateHP()
    {
        _sliderHPBar.value = _chraracterRuntime.CurrentHP;
        _txtHP.text = $"{_chraracterRuntime.CurrentHP} / {_chraracterRuntime.MaxHP}";
    }
}
