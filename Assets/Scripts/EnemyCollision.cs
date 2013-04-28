using UnityEngine;
using System.Collections;

public class EnemyCollision : MonoBehaviour
{
	public System.Action<EnemyCollision> OnEnemyDestroyed;
	public System.Action<EnemyCollision> OnEnemyReachedTarget;
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Bullets"))
		{
			Destroy(collision.gameObject);
			
			if (OnEnemyDestroyed != null)
				OnEnemyDestroyed(this);
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (OnEnemyReachedTarget != null)
				OnEnemyReachedTarget(this);
		}
	}
}
