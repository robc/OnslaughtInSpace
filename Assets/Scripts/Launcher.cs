using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private Vector3 launchOffset;
	[SerializeField] private float bulletLife = 2f;
	[SerializeField] private float bulletForce = 2f;
	
	public void FireBullet()
	{
		GameObject bullet = (GameObject)Instantiate(bulletPrefab);
		bullet.transform.rotation = transform.rotation;
		bullet.transform.position = transform.position + bullet.transform.TransformDirection(launchOffset);
		bullet.rigidbody.AddForce(bullet.transform.TransformDirection(Vector3.forward) * bulletForce);

		Destroy(bullet, bulletLife);
	}
}
