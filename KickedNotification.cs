// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class KickedNotification : SingletonMono<KickedNotification>
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0f)
			{
				SingletonMono<KickedNotification>.Instance.gameObject.SetChildrenActive(false);
			}
		}
	}

	public static void Show(string playerWhoKickedYou)
	{
		SingletonMono<KickedNotification>.Instance.playerName.text = playerWhoKickedYou;
		SingletonMono<KickedNotification>.Instance.gameObject.SetChildrenActive(true);
		SingletonMono<KickedNotification>.Instance.timer = 11f;
	}

	public TextMesh playerName;

	private float timer;
}
