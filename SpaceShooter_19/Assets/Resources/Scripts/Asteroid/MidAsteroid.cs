using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidAsteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject smallAsteroid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            for (int i = 0; i < 3; ++i)
            {
                Instantiate(smallAsteroid, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            }
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
