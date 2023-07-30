using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeLoader : MonoBehaviour
{
    public bool isActivate;

    [Header("============= UI =============")]
    public float LoadingImageFillAmount;
    public TMP_Text priceTXT;
    public Image LodingImage;


    [Header("============= Object =============")]
    public GameObject MoneyPoint;
    public string LoaderType;
    public GameObject CanvasObject;
    public GameObject GameManagerObject; //for upgrade save


    [Header("============= PriceControl =============")]
    public int Price; //player change this
    public float increaseAmount;

    //prices
    int[] StandPrices = new int[4];
    int[] BoardPrices = new int[4];
    int[] PearTreePrices = new int[1];
    int[] BananaTreePrices = new int[1];
    int[] CowPrices = new int[1];
    int[] UsedPrices;
  


    void Start()
    {
        PriceValues();
    }


    public void PriceValues()
    {
        StandPrices[0] = 100;
        StandPrices[1] = 200;
        StandPrices[2] = 350;
        StandPrices[3] = 550;
         
        BoardPrices[0] = 150;
        BoardPrices[1] = 250;
        BoardPrices[2] = 400;
        BoardPrices[3] = 600;


        PearTreePrices[0] = 250;
        BananaTreePrices[0] = 250;


        CowPrices[0] = 400;


        if (LoaderType == "Stand")
        {
            UsedPrices = StandPrices;
        }
        else if (LoaderType == "Board")
        {
            UsedPrices = BoardPrices;
        }
        else if (LoaderType == "TreePear")
        {
            UsedPrices = PearTreePrices;
        }
        else if (LoaderType == "TreeBanana")
        {
            UsedPrices = BananaTreePrices;
        }
        else if (LoaderType == "Cow")
        {
            UsedPrices = CowPrices;
        }


        if (PlayerPrefs.GetInt("Active" + LoaderType) == UsedPrices.Length)//max control
        {
            gameObject.SetActive(false);
            Price = 999;
            isActivate = false;


        }
        else
        {
            Price = UsedPrices[PlayerPrefs.GetInt("Active" + LoaderType)];

            increaseAmount = (float)10 / (float)Price;
            isActivate = true;



            if (PlayerPrefs.GetInt(LoaderType + "CurrentPrice") !=0)
            {
               
                LoadingImageFillAmount = increaseAmount /10 * (float)(Price - PlayerPrefs.GetInt(LoaderType + "CurrentPrice"));


                Price = PlayerPrefs.GetInt(LoaderType + "CurrentPrice");
            }
            else
            {

                LoadingImageFillAmount = 0f;
            }

        }

    }


    void FixedUpdate()
    {


        PlayerPrefs.SetInt(LoaderType + "CurrentPrice", Price);
        
     
        LodingImage.fillAmount = LoadingImageFillAmount;
        priceTXT.text = "$" + Price;



    }

    public void Purchased()
    {

            isActivate = false;
            StartCoroutine(UPGRADE());
        

          
    }

  
    IEnumerator UPGRADE()
    {
        CanvasObject.SetActive(false);
        GameManagerObject.GetComponent<GameManager>().Upgrade(LoaderType);
        yield return new WaitForSeconds(3f);

       

        if (PlayerPrefs.GetInt("Active" + LoaderType) == UsedPrices.Length)//max
        {
            gameObject.SetActive(false);
            Price = 999;
        }
        else
        {
            Price = UsedPrices[PlayerPrefs.GetInt("Active"+LoaderType)];

            increaseAmount = (float)10 / (float)Price;
            isActivate = true;

            LoadingImageFillAmount = 0f;
        }
        CanvasObject.SetActive(true);




    }
}
