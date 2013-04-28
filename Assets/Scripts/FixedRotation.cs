using UnityEngine;
using System.Collections;

public class FixedRotation : MonoBehaviour
{
	[SerializeField] private float rotationRadiansForward	= (10f * Mathf.Deg2Rad);
	[SerializeField] private float rotationRadiansUp		= (10f * Mathf.Deg2Rad);
	[SerializeField] private float rotationRadiansRight		= (10f * Mathf.Deg2Rad);

	private Transform cachedTransform;
	private float timeSinceStartup;
	
	void Awake()
	{
		cachedTransform = transform;
		timeSinceStartup = Time.realtimeSinceStartup;
	}
	
	void Update()
	{
		float delta = Time.realtimeSinceStartup - timeSinceStartup;
		timeSinceStartup = Time.realtimeSinceStartup;
		
		cachedTransform.RotateAround(cachedTransform.TransformDirection(Vector3.forward), rotationRadiansForward * delta);
		cachedTransform.RotateAround(cachedTransform.TransformDirection(Vector3.up), rotationRadiansUp * delta);
		cachedTransform.RotateAround(cachedTransform.TransformDirection(Vector3.right), rotationRadiansRight * delta);
	}
}
