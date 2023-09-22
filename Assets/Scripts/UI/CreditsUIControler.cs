using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CreditsUIControler : MonoBehaviour
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
        //home
        buttons[0].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[1]);
        });

        //back
        buttons[1].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[1]);
        });
    }

    void OnDisable()
    {
        //home
        buttons[0].onClick.RemoveAllListeners();

        //back
        buttons[1].onClick.RemoveAllListeners();
    }

}
