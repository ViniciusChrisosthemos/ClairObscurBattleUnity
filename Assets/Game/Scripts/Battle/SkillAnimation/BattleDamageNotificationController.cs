using TMPro;
using UnityEngine;

public class BattleDamageNotificationController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_txtDamage;

    public void SetContent(int damage)
    {
        m_txtDamage.text = damage.ToString();
    }
}
