using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pipe;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(1);
        Vector3 pipePos = pipe.transform.position;
        pipePos.y = Random.Range(-2.5f, 2.5f) ;
        Instantiate(pipe, pipePos, Quaternion.identity);
        StartCoroutine(Spawner());
    }
}
