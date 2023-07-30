using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPoints : MonoBehaviour
{
    public Transform[] LeftPoints;
    public Transform[] RightPoints;
    public Transform[] QuePoints;

    public GameObject[] NPCs;

    bool isSpawnTime = false;

    public int maxQue;
    public static int currentQue = 0;


    public GameObject[] QUEnpcs;

    bool isShifting = false;
    public int ShiftCounter = 0;
    void Start()
    {
        currentQue = 0;
    }

    void FixedUpdate()
    {
        if (!isSpawnTime)
        {
            StartCoroutine(SpawnNPC());
        }

        if (ShiftCounter!=0 && !isShifting)
        {
            ShiftCounter--;
            StartCoroutine(ShiftCustomers());
        }

    }

    public Transform RandomLeftP()
    {
        return LeftPoints[Random.Range(0, LeftPoints.Length)];
    }
    public Transform RandomRightP()
    {
        return RightPoints[Random.Range(0, RightPoints.Length)];
    }

    IEnumerator SpawnNPC()
    {
        isSpawnTime = true;
        yield return new WaitForSeconds(Random.Range(1f, 2f));

        GameObject Npcc = Instantiate(NPCs[Random.Range(0, NPCs.Length)], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 20), Quaternion.identity);

        if (Random.Range(0, 2) == 0)
        {
            Npcc.GetComponent<NPC>().StartPoint = LeftPoints[Random.Range(0, LeftPoints.Length)];
            Npcc.GetComponent<NPC>().EndPoint = RightPoints[Random.Range(0, RightPoints.Length)];


        }
        else
        {
            Npcc.GetComponent<NPC>().EndPoint = LeftPoints[Random.Range(0, LeftPoints.Length)];
            Npcc.GetComponent<NPC>().StartPoint = RightPoints[Random.Range(0, RightPoints.Length)];

        }
        if (currentQue < maxQue && Random.Range(0, 2) == 0) //customer
        {
            Npcc.GetComponent<NPC>().isCustomer = true;
            Npcc.GetComponent<NPC>().EndPoint = QuePoints[currentQue];
            Npcc.GetComponent<NPC>().QueNumber = currentQue;



            QUEnpcs[currentQue] = Npcc;
            currentQue++;
        }

        isSpawnTime = false;

    }
    public Transform RandomRigh3tP()
    {
        return RightPoints[Random.Range(0, RightPoints.Length)];
    }
    public IEnumerator ShiftCustomers()
    {
        currentQue--;
        isShifting = true;
        for (int i = 1; i < QUEnpcs.Length; i++)
        {
            if (i != maxQue)
            {
                QUEnpcs[i - 1] = QUEnpcs[i];

                if(i +1 == maxQue)
                {
                    QUEnpcs[i] = null;
                }
            }
            else
            {
               
            }

        }
        for (int i = 0; i < QUEnpcs.Length; i++)
        {
            if (QUEnpcs[i] != null)
            {
                QUEnpcs[i].gameObject.GetComponent<NPC>().EndPoint = QuePoints[i];
                QUEnpcs[i].gameObject.GetComponent<NPC>().QueNumber = i;
                QUEnpcs[i].gameObject.GetComponent<NPC>().isReached = false;
            }

           yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

        }

        isShifting = false;
    }

    public void ShiftCustomersMetot()
    {
        ShiftCounter++;
       
    }
}
