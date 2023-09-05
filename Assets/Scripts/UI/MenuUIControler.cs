using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class MenuUIControler : MonoBehaviour
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
        //play
        buttons[0].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[4]);
            //MenuCanvasManager.Instance.LoadScene("Game");
        });

        //config
        buttons[1].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[2]);
        });

        //credits
        buttons[2].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[3]);
        });
    }

    void OnDisable()
    {
        //play
        buttons[0].onClick.RemoveAllListeners();

        //config
        buttons[1].onClick.RemoveAllListeners();

        //credits
        buttons[2].onClick.RemoveAllListeners();
    }

}
