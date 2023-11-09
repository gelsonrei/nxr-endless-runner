using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class AppMenuUIControler : MonoBehaviour
{
    private Button[] buttons;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnEnable() 
    {
        //close menu
        buttons[0].onClick.AddListener(
        () => {
            gameObject.SetActive(false);
        });

        //quit
        buttons[1].onClick.AddListener(
        () => {
            Application.Quit();
        });
    }

    private void OnDisable()
    {

    }
}
