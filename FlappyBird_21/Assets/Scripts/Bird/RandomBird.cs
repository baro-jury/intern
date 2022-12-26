using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBird : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Birds;

    // Start is called before the first frame update
    void Start()
    {
        int birdIndex = Random.Range(0, Birds.Length);
        Vector3 birdPos = Birds[birdIndex].transform.position;
        birdPos.y = 0;
        Instantiate(Birds[birdIndex], birdPos, Quaternion.identity);
    }
}
