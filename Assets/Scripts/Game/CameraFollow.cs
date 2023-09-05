using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraFollow : MonoBehaviour 
{

	private Transform target;
	public float distance = 5.0f;
    public float height = 2.0f;
    public float smoothSpeed = 10.0f;

	private Vector3 offset;

	private void Awake()
	{
		
	}
	void Start () 
	{
		target = GameManager.Instance.player.transform;
		offset = transform.position - target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector3 desiredPosition = target.position + offset + (-target.forward * distance) + (Vector3.up * height);
		
		transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
		//transform.LookAt(target);
	}
}