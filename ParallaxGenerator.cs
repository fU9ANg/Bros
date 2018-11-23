// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParallaxGenerator : MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < 15; i++)
		{
			for (int j = 0; j < this.themeHolder.parallaxClouds.Length; j++)
			{
				int num = j + 1;
				ParallaxFollow parallaxFollow = UnityEngine.Object.Instantiate(this.themeHolder.parallaxClouds[j], new Vector3((float)(-1000 + i * 1024) + UnityEngine.Random.value * 768f, (float)(25 + num * num * 20 + num * 60) + UnityEngine.Random.value * (float)(34 + num * 100), 8f), Quaternion.identity) as ParallaxFollow;
				parallaxFollow.transform.parent = base.transform;
				parallaxFollow.SetFollow(this.parallaxFollowTransform);
			}
		}
		if (this.levelTheme == LevelTheme.Jungle || this.levelTheme == LevelTheme.BurningJungle)
		{
			int num2 = 0;
			float num3 = this.themeHolder.parallax3[0].parallaxXM;
			float num4 = -512f / num3;
			while (num4 < 10000f && num2 < 800)
			{
				num4 += (256f + UnityEngine.Random.value * 128f) * num3;
				num2++;
				int num5 = UnityEngine.Random.Range(0, this.themeHolder.parallax3.Length);
				ParallaxFollow parallaxFollow2 = UnityEngine.Object.Instantiate(this.themeHolder.parallax3[num5], new Vector3(num4, -75f + UnityEngine.Random.value * 50f, 8f), Quaternion.identity) as ParallaxFollow;
				parallaxFollow2.transform.parent = base.transform;
				parallaxFollow2.SetFollow(this.parallaxFollowTransform);
			}
			num2 = 0;
			num3 = this.themeHolder.parallax1[0].parallaxXM;
			num4 = -512f / num3;
			while (num4 < 10000f && num2 < 800)
			{
				num4 += (256f + UnityEngine.Random.value * 128f) * num3;
				num2++;
				int num6 = UnityEngine.Random.Range(0, this.themeHolder.parallax1.Length);
				ParallaxFollow parallaxFollow3 = UnityEngine.Object.Instantiate(this.themeHolder.parallax1[num6], new Vector3(num4, -50f + UnityEngine.Random.value * 50f, 8f), Quaternion.identity) as ParallaxFollow;
				parallaxFollow3.transform.parent = base.transform;
				parallaxFollow3.SetFollow(this.parallaxFollowTransform);
			}
			if (this.themeHolder.foliageBackground.Length > 0)
			{
				num2 = 0;
				num3 = 0.2f;
				num4 = -512f / num3;
				while (num4 < 10000f && num2 < 800)
				{
					num4 += (384f + UnityEngine.Random.value * 128f) * num3;
					num2++;
					int num7 = UnityEngine.Random.Range(0, 3);
					SpriteSM spriteSM = UnityEngine.Object.Instantiate(this.themeHolder.foliageBackground[num7], new Vector3(num4, -170f + UnityEngine.Random.value * 50f, 8f), Quaternion.identity) as SpriteSM;
					spriteSM.transform.parent = base.transform;
				}
			}
		}
		else if (this.levelTheme == LevelTheme.Forest)
		{
			int num8 = 0;
			float num9 = this.themeHolder.parallax3[0].parallaxXM;
			float num10 = -384f / num9;
			while (num10 < 2048f && num8 < 200)
			{
				num10 += (768f + UnityEngine.Random.value * 256f) * num9;
				num8++;
				int num11 = UnityEngine.Random.Range(0, this.themeHolder.parallax1.Length);
				ParallaxFollow parallaxFollow4 = UnityEngine.Object.Instantiate(this.themeHolder.parallax1[num11], new Vector3(num10, -75f + UnityEngine.Random.value * 50f, 8f), Quaternion.identity) as ParallaxFollow;
				parallaxFollow4.transform.parent = base.transform;
				parallaxFollow4.SetFollow(this.parallaxFollowTransform);
			}
			num8 = 0;
			num9 = this.themeHolder.parallax1[0].parallaxXM;
			num10 = -384f / num9;
			while (num10 < 2048f && num8 < 200)
			{
				num10 += (384f + UnityEngine.Random.value * 128f) * num9;
				num8++;
				int num12 = UnityEngine.Random.Range(0, this.themeHolder.parallax2.Length);
				ParallaxFollow parallaxFollow5 = UnityEngine.Object.Instantiate(this.themeHolder.parallax2[num12], new Vector3(num10, -120f + UnityEngine.Random.value * 50f, 8f), Quaternion.identity) as ParallaxFollow;
				parallaxFollow5.transform.parent = base.transform;
				parallaxFollow5.SetFollow(this.parallaxFollowTransform);
			}
			if (this.themeHolder.parallax3.Length > 0)
			{
				num8 = 0;
				num9 = 0.2f;
				num10 = -384f / num9;
				while (num10 < 2048f && num8 < 200)
				{
					num10 += (580f + UnityEngine.Random.value * 128f) * num9;
					num8++;
					int num13 = UnityEngine.Random.Range(0, 3);
					ParallaxFollow parallaxFollow6 = UnityEngine.Object.Instantiate(this.themeHolder.parallax3[num13], new Vector3(num10, -200f + UnityEngine.Random.value * 50f, 8f), Quaternion.identity) as ParallaxFollow;
					parallaxFollow6.transform.parent = base.transform;
					parallaxFollow6.SetFollow(this.parallaxFollowTransform);
				}
			}
			if (this.themeHolder.parallax3.Length > 0)
			{
				num8 = 0;
				num9 = 1f;
				num10 = -384f / num9;
				while (num10 < 2048f && num8 < 200)
				{
					num10 += (128f + UnityEngine.Random.value * 256f) * num9;
					num8++;
					int num14 = UnityEngine.Random.Range(3, 5);
					ParallaxFollow parallaxFollow7 = UnityEngine.Object.Instantiate(this.themeHolder.parallax3[num14], new Vector3(num10, -80f + UnityEngine.Random.value * 50f, 8f), Quaternion.identity) as ParallaxFollow;
					parallaxFollow7.transform.parent = base.transform;
					parallaxFollow7.SetFollow(this.parallaxFollowTransform);
				}
			}
		}
		else
		{
			float num15 = UnityEngine.Random.value * -384f;
			for (int k = 0; k < 2; k++)
			{
				ParallaxFollow parallaxFollow8 = UnityEngine.Object.Instantiate(this.themeHolder.parallax3[0], new Vector3(num15 + (float)(k * 768), -256f + UnityEngine.Random.value * 50f, 8f), Quaternion.identity) as ParallaxFollow;
				parallaxFollow8.transform.parent = base.transform;
				parallaxFollow8.SetFollow(this.parallaxFollowTransform);
			}
			int num16 = 0;
			float parallaxXM = this.themeHolder.parallax3[0].parallaxXM;
			float num17 = -384f / parallaxXM;
			while (num17 < 2048f && num16 < 20)
			{
				num17 += (1024f + UnityEngine.Random.value * 1024f) * parallaxXM;
				num16++;
				ParallaxFollow parallaxFollow9 = UnityEngine.Object.Instantiate(this.themeHolder.parallax3[1], new Vector3(num17, -256f + UnityEngine.Random.value * 128f, 8f), Quaternion.identity) as ParallaxFollow;
				parallaxFollow9.transform.parent = base.transform;
				parallaxFollow9.SetFollow(this.parallaxFollowTransform);
			}
			num16 = 0;
			parallaxXM = this.themeHolder.parallax1[0].parallaxXM;
			num17 = -384f / parallaxXM;
			while (num17 < 2048f && num16 < 200)
			{
				num17 += (float)(96 + UnityEngine.Random.Range(0, 3) * 16);
				num16++;
				int num18 = UnityEngine.Random.Range(0, this.themeHolder.parallax1.Length);
				ParallaxFollow parallaxFollow10 = UnityEngine.Object.Instantiate(this.themeHolder.parallax1[num18], new Vector3(num17, (float)(-288 + UnityEngine.Random.Range(0, 3) * 16), 8f), Quaternion.identity) as ParallaxFollow;
				parallaxFollow10.transform.parent = base.transform;
				parallaxFollow10.SetFollow(this.parallaxFollowTransform);
			}
		}
	}

	private void Update()
	{
	}

	public ThemeHolder themeHolder;

	public Transform parallaxFollowTransform;

	public LevelTheme levelTheme;
}
