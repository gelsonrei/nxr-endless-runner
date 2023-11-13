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
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("EU, - " + gameObject.name + ", BATI EM - " + other.name);
        
        if (other.CompareTag("Player") && !pm.GetComponent<Animator>().GetBool("isColide"))
        {
            boxCollider.enabled = false;
            
            PlaySound();
            pm.takeHit();
        }
    }

    private void PlaySound()
    {
        audioSource.Play();
    }
}