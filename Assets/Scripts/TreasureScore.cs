using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public AudioSource collectSound;
    public float delayTime = 2f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Treasure Collected!");

            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            playerInventory.treasureCollected();

            collectSound.Play();
            Destroy(this.gameObject);

            if (playerInventory.NumberOfTreasures == 5)
            {
                Debug.Log("Collected all treasures! Number of Treasures: " + playerInventory.NumberOfTreasures);
                GameManager.Instance.LoadNextLevel(delayTime);
            }
        }
    }
}