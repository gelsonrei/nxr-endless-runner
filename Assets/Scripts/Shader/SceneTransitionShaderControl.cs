using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneTransitionShaderControl : MonoBehaviour
{
    private AudioSource m_audioSource;

    [SerializeField]
    private Material screenTransitionMaterial;

    [SerializeField]
    private string propertyName = "_Progress";

    public UnityEvent OnTransitionDone;
    
    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        screenTransitionMaterial.SetFloat(propertyName, 1.5f);
    }


    void Update()
    {
        
    }

    public void MakeTransition(float time)
    {
        StartCoroutine(TransitionCoroutine(time));
    }

    private IEnumerator TransitionCoroutine(float duration)
    {
        float time = 0;
        float targetValue = 0;
        float startValue = screenTransitionMaterial.GetFloat(propertyName);

        if (startValue < 1.0f)
        {
            targetValue = 1.5f;
        }

        m_audioSource.Play();

        while (time <= duration)
        {
            screenTransitionMaterial.SetFloat(propertyName, Mathf.Lerp(startValue, targetValue, time / duration));
            time += Time.deltaTime;

            yield return null;
        }

        OnTransitionDone?.Invoke();
    }

    private void OnApplicationQuit() 
    {
        screenTransitionMaterial.SetFloat(propertyName, 1.5f);
    }
}
