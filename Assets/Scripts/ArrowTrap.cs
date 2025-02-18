using UnityEngine;
using System.Collections;

public class ArrowTrap : MonoBehaviour
{
    public Transform[] arrows;
    public float launchSpeed = 2f;
    public float retractTime = 2f;
    public AudioSource arrowSound;

    private bool isShooting = false;
    private bool isRetracting = false; // New flag to prevent re-triggering during retract time
    private Vector3[] initialPositions;

    private void Start()
    {
        initialPositions = new Vector3[arrows.Length];
        for (int i = 0; i < arrows.Length; i++)
        {
            if (arrows[i] != null)
            {
                initialPositions[i] = arrows[i].position;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isShooting && !isRetracting)
        {
            isShooting = true;
            StartCoroutine(LaunchArrows());
        }
    }

    private IEnumerator LaunchArrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (arrows[i] != null)
            {
                StartCoroutine(LaunchArrow(arrows[i]));
            }
        }

        isShooting = false;
        isRetracting = true; // Set retracting state

        yield return new WaitForSeconds(retractTime); // Wait for retract time

        ResetArrows();
        isRetracting = false; // Allow re-triggering after full cycle
    }

    private IEnumerator LaunchArrow(Transform arrow)
    {
        arrowSound.Play();
        Vector3 targetPosition = arrow.position + (arrow.forward * 5f);
        float journeyLength = Vector3.Distance(arrow.position, targetPosition);
        float startTime = Time.time;

        while (Time.time < startTime + journeyLength / launchSpeed)
        {
            float distanceCovered = (Time.time - startTime) * launchSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            arrow.position = Vector3.Lerp(arrow.position, targetPosition, fractionOfJourney);
            yield return null;
        }
    }

    private void ResetArrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (arrows[i] != null)
            {
                arrows[i].position = initialPositions[i];
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isShooting = false; // Stop shooting
        }
    }
}
