using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : MonoBehaviour
{
    [Header("============= Move =============")]
    Animator anim;
    public Transform LookAt;
    public float speed;
    public bool isTooFar;

    public Transform Target;


    public bool FenceAlert = false;
    public bool isReadyToMilk = true;


    [Header("============= Item Throw =============")]
    public GameObject MilkObject;

    public float ForceHorizontal; //9
    public float ForceVertical; //5 
    public float ForceMultiplier;//15


    [Header("============= Patrol =============")]
    public Transform WaitTarget;
    public Transform[] PatrolPoints;
    public bool patrolWait;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();

        Target = GameObject.FindGameObjectWithTag("Player").transform;


        LookAt = Target;

        WaitTarget = PatrolPoints[Random.Range(0, PatrolPoints.Length)];
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, Target.position) > 4.5f) //too far
        {
            isTooFar = true;


            if(!FenceAlert && isReadyToMilk && Player.isSneekTime)
            {
                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", false);
            }
        }
        else if (!isTooFar && !FenceAlert && isReadyToMilk && Player.isSneekTime)  //run
        {
            anim.SetBool("isRunning", true);
            anim.SetBool("isWalking", false);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, speed * Time.deltaTime);
            var lookPos = LookAt.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos * -1);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20);
        }      
        else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
            isTooFar = false;
        }



        if (Vector3.Distance(transform.position, WaitTarget.position) < 0.01f  && isTooFar && !Player.isSneekTime) 
        {

            if (!patrolWait)
            {
                patrolWait = true;
                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", false);
                StartCoroutine(WaitForPatrol());
            }

        }
        else if (isTooFar && !patrolWait && isReadyToMilk && Vector3.Distance(transform.position, Target.position) > 5.5f)  //player too far
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", true);
            transform.position = Vector3.MoveTowards(transform.position, WaitTarget.position,- speed/2f * Time.deltaTime);
            var lookPos = WaitTarget.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos * 1);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20);
        }
     
      






    }

    public void isMilked()
    {
        isReadyToMilk = false;
       
        StartCoroutine(SpawnMilk());
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Fence")
        {
            FenceAlert = true;
        }

    }
    private void OnTriggerExit(Collider collision)
    {

        if (collision.gameObject.tag == "Fence")
        {
            FenceAlert = false;
        }

    }

    IEnumerator SpawnMilk()
    {
        int MilkAmount = Random.Range(1, 3);
        for (int i = 0; i < MilkAmount; i++)
        {
            GameObject Milk = Instantiate(MilkObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z), Quaternion.identity);
            Vector3 force = new Vector3(Random.Range(-ForceHorizontal, ForceHorizontal), ForceVertical, Random.Range(-ForceHorizontal, ForceHorizontal));
            Milk.GetComponent<Rigidbody>().AddForce(force* ForceMultiplier);
            yield return new WaitForSeconds(.1f);
        }
        anim.SetBool("isFeeding", true);

        //reload anim

        Debug.Log("Milk empty");

        yield return new WaitForSeconds(10f);
        //reload anim done
        Debug.Log("Milk fulll");
        isReadyToMilk = true;
        anim.SetBool("isFeeding", false);

    }

    IEnumerator WaitForPatrol()
    {
        WaitTarget = PatrolPoints[Random.Range(0, PatrolPoints.Length)];

        
        yield return new WaitForSeconds(Random.Range(2f,4f));
        patrolWait = false;

    }
}