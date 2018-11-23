// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PlayerScoreDisplay : MonoBehaviour
{
	public void SetColor(Color col)
	{
		this.textBox.SetColor(col);
		this.badgeBox.SetColor(col);
		this.avatarBox.SetColor(col);
	}

	private void Start()
	{
		if (this.slideIn)
		{
			this.targetPos = base.transform.localPosition;
			base.transform.localPosition = this.targetPos + Vector3.left * 300f;
			this.velocity = Vector3.right * 1800f;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Slide in from ",
				base.transform.localPosition,
				" to ",
				this.targetPos
			}));
		}
	}

	public void SlideAway()
	{
		Sound.GetInstance().PlaySoundEffect(this.soundHolder.deathSounds, 0.1f);
		this.velocity = 200f * Vector3.right;
		this.slideOut = true;
		this.slideIn = false;
		UnityEngine.Debug.Log("Slide Away ");
	}

	private void Update()
	{
		if (this.slideIn)
		{
			float deltaTime = Time.deltaTime;
			if (this.slideDelay > 0f)
			{
				this.slideDelay -= deltaTime;
				Sound.GetInstance().PlaySoundEffect(this.soundHolder.effortSounds, 0.1f);
			}
			else
			{
				if (!this.slideInOnce)
				{
					this.slideInOnce = true;
				}
				base.transform.localPosition += this.velocity * deltaTime;
				if (this.velocity.x > 0f)
				{
					if (base.transform.localPosition.x >= this.targetPos.x + this.velocity.x * 0.005f)
					{
						this.velocity.x = this.velocity.x * -0.2f;
						if (!this.hitOnce)
						{
							this.hitOnce = true;
							Sound.GetInstance().PlaySoundEffect(this.soundHolder.hitSounds, 0.15f);
							SortOfFollow.Shake(0.5f);
						}
					}
				}
				else if (base.transform.localPosition.x <= this.targetPos.x + this.velocity.x * 0.005f)
				{
					this.velocity.x = this.velocity.x * -0.2f;
					if (!this.hitOnce)
					{
						this.hitOnce = true;
						Sound.GetInstance().PlaySoundEffect(this.soundHolder.hitSounds, 0.15f);
						SortOfFollow.Shake(0.5f);
					}
				}
				if (Mathf.Abs(this.velocity.x) < 1f)
				{
					this.velocity = Vector3.zero;
					base.transform.localPosition = this.targetPos;
					this.slideIn = false;
				}
			}
		}
		else if (this.slideOut)
		{
			float deltaTime2 = Time.deltaTime;
			this.velocity.x = this.velocity.x + 5000f * deltaTime2;
			base.transform.localPosition += this.velocity * deltaTime2;
			if (base.transform.localPosition.x > 600f)
			{
				this.slideOut = false;
				this.slideIn = false;
			}
		}
	}

	public TextMesh textMesh;

	public ScalingBox textBox;

	public ScalingBox badgeBox;

	public ScalingBox avatarBox;

	public BadgeHolder badgeHolder;

	public SpriteSM Avatar;

	public float slideDelay;

	public bool slideIn;

	public bool slideOut;

	protected Vector3 targetPos = Vector3.zero;

	protected Vector3 velocity;

	protected bool hitOnce;

	protected bool slideInOnce;

	public SoundHolder soundHolder;
}
