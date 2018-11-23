// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class VictoryFireWork : MonoBehaviour
{
	private void Start()
	{
		this.particles = base.GetComponent<ParticleEmitter>();
		for (int i = 0; i < this.particlesCount; i++)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			this.particles.Emit(Vector3.forward * this.zOffset, new Vector3(insideUnitCircle.x * this.tangentForce, insideUnitCircle.y * this.tangentForce + this.yForce, 0f), Mathf.Lerp(this.minSize, this.maxSize, UnityEngine.Random.value), Mathf.Lerp(this.minEnergy, this.maxEnergy, UnityEngine.Random.value), Color.white);
		}
		this.startPos = base.transform.position;
		this.followStartPos = SortOfFollow.followPos;
		Sound.GetInstance().PlaySoundEffectAt(this.sounds[UnityEngine.Random.Range(0, this.sounds.Length)], this.volume, this.startPos, 0.85f + UnityEngine.Random.value * 0.3f);
	}

	private void LateUpdate()
	{
		if (this.particles.particleCount <= 0)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			base.transform.position = this.startPos + (SortOfFollow.followPos - this.followStartPos) * this.parralaxM;
		}
	}

	protected ParticleEmitter particles;

	public float parralaxM = 0.7f;

	public float yForce = 60f;

	public float tangentForce = 300f;

	public int particlesCount = 200;

	public float minSize = 1f;

	public float maxSize = 2f;

	public float minEnergy = 2f;

	public float maxEnergy = 4f;

	public float zOffset;

	protected Vector3 followStartPos;

	protected Vector3 startPos;

	public AudioClip[] sounds;

	public float volume = 0.3f;
}
