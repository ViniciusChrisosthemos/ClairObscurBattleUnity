using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleCharacterView : UIItemController
{
    [SerializeField] private Image m_imgCharacterFace;
    [SerializeField] private Slider m_sliderHPBar;
    [SerializeField] private TextMeshProUGUI m_txtHP;

    [Header("Character Death")]
    [SerializeField] private GameObject m_deadOverlay;
    [SerializeField] private Color m_colorOnDeath;

    private BattleCharacterView m_characterView;

    protected override void HandleInit(object obj)
    {
        m_characterView = obj as BattleCharacterView;

        m_imgCharacterFace.sprite = m_characterView.BattleCharacter.CharacterRuntime.BaseCharacterData.CharacterIcon;
        m_sliderHPBar.maxValue = m_characterView.BattleCharacter.MaxpHP;

        UpdateHP();

        m_deadOverlay.SetActive(false);

        m_characterView.OnTakeDamage.AddListener((damage) => UpdateHP());
    }

    public void UpdateHP()
    {
        m_sliderHPBar.value = m_characterView.BattleCharacter.CurrentHP;
        m_txtHP.text = $"{m_characterView.BattleCharacter.CurrentHP} / {m_characterView.BattleCharacter.MaxpHP}";

        if (!m_characterView.IsActive())
        {
            SetCharacterDeath();
        }
    }

    private void SetCharacterDeath()
    {
        m_imgCharacterFace.color = m_colorOnDeath;
        m_txtHP.color = m_colorOnDeath;
        m_deadOverlay.SetActive(true);
    }
}
