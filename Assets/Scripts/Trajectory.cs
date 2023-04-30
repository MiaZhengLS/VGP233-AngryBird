using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public GameObject trajectoryDot;
    public float groundPosY;
    public float dotSpace;
    public float maxScale;
    public float minScale;

    private List<Vector3> posDotList;
    private List<GameObject> activeDotPool;
    private List<GameObject> inactiveDotPool;

    void Start()
    {
        posDotList = new List<Vector3>();
        activeDotPool = new List<GameObject>();
        inactiveDotPool = new List<GameObject>();
    }

    public void ShowTrajectory(Vector3 p0, Vector3 v0, Vector3 accer)
    {
        int validDotCnt = 0;
        for(int i = 0; i < 30; i++)
        {
            var timeStamp = i * dotSpace;
            // p1 = p0 + v0*t + 1/2*a*t^2
            var p1 = p0 + v0 * timeStamp + 0.5f * accer * Mathf.Pow(timeStamp, 2);
            if(p1.y <= groundPosY)
            {
                PoolDots(i);
                break;
            }

            if(i >= posDotList.Count)
            {
                posDotList.Add(p1);
            }
            else
            {
                posDotList[i] = p1;
            }
            validDotCnt++;
        }
        var scaleStep = (maxScale - minScale) / validDotCnt;

        for (int i = 0;i< validDotCnt;i++)
        {
            var dot = GetDotFromPool(i);
            dot.transform.position = posDotList[i];
            dot.transform.localScale = (maxScale - scaleStep * i) * Vector3.one;
        }

    }

    public void HideTrajectory()
    {
        PoolDots(0);
    }

    GameObject GetDotFromPool(int dotIdx)
    {
        GameObject dot = null;

        if(dotIdx < activeDotPool.Count)
        {
            dot = activeDotPool[dotIdx];
        }
        else if(inactiveDotPool.Count > 0)
        {
            dot = inactiveDotPool[inactiveDotPool.Count - 1];
            inactiveDotPool.RemoveAt(inactiveDotPool.Count - 1);
            activeDotPool.Add(dot);
        }
        else
        {
            dot = Instantiate(trajectoryDot);
            activeDotPool.Add(dot);
        }

        dot.SetActive(true);
        return dot;
    }

    void PoolDots(int startIdx)
    {
        for(int i = activeDotPool.Count - 1; i >= startIdx; --i)
        {
            var dot = activeDotPool[i];
            dot.SetActive(false);
            activeDotPool.Remove(dot);
            inactiveDotPool.Add(dot);
        }
    }
}
