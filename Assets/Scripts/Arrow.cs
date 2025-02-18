using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 3; // Arrow damage

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player hit by arrow! - " + damage + " damage taken.");
                
            }
        }
    }
}
