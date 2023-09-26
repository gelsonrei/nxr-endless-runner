using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LGPDPanelControler : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    void OnEnable()
    {
        gameObject.GetComponent<Animator>().SetBool("show", true);

        foreach (Button b in gameObject.GetComponentsInChildren<Button>() )
        {
            b.onClick.AddListener(delegate
            {
                gameObject.GetComponent<Animator>().SetBool("show", false);
            });
        }
    }

    void OnDisable()
    {
        foreach (Button b in gameObject.GetComponentsInChildren<Button>() )
        {
            b.onClick.RemoveAllListeners();
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
