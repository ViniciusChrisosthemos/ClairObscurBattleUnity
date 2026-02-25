using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetSeletionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_targetSelectionCameraPivot;
    [SerializeField] private Transform m_targetIconParent;
    [SerializeField] private GameObject m_targetIconPrefab;

    [Header("Events")]
    public UnityEvent<List<BattleCharacterView>> OnTargetSelected;

    private List<BattleCharacterView> m_enemiesViews;
    private List<BattleCharacterView> m_playerViews;

    private bool m_isSelectingTargets = false;
    private bool m_isSingeTarget = false;

    private GameObject m_singleTargetInstace;

    public void Setup(List<BattleCharacterView> playerViews, List<BattleCharacterView> enemiesViews)
    {
        m_playerViews = playerViews;
        m_enemiesViews = enemiesViews;

        foreach (var view in enemiesViews)
        {
            view.OnCharacterSelected.AddListener(HandleCharacterSelected);
            view.OnCharacterHoverEnter.AddListener(HandleCharacterHoverEnter);
            view.OnCharacterHoverExit.AddListener(HandleCharacterHoverExit);
        }

        m_singleTargetInstace = null;
        m_targetIconParent.ClearChilds();
    }

    public void SetSingleTargetSelection()
    {
        m_isSingeTarget = true;
        m_isSelectingTargets = true;

        m_singleTargetInstace = Instantiate(m_targetIconPrefab, m_targetIconParent);
        m_singleTargetInstace.SetActive(false);
    }

    public void SetAlltargetSelection()
    {
        m_isSingeTarget = false;
        m_isSelectingTargets = true;

        foreach (var view in m_enemiesViews)
        {
            var targetIconInstance = Instantiate(m_targetIconPrefab, m_targetIconParent);
            targetIconInstance.transform.position = view.SelectionSpot.position;
        }
    }

    private void HandleCharacterSelected(BattleCharacterView characterView)
    {
        if (!m_isSelectingTargets) return;

        var targets = new List<BattleCharacterView>();

        if (m_isSingeTarget)
        {
            targets.Add(characterView);
        }
        else
        {
            targets.AddRange(m_enemiesViews);
        }

        OnTargetSelected?.Invoke(targets);

        DisableSelection();
    }

    private void HandleCharacterHoverEnter(BattleCharacterView characterView)
    {
        if (!m_isSelectingTargets) return;

        if (m_isSingeTarget)
        {
            m_singleTargetInstace.SetActive(true);
            m_singleTargetInstace.transform.position = characterView.SelectionSpot.position;
        }
    }

    private void HandleCharacterHoverExit(BattleCharacterView characterSpot)
    {
        if (!m_isSelectingTargets) return;

        if (m_isSingeTarget)
        {
            m_singleTargetInstace.SetActive(false);
        }
    }

    public void DisableSelection()
    {
        m_isSelectingTargets = false;
        m_targetIconParent.ClearChilds();
    }

    public Transform TargetSelectionCameraPivot => m_targetSelectionCameraPivot;
}
