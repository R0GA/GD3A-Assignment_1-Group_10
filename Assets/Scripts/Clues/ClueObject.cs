using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueObject : MonoBehaviour
{
    public bool IsScanned { get; private set; } = false;

    public void Scan()
    {
        IsScanned = true;
        // Add any additional scan logic here
        Debug.Log("Clue scanned: " + gameObject.name);
    }
}
