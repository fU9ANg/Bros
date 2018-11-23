// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TestExplosions : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.counter += Time.deltaTime;
		if (this.counter > this.explosionRate)
		{
			this.counter -= this.explosionRate * (0.5f + UnityEngine.Random.value * 0.75f);
			Puff puff = UnityEngine.Object.Instantiate(this.explosionLayeredPrefab, new Vector3(Mathf.Round(-this.xExtent + UnityEngine.Random.value * this.xExtent * 2f), -5f - this.yExtent + Mathf.Round(UnityEngine.Random.value * this.yExtent * 2f), 5f), Quaternion.identity) as Puff;
			if (puff != null)
			{
				puff.SetVelocity(Vector3.forward * 2f);
			}
		}
		if (Input.GetKeyDown(KeyCode.F1))
		{
			if (Time.timeScale >= 1f)
			{
				Time.timeScale = 0.25f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}
	}

	//public GameObject explosionLayeredPrefab;
    public Puff explosionLayeredPrefab;

	protected float counter;

	public float explosionRate = 0.0334f;

	public float xExtent = 20f;

	public float yExtent = 20f;
}
