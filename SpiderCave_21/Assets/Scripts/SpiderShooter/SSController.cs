using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSController : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1);
        Vector3 bulletPos = transform.position;
        bulletPos.y -= 1f;
        Instantiate(bullet, bulletPos, Quaternion.identity);
        StartCoroutine(Attack());
    }
}
