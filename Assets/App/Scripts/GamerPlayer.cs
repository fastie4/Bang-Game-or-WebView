using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamerPlayer : Player {
	public bool upPlayer = false;
	public override void Ready() {
		base.Ready();
		StartCoroutine(WaitForShoot());
	}

	IEnumerator WaitForShoot() {
		while (isAlive && !isShoot) {
			if (Input.GetMouseButtonDown(0)) {
				Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
				if (pos.y < 0.5f ^ upPlayer) {
					Shoot();
				}
			}
			yield return null;
		}
	}
}
