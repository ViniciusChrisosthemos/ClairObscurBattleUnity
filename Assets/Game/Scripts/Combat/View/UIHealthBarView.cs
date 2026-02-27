using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarView : MonoBehaviour
{
    [SerializeField] private Slider m_slider;
    [SerializeField] private BattleCharacterView m_battleCharacterView;

    private void Awake()
    {
        m_battleCharacterView.OnTakeDamage.AddListener(UpdateHP);
    }

    private void UpdateHP(int damage)
    {
        m_slider.value = m_battleCharacterView.BattleCharacter.CurrentHP / (float)m_battleCharacterView.BattleCharacter.MaxpHP;
    }
}
