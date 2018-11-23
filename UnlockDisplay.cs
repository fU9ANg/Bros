// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnlockDisplay : MonoBehaviour
{
	private void Start()
	{
		Vector3 vector = new Vector3(0f, 0f, 0f);
		Vector3 b = Vector3.right * 10f;
		Vector3 b2 = Vector3.right * 20f;
		int num = HeroUnlockController.heroUnlockIntervals.Keys.ToList<int>().Last<int>();
		int i = 1;
		foreach (KeyValuePair<int, HeroType> keyValuePair in HeroUnlockController.heroUnlockIntervals)
		{
			int key = keyValuePair.Key;
			while (i < key)
			{
				vector += b;
				SpriteSM item = this.InstantiateAndParent(this.markerPrefab, vector);
				vector += b;
				this.avatars.Add(item);
				i++;
				if (i == num / 2)
				{
					vector.x = 0f;
					vector.y = -50f;
				}
			}
			vector += b2;
			SpriteSM spriteSM = this.InstantiateAndParent(this.AvatarPrefab, vector);
			HeroController.SwitchAvatarMaterial(spriteSM, keyValuePair.Value);
			vector += b2;
			this.avatars.Add(spriteSM);
			i++;
			if (i == num / 2)
			{
				vector.x = 0f;
				vector.y = -50f;
			}
		}
		base.transform.localScale *= 0.7f;
	}

	public void UpdateDisplayedUnlocks(int displayCount)
	{
		for (int i = 0; i < this.avatars.Count; i++)
		{
			if (displayCount > i)
			{
				this.avatars[i].SetColor(Color.white);
			}
			else
			{
				this.avatars[i].SetColor(Color.black);
			}
		}
		this.numberOfBrosDisplayed = displayCount;
	}

	private SpriteSM InstantiateAndParent(SpriteSM prefab, Vector3 pos)
	{
		SpriteSM spriteSM = UnityEngine.Object.Instantiate(prefab) as SpriteSM;
		spriteSM.transform.parent = base.transform;
		spriteSM.transform.localPosition = pos;
		return spriteSM;
	}

	private void Update()
	{
	}

	public IEnumerator SetDisplayCount(int Count)
	{
		yield return new WaitForSeconds(1.2f);
		while (Count > this.numberOfBrosDisplayed)
		{
			this.numberOfBrosDisplayed++;
			this.UpdateDisplayedUnlocks(this.numberOfBrosDisplayed);
			this.clipCounter++;
			this.clipCounter %= this.gunShots.Length;
			Sound.GetInstance().PlayAudioClip(this.gunShots[UnityEngine.Random.Range(0, this.gunShots.Length)], Camera.main.transform.position, 0.3f);
			yield return new WaitForSeconds(0.2f);
		}
		yield break;
	}

	public SpriteSM AvatarPrefab;

	public SpriteSM markerPrefab;

	private List<SpriteSM> avatars = new List<SpriteSM>();

	public AudioClip[] gunShots;

	private int clipCounter;

	private int numberOfBrosDisplayed;
}
