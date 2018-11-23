// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class StarfieldStar : MonoBehaviour
{
	protected void Awake()
	{
		global::Math.SetupLookupTables();
		this.counter = UnityEngine.Random.value * 100f;
		this.sprite = base.GetComponent<SpriteSM>();
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.counter += num;
		float a = Mathf.Clamp(Mathf.Sin(this.counter), 0f, 1f);
		this.sprite.SetColor(new Color(0.4f, 0.6f, 0.88f, a));
	}

	protected float counter;

	protected SpriteSM sprite;
}
