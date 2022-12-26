using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpin : MonoBehaviour
{
    [SerializeField] GameObject flag;
    
    bool hasSpin = false;

    float speed = 0.3f;

    // Update is called once per frame
    void Update()
    {
        if (GlobalWinLose.instance.winFlag && !hasSpin)
        {
            hasSpin = true;
            StartCoroutine(SpinMultipleTimes(2));
        }
    }

    IEnumerator SpinMultipleTimes(int times)
    {
        yield return SpinFlag();

        for (int i = 0; i < times; ++i)
        {
            yield return SpinFlag();
            yield return SpinFlag();
        }    
    }

    IEnumerator SpinFlag()
    {
        float goalXScale = -flag.transform.localScale.x;
        while (Mathf.Abs(flag.transform.localScale.x - goalXScale) > Mathf.Epsilon)
        {
            Vector3 temp = flag.transform.localScale;
            temp.x = Mathf.MoveTowards(temp.x, goalXScale, speed);
            flag.transform.localScale = temp;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
