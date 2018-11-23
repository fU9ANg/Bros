// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class TextHolder : MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < this.text.Length; i++)
		{
			TextMesh textMesh = UnityEngine.Object.Instantiate(this.textPrefab) as TextMesh;
			textMesh.text = string.Empty + this.text[i];
			textMesh.transform.parent = base.transform;
			textMesh.transform.localPosition = Vector3.right * (float)i * this.offset;
			this.letters.Add(textMesh);
		}
		this.textPrefab.gameObject.SetActive(false);
	}

	private void Update()
	{
	}

	public TextMesh textPrefab;

	private List<TextMesh> letters = new List<TextMesh>();

	public string text = "default";

	public float offset = 14f;
}
