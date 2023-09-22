using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuCanvasManager : MonoBehaviour
{
    public static MenuCanvasManager Instance;

    [Header("Screens")]
    public GameObject[] screens;

    public GameObject loaderScreen;
    public GameObject transitionScreen;

    private AudioSource audioSource;
    private UnityEvent transitionEvent;
    private GraphicRaycaster graphicRaycaster;

    private void Awake()
    {
        if ( Instance != null && Instance != this ) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
        }

        audioSource = GetComponent<AudioSource>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();

        transitionEvent = transitionScreen.GetComponent<SceneTransitionShaderControl>().OnTransitionDone;
    }

    void Start()
    {
        ShowScreen(screens[0]);
        ChangeScreen(screens[1], 2);
    }

    void Update()
    {
        
    }

    public void ChangeScreen(GameObject screen, int delay = 0, Action callback = null)
    {
        graphicRaycaster.enabled = false;
        
        if (delay > 0)
        {
            StartCoroutine(Delay(delay));
        }
        
        transitionScreen.GetComponent<SceneTransitionShaderControl>().MakeTransition(1);
        transitionEvent.AddListener(delegate {
            transitionEvent.RemoveAllListeners();

            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i].activeSelf)
                {
                    screens[i].SetActive(false);
                }
            }

            screen.SetActive(true);

            transitionScreen.GetComponent<SceneTransitionShaderControl>().MakeTransition(1);
            transitionEvent.AddListener(delegate {
                transitionEvent.RemoveAllListeners();  

                graphicRaycaster.enabled = true;

                if (callback != null)
                {
                    callback?.Invoke();
                }     
            });
        });
    }

    private IEnumerator Delay(int delay)
    {
        yield return new WaitForSeconds(delay);
    }

    private void ShowScreen(GameObject screen)
    {
        screen.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        ChangeScreen(loaderScreen,0, delegate { loaderScreen.GetComponent<SceneManagement>().LoadScene(sceneName); } );
    }
}