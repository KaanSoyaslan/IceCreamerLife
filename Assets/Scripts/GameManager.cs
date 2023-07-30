using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("============= Upgrade+Expand =============")]
    public GameObject[] Stands;
    public GameObject[] Boards;

    public GameObject ExpandedPearTree;
    public GameObject ExpandedBananaTree;
    public GameObject ExpandedCowArea;

    public GameObject ExpandedPearTreeLOCK;
    public GameObject ExpandedBananaTreeLOCK;
    public GameObject ExpandedCowAreaLOCK;

    public GameObject NpcPointObject; //to get que



    [Header("============= Tutorial =============")]
    public GameObject PlayerObject;
    public GameObject GameCamera;
    public GameObject CowObject;
    public GameObject CowArrow;

    public GameObject MachineObject;
    public GameObject MachineArrow;

    public GameObject StandObject;
    public GameObject StandArrow;
    public GameObject TreeObject;
    public GameObject TreeArrow;



    [Header("============= Test =============")]
    public GameObject OptionsPanel;
    public GameObject GraphPanel;
    public GameObject EnvObjects;
    public GameObject ReadyIceCreams;
    public GameObject DeveloperPanel;



    [Header("============= Sound+SFX =============")]
    public GameObject MusicPlayer;
    public GameObject SfxPlayer;
    public GameObject MusicONimage;
    public GameObject MusicOFFimage;
    public GameObject SfxONimage;
    public GameObject SfxOFFimage;



    [Header("============= Others =============")]
    public GameObject MoveInspector; //before start
    public GameObject LoadingScreen;




    //PlayerPrefs.GetInt("FruitStatus") == 0)  //
    //0  just apple
    //1  apple + pear
    //2  apple + banana
    //3  apple + pear + banana

    void Start()
    {
        if (PlayerPrefs.GetInt("MaxNPCQue") == 0)
        {
            PlayerPrefs.SetInt("MaxNPCQue", 2);

            NpcPointObject.GetComponent<NpcPoints>().maxQue = PlayerPrefs.GetInt("MaxNPCQue");
        }
        else
        {
            NpcPointObject.GetComponent<NpcPoints>().maxQue = PlayerPrefs.GetInt("MaxNPCQue");
        }

        for (int i = 0; i < Stands.Length; i++) 
        {
            if (i == PlayerPrefs.GetInt("ActiveStand"))
            {
                Stands[i].SetActive(true);
            }
            else
            {
                Stands[i].SetActive(false);
            }
        }
        for (int i = 0; i < Boards.Length; i++)
        {
            if (i == PlayerPrefs.GetInt("ActiveBoard"))
            {
                Boards[i].SetActive(true);
            }
            else
            {
                Boards[i].SetActive(false);
            }
        }

        if (PlayerPrefs.GetInt("FruitStatus") == 1)//pear  purchased?? 
        {
            ExpandedPearTree.SetActive(true);
            ExpandedPearTreeLOCK.SetActive(false);
           
        }
        else if (PlayerPrefs.GetInt("FruitStatus") == 2)//banana already purchased??
        {
            ExpandedBananaTree.SetActive(true);
            ExpandedBananaTreeLOCK.SetActive(false);
        
        }
        else if (PlayerPrefs.GetInt("FruitStatus") == 3)
        {
            ExpandedPearTree.SetActive(true);
            ExpandedBananaTree.SetActive(true);
            ExpandedPearTreeLOCK.SetActive(false);
            ExpandedBananaTreeLOCK.SetActive(false);
        }


        if (PlayerPrefs.GetInt("ActiveCow") == 1)
        {
            ExpandedCowArea.SetActive(true);
            ExpandedCowAreaLOCK.SetActive(false);
        }


        if (PlayerPrefs.GetInt("MusicStatus") == 0)//musicNot Set
        {
            PlayerPrefs.SetInt("MusicStatus", 1);
            PlayerPrefs.SetInt("SfxStatus", 1);

            MusicPlayer.SetActive(true);
            SfxPlayer.SetActive(true);
            MusicONimage.SetActive(false);
            MusicOFFimage.SetActive(true);
            SfxONimage.SetActive(false);
            SfxOFFimage.SetActive(true);

        }
        if (PlayerPrefs.GetInt("MusicStatus") == 1)
        {
            MusicPlayer.SetActive(true);
            MusicONimage.SetActive(false);
            MusicOFFimage.SetActive(true);
        }
        else
        {
            MusicPlayer.SetActive(false);
            MusicONimage.SetActive(true);
            MusicOFFimage.SetActive(false);
        }
        if (PlayerPrefs.GetInt("SfxStatus") == 1)
        {
            SfxPlayer.SetActive(true);
            SfxONimage.SetActive(false);
            SfxOFFimage.SetActive(true);
        }
        else
        {
            SfxPlayer.SetActive(false);
            SfxONimage.SetActive(true);
            SfxOFFimage.SetActive(false);
        }


        if (PlayerPrefs.GetInt("TutorialDone") == 0)
        {
            //tutorialStart
            StartCoroutine(Tutorial());
        }


        LoadingScreen.SetActive(false);
    }

    void FixedUpdate()
    {
        if (MoveInspector.activeInHierarchy && Input.touchCount != 0)
        {
            MoveInspector.SetActive(false);
        }
    }


    public void Upgrade(string UpgradeITEM)
    {
        PlayerPrefs.SetInt("Active"+ UpgradeITEM, PlayerPrefs.GetInt("Active"+ UpgradeITEM) + 1);

        if (UpgradeITEM == "Stand")
        {
            Stands[PlayerPrefs.GetInt("Active" + UpgradeITEM)].SetActive(true);
            Stands[PlayerPrefs.GetInt("Active" + UpgradeITEM) - 1].SetActive(false);
        }
        else if (UpgradeITEM == "Board")
        {
            Boards[PlayerPrefs.GetInt("Active" + UpgradeITEM)].SetActive(true);
            Boards[PlayerPrefs.GetInt("Active" + UpgradeITEM) - 1].SetActive(false);
        }
        else if (UpgradeITEM == "TreePear")
        {
            ExpandedPearTree.SetActive(true);
            ExpandedPearTreeLOCK.SetActive(false);


            if (PlayerPrefs.GetInt("FruitStatus") == 0)//first next fruit
            {
                PlayerPrefs.SetInt("FruitStatus", 1);
            }
            else if (PlayerPrefs.GetInt("FruitStatus") == 2)//banana already purchased??
            {
                PlayerPrefs.SetInt("FruitStatus", 3);
            }
            
        }
        else if (UpgradeITEM == "TreeBanana")
        {
            ExpandedBananaTree.SetActive(true);
            ExpandedBananaTreeLOCK.SetActive(false);


            if (PlayerPrefs.GetInt("FruitStatus") == 0)//first next fruit
            {
                PlayerPrefs.SetInt("FruitStatus", 2);
            }
            else if (PlayerPrefs.GetInt("FruitStatus") == 1)//pear already purchased??
            {
                PlayerPrefs.SetInt("FruitStatus", 3);
            }

        }
        else if (UpgradeITEM == "Cow")
        {
            ExpandedCowArea.SetActive(true);
            ExpandedCowAreaLOCK.SetActive(false);
        }



        if (PlayerPrefs.GetInt("MaxNPCQue") < 10) //Npc que long ++
        {
            PlayerPrefs.SetInt("MaxNPCQue", PlayerPrefs.GetInt("MaxNPCQue") +1);

            NpcPointObject.GetComponent<NpcPoints>().maxQue = PlayerPrefs.GetInt("MaxNPCQue");
        }

    }

    
    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(2f);
        GameCamera.GetComponent<CameraFollow>().target = CowObject.transform;
        CowArrow.SetActive(true);
        GameCamera.GetComponent<CameraFollow>().isShowTime = true;

        yield return new WaitForSeconds(3f);

        GameCamera.GetComponent<CameraFollow>().target = PlayerObject.transform; //cameraCome back


        yield return new WaitForSeconds(2f);
        GameCamera.GetComponent<CameraFollow>().isShowTime = false;

    }
    public void TutorialMilkDone()
    {
       
        StartCoroutine(TutorialMachineTime());
    }
    IEnumerator TutorialMachineTime()
    {
        CowArrow.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameCamera.GetComponent<CameraFollow>().target = MachineObject.transform;
        MachineArrow.SetActive(true);
        GameCamera.GetComponent<CameraFollow>().isShowTime = true;

        yield return new WaitForSeconds(3f);

        GameCamera.GetComponent<CameraFollow>().target = PlayerObject.transform; //cameraCome back
        yield return new WaitForSeconds(2f);
        GameCamera.GetComponent<CameraFollow>().isShowTime = false;
    }

    public void TutorialMachineDone()
    {
        StartCoroutine(TutorialTreeTime());
    }
    IEnumerator TutorialTreeTime()
    {
        MachineArrow.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameCamera.GetComponent<CameraFollow>().target = TreeObject.transform;
        TreeArrow.SetActive(true);
        GameCamera.GetComponent<CameraFollow>().isShowTime = true;

        yield return new WaitForSeconds(3f);

        GameCamera.GetComponent<CameraFollow>().target = PlayerObject.transform; //cameraCome back
        yield return new WaitForSeconds(2f);
        GameCamera.GetComponent<CameraFollow>().isShowTime = false;
    }

    public void TutorialIceCreamDone()
    {

        StartCoroutine(TutorialCustomerTime());
    }
    IEnumerator TutorialCustomerTime()
    {
        TreeArrow.SetActive(false);
        MachineArrow.SetActive(false);
        yield return new WaitForSeconds(2f);
        GameCamera.GetComponent<CameraFollow>().target = StandObject.transform;
        StandArrow.SetActive(true);
        GameCamera.GetComponent<CameraFollow>().isShowTime = true;

        yield return new WaitForSeconds(3f);

        GameCamera.GetComponent<CameraFollow>().target = PlayerObject.transform; //cameraCome back


        yield return new WaitForSeconds(2f);
        GameCamera.GetComponent<CameraFollow>().isShowTime = false;

    }

    public void TutorialIceCreamSelled()
    {
        StandArrow.SetActive(false);
        TreeArrow.SetActive(false);
        MachineArrow.SetActive(false);

        Debug.Log("TutorialDone");
        PlayerPrefs.SetInt("TutorialDone", 1);

    }


    public void OptionsPanelOnOff()
    {

        if (OptionsPanel.activeInHierarchy)
        {
            OptionsPanel.SetActive(false);
        }
        else
        {
            OptionsPanel.SetActive(true);
        }
       
    }
    public void MusicONOFF(bool OnOff)
    {
        MusicPlayer.SetActive(OnOff);
        MusicONimage.SetActive(!OnOff);
        MusicOFFimage.SetActive(OnOff);

        if (OnOff == true)
        {
            PlayerPrefs.SetInt("MusicStatus", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MusicStatus", 2);
        }
     
    }
    public void SfxONOFF(bool OnOff)
    {
        SfxPlayer.SetActive(OnOff);
        SfxONimage.SetActive(!OnOff);
        SfxOFFimage.SetActive(OnOff);

        if (OnOff == true)
        {
            PlayerPrefs.SetInt("SfxStatus", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SfxStatus", 2);
        }
    }



    //Testing Buttons
    public void DeveloperPanelOnOff()
    {
        if (DeveloperPanel.activeInHierarchy)
        {
            DeveloperPanel.SetActive(false);
        }
        else
        {
            DeveloperPanel.SetActive(true);
        }
        
    }

    public void ResetSaves()
    {
        PlayerPrefs.SetInt("ActiveStand", 0);
        PlayerPrefs.SetInt("StandCurrentPrice", 0);
        PlayerPrefs.SetInt("ActiveBoard", 0);
        PlayerPrefs.SetInt("BoardCurrentPrice", 0);


        PlayerPrefs.SetInt("ActiveTreePear", 0);
        PlayerPrefs.SetInt("TreePearCurrentPrice", 0);

        PlayerPrefs.SetInt("ActiveTreeBanana", 0);
        PlayerPrefs.SetInt("TreeBananaCurrentPrice", 0);
        PlayerPrefs.SetInt("FruitStatus", 0);


        PlayerPrefs.SetInt("ActiveCow", 0);
        PlayerPrefs.SetInt("CowCurrentPrice", 0);


        PlayerPrefs.SetInt("MaxNPCQue", 2);
        PlayerPrefs.SetInt("Money", 0);



        SceneManager.LoadScene("Game");


    }
    public void TutorialResetBTN()
    {
        PlayerPrefs.SetInt("TutorialDone", 0);
    }
    public void GiveMoney(int Num)
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + Num);

    }
    public void GraphPanelOnOff(bool OnOff)
    {
        GraphPanel.SetActive(OnOff);
    }
    public void EnvObjectsOnOff(bool OnOff)
    {
        EnvObjects.SetActive(OnOff);
    }
    public void ReadyIceCreamsOnOff(bool OnOff)
    {
        ReadyIceCreams.SetActive(OnOff);
    }



}
