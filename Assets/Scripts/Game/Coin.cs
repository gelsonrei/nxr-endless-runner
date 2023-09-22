using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;
    public float delay = 0.5f;

    private ParticleSystem particleSystemarticles;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;

    private Transform targetPosition = null;

    private void Awake()
    {
        particleSystemarticles = GetComponent<ParticleSystem>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    } 

    void Start()
    {

    }

    void Update()
    {
        if ( GameManager.Instance.player.GetComponent<PlayerControl>().isMagnetic == true && targetPosition )
        {
            if( Vector3.Distance(targetPosition.position, transform.position) >= 1.0 )
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition.position, Time.deltaTime * 5.0f);
            }
            else
            {
                if ( meshRenderer.enabled )
                {
                    RenderMesh(false);
                    PlayParticles();
                    PlaySound();
                    SumPoints();

                    StartCoroutine(DestroyGameObject());
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( !GameManager.Instance.player.GetComponent<PlayerControl>().isMagnetic )
        {
            RenderMesh(false);
            PlayParticles();
            PlaySound();
            SumPoints();

            StartCoroutine(DestroyGameObject());
        }
        else
        {
            targetPosition = other.transform;
        }
    }

    private void SumPoints()
    {
        GameManager.Instance.points += value;
    }

    private void RenderMesh(bool status)
    {
        meshRenderer.enabled = status;
    }

    private void PlayParticles () 
    {
        particleSystemarticles.Play();
    }

    private void PlaySound ()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
