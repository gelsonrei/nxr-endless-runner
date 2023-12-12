using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static MenuCanvasManager Instance;

    public GameObject loaderScreen;
    public GameObject transitionScreen;

    private UnityEvent transitionEvent;
    public string gameScene;
	

    private void Awake()
    {
        transitionEvent = transitionScreen.GetComponent<SceneTransitionShaderControl>().OnTransitionDone;
    }

    void Start()
    {
		LoadScene(gameScene);
    }

    void Update()
    {

    }

    public void ChangeScreen(GameObject screen, int delay = 0, Action callback = null)
    {
        if (delay > 0)
        {
            StartCoroutine(Delay(delay));
        }

        transitionScreen.GetComponent<SceneTransitionShaderControl>().MakeTransition(1);
        transitionEvent.AddListener(delegate
        {
            transitionEvent.RemoveAllListeners();

            screen.SetActive(true);

            transitionScreen.GetComponent<SceneTransitionShaderControl>().MakeTransition(1);
            transitionEvent.AddListener(delegate
            {
                transitionEvent.RemoveAllListeners();

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
        ChangeScreen(loaderScreen, 0, delegate { loaderScreen.GetComponent<SceneManagement>().LoadScene(sceneName); });
    }
}