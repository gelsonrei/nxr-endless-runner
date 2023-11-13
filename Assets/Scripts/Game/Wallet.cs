using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Wallet : MonoBehaviour
{
    public float delay = 0.5f;
    public float powerUpTime = 5.0f;

    private ParticleSystem particleSystemarticles;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;
    private PlayerControl playerControl;

    private void Awake()
    {
        particleSystemarticles = GetComponent<ParticleSystem>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>(); 
        playerControl = GameManager.Instance.player.GetComponent<PlayerControl>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerControl.isMagnetic )
        {
            EnablePowerUp();
        }
    }

    private void EnablePowerUp()
    {
        playerControl.enableMagnetic(powerUpTime);

        RenderMesh(false);
        PlayParticles();
        PlaySound();
        StartCoroutine(DestroyGameObject());
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
        audioSource.Play();
    }

    private IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
