using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
    private AsyncOperation operation;

    void Awake()
    {
    }

    void Start()
    {
        operation = null;
        
        gameObject.GetComponentInChildren<Slider>().value = 0.0f;
    }

    void Update()
    {
        if (operation != null)
        {
            gameObject.GetComponentInChildren<Slider>().value = Mathf.Clamp01(operation.progress);

            if(operation.isDone)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);

        operation = SceneManager.LoadSceneAsync(sceneName);
    }
}
