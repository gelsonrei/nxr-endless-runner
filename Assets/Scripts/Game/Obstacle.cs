using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private AudioSource audioSource;
    private BoxCollider boxCollider;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {

            if (boxCollider.isTrigger == false)
            {
                PlaySound();

                PlayerControl pm = GameManager.Instance.player.GetComponent<PlayerControl>();
                //pm.OnToggleOff("isRunning");
                //pm.OnToggleOn("isColide");
                pm.takeHit();

                //GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();
                //gmc.ShowLoseUI();
            }
            
            boxCollider.isTrigger = true;
        }
    }

    private void PlaySound ()
    {
        audioSource.Play();
    }
}
