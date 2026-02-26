using System;
using System.Collections.Generic;
using System.Linq;

public class TurnManager
{
    private TimelineController<BattleCharacter> m_timeLineController;
    
    public TurnManager(List<BattleCharacter> allCharacters)
    {
        m_timeLineController = new TimelineController<BattleCharacter>(allCharacters);

        m_timeLineController.UpdateTimeLine();
    }

    public BattleCharacter Next()
    {
        if (m_timeLineController.CurrentSize == 0)
        {
            m_timeLineController.UpdateTimeLine();
        }

        Current = m_timeLineController.Dequeue();

        return Current;
    }

    public List<BattleCharacter> GetCharacterQueue()
    {
        var queue = m_timeLineController.GetTimeline();

        return queue.Where(c => c.IsActive()).ToList();
    }

    public BattleCharacter Current {  get; private set; }
}
