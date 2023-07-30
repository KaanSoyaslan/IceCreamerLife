using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("============= Move =============")]
    [SerializeField] float speed = 10f;
    public Animator anim;
    public Joystick movementJoystick;
    private Vector3 lookDir;


    [Header("============= Envanter =============")]
    public Transform[] EnvanterItemPos;
    public int EnvanterStatus;  //Slots status
    public GameObject[] EnvanterItems;
    public int Sorter = 0;   //for item sorting
    public int MaxItemAmount;

    public GameObject CollectedApple;
    public GameObject CollectedICApple;
    public GameObject CollectedMilk;
    public GameObject CollectedPear;
    public GameObject CollectedICPear;
    public GameObject CollectedBanana;
    public GameObject CollectedICBanana;

   

    //IEnumeratorControls
    bool isShakingTree = false;
    bool isShakingTreeAnimPlaying = false;

    bool isIceCreamMachineAppleTime = false;
    bool isIceCreamMachinePearTime = false;
    bool isIceCreamMachineBananaTime = false;
    //CheckMachines
    bool isTouchingICMapple = false;
    bool isTouchingICMpear = false;
    bool isTouchingICMbanana = false;

    bool isIceCreamTime = false;

    bool isExpandTime = false;


    //Machine FruitControl
    int applefruit = 0;
    int pearfruit = 0;
    int bananafruit = 0;


    [Header("============= Money =============")]
    public GameObject MoneyObject;
    public GameObject[] MoneyObjects;
    public TMP_Text MoneyAmountTXT;


    [Header("============= Others =============")]
    public static bool isSneekTime; //cowArea

    public GameObject EmptyBottle;//for milking

    public GameObject GameManagerObject;//for tutorial





    void Start()
    {
        Application.targetFrameRate = 150; //unity sometimes locks to 30 FPS

        Physics.IgnoreLayerCollision(7, 10, true); //loader and playerfront
        Physics.IgnoreLayerCollision(7, 12, true); //cowfence and Player
        Physics.IgnoreLayerCollision(6, 12, true); //cowfence and playerfront
    }

    void FixedUpdate()
    {
        
        MoneyAmountTXT.text = "" + PlayerPrefs.GetInt("Money");
        var deltaX = movementJoystick.joystickVec.x * Time.deltaTime * speed;
        var deltaY = movementJoystick.joystickVec.y * Time.deltaTime * speed;

        //Run
        if (deltaX == 0 && deltaY == 0)
        {
            anim.SetBool("isRunning", false);

            anim.SetBool("isSneeking", false); //cowArea
        }
        else if (isSneekTime)
        {
            anim.SetBool("isBuying", false);
            anim.SetInteger("isTreeShakeTime", 0);
            anim.SetBool("isSneeking", true);
            lookDir = new Vector3(deltaX, 0, deltaY);
        }
        else
        {
            anim.SetBool("isBuying", false);
            anim.SetInteger("isTreeShakeTime", 0);

            anim.SetBool("isSneeking", false);
            anim.SetBool("isRunning", true);

            lookDir = new Vector3(deltaX, 0, deltaY);
        }



        //LookRotation
        var newXpos = transform.position.x + deltaX;
        var newYpos = transform.position.z + deltaY;

        transform.rotation = Quaternion.LookRotation(lookDir);
        transform.position = new Vector3(newXpos, 0f, newYpos);



    }

    private void OnTriggerStay(Collider collision)
    {

        if (collision.gameObject.tag == "FadeArea")//StandBoard
        {
            ObjectFader.DoFade = true;
        }


        if (collision.gameObject.tag == "FruitTree" && !anim.GetBool("isRunning")) // ShakeTime
        {
            isShakingTree = true;

            if (!isShakingTreeAnimPlaying)
            {          
                StartCoroutine(ShakeTheTree(collision.gameObject));
            }
       
            collision.gameObject.transform.parent.GetChild(0).GetComponent<FruitTree>().anim.SetBool("isPlayerHitting", true);
        }

        //Machines
        if (collision.gameObject.tag == "IceCreamMachineApple" && !isIceCreamMachineAppleTime && collision.gameObject.GetComponent<IceCreamMachine>().isAvailable == true)
        {
            isTouchingICMapple = true;
            StartCoroutine(ReleaseApples(collision.gameObject));
        }
        if (collision.gameObject.tag == "IceCreamMachinePear" && !isIceCreamMachinePearTime && collision.gameObject.GetComponent<IceCreamMachine>().isAvailable == true)
        {
            isTouchingICMpear = true;
            StartCoroutine(ReleasePears(collision.gameObject));
        }
        if (collision.gameObject.tag == "IceCreamMachineBanana" && !isIceCreamMachineBananaTime && collision.gameObject.GetComponent<IceCreamMachine>().isAvailable == true)
        {
            isTouchingICMbanana = true;
            StartCoroutine(ReleaseBananas(collision.gameObject));
        }


        if (collision.gameObject.tag == "ExpandLoader" && !anim.GetBool("isRunning") && !isExpandTime) //+UPGRADE
        {
            
            StartCoroutine(ExpandLoad(collision.gameObject));            
        }

        if (collision.gameObject.tag == "CowArea")
        {
            EmptyBottle.SetActive(true);
            isSneekTime = true;

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        
        if (collision.gameObject.tag == "FadeArea")//StandBoard
        {
            ObjectFader.DoFade = false;
        }

        if (collision.gameObject.tag == "FruitTree")//not shake time
        {
            collision.gameObject.transform.parent.GetChild(0).GetComponent<FruitTree>().anim.SetBool("isPlayerHitting", false);
            isShakingTree = false;
        }
        if (collision.gameObject.tag == "FruitTree" && collision.gameObject.transform.parent.GetChild(0).GetComponent<FruitTree>().anim.GetBool("isPlayerHitting") == false)
        {
            anim.SetInteger("isTreeShakeTime", 0);
        }
        if (collision.gameObject.tag == "IceCreamMachineApple")
        {
            isTouchingICMapple = false;
        }
        if (collision.gameObject.tag == "IceCreamMachinePear")
        {
            isTouchingICMpear = false;
        }
        if (collision.gameObject.tag == "IceCreamMachineBanana")
        {
            isTouchingICMbanana = false;
        }


        if (collision.gameObject.tag == "CowArea")
        {
            EmptyBottle.SetActive(false);
            isSneekTime = false;
        }
    }



    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "FadeArea")
        {
            ObjectFader.DoFade = true;
        }


        if (collision.gameObject.tag == "Apple" && EnvanterStatus < MaxItemAmount) //collect apple
        {

            SoundManager.PlaySound("CollectSFX");
            GameObject Item = Instantiate(CollectedApple, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject.transform.gameObject);


            Item.GetComponent<CollectedItem>().Id = -1; //for collected
            Item.GetComponent<CollectedItem>().PotansParent = EnvanterItemPos[EnvanterStatus].transform; //for collected

            Item.GetComponent<CollectedItem>().EndPoint = EnvanterItemPos[EnvanterStatus].transform;
            EnvanterItems[EnvanterStatus] = Item;
            EnvanterStatus++;


        }
        if (collision.gameObject.tag == "Pear" && EnvanterStatus < MaxItemAmount) //collect pear
        {

            SoundManager.PlaySound("CollectSFX");
            GameObject Item = Instantiate(CollectedPear, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject.transform.gameObject);


            Item.GetComponent<CollectedItem>().Id = -1; //for collected
            Item.GetComponent<CollectedItem>().PotansParent = EnvanterItemPos[EnvanterStatus].transform; //for collected

            Item.GetComponent<CollectedItem>().EndPoint = EnvanterItemPos[EnvanterStatus].transform;
            EnvanterItems[EnvanterStatus] = Item;
            EnvanterStatus++;
        }

        if (collision.gameObject.tag == "Banana" && EnvanterStatus < MaxItemAmount) //collect banana
        {

            SoundManager.PlaySound("CollectSFX");
            GameObject Item = Instantiate(CollectedBanana, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject.transform.gameObject);
            Item.GetComponent<CollectedItem>().Id = -1; //for collected
            Item.GetComponent<CollectedItem>().PotansParent = EnvanterItemPos[EnvanterStatus].transform; //for collected

            Item.GetComponent<CollectedItem>().EndPoint = EnvanterItemPos[EnvanterStatus].transform;
            EnvanterItems[EnvanterStatus] = Item;
            EnvanterStatus++;
        }

        if (collision.gameObject.tag == "IceCreamApple" && EnvanterStatus < MaxItemAmount)
        {

            if (collision.transform.GetComponentInParent<IceCreamMachine>() != null)//for testing ,maybe I dont take this from machine
            {
                collision.transform.GetComponentInParent<IceCreamMachine>().IceCreamGone();
            }
            
            SoundManager.PlaySound("CollectSFX");
            GameObject Item = Instantiate(CollectedICApple, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject.transform.gameObject);


            Item.GetComponent<CollectedItem>().Id = -1; //for collected

            Item.GetComponent<CollectedItem>().PotansParent = EnvanterItemPos[EnvanterStatus].transform; //for collected

            Item.GetComponent<CollectedItem>().EndPoint = EnvanterItemPos[EnvanterStatus].transform;
            EnvanterItems[EnvanterStatus] = Item;
            EnvanterStatus++;


            if (PlayerPrefs.GetInt("TutorialDone") == 0) //tutorial just for apple
            {
                GameManagerObject.GetComponent<GameManager>().TutorialIceCreamSelled();
            }
        }


        if (collision.gameObject.tag == "IceCreamPear" && EnvanterStatus < MaxItemAmount)
        {

            if (collision.transform.GetComponentInParent<IceCreamMachine>() != null)//for testing ,maybe I dont take this from machine
            {
                collision.transform.GetComponentInParent<IceCreamMachine>().IceCreamGone();
            }

            SoundManager.PlaySound("CollectSFX");
            GameObject Item = Instantiate(CollectedICPear, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject.transform.gameObject);


            Item.GetComponent<CollectedItem>().Id = -1; //for collected

            Item.GetComponent<CollectedItem>().PotansParent = EnvanterItemPos[EnvanterStatus].transform; //for collected

            Item.GetComponent<CollectedItem>().EndPoint = EnvanterItemPos[EnvanterStatus].transform;
            EnvanterItems[EnvanterStatus] = Item;
            EnvanterStatus++;
        }

        if (collision.gameObject.tag == "IceCreamBanana" && EnvanterStatus < MaxItemAmount)
        {

            if (collision.transform.GetComponentInParent<IceCreamMachine>() != null)//for testing ,maybe I dont take this from machine
            {
                collision.transform.GetComponentInParent<IceCreamMachine>().IceCreamGone();
            }
        
            SoundManager.PlaySound("CollectSFX");
            GameObject Item = Instantiate(CollectedICBanana, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject.transform.gameObject);


            Item.GetComponent<CollectedItem>().Id = -1; //for collected

            Item.GetComponent<CollectedItem>().PotansParent = EnvanterItemPos[EnvanterStatus].transform; //for collected

            Item.GetComponent<CollectedItem>().EndPoint = EnvanterItemPos[EnvanterStatus].transform;
            EnvanterItems[EnvanterStatus] = Item;
            EnvanterStatus++;
        }

        if (collision.gameObject.tag == "Milk" && EnvanterStatus < MaxItemAmount)
        {

            SoundManager.PlaySound("CollectSFX");
            GameObject Item = Instantiate(CollectedMilk, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject.transform.gameObject);


            Item.GetComponent<CollectedItem>().Id = -1; //for collected
            Item.GetComponent<CollectedItem>().PotansParent = EnvanterItemPos[EnvanterStatus].transform; //for collected

            Item.GetComponent<CollectedItem>().EndPoint = EnvanterItemPos[EnvanterStatus].transform;
            EnvanterItems[EnvanterStatus] = Item;
            EnvanterStatus++;


            if (PlayerPrefs.GetInt("TutorialDone") == 0) //for tutorial
            {
                GameManagerObject.GetComponent<GameManager>().TutorialMilkDone();
            }
                        
        }



        if (collision.gameObject.tag == "WantAppleIC" &&
            !isIceCreamTime && collision.gameObject.transform.parent.GetComponent<NPC>().JobDone == false) //cutomer
        {           
                StartCoroutine(ReleaseIceCream(collision.gameObject.transform.parent.gameObject, "CollectedIceCreamApple"));        
        }
        if (collision.gameObject.tag == "WantPearIC" && 
            !isIceCreamTime && collision.gameObject.transform.parent.GetComponent<NPC>().JobDone == false) //cutomer
        {      
                StartCoroutine(ReleaseIceCream(collision.gameObject.transform.parent.gameObject, "CollectedIceCreamPear"));          
        }
        if (collision.gameObject.tag == "WantBananaIC"&& 
            !isIceCreamTime && collision.gameObject.transform.parent.GetComponent<NPC>().JobDone == false) //cutomer
        {           
                StartCoroutine(ReleaseIceCream(collision.gameObject.transform.parent.gameObject, "CollectedIceCreamBanana"));          
        }


        if (collision.gameObject.tag == "Trash" && EnvanterStatus > 0)
        {
            SoundManager.PlaySound("CollectSFX");          
      
            GameObject TrashObject = EnvanterItems[EnvanterStatus - 1];         

            EnvanterItems[EnvanterStatus - 1] = null;
            EnvanterStatus--;


            TrashObject.transform.SetParent(null);
            TrashObject.GetComponent<CollectedItem>().Id = -3; //trashId

            TrashObject.GetComponent<CollectedItem>().CollectedFor = "Trash";
            TrashObject.GetComponent<CollectedItem>().isReached = false;
            TrashObject.GetComponent<CollectedItem>().EndPoint = collision.gameObject.transform;
        }


        if (collision.gameObject.tag == "Cow" && collision.GetComponent<Cow>().isReadyToMilk == true)
        {

            EmptyBottle.SetActive(false);
            SoundManager.PlaySound("CowMilkSFX");

            collision.GetComponent<Cow>().isMilked();

        }
    }
   
    IEnumerator ShakeTheTree(GameObject Tree)
    {
        isShakingTreeAnimPlaying = true;
        anim.SetInteger("isTreeShakeTime", 0);
        anim.SetInteger("isTreeShakeTime", Random.Range(1, 6));
        yield return new WaitForSeconds(1.2f);
        if (isShakingTree)
        {
           // SoundManager.PlaySound("PunchSFX");
            SoundManager.PlaySound("HitSFX");
            Tree.transform.parent.GetChild(0).GetComponent<FruitTree>().DropFruit();
        }
        yield return new WaitForSeconds(.2f);
        isShakingTreeAnimPlaying = false;
    }
 
    IEnumerator ReleaseApples(GameObject Machine)
    {
        isIceCreamMachineAppleTime = true;
        yield return new WaitForSeconds(.2f);
        int EnvItemAmount = EnvanterStatus;
        for (int i = 0; i < EnvItemAmount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            if (EnvanterItems[EnvanterStatus - 1].tag == "CollectedApple" && Machine.GetComponent<IceCreamMachine>().isAvailable == true &&
                Machine.GetComponent<IceCreamMachine>().FruitsCome == false && applefruit < 3 && isTouchingICMapple)
            {
                EnvanterItems[EnvanterStatus - 1].transform.SetParent(null);

                int FruitAmount = Machine.GetComponent<IceCreamMachine>().FruitAmount;

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().Id = applefruit;

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().CollectedFor = "AppleIC";
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().isReached = false;
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().EndPoint = Machine.GetComponent<IceCreamMachine>().Fruits[applefruit].transform;

                SoundManager.PlaySound("GiveSFX");
                applefruit++;
                if (applefruit == 3 && Machine.GetComponent<IceCreamMachine>().MilkCome == true) //machine work
                {
                    Machine.GetComponent<IceCreamMachine>().FruitAmount = 3;

                    Machine.GetComponent<IceCreamMachine>().isAvailable = false;
                    applefruit = 0;


                    /////////////////////////////////////
                    if (PlayerPrefs.GetInt("TutorialDone") == 0) //tutorial
                    {
                        GameManagerObject.GetComponent<GameManager>().TutorialIceCreamDone();
                    }
                    /////////////////////////////////////


                }
                if (FruitAmount == 3)
                {
                    Machine.GetComponent<IceCreamMachine>().FruitsCome = true;
                }

                EnvanterItems[EnvanterStatus - 1] = null;


                if (Sorter != 0)
                {
                    EnvanterItems[EnvanterStatus - 1] = EnvanterItems[EnvanterStatus - 1 + Sorter];
                    EnvanterItems[EnvanterStatus - 1].transform.position = EnvanterItemPos[EnvanterStatus - 1].transform.position;
                    EnvanterItems[EnvanterStatus - 1].transform.SetParent(EnvanterItemPos[EnvanterStatus - 1].transform);
                    EnvanterStatus += Sorter - 1;
                }
                else
                {
                    EnvanterStatus--;
                }
                Sorter = 0;
            }
            else if (EnvanterItems[EnvanterStatus - 1].tag == "CollectedMilk" && Machine.GetComponent<IceCreamMachine>().isAvailable == true && Machine.GetComponent<IceCreamMachine>().MilkCome == false && isTouchingICMapple)
            {
                /////////////////////////////////////
                if (PlayerPrefs.GetInt("TutorialDone") == 0) //tutorial
                {
                    GameManagerObject.GetComponent<GameManager>().TutorialMachineDone();
                }
                /////////////////////////////////////



                EnvanterItems[EnvanterStatus - 1].transform.SetParent(null);

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().Id = -2;//for milk
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().CollectedFor = "AppleIC";
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().isReached = false;
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().EndPoint = Machine.GetComponent<IceCreamMachine>().MilkObject.transform;

                Machine.GetComponent<IceCreamMachine>().MilkCome = true;

                SoundManager.PlaySound("GiveSFX");

                if (applefruit == 3 && Machine.GetComponent<IceCreamMachine>().MilkCome == true)
                {

                    Machine.GetComponent<IceCreamMachine>().FruitAmount = 3;
                    Machine.GetComponent<IceCreamMachine>().isAvailable = false;
                    applefruit = 0;
                }


                EnvanterItems[EnvanterStatus - 1] = null;


                if (Sorter != 0)
                {
                    EnvanterItems[EnvanterStatus - 1] = EnvanterItems[EnvanterStatus - 1 + Sorter];
                    EnvanterItems[EnvanterStatus - 1].transform.position = EnvanterItemPos[EnvanterStatus - 1].transform.position;
                    EnvanterItems[EnvanterStatus - 1].transform.SetParent(EnvanterItemPos[EnvanterStatus - 1].transform);
                    EnvanterStatus += Sorter - 1;
                }
                else
                {
                    EnvanterStatus--;
                }
                Sorter = 0;
            }
            else
            {
                Sorter++;
                EnvanterStatus--;
            }
        }

        if (Sorter != 0)
        {
            EnvanterStatus += Sorter;
            Sorter = 0;
        }
        yield return new WaitForSeconds(0.2f);
        isIceCreamMachineAppleTime = false;
    }
    IEnumerator ReleasePears(GameObject Machine)
    {
        isIceCreamMachinePearTime = true;
        yield return new WaitForSeconds(.2f);
        int EnvItemAmount = EnvanterStatus;
        for (int i = 0; i < EnvItemAmount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            if (EnvanterItems[EnvanterStatus - 1].tag == "CollectedPear" && Machine.GetComponent<IceCreamMachine>().isAvailable == true && 
                Machine.GetComponent<IceCreamMachine>().FruitsCome == false && pearfruit < 3 && isTouchingICMpear)
            {
                EnvanterItems[EnvanterStatus - 1].transform.SetParent(null);

                int FruitAmount = Machine.GetComponent<IceCreamMachine>().FruitAmount;

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().Id = pearfruit;

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().CollectedFor = "PearIC";
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().isReached = false;
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().EndPoint = Machine.GetComponent<IceCreamMachine>().Fruits[pearfruit].transform;

                SoundManager.PlaySound("GiveSFX");
                pearfruit++;
                if (pearfruit == 3 && Machine.GetComponent<IceCreamMachine>().MilkCome == true)
                {
                    Machine.GetComponent<IceCreamMachine>().FruitAmount = 3;

                    Machine.GetComponent<IceCreamMachine>().isAvailable = false;
                    pearfruit = 0;
                }
                if (FruitAmount == 3)
                {
                    Machine.GetComponent<IceCreamMachine>().FruitsCome = true;
                }

                EnvanterItems[EnvanterStatus - 1] = null;


                if (Sorter != 0)
                {
                    EnvanterItems[EnvanterStatus - 1] = EnvanterItems[EnvanterStatus - 1 + Sorter];
                    EnvanterItems[EnvanterStatus - 1].transform.position = EnvanterItemPos[EnvanterStatus - 1].transform.position;
                    EnvanterItems[EnvanterStatus - 1].transform.SetParent(EnvanterItemPos[EnvanterStatus - 1].transform);
                    EnvanterStatus += Sorter - 1;
                }
                else
                {
                    EnvanterStatus--;
                }
                Sorter = 0;
            }
            else if (EnvanterItems[EnvanterStatus - 1].tag == "CollectedMilk" && Machine.GetComponent<IceCreamMachine>().isAvailable == true && Machine.GetComponent<IceCreamMachine>().MilkCome == false && isTouchingICMpear)
            {

                EnvanterItems[EnvanterStatus - 1].transform.SetParent(null);

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().Id = -2;//for milk
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().CollectedFor = "PearIC";
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().isReached = false;
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().EndPoint = Machine.GetComponent<IceCreamMachine>().MilkObject.transform;
                SoundManager.PlaySound("GiveSFX");
                Machine.GetComponent<IceCreamMachine>().MilkCome = true;
                if (pearfruit == 3 && Machine.GetComponent<IceCreamMachine>().MilkCome == true)
                {

                    Machine.GetComponent<IceCreamMachine>().FruitAmount = 3;
                    Machine.GetComponent<IceCreamMachine>().isAvailable = false;
                    pearfruit = 0;
                }
             
                EnvanterItems[EnvanterStatus - 1] = null;


                if (Sorter != 0)
                {
                    EnvanterItems[EnvanterStatus - 1] = EnvanterItems[EnvanterStatus - 1 + Sorter];
                    EnvanterItems[EnvanterStatus - 1].transform.position = EnvanterItemPos[EnvanterStatus - 1].transform.position;
                    EnvanterItems[EnvanterStatus - 1].transform.SetParent(EnvanterItemPos[EnvanterStatus - 1].transform);
                    EnvanterStatus += Sorter - 1;
                }
                else
                {
                    EnvanterStatus--;
                }
                Sorter = 0;
            }
            else
            {
                Sorter++;
                EnvanterStatus--;
            }
        }

        if (Sorter != 0)
        {
            EnvanterStatus += Sorter;
            Sorter = 0;
        }
        yield return new WaitForSeconds(0.2f);
        isIceCreamMachinePearTime = false;
    }
    IEnumerator ReleaseBananas(GameObject Machine)
    {
        isIceCreamMachineBananaTime = true;
        yield return new WaitForSeconds(.2f);
        int EnvItemAmount = EnvanterStatus;
        for (int i = 0; i < EnvItemAmount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            if (EnvanterItems[EnvanterStatus - 1].tag == "CollectedBanana" && Machine.GetComponent<IceCreamMachine>().isAvailable == true &&
                Machine.GetComponent<IceCreamMachine>().FruitsCome == false && bananafruit < 3 && isTouchingICMbanana)
            {
                EnvanterItems[EnvanterStatus - 1].transform.SetParent(null);

                int FruitAmount = Machine.GetComponent<IceCreamMachine>().FruitAmount;

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().Id = bananafruit;

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().CollectedFor = "BananaIC";
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().isReached = false;
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().EndPoint = Machine.GetComponent<IceCreamMachine>().Fruits[bananafruit].transform;

                SoundManager.PlaySound("GiveSFX");
                bananafruit++;
                if (bananafruit == 3 && Machine.GetComponent<IceCreamMachine>().MilkCome == true)
                {
                    Machine.GetComponent<IceCreamMachine>().FruitAmount = 3;

                    Machine.GetComponent<IceCreamMachine>().isAvailable = false;
                    bananafruit = 0;
                }
                if (FruitAmount == 3)
                {
                    Machine.GetComponent<IceCreamMachine>().FruitsCome = true;
                }

                EnvanterItems[EnvanterStatus - 1] = null;


                if (Sorter != 0)
                {
                    EnvanterItems[EnvanterStatus - 1] = EnvanterItems[EnvanterStatus - 1 + Sorter];
                    EnvanterItems[EnvanterStatus - 1].transform.position = EnvanterItemPos[EnvanterStatus - 1].transform.position;
                    EnvanterItems[EnvanterStatus - 1].transform.SetParent(EnvanterItemPos[EnvanterStatus - 1].transform);
                    EnvanterStatus += Sorter - 1;
                }
                else
                {
                    EnvanterStatus--;
                }
                Sorter = 0;
            }
            else if (EnvanterItems[EnvanterStatus - 1].tag == "CollectedMilk" && Machine.GetComponent<IceCreamMachine>().isAvailable == true && Machine.GetComponent<IceCreamMachine>().MilkCome == false && isTouchingICMbanana)
            {

                EnvanterItems[EnvanterStatus - 1].transform.SetParent(null);              

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().Id = -2;//for milk
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().CollectedFor = "BananaIC";
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().isReached = false;
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().EndPoint = Machine.GetComponent<IceCreamMachine>().MilkObject.transform;
                SoundManager.PlaySound("GiveSFX");
                Machine.GetComponent<IceCreamMachine>().MilkCome = true;
                if (bananafruit == 3 && Machine.GetComponent<IceCreamMachine>().MilkCome == true)
                {

                    Machine.GetComponent<IceCreamMachine>().FruitAmount = 3;
                    Machine.GetComponent<IceCreamMachine>().isAvailable = false;
                    bananafruit = 0;
                }
                
                EnvanterItems[EnvanterStatus - 1] = null;


                if (Sorter != 0)
                {
                    EnvanterItems[EnvanterStatus - 1] = EnvanterItems[EnvanterStatus - 1 + Sorter];
                    EnvanterItems[EnvanterStatus - 1].transform.position = EnvanterItemPos[EnvanterStatus - 1].transform.position;
                    EnvanterItems[EnvanterStatus - 1].transform.SetParent(EnvanterItemPos[EnvanterStatus - 1].transform);
                    EnvanterStatus += Sorter - 1;
                }
                else
                {
                    EnvanterStatus--;
                }
                Sorter = 0;
            }
            else
            {
                Sorter++;
                EnvanterStatus--;
            }
        }

        if (Sorter != 0)
        {
            EnvanterStatus += Sorter;
            Sorter = 0;
        }
        yield return new WaitForSeconds(0.2f);
        isIceCreamMachineBananaTime = false;
    }   
    IEnumerator ReleaseIceCream(GameObject NPC, string ItemTag)
    {
        isIceCreamTime = true;
        bool ICgived = false;

        yield return new WaitForSeconds(.2f);
        int EnvItemAmount = EnvanterStatus;


        for (int i = 0; i < EnvItemAmount; i++)
        {
            yield return new WaitForSeconds(.1f);
            if (EnvanterItems[EnvanterStatus - 1].tag == ItemTag && !ICgived)
            {
                ICgived = true;
                EnvanterItems[EnvanterStatus - 1].transform.SetParent(null);
               

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().Id = -2;
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().CollectedFor = "IceCreamGO";
                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().isReached = false;

                EnvanterItems[EnvanterStatus - 1].GetComponent<CollectedItem>().EndPoint = NPC.transform;
                NPC.GetComponent<NPC>().JobDone = true;

                SoundManager.PlaySound("MoneySFX");


                EnvanterItems[EnvanterStatus - 1] = null;


                if (Sorter != 0)
                {
                    EnvanterItems[EnvanterStatus - 1] = EnvanterItems[EnvanterStatus - 1 + Sorter];
                    EnvanterItems[EnvanterStatus - 1].transform.position = EnvanterItemPos[EnvanterStatus - 1].transform.position;
                    EnvanterItems[EnvanterStatus - 1].transform.SetParent(EnvanterItemPos[EnvanterStatus - 1].transform);
                    EnvanterStatus += Sorter - 1;
                }
                else
                {
                    EnvanterStatus--;
                }
                Sorter = 0;


                int MoneyG = Random.Range(40, 71);

                for (int j = 0; j < ((MoneyG - 30) / 5); j++)
                {
                    GameObject money = Instantiate(MoneyObjects[Random.Range(0, MoneyObjects.Length)], new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
                    Destroy(money, 0.95f);
                    yield return new WaitForSeconds(.2f);
                }
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + MoneyG);
            //    SoundManager.PlaySound("MoneySFX");
            }
            else
            {
                Sorter++;
                EnvanterStatus--;
            }
        }

        if (Sorter != 0)
        {
            EnvanterStatus += Sorter;
            Sorter = 0;
        }

        isIceCreamTime = false;
    }
    IEnumerator ExpandLoad(GameObject Loader) //+Upgrade
    {
        isExpandTime = true;

        if (Loader.GetComponent<UpgradeLoader>().LoadingImageFillAmount <0.99f && PlayerPrefs.GetInt("Money") >= 10 && Loader.GetComponent<UpgradeLoader>().isActivate) 
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") -10);
            SoundManager.PlaySound("StackSFX");
            Loader.GetComponent<UpgradeLoader>().LoadingImageFillAmount += Loader.GetComponent<UpgradeLoader>().increaseAmount;
            Loader.GetComponent<UpgradeLoader>().Price -= 10;
            GameObject Item = Instantiate(MoneyObject, EnvanterItemPos[2].transform.position, Quaternion.Euler(90, 0, 0));

            Item.GetComponent<CollectedItem>().CollectedFor = "MoneyGO"; 


            Item.GetComponent<CollectedItem>().EndPoint = Loader.GetComponent<UpgradeLoader>().MoneyPoint.transform;
            anim.SetBool("isBuying", true);
        }
        else if (Loader.GetComponent<UpgradeLoader>().LoadingImageFillAmount > 0.99f && Loader.GetComponent<UpgradeLoader>().isActivate)
        {
            Loader.GetComponent<UpgradeLoader>().isActivate = false;
            Debug.Log("LoaderFull");
            Loader.GetComponent<UpgradeLoader>().LoadingImageFillAmount = 1f;
            Loader.GetComponent<UpgradeLoader>().Purchased();

            SoundManager.PlaySound("PurchasedSFX");

            anim.SetBool("isBuying", false);
        }
        else
        {
            anim.SetBool("isBuying", false);
        }  
        yield return new WaitForSeconds(.05f);
        isExpandTime = false;
    }
}