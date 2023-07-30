using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectFader : MonoBehaviour
{
    public float fadeSpeed, fadeAmount;
    float originalOpacity;
    public Material Mat;
    public Material Mat1;
    TMP_Text tmp_Text;
    public static bool DoFade= false;
    public bool isTmp;

    void Start()
    {

        if (isTmp) //text ?
        {
            tmp_Text= GetComponent<TextMeshProUGUI>();
            originalOpacity = tmp_Text.color.a;
        }
        else
        {
            Mat = GetComponent<Renderer>().material;
            originalOpacity = Mat.color.a;


            MeshRenderer renderer = gameObject.GetComponentInChildren<MeshRenderer>();

            Material[] mats = renderer.materials;

            mats[0] = Mat;

            renderer.materials = mats;
        }
    }

    void Update()
    {
        if (DoFade && isTmp)
        {
            FadeNowTMP();
        }
        else if (DoFade && !isTmp)
        {
            MeshRenderer renderer = gameObject.GetComponentInChildren<MeshRenderer>();

            Material[] mats = renderer.materials;

            mats[0] = Mat1;
 
            renderer.materials = mats;

            FadeNow();
        }
        else if (!DoFade && isTmp)
        {
            ResetFadeTMP();
        }
        else
        {
            MeshRenderer renderer = gameObject.GetComponentInChildren<MeshRenderer>();

            Material[] mats = renderer.materials;

            mats[0] = Mat;

            renderer.materials = mats;

            ResetFade();
        }
    }
    void FadeNow()
    {
        Color currentColor = Mat1.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed));

        Mat1.color = smoothColor;
    }
    void ResetFade()
    {
        Color currentColor = Mat.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed));

        Mat1.color = smoothColor;
    }
    void FadeNowTMP()
    {
        Color currentColor = tmp_Text.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, fadeAmount, fadeSpeed));

        tmp_Text.color = smoothColor;
    }
    void ResetFadeTMP()
    {
        Color currentColor = tmp_Text.color;
        Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, originalOpacity, fadeSpeed));

        tmp_Text.color = smoothColor;
    }
}
