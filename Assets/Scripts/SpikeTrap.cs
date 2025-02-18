using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{
    public Transform spikes;
    public Vector3 raisedOffset = new Vector3(0, 1.0f, 0);
    public float spikeSpeed = 10f;
    public float retractTime = 2f;
    public int damage = 20;
    public float damageInterval = 1f;
    public AudioSource spikeSound;

    private Vector3 loweredPosition;
    private Vector3 raisedPosition;
    private bool isRaised = false;
    private bool isPlayerOnTrap = false;

    private void Start()
    {
        loweredPosition = spikes.localPosition;
        raisedPosition = loweredPosition + raisedOffset;
        DisableSpikeMeshRenderers();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnTrap = true;

            if (!isRaised) // Raise spikes if they are down
            {
                StartCoroutine(RaiseSpikes());
            }

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                StartCoroutine(DamageLoop(playerHealth)); // Start damage loop
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnTrap = false;
        }
    }

    private IEnumerator DamageLoop(PlayerHealth playerHealth)
    {
        while (isPlayerOnTrap && isRaised)  // Keep dealing damage while player is inside
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Player takes damage from spikes!");
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private IEnumerator RaiseSpikes()
    {
        EnableSpikeMeshRenderers();
        isRaised = true;
        spikeSound.Play();

        while (Vector3.Distance(spikes.localPosition, raisedPosition) > 0.01f)
        {
            spikes.localPosition = Vector3.MoveTowards(spikes.localPosition, raisedPosition, spikeSpeed * Time.deltaTime * 1000);
            yield return null;
        }

        yield return new WaitForSeconds(retractTime);
        StartCoroutine(LowerSpikes());
    }

    private IEnumerator LowerSpikes()
    {
        while (Vector3.Distance(spikes.localPosition, loweredPosition) > 0.01f)
        {
            spikes.localPosition = Vector3.MoveTowards(spikes.localPosition, loweredPosition, spikeSpeed * Time.deltaTime);
            yield return null;
        }

        isRaised = false;
        DisableSpikeMeshRenderers();

        // Raise spikes again if player is still on the trap**
        if (isPlayerOnTrap)
        {
            StartCoroutine(RaiseSpikes());
        }

    }

    // Disable MeshRenderers for all spikes inside the Spikes object
    private void DisableSpikeMeshRenderers()
    {
        foreach (Transform spike in spikes)
        {
            MeshRenderer meshRenderer = spike.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }
    }

    // Enable MeshRenderers for all spikes inside the Spikes object
    private void EnableSpikeMeshRenderers()
    {
        foreach (Transform spike in spikes)
        {
            MeshRenderer meshRenderer = spike.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }
        }
    }
}
