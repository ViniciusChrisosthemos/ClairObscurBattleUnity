using UnityEngine;

public interface IBattleCharacter
{

    void TakeDamage(int damage);

    bool IsAlive();
}
