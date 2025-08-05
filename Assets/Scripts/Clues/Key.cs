using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] private ClueObject clue;
    void Start()
    {
        clue = GetComponent<ClueObject>();
    }

    void Update()
    {
        if (clue.IsScanned)
            Destroy(this.gameObject);
    }
}
