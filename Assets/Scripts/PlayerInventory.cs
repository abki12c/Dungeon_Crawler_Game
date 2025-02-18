using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfTreasures { get; private set;}
    public UnityEvent<PlayerInventory> OnPlayerInventory;

    public void treasureCollected()
    {
        NumberOfTreasures++;
        OnPlayerInventory.Invoke(this);
    }
}
