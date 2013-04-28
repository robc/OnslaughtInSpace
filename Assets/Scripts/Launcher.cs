using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Vector3 launchOffset;
	[SerializeField] private float bulletLife = 2f;
	[SerializeField] private float bulletForce = 2f;
	
	private bool isFiring;
	
	void Awake()
	{
		isFiring = false;
	}
	
	public void StartAutoFire()
	{
		if (!isFiring)
			StartCoroutine("FireRoutine");
	}
	
	public void StopAutoFire()
	{
		if (!isFiring) return;

		StopCoroutine("FireRoutine");
		isFiring = false;
	}
	
	private IEnumerator FireRoutine()
	{
		isFiring = true;
		
		StartFiring:
		GameObject bullet = (GameObject)Instantiate(bulletPrefab);
		bullet.transform.rotation = transform.rotation;
		bullet.transform.position = transform.position + bullet.transform.TransformDirection(launchOffset);
		bullet.rigidbody.AddForce(bullet.transform.TransformDirection(Vector3.forward) * bulletForce);

		Destroy(bullet, bulletLife);
		
		yield return new WaitForSeconds(0.166666667f);
		goto StartFiring;
	}
}
