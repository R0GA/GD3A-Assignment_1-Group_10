using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    [SerializeField] private ClueObject clue;
    [SerializeField] private GameObject steps;

    private void Start()
    {
        clue = GetComponent<ClueObject>();
    }

    private void Update()
    {
       if (clue.IsScanned)
        {
            if(!steps.activeSelf)
            {
                steps.SetActive(true);
            }
        }
    }

    

}
