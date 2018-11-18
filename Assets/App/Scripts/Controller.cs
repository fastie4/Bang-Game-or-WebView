using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Controller : MonoBehaviour {
	enum State {
		IDLE,
		READY,
		STEADY,
		BANG,
		ENDED
	}
	public Player playerUp, playerDown;
	public SimpleButton StartButton, WinButton;
	public ReadySteadyBang ReadySteadyBang;
	public float readyTime, minSteadyTime;
	public GameObject BulletPrefab, SpotPrefab;
	public SpriteRenderer Fading;
	private State state;
	private List<GameObject> ShootsObjects = new List<GameObject>();
	private Coroutine game;

	private void Awake() {
		Application.targetFrameRate = 60;
	}

	void Start () {
		state = State.IDLE;
		StartButton.SetClickListener(StartGame);
		WinButton.SetClickListener(Reload);

		playerUp.ShootEvent += PlayerUp_Shoot;
		playerDown.ShootEvent += PlayerDown_Shoot;
	}

	private void Update() {
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	private void PlayerDown_Shoot() {
		OnShoot(playerDown, playerUp);
	}

	private void PlayerUp_Shoot() {
		OnShoot(playerUp, playerDown);
	}

	private void OnShoot(Player shooter, Player victim) {
		if (state == State.BANG) {
			victim.Die();
			shooter.Win();
			SetState(State.ENDED);
			ReadySteadyBang.OnBang();
			MakeSpot(victim.transform.position);
			Victory(shooter);
		} else {
			MakeMissedShoot(victim.transform.position);
			shooter.OnMissed();
			if (victim.isShoot) {
				SetState(State.ENDED);
				DeadHeat();
			}
		}
	}

	void StartGame() {
		StartButton.gameObject.SetActive(false);
		ReadySteadyBang.gameObject.SetActive(true);

		game = StartCoroutine(Game());
	}

	IEnumerator Game() {
		yield return new WaitForSeconds(0.5f);

		Ready();
		yield return new WaitForSeconds(readyTime);
		if (state == State.ENDED) yield break;

		Steady();
		float steadyTime = minSteadyTime + Random.Range(0f, 5f);
		yield return new WaitForSeconds(steadyTime);
		if (state == State.ENDED) yield break;

		Bang();
	}

	void Reload() {
		WinButton.gameObject.SetActive(false);
		StartCoroutine(Reloading());
	}

	IEnumerator Reloading() {
		StopCoroutine(game);
		yield return new WaitForSeconds(1f);
		Fading.gameObject.SetActive(true);
		Fading.DOFade(1f, 1f)
			.ChangeStartValue(Color.clear)
			.SetLoops(2, LoopType.Yoyo)
			.OnComplete(() => {
				Fading.gameObject.SetActive(false);
			});
		yield return new WaitForSeconds(1f);

		state = State.IDLE;
		playerUp.Reload();
		playerDown.Reload();
		ShootsObjects.ForEach(obj => Destroy(obj));
		ShootsObjects.Clear();
		StartButton.gameObject.SetActive(true);
	}

	void Ready() {
		SetState(State.READY);
		ReadySteadyBang.Ready();
		playerUp.Ready();
		playerDown.Ready();
	}

	void Steady() {
		SetState(State.STEADY);
		ReadySteadyBang.Steady();
		playerUp.Steady();
		playerDown.Steady();
	}

	void Bang() {
		SetState(State.BANG);
		ReadySteadyBang.Bang();
		playerUp.Bang();
		playerDown.Bang();
	}

	private void SetState(State state) {
		this.state = state;
	}

	private void MakeSpot(Vector3 position) {
		float x = Random.Range(0f, 0.3f);
		float y = Random.Range(0.4f, 0.52f) * (position.y < 0f ? -1f : 1f);
		ShootsObjects.Add(Instantiate(SpotPrefab, position + new Vector3(x, y), Quaternion.identity));
	}

	private void MakeMissedShoot(Vector3 position) {
		float x = Random.Range(-3.5f, 3.5f);
		float mult = position.y > 0f ? 1f : -1f;
		float yMin = position.y + (Mathf.Abs(x) < 0.65f ? 0.8f : 0f) * mult;
		float yMax = position.y + 2f * mult;
		float y = Random.Range(yMin, yMax);
		ShootsObjects.Add(Instantiate(BulletPrefab, new Vector3(x, y), Quaternion.identity));
	}

	private void Victory(Player player) {
		StartCoroutine(ShowWin(player));
	}

	private IEnumerator ShowWin(Player player) {
		yield return new WaitForSeconds(0.5f);

		Vector3 pos = new Vector3(0f, -6f);
		Quaternion rotation = Quaternion.identity;
		if (player.transform.position.y > 0f) {
			pos *= -1f;
			rotation = Quaternion.Euler(0f, 0f, -180f);
		}
		WinButton.gameObject.transform.position = pos;
		WinButton.gameObject.transform.rotation = rotation;
		WinButton.gameObject.SetActive(true);
	}

	private void DeadHeat() {
		StartCoroutine(Reloading());
	}
}
