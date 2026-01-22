using TMPro;
using UnityEngine;

public class UIBattleCanvasSkillButtonView : UIItemController
{
    [SerializeField] private TextMeshProUGUI _txtSkillName;
    [SerializeField] private TextMeshProUGUI _txtSkillDescription;

    protected override void HandleInit(object obj)
    {
        var skillSO = obj as SkillSO;

        _txtSkillName.text = skillSO.SkillName;
        _txtSkillDescription.text = skillSO.Description;
    }
}
