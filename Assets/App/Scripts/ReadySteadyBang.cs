using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ReadySteadyBang : MonoBehaviour {
	private TextMeshPro Text;
	private AudioSource AudioSource;
	[SerializeField]
	SpriteRenderer Star;
	[SerializeField]
	Color textColor, bangTextColor;
	[SerializeField]
	AudioClip ready, steady, bang;

	void Awake () {
		Text = GetComponent<TextMeshPro>();
		AudioSource = GetComponent<AudioSource>();
		SetTransparent(Text);
	}

	void OnEnable() {
		Star.gameObject.SetActive(false);
		Text.gameObject.SetActive(true);
		Text.color = textColor;
		SetTransparent(Text);
	}

	public void Ready() {
		Text.SetText("ready");
		Sequence s = DOTween.Sequence();
		s.Append(Text.DOFade(1f, 0.5f))
			.Append(Text.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, -180), 0.5f))
			.Append(Text.DOFade(0f, 0.5f))
			.OnComplete(() => {
				Text.transform.localRotation = Quaternion.identity;
			});
		AudioSource.PlayOneShot(ready);
	}

	public void Steady() {
		Text.SetText("steady");
		Sequence s = DOTween.Sequence();
		s.Append(Text.DOFade(1f, 0.5f))
			.Append(Text.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, -180), 0.5f))
			.Append(Text.DOFade(0f, 0.5f))
			.OnComplete(() => {
				Text.transform.localRotation = Quaternion.identity;
			});
		AudioSource.PlayOneShot(steady);
	}

	public void Bang() {
		Text.color = bangTextColor;

		Text.SetText("bang");
		Text.DOFade(1f, 0f);
		Star.gameObject.SetActive(true);
		AudioSource.PlayOneShot(bang);
	}

	public void OnBang() {
		Text.gameObject.SetActive(false);
	}

	private void SetTransparent(TextMeshPro text) {
		Color c = text.color;
		c.a = 0f;
		text.color = c;
	}
}
