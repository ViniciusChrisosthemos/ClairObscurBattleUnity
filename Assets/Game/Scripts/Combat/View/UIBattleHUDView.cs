using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBattleHUDView : MonoBehaviour
{
    [Header("Player Actions References")]
    [SerializeField] private Button m_btnRollDices;
    [SerializeField] private Button m_passTurn;

    public UnityEvent OnRollDiceEvent;
    public UnityEvent OnPassTurnEvent;

    private void Awake()
    {
        m_btnRollDices.onClick.AddListener(() => OnRollDiceEvent?.Invoke());
        m_passTurn.onClick.AddListener(() => OnPassTurnEvent?.Invoke());
    }

    public void SetPosition(Transform location)
    {
        transform.position = location.position;
        transform.rotation = location.rotation;
    }
}
