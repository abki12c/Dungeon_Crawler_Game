using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 3; // Arrow damage

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>(); // Add Rigidbody if missing
        }
        rb.isKinematic = true; // Prevent physics simulation
        rb.useGravity = false; // No gravity
    }


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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ArrowTrapExit"))
        {
            gameObject.SetActive(false);
        }
    }
}
