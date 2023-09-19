using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayCharacterEffects : MonoBehaviour
{
    [Header("VFX")]
    public GameObject FootStepEfx;
    public GameObject JumpEfx;
    public GameObject HitEfx;
    public GameObject SlideLeft;
    public GameObject SlideRight;
    public GameObject LeavesEfx;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /*
    * Efx
    */

    public void PlayFootStepEfx()
    {
        FootStepEfx.GetComponent<ParticleSystem>().Play();
    }

    public void PlayJumpEfx()
    {
        JumpEfx.GetComponent<ParticleSystem>().Play();
    }

    public void PlayHitEfx()
    {
        HitEfx.GetComponent<ParticleSystem>().Play();
        HitEfx.GetComponentInChildren<ParticleSystem>().Play();
    }
    
    public void PlaySlideLeftEfx()
    {
        SlideLeft.GetComponent<ParticleSystem>().Play();
    }
    
    public void PlaySlideRightEfx()
    {
        SlideRight.GetComponent<ParticleSystem>().Play();
    }
    
    public void PlayLeavesfx()
    {
        LeavesEfx.GetComponent<ParticleSystem>().Play();
        LeavesEfx.GetComponentInChildren<ParticleSystem>().Play();
    }
    
    public void StopLeavesfx()
    {
        LeavesEfx.GetComponent<ParticleSystem>().Stop();
        LeavesEfx.GetComponentInChildren<ParticleSystem>().Play();
    }
}
