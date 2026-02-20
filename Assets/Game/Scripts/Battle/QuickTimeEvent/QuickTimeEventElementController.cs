using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuickTimeEventElementController : MonoBehaviour
{
    [SerializeField] private GameObject m_view;
    [SerializeField] private TextMeshProUGUI m_txtKey;
    [SerializeField] private Image m_imgPerfectTimeArea;
    [SerializeField] private Image m_imgGoodArea;
    [SerializeField] private Transform m_handle;

    [Header("Parameters")]
    [SerializeField] private float m_duration = 1f;
    [SerializeField] private float m_perfectTime = 0.15f;
    [SerializeField] private float m_goodTime = 0.3f;

    [Header("Animator")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private string m_eventCompletedPerfectString = "Perfect";
    [SerializeField] private string m_eventGoodString = "Good";
    [SerializeField] private string m_eventFailedString = "Failed";

    [Header("Destroy Safely Parameters")]
    [SerializeField] private float m_timeToDestroy = 2f;

    private Key m_targetKey;
    private bool m_isPlaying = false;
    private float m_startTime = 0f;

    public enum QuickTimeEventResultType
    {
        Miss,
        Regular,
        Perfect
    }

    private void Awake()
    {
    }

    public void Setup(float qteTime, Key key)
    {
        m_duration = qteTime;
        m_targetKey = key;

        m_txtKey.text = m_targetKey.ToString();
        m_imgPerfectTimeArea.fillAmount = m_perfectTime / m_duration;
        m_imgGoodArea.fillAmount = m_goodTime / m_duration;

        m_handle.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (m_isPlaying)
        {
            HandleEventAnimation();
        }
    }

    public QuickTimeEventResultType HandlePlayerInput(Key keyGiven)
    {
        m_isPlaying = false;

        if (keyGiven == m_targetKey)
        {
            return HandleInputGivenInTime();
        }
        else
        {
            return HandleEventFailed();
        }
    }

    private QuickTimeEventResultType HandleInputGivenInTime()
    {
        var normalizedRemainingTime = 1 - ((Time.time - m_startTime) / m_duration);

        if (normalizedRemainingTime < m_perfectTime)
        {
            return HandleEventCompletedPerfect();
        }
        else if (normalizedRemainingTime < m_goodTime)
        {
            return HandleEventCompletedGood();
        }
        else
        {
            return HandleEventFailed();
        }
    }

    private QuickTimeEventResultType HandleEventFailed()
    {
        Debug.Log("Event Failed");

        m_animator.SetTrigger(m_eventFailedString);
        
        HasFinished = true;

        return QuickTimeEventResultType.Miss;
    }

    private QuickTimeEventResultType HandleEventCompletedPerfect()
    {
        Debug.Log("Event Completed Perfect");

        m_animator.SetTrigger(m_eventCompletedPerfectString);

        return QuickTimeEventResultType.Perfect;
    }

    private QuickTimeEventResultType HandleEventCompletedGood()
    {
        Debug.Log("Event Completed Good");

        m_animator.SetTrigger(m_eventGoodString);

        return QuickTimeEventResultType.Regular;
    }

    private void HandleEventAnimation()
    {
        var timeSinceStart = Time.time - m_startTime;

        if (timeSinceStart > m_duration)
        {
            m_isPlaying = false;

            HandleEventFailed();
        }
        else
        {
            var normalizedProgress = timeSinceStart / m_duration;

            var angle = -360 * normalizedProgress;

            m_handle.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void StartEvent()
    {
        m_isPlaying = true;
        m_startTime = Time.time;

        HasFinished = false;
    }

    private void DestroySafely()
    {
        m_view.SetActive(false);

        Destroy(gameObject, m_timeToDestroy);
    }

    public bool HasFinished { get; private set; }
}
