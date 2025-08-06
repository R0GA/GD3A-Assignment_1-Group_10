using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectiveModeController : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask clueLayer;

    [Header("Materials")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material emissiveMaterial;
    [SerializeField] private Material scannedMaterial;
    [SerializeField] private GameObject postProcessing;


    [Header("UI")]
    [SerializeField] private GameObject scanPromptPrefab;

    // Input System
    private PlayerControls playerInputActions;

    private bool isInDetectiveMode = false;
    private Collider[] nearbyClues = new Collider[20];
    private int cluesInRange;
    private GameObject currentPrompt;
    private GameObject currentClue;

    private void Awake()
    {
        // Initialize input actions
        playerInputActions = new PlayerControls();
    }

    private void OnEnable()
    {
        // Enable detective mode actions
        playerInputActions.Player.Enable();

        // Bind input events
        playerInputActions.Player.Detect.performed += ctx => ToggleDetectiveMode();
        playerInputActions.Player.Interact.performed += ctx => TryScanClue();
    }

    private void OnDisable()
    {
        // Unbind input events
        playerInputActions.Player.Detect.performed -= ctx => ToggleDetectiveMode();
        playerInputActions.Player.Interact.performed -= ctx => TryScanClue();

        // Disable actions
        playerInputActions.Player.Disable();
    }

    private void ToggleDetectiveMode()
    {
        postProcessing.SetActive(true);
        isInDetectiveMode = !isInDetectiveMode;
        Debug.Log($"Detective Mode: {isInDetectiveMode}");

        if (!isInDetectiveMode)
        {
            postProcessing.SetActive(false);
            ReturnAllCluesToNormal();
            if (currentPrompt != null) Destroy(currentPrompt);
        }
    }

    private void TryScanClue()
    {
        if (!isInDetectiveMode || currentClue == null) return;

        Debug.Log("Attempting to scan clue");

        var clueObject = currentClue.GetComponent<ClueObject>();
        if (clueObject != null && !clueObject.IsScanned)
        {
            clueObject.Scan();
            var renderer = currentClue.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = scannedMaterial;
            }

            if (currentPrompt != null) Destroy(currentPrompt);
        }
    }

    private void Update()
    {
        if (isInDetectiveMode)
        {
            DetectClues();
            CheckClosestClue();
        }
    }


    private void DetectClues()
    {
        cluesInRange = Physics.OverlapSphereNonAlloc(
            transform.position,
            detectionRadius,
            nearbyClues,
            clueLayer
        );

        // First reset all previously affected clues
        for (int i = 0; i < nearbyClues.Length; i++)
        {
            var clue = nearbyClues[i];
            if (clue == null) continue;

            var distance = Vector3.Distance(transform.position, clue.transform.position);
            if (distance > detectionRadius)
            {
                var renderer = clue.GetComponent<Renderer>();
                var clueObject = clue.GetComponent<ClueObject>();

                if (renderer != null && clueObject != null && !clueObject.IsScanned)
                {
                    renderer.material = normalMaterial;
                }
                nearbyClues[i] = null;
            }
        }

        // Then process new ones
        for (int i = 0; i < cluesInRange; i++)
        {
            var clue = nearbyClues[i];
            if (clue == null) continue;

            var renderer = clue.GetComponent<Renderer>();
            var clueObject = clue.GetComponent<ClueObject>();

            if (renderer != null && clueObject != null && !clueObject.IsScanned)
            {
                renderer.material = emissiveMaterial;
            }
        }
    }

    private void CheckClosestClue()
    {
        GameObject closestClue = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < cluesInRange; i++)
        {
            var clue = nearbyClues[i];
            if (clue == null) continue;

            var distance = Vector3.Distance(transform.position, clue.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestClue = clue.gameObject;
            }
        }

        // Handle UI prompt
        if (closestClue != null && closestDistance < 4f) // 4m interaction range
        {
            var clueObject = closestClue.GetComponent<ClueObject>();
            if (clueObject != null && !clueObject.IsScanned)
            {
                currentClue = closestClue;
                ShowPrompt(closestClue);
                return;
            }
        }

        // No valid clue nearby
        currentClue = null;
        if (currentPrompt != null) Destroy(currentPrompt);
    }

    private void ShowPrompt(GameObject clue)
    {
        if (currentPrompt == null)
        {
            currentPrompt = Instantiate(scanPromptPrefab);
        }

        // Position prompt above the clue
        currentPrompt.transform.position = clue.transform.position + Vector3.up * 1f;
        currentPrompt.transform.LookAt(Camera.main.transform);
        currentPrompt.transform.Rotate(0, 180, 0); // Make it face the camera properly
        currentPrompt.GetComponentInChildren<TMP_Text>().text = clue.GetComponent<ClueObject>().scanTXT;
    }


    private void ReturnAllCluesToNormal()
    {
        for (int i = 0; i < cluesInRange; i++)
        {
            var clue = nearbyClues[i];
            if (clue == null) continue;

            var clueObject = clue.GetComponent<ClueObject>();
            if (clueObject != null && !clueObject.IsScanned)
            {
                var renderer = clue.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = normalMaterial;
                }
            }
        }
        cluesInRange = 0;
    }

    // Visualize detection radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
