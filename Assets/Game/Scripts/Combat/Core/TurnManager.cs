using System.Collections.Generic;

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

    public BattleCharacter Current {  get; private set; }
}
