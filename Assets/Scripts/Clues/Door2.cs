using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door2 : MonoBehaviour
{
    [SerializeField] private ClueObject clue;
    void Start()
    {
        clue = GetComponent<ClueObject>();
    }
    private void Update()
    {
        if (clue.IsScanned)
            SceneManager.LoadScene("EndScene");
    }
}
