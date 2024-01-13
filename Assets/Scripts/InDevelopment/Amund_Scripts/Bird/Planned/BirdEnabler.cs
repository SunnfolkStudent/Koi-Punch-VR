using System.Collections;
using UnityEngine;

public class BirdEnabler : MonoBehaviour
{
    [SerializeField] private GameObject[] BirdsForLevel;
    [SerializeField] private float EnableTime;
    private int NumberOfBirds;
    private int BirdIndex;
    
    private void Start()
    {
        NumberOfBirds = BirdsForLevel.Length;
        StartCoroutine(EnableBirdStart());
    }

    private IEnumerator EnableBirdStart()
    {
        for (BirdIndex = 0; BirdIndex < NumberOfBirds; BirdIndex++)
        {
            BirdsForLevel[BirdIndex].SetActive(true);
            yield return new WaitForSeconds(EnableTime);
            if (BirdIndex == NumberOfBirds)
            {
                yield break;
            }
        }
    }
}
