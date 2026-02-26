using System.Collections;
using TMPro;
using UnityEngine;

public class UIEndBattleView : MonoBehaviour
{
    [SerializeField] private GameObject m_view;
    [SerializeField] private TextMeshProUGUI m_txtResult;
    [SerializeField] private BattleCameraManager m_battleCameraManager;

    [SerializeField] private Transform m_cameraPivot;
    [SerializeField] private float m_forwardDistance = 1f;
    [SerializeField] private float m_zoomDuration = 2f;

    private void Awake()
    {
        m_view.SetActive(false);
    }

    public void Setup(BattleResult battleResult)
    {
        m_txtResult.text = battleResult.PlayerWin ? "Player Win" : "Player Lose";

        StartCoroutine(AnimteScreenCoroutine());
    }

    private IEnumerator AnimteScreenCoroutine()
    {
        var cameraPosition = m_battleCameraManager.GetCameraTransform();
        var targetCameraPosition = cameraPosition.position + cameraPosition.forward * m_forwardDistance;

        m_cameraPivot.position = targetCameraPosition;
        m_cameraPivot.rotation = cameraPosition.rotation;

        yield return m_battleCameraManager.AnimateCameraMovementCoroutine(m_cameraPivot, m_zoomDuration);


        m_view.SetActive(true);
    }
}
