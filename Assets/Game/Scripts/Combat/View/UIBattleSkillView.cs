using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleSkillView : UIItemController
{
    [SerializeField] private Button m_btnButton;
    [SerializeField] private TextMeshProUGUI m_txtSkillName;
    [SerializeField] private TextMeshProUGUI m_description;

    protected override void HandleInit(object obj)
    {
        var skill = obj as BaseSkillSO;

        m_txtSkillName.text = skill.SkillName;
        m_description.text = skill.Description;

        m_btnButton.onClick.AddListener(SelectItem);
    }
}
