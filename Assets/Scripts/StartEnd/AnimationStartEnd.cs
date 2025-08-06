using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationStartEnd : MonoBehaviour
{
    public RawImage[] rawImages;          
    public float switchInterval = 0.25f; 

    private int currentIndex = 0;

    void Start()
    {
        if (rawImages == null || rawImages.Length == 0)
        {
            Debug.LogWarning("No RawImages assigned.");
            return;
        }

        StartCoroutine(CycleImages());
    }

    IEnumerator CycleImages()
    {
        while (true)
        {
            
            for (int i = 0; i < rawImages.Length; i++)
            {
                rawImages[i].gameObject.SetActive(i == currentIndex);
            }

           
            currentIndex = (currentIndex + 1) % rawImages.Length;

            yield return new WaitForSeconds(switchInterval);
        }
    }

    public void startGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
