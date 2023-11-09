using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType
    {
        Slide,
        Jump,
        Dodge
    }

    private AudioSource audioSource;
    private BoxCollider boxCollider;

    private PlayerControl pm;

    [SerializeField] private Vector2Int obstacleSize;
    [SerializeField] private ObstacleType obstacleType;

    public Vector2Int Size => obstacleSize;
    public ObstacleType Type => obstacleType;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();

        pm = GameManager.Instance.player.GetComponent<PlayerControl>();
    }

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!pm.GetComponent<Animator>().GetBool("isColide"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (boxCollider.isTrigger == false)
                {
                    PlaySound();

                    //pm.OnToggleOff("isRunning");
                    //pm.OnToggleOn("isColide");
                    pm.takeHit();

                    //GameCanvasManager gmc = GameManager.Instance.playerCanvas.GetComponent<GameCanvasManager>();
                    //gmc.ShowLoseUI();
                }

                boxCollider.isTrigger = true;
            }
        }
        else
        {
            boxCollider.isTrigger = false;
        }
    }

    private void PlaySound()
    {
        audioSource.Play();
    }
}