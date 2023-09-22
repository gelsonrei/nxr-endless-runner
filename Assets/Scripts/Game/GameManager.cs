using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject levelManager;
    public GameObject player;
    public GameObject playerCamera;
    public Canvas playerCanvas;

    [HideInInspector]
    public int points = 0;
    public int pointsAvarage = 50; //how points need to do something
    public float tickInterval = 10f; //how long time the velocity is increase

    [HideInInspector]
    public int distance = 0;

    [HideInInspector]
    public int currentLevel = 0;

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
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        playerCanvas.GetComponent<GameCanvasManager>().PopulatePoints(points);
        playerCanvas.GetComponent<GameCanvasManager>().PopulateDistance(distance);
    }

    public void PauseGame ()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame ()
    {
        Time.timeScale = 1;
    }

    public void QuitGame()
    {

    }

    public void RessetGame()
    {

    }
}
