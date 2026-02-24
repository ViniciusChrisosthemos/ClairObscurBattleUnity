using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class QuickTimeEventManager : MonoBehaviour
{
    [SerializeField] private List<Key> m_possibleKeyCode;
    [SerializeField] private Transform m_parentView;
    [SerializeField] private List<Transform> m_locations;
    [SerializeField] private QuickTimeEventElementController m_quickTimeEventPrefab;

    private Key m_keyPressed;
    private Queue<QuickTimeEventElementController> m_quickTimeEvents;
    private List<QuickTimeEventElementController.QuickTimeEventResultType> m_results;
    private Action<QuickTimeEventResult> m_callback;

    private void Awake()
    {
        m_keyPressed = Key.None;
        m_quickTimeEvents = new Queue<QuickTimeEventElementController>();
    }

    private void Update()
    {
        if (m_quickTimeEvents.Count == 0) return;

        m_keyPressed = GetKeyPressed(m_possibleKeyCode);

        if (m_keyPressed != Key.None)
        {
            var qtEvent = m_quickTimeEvents.Dequeue();

            var result = qtEvent.HandlePlayerInput(m_keyPressed);

            m_results.Add(result);

            CheckEvents();
        }
        else
        {
            if (m_quickTimeEvents.First().HasFinished)
            {
                m_quickTimeEvents.Dequeue();

                m_results.Add(QuickTimeEventElementController.QuickTimeEventResultType.Miss);

                CheckEvents();
            }
        }
    }

    public void StartEvents(float qteTime, int amount, float interval, Action<QuickTimeEventResult> callback)
    {
        m_callback = callback;
        m_quickTimeEvents = new Queue<QuickTimeEventElementController>();
        m_results = new List<QuickTimeEventElementController.QuickTimeEventResultType>();

        StartCoroutine(SpawmEventsCoroutine(qteTime, amount, interval));
    }

    private IEnumerator SpawmEventsCoroutine(float qteTime, int amount, float interval)
    {
        var locations = m_locations.GetRandomItems(amount);

        for (int i = 0; i < amount; i++)
        {
            var location = locations[i];
            var key = m_possibleKeyCode.GetRandomItem();

            var instance = Instantiate(m_quickTimeEventPrefab, m_parentView);

            instance.Setup(qteTime, key);
            instance.transform.position = location.position;

            m_quickTimeEvents.Enqueue(instance);

            yield return new WaitForSeconds(interval);
        }
    }

    private void CheckEvents()
    {
        if (m_quickTimeEvents.Count == 0)
        {
            m_callback?.Invoke(new QuickTimeEventResult(m_results));
        }
    }

    private Key GetKeyPressed(List<Key> possivelKeys)
    {
        foreach (var key in possivelKeys)
        {
            if (Keyboard.current[key].wasPressedThisFrame) return key;
        }

        return Key.None;
    }
}
