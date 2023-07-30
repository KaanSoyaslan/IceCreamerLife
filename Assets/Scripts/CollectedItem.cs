using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItem : MonoBehaviour
{
    public Transform EndPoint;
    public bool isReached = false;

    public int Id;

    public Transform PotansParent;

    public string CollectedFor;

    void FixedUpdate()
    {
        if(EndPoint != null && Vector3.Distance(transform.position, EndPoint.position) < 0.1f)
        {

            if (Id != -1) //not to env
            {
                if (CollectedFor == "AppleIC")
                {
                    GameObject.FindGameObjectWithTag("IceCreamMachineApple").GetComponent<IceCreamMachine>().TakeItem(Id);
                    Destroy(gameObject);
                }
                if (CollectedFor == "PearIC")
                {
                    GameObject.FindGameObjectWithTag("IceCreamMachinePear").GetComponent<IceCreamMachine>().TakeItem(Id);
                    Destroy(gameObject);
                }
                if (CollectedFor == "BananaIC")
                {
                    GameObject.FindGameObjectWithTag("IceCreamMachineBanana").GetComponent<IceCreamMachine>().TakeItem(Id);
                    Destroy(gameObject);
                }


                if (CollectedFor == "IceCreamGO" || CollectedFor == "MoneyGO" || CollectedFor == "Trash")
                {
                    Destroy(gameObject);
                }


            }
            else //envanter
            {
                isReached = true;
                gameObject.transform.SetParent(PotansParent);
            }



        }
        else if (!isReached)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPoint.position, 10f * Time.deltaTime);
        }

    }
}