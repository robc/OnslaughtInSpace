using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 45f;
	
	public void UpdatePlayerRotation(float inputAxis, float delta)
	{
		transform.RotateAround(Vector3.up, inputAxis * rotationSpeed * delta);
	}
}
