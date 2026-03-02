using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEndBattleView : MonoBehaviour
{
    [SerializeField] private GameObject m_view;
    [SerializeField] private TextMeshProUGUI m_txtResult;
    [SerializeField] private BattleCameraManager m_battleCameraManager;
    [SerializeField] private Button m_btnPlayAgain;

    [SerializeField] private Transform m_cameraPivot;
    [SerializeField] private float m_forwardDistance = 1f;
    [SerializeField] private float m_zoomDuration = 2f;

    private void Awake()
    {
        Close();
    }

    public void Setup(BattleResult battleResult, Action callback)
    {
        m_txtResult.text = battleResult.PlayerWin ? "Player Win" : "Player Lose";

        StartCoroutine(AnimteScreenCoroutine());

        m_btnPlayAgain.onClick.RemoveAllListeners();
        m_btnPlayAgain.onClick.AddListener(() => callback?.Invoke());
    }

    private IEnumerator AnimteScreenCoroutine()
    {
        var cameraPosition = m_battleCameraManager.GetCameraTransform();
        var targetCameraPosition = cameraPosition.position + cameraPosition.forward * m_forwardDistance;

        m_cameraPivot.position = targetCameraPosition;
        m_cameraPivot.rotation = cameraPosition.rotation;

        yield return m_battleCameraManager.AnimateCameraMovementCoroutine(m_cameraPivot, m_zoomDuration);


        m_view.SetActive(true);

        m_battleCameraManager.SetBlur(true);
    }

    public void Close()
    {
        m_view.SetActive(false);
        m_battleCameraManager.SetBlur(false);
    }
}
