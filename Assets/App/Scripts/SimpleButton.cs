using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SimpleButton : MonoBehaviour {
	[SerializeField]
	Color color, pressedColor;

	private SpriteRenderer spriteRenderer;
	public delegate void OnClick();
	private event OnClick Click;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.color = color;
	}
	
	void OnMouseUp() {
		spriteRenderer.color = color;
		if (Click != null) {
			Click();
		}
	}

	void OnMouseDown() {
		spriteRenderer.color = pressedColor;
	}

	public void SetClickListener(OnClick onClick) {
		Click += onClick;
	}
}