// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CutsceneForegroundElement : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.startX = base.transform.localPosition.x; this.endX = (this.startX );
		this.startY = base.transform.localPosition.y; this.endY = (this.startY );
		base.transform.position += this.offsetOnStart;
		switch (this.transitionFromDirection)
		{
		case DirectionEnum.Down:
			this.startY = CutsceneController.MinY - this.sprite.height / 2f;
			break;
		case DirectionEnum.Left:
			this.startX = CutsceneController.MinX - this.sprite.width / 2f;
			break;
		case DirectionEnum.Right:
			this.startX = CutsceneController.MaxX + this.sprite.width * 20f;
			MonoBehaviour.print(this.sprite.width);
			this.endX = CutsceneController.MinX + this.sprite.width / 2f;
			break;
		}
	}

	private void Update()
	{
		if (Mathf.Abs(base.transform.position.x - this.endX) < 0.01f)
		{
			return;
		}
		float num = Mathf.Clamp(this.transitionCounter / this.transitionTime, 0f, 1f);
		this.transitionCounter += Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.lerpTween)
		{
			base.transform.localPosition = new Vector3(Mathf.Lerp(base.transform.localPosition.x, this.endX, Time.deltaTime / (this.transitionTime / 2f)), base.transform.localPosition.y, base.transform.localPosition.z);
		}
		else
		{
			base.transform.localPosition = new Vector3(this.startX * (1f - num) + this.endX * num, this.startY * (1f - num) + this.endY * num, base.transform.localPosition.z);
		}
		if (num > 0.99f && UnityEngine.Random.value < 0.02f && this.pecShineLocations != null && this.pecShineIndex < this.pecShineLocations.Length)
		{
			CutsceneController.Instance.CreatePecShine(this.pecShineLocations[this.pecShineIndex++].position);
		}
	}

	public CutsceneForegroundType type;

	[HideInInspector]
	public float timeLeft;

	public Transform[] pecShineLocations;

	private int pecShineIndex;

	public Vector3 offsetOnStart;

	public float transitionTime = 1f;

	protected float transitionCounter;

	public DirectionEnum transitionFromDirection = DirectionEnum.Right;

	protected bool tweening = true;

	public bool lerpTween = true;

	protected float endX;

	protected float endY;

	protected float startX;

	protected float startY;

	protected SpriteSM sprite;
}
