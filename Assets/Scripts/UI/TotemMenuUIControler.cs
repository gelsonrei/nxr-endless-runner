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

        //ranking
        buttons[1].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[3]);
        });

        //credits
        buttons[2].onClick.AddListener(
        () => {
            m_audioSource.Play();

            MenuCanvasManager.Instance.ChangeScreen(MenuCanvasManager.Instance.screens[4]);
        });

    }

    void OnDisable()
    {
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
        buttons[2].onClick.RemoveAllListeners();
    }

}
