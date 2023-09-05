using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SpeedParticles : MonoBehaviour
{
    private ParticleSystem leaves;
    private ParticleSystem dust;
    private float speed = 1.0f;

    void Awake()
    {
        leaves = transform.GetComponent<ParticleSystem>();
        dust = transform.GetComponentInChildren<ParticleSystem>();
    }

    void Start()
    {
        speed = (GameManager.Instance.player.GetComponent<PlayerControl>().velocity / 6);
    }

    void Update()
    {
        var leavesPs = leaves.main;
        var dustPs = dust.main;
        
        leavesPs.simulationSpeed = speed;
        dustPs.simulationSpeed = speed;
    }
}