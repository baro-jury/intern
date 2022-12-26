using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] asteroidPrefabs;

    private float width, height;
    private float bound = 50;
    private float innerBound = 7;

    // Start is called before the first frame update
    void Awake()
    {
        width = Camera.main.pixelWidth;
        height = Camera.main.pixelHeight;

        StartCoroutine("SpawnAsteroid");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnAsteroid()
    {
        //yield return new WaitForSeconds(Random.Range(1, 3));
        yield return new WaitForSeconds(0.5f);

        float positionX = 0, positionY = 0, rotation = 0;

        int spawnPosi = Random.Range(0, 4); //0 = up, 1 = right, 2 = down, 3 = left
        switch (spawnPosi)
        {
            case 0:
                positionX = Random.Range(0, width);
                positionY = Random.Range(height + innerBound, height + bound);
                rotation = Random.Range(135, 225);
                break;
            case 1:
                positionX = Random.Range(width + innerBound, width + bound);
                positionY = Random.Range(0, height);
                rotation = Random.Range(45, 135);
                break;
            case 2:
                positionX = Random.Range(0, width);
                positionY = Random.Range(-bound, -innerBound);
                rotation = Random.Range(-45, 45);
                break;
            case 3:
                positionX = Random.Range(-bound, -innerBound);
                positionY = Random.Range(0, height);
                rotation = Random.Range(225, 315);
                break;
        }

        Vector3 position = new Vector3(positionX, positionY, -Camera.main.transform.position.z);

        Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], Camera.main.ScreenToWorldPoint(position), 
            Quaternion.Euler(0, 0, rotation));

        StartCoroutine("SpawnAsteroid");
    }
}
