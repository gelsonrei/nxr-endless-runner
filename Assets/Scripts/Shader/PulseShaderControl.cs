using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class PulseShaderControl : MonoBehaviour
{
    [SerializeField]
    private Material[] myMaterials;

    [SerializeField]
    private string[] propertyName = { "_GlowColor", "_Frequency", "_MinPulseVal", "_MaxPulseVal" };

    private Color c;

    void Awake()
    {
       myMaterials = GetComponentInChildren<SkinnedMeshRenderer>().materials;    
    }

    void Start()
    {
        c = myMaterials[0].GetColor(propertyName[0]); 
    }

    void Update()
    {
        
    }

    public void enableShader()
    {
        //Debug.Log(myMaterials[0].name);
        Color tempColor = c;
        float alpha = 1.0f;
        tempColor.a = alpha;

        myMaterials[0].SetColor(propertyName[0], tempColor);
    }

    public void disableShader()
    {
       //Debug.Log(myMaterials[0].name);
       Color tempColor = c;
        float alpha = 0.0f;
        tempColor.a = alpha;

        myMaterials[0].SetColor(propertyName[0], tempColor);
    }
}
