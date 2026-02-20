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

    public UnityEvent OnInputGiven;

    private Queue<QuickTimeEventElementController> m_quickTimeEvents;

    private void Awake()
    {
        m_quickTimeEvents = new Queue<QuickTimeEventElementController>();
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (m_quickTimeEvents.Count == 0) return;

            var qtEvent = m_quickTimeEvents.Dequeue();

            qtEvent.HandlePlayerInput(GetKeyPressed(m_possibleKeyCode));
        }

        if (m_quickTimeEvents.Count > 0)
        {
            if (m_quickTimeEvents.First().HasFinished)
            {
                m_quickTimeEvents.Dequeue();
            }
        }
    }

    public void StartEvents(float qteTime, int amount, float interval)
    {
        m_quickTimeEvents = new Queue<QuickTimeEventElementController>();

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

    private Key GetKeyPressed(List<Key> possivelKeys)
    {
        foreach (var key in possivelKeys)
        {
            if (Keyboard.current[key].wasPressedThisFrame) return key;
        }

        return Key.None;
    }
}
