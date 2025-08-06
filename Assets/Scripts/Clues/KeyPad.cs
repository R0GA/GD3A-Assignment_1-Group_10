using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPad : MonoBehaviour
{
    [SerializeField] private ClueObject clue;
    [SerializeField] private Listen listen;
    [SerializeField] private Door door;
    void Start()
    {
        gameObject.GetComponent<MeshCollider>().enabled = false;
        clue = GetComponent<ClueObject>();
    }

   
    void Update()
    {
        if (listen.codeUnlocked)
        {
            gameObject.GetComponent<MeshCollider>().enabled = true;
        }
        if(listen.codeUnlocked && clue.IsScanned)
        {
            door.isUnlocked = true;
        }
    }
}
