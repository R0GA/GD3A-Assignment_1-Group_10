using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private ClueObject clue;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private string hint;
    private GameObject currentPrompt;
    public bool isUnlocked = false;
 
    private void Start()
    {
        clue = GetComponent<ClueObject>();
    }

    private void Update()
    {
        if (clue.IsScanned)
        {
            ShowPrompt();
        }
        if(isUnlocked)
        {
            Destroy(this.gameObject);
            Destroy(currentPrompt.gameObject);
        }

    }

    private void ShowPrompt()
    {
        if (currentPrompt == null)
        {
            currentPrompt = Instantiate(textPrefab);
        }

        // Position prompt above the clue
        currentPrompt.transform.position = clue.transform.position + Vector3.up * 0.5f;
        currentPrompt.transform.LookAt(Camera.main.transform);
        currentPrompt.transform.Rotate(0, 180, 0); // Make it face the camera properly
        currentPrompt.GetComponentInChildren<TMP_Text>().text = hint;
    }

}
