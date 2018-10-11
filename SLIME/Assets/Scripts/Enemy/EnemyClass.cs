using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyClass {
	
	void Respawn();
	void KillEnemy();

	void AddVelocity(Vector2 v);
	void MultiplyVelocity(float f);
	void ApplyGravity(ref Vector3 velocity, float gravity);
}
