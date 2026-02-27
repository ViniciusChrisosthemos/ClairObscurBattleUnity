using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleEnvironmentManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_playerCharacterParent;
    [SerializeField] private Transform m_enemyCharacterParent;

    [Header("Prefabs")]
    [SerializeField] private BattleCharacterView m_playerBattleCharacterViewPrefab;
    [SerializeField] private BattleCharacterView m_enemyBattleCharacterViewPrefab;

    [Header("Paramters")]
    [SerializeField] private Vector3 m_spacing;
    [SerializeField] private Vector3 m_characterSize;

    private Dictionary<BattleCharacter, BattleCharacterView> m_battleCharacterViewDict;

    public void Setup(List<BattleCharacter> playerBattleCharacters, List<BattleCharacter> enemiesBattleCharacters)
    {
        m_playerCharacterParent.ClearChilds();
        m_enemyCharacterParent.ClearChilds();

        PlayerBattleViews = InstantiateBattleViews(playerBattleCharacters, m_playerBattleCharacterViewPrefab, m_playerCharacterParent);
        EnemyBattleViews = InstantiateBattleViews(enemiesBattleCharacters, m_enemyBattleCharacterViewPrefab, m_enemyCharacterParent);

        m_battleCharacterViewDict = new Dictionary<BattleCharacter, BattleCharacterView>();

        foreach (var view in PlayerBattleViews) m_battleCharacterViewDict.Add(view.BattleCharacter, view);
        foreach (var view in EnemyBattleViews) m_battleCharacterViewDict.Add(view.BattleCharacter, view);
    }

    private List<BattleCharacterView> InstantiateBattleViews(List<BattleCharacter> characters, BattleCharacterView prefab, Transform parent)
    {
        var views = new List<BattleCharacterView>();

        var initialPosition = (m_characterSize * characters.Count + m_spacing * (characters.Count - 1)) / -2f;
        var positionOffset = m_characterSize + m_spacing;

        foreach (var character in characters)
        {
            var battleView = Instantiate(prefab, parent);
            battleView.Setup(character);
            battleView.transform.localPosition = initialPosition;
            battleView.transform.localRotation = Quaternion.identity;

            initialPosition += positionOffset;

            views.Add(battleView);
        }

        return views;
    }

    public BattleCharacterView GetCharacterView(BattleCharacter battleCharacter) => m_battleCharacterViewDict[battleCharacter];

    public List<BattleCharacterView> PlayerBattleViews { get; private set; }
    public List<BattleCharacterView> EnemyBattleViews { get; private set; }
}
