using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("============= Move =============")]
    public Animator anim;
    public float speed;
    public int startingPoint;

    public Transform StartPoint;
    public Transform EndPoint;
    // public Rigidbody rb;


    [Header("============= Status Check =============")]
    public bool isReached = false;
    public bool isCustomer = false;
    public bool JobDone= false;
    public bool isSneek= false;
    public int QueNumber;



    GameObject NpcPointHolder;
    Transform Stand;
    Transform LookAt;

    public int Gender; //0 male  1 female

    [Header("============= Customer =============")]
    public GameObject TakeCollider;
    public GameObject IceCreamApple;
    public GameObject IceCreamPear;
    public GameObject IceCreamBanana;


    public GameObject CanvasObject;
    public GameObject[] ICimages; //0 pear  1 apple  2 banana

    int WantedIceCream= 0;
    void Start()
    {
        Physics.IgnoreLayerCollision(9, 9, true); //npc to npc
        Physics.IgnoreLayerCollision(6, 9, true); //npc to player
        Physics.IgnoreLayerCollision(11, 9, true); //npc to invWALLS

        LookAt = EndPoint;

        if (!isCustomer)
        {
            speed = Random.Range(2f, 3f);
        }
        else
        {

            NpcPointHolder = GameObject.FindGameObjectWithTag("NpcPointHolder");
            speed = 2.5f;

            Stand = GameObject.FindGameObjectWithTag("Stand").transform;


            //random Ice Cream

            //0  just apple
            //1  apple + pear
            //2  apple + banana
            //3  apple + pear + banana
            if (PlayerPrefs.GetInt("FruitStatus") == 0)
            {
                TakeCollider.gameObject.tag = "WantAppleIC";

                ICimages[1].SetActive(true);
            }
            else if (PlayerPrefs.GetInt("FruitStatus") == 1)
            {
                WantedIceCream = Random.Range(0, 2);
                if (WantedIceCream == 0)
                {
                    TakeCollider.gameObject.tag = "WantAppleIC";

                    ICimages[1].SetActive(true);
                }
                else if (WantedIceCream == 1)
                {
                    TakeCollider.gameObject.tag = "WantPearIC";


                    ICimages[0].SetActive(true);
                }
            }
            else if (PlayerPrefs.GetInt("FruitStatus") == 2)
            {
                WantedIceCream = Random.Range(0, 2);
                if (WantedIceCream == 0)
                {
                    TakeCollider.gameObject.tag = "WantAppleIC";

                    ICimages[1].SetActive(true);
                }
                else if (WantedIceCream == 1)
                {
                    TakeCollider.gameObject.tag = "WantBananaIC";
                    
                    ICimages[2].SetActive(true);
                }
            }
            else if (PlayerPrefs.GetInt("FruitStatus") == 3)
            {
                WantedIceCream = Random.Range(0, 3);
                if (WantedIceCream == 0)
                {
                    TakeCollider.gameObject.tag = "WantAppleIC";
                  
                    ICimages[1].SetActive(true);
                }
                else if (WantedIceCream == 1)
                {
                    TakeCollider.gameObject.tag = "WantPearIC";
                   

                    ICimages[0].SetActive(true);
                }
                else if (WantedIceCream == 2)
                {
                    TakeCollider.gameObject.tag = "WantBananaIC";
                 

                    ICimages[2].SetActive(true);
                }
            }
        }


        transform.position = StartPoint.position;

    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, EndPoint.position) < 0.1f)
        {
             

            if (!isCustomer)
            {
                Destroy(gameObject);
            }

            isReached = true;
        }
        else if(!isReached)
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPoint.position, speed * Time.deltaTime);
        }
      


        var lookPos = LookAt.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20);



        if (isReached)
        {
            anim.SetBool("isWalking" + Gender, false);
            anim.SetBool("isSneek", false);
            LookAt = Stand;

            if(QueNumber == 0)//next customer
            {
                CanvasObject.SetActive(true);
                TakeCollider.SetActive(true);
            }
          
        }
        else
        {
            
            LookAt = EndPoint;
           


            if (isSneek)
            {
                anim.SetBool("isSneek", true);
            }
            else
            {
                anim.SetBool("isWalking" + Gender, true);

            }
        }

        if (JobDone)
        {
            CanvasObject.SetActive(false);
            TakeCollider.SetActive(false);
            if (WantedIceCream == 0)
            {
                IceCreamApple.SetActive(true);

            }
            if (WantedIceCream == 1)
            {
                IceCreamPear.SetActive(true);

            }
            if (WantedIceCream == 2)
            {
                IceCreamBanana.SetActive(true);

            }


           
            TakeCollider.SetActive(false);
            JobDone = false;
            isReached = false;
            isCustomer = false;

            if (Random.Range(0, 2) == 0)
            {
               
                EndPoint = NpcPointHolder.GetComponent<NpcPoints>().RandomRightP();
            }
            else
            {
                EndPoint = NpcPointHolder.GetComponent<NpcPoints>().RandomLeftP();

            }


            NpcPointHolder.GetComponent<NpcPoints>().ShiftCustomersMetot();
        }
    }
}
