using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    public Transform target;
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeed;
    public float smoothedSpeed;
    public bool isShowTime;
    private void Start()
    {
        isShowTime = true;

        if(PlayerPrefs.GetInt("TutorialDone") !=0)
        {
            StartCoroutine(CameraToPlayer()); //player running from camera
        }

    }

    void Update()
    {

        if (!isShowTime)
        {       

            TargetPointCalculate();
        }

        if (target != null && isShowTime)
        {
            FollowObject();
        }
       
    }

    private void TargetPointCalculate()
    {
        gameObject.transform.position = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z -6);     
    }

    private void FollowObject()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothedSpeed);
        transform.position = smoothedPosition;

        if (Vector3.Distance(transform.position, target.position + offset) < 0.5f && target.tag == GameObject.FindGameObjectWithTag("Player").tag)
        {
            isShowTime = false;

        }
    }


    IEnumerator CameraToPlayer() //player running from camera
    {
        
        yield return new WaitForSeconds(3f);
        isShowTime = false;

    }

}