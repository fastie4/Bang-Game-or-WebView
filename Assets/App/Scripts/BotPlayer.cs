using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPlayer : Player {
	public override void Bang() {
		base.Bang();
		StartCoroutine(OnBang());
	}

	private IEnumerator OnBang() {
		float random = Random.Range(0.25f, 0.4f);
		yield return new WaitForSeconds(random);
		if (isAlive && !isShoot) {
			Shoot();
		}
	}
}
