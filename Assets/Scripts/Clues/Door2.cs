using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door2 : MonoBehaviour
{
    [SerializeField] private ClueObject clue;
    [SerializeField] private GameObject ui;
    [SerializeField] private CharacterController character;
    void Start()
    {
        clue = GetComponent<ClueObject>();
    }
    private void Update()
    {
        if (clue.IsScanned)
        {
            SceneManager.LoadScene("STARTEND"); 
        }

    }
}
