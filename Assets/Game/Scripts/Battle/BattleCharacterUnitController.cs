using UnityEngine;

public class BattleCharacterUnitController : MonoBehaviour
{

    public virtual void TakeDamage(int damage)
    {
        Debug.Log("Damage Taken");
    }
}
