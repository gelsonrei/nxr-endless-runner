using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class TotemMenuUIControler : MonoBehaviour
{
    private AudioSource m_audioSource;
    private Button[] buttons;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnEnable()
    {
        //leads
        buttons[0].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[2]);
            //MenuCanvasManager.Instance.LoadScene("Game");
        });

    }

    void OnDisable()
    {
        //leads
        buttons[0].onClick.RemoveAllListeners();

    }

}
