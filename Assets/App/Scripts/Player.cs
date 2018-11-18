using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public bool isShoot = false;
	public bool isAlive = true;
	public delegate void OnShoot();
	public event OnShoot ShootEvent;
	public AudioClip ShootSound, FallSound;

	private Animator animator;
	private AudioSource source;

	void Start() {
		animator = GetComponent<Animator>();
		source = GetComponent<AudioSource>();
		ShootEvent += MakeShoot;
	}

	void MakeShoot() {
		isShoot = true;
		animator.SetTrigger("Bang");
		source.PlayOneShot(ShootSound);
	}

	public void Die() {
		isAlive = false;
		animator.SetTrigger("Death");
		source.clip = FallSound;
		source.PlayDelayed(1.2f);
	}

	public void Win() {
		animator.SetTrigger("Win");
	}

	public void OnMissed() {
	}

	public virtual void Ready() {
		animator.SetTrigger("Position");
	}

	public virtual void Steady() {
	}

	public virtual void Bang() {
	}

	protected void Shoot() {
		ShootEvent();
	}
	
	public void Reload() {
		isShoot = false;
		isAlive = true;
		animator.SetTrigger("Repair");
	}
}
