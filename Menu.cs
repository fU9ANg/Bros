// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public int Index
	{
		get
		{
			return this.highlightIndex;
		}
	}

	public MenuBarItem GetSelectedItem()
	{
		if (this.highlightIndex < this.itemNames.Length)
		{
			return this.itemNames[this.highlightIndex];
		}
		return null;
	}

	public bool IsAnyItemActive
	{
		get
		{
			bool flag = false;
			for (int i = 0; i < this.items.Length; i++)
			{
				flag = (flag || this.itemEnabled[i]);
			}
			return flag;
		}
	}

	public virtual bool MenuActive
	{
		get
		{
			return this.menuActive;
		}
		set
		{
			this.menuActive = value;
			foreach (TextMesh textMesh in this.items)
			{
				textMesh.gameObject.SetActive(this.menuActive);
			}
			this.menuHighlight.gameObject.SetActive(this.menuActive);
			if (this.menuHolder != null)
			{
				this.menuHolder.SetActive(this.menuActive);
			}
			this.lastInputTime = Time.time;
		}
	}

	public virtual void TransitionIn()
	{
		this.highlightIndex = 0;
		for (int i = 0; i < this.items.Length; i++)
		{
			this.items[i].transform.position = this.menuHighlight.transform.position;
		}
	}

	protected virtual void SetupItems()
	{
	}

	protected virtual void Awake()
	{
		global::Math.SetupLookupTables();
		if (this.overrideIndivdualItemCharacterSizes && this.itemNames != null)
		{
			foreach (MenuBarItem menuBarItem in this.itemNames)
			{
				menuBarItem.size = this.characterSizes;
			}
		}
		this.InstantiateItems();
		this.lastInputTime = Time.time;
		if (!this.startActive)
		{
			this.MenuActive = this.startActive;
		}
	}

	public void DestroyItems()
	{
		if (this.items != null && this.items.Length > 0)
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				UnityEngine.Object.Destroy(this.items[i]);
				UnityEngine.Object.Destroy(this.backDrops[i]);
			}
		}
		this.backDrops = null; this.items = (this.backDrops );
		this.highlightIndex = 0;
	}

	public virtual void InstantiateItems()
	{
		this.DestroyItems();
		this.SetupItems();
		this.items = new TextMesh[this.itemNames.Length];
		this.backDrops = new TextMesh[this.itemNames.Length];
		this.itemEnabled = new bool[this.itemNames.Length];
		for (int i = 0; i < this.itemNames.Length; i++)
		{
			this.items[i] = (UnityEngine.Object.Instantiate(this.textPrefab) as TextMesh);
			this.items[i].text = this.itemNames[i].name;
			this.items[i].transform.parent = base.transform;
			this.items[i].transform.localPosition = Vector3.down * this.verticalSpacing * (float)i;
			this.items[i].characterSize = this.itemNames[i].size;
			this.items[i].lineSpacing = this.lineSpacing;
			this.items[i].GetComponent<Renderer>().material.color = this.itemNames[i].color;
			if (this.items[i].GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				this.items[i].GetComponent<Renderer>().material.SetColor("_TintColor", this.itemNames[i].color);
			}
			if (this.itemNames[i].isBetaAccess)
			{
				if (PlaytomicController.isExhibitionBuild)
				{
					SpriteSM spriteSM = UnityEngine.Object.Instantiate(this.experiencedTextPrefab) as SpriteSM;
					spriteSM.transform.parent = this.items[i].transform;
					spriteSM.transform.localPosition = new Vector3(this.itemNames[i].alphaBetaTextXOffset, 5f, -1f);
				}
				else
				{
					SpriteSM spriteSM2 = UnityEngine.Object.Instantiate(this.paidBetaTextPrefab) as SpriteSM;
					spriteSM2.transform.parent = this.items[i].transform;
					spriteSM2.transform.localPosition = new Vector3(this.itemNames[i].alphaBetaTextXOffset, 5f, -1f);
				}
			}
			if (this.itemNames[i].isAlphaAccess)
			{
				SpriteSM spriteSM3 = UnityEngine.Object.Instantiate(this.paidAlphaTextPrefab) as SpriteSM;
				spriteSM3.transform.parent = this.items[i].transform;
				spriteSM3.transform.localPosition = new Vector3(this.itemNames[i].alphaBetaTextXOffset, 5f, -1f);
			}
			this.itemEnabled[i] = true;
			TextMesh textMesh = UnityEngine.Object.Instantiate(this.textBackdropPrefab) as TextMesh;
			textMesh.text = this.itemNames[i].name;
			textMesh.lineSpacing = this.lineSpacing;
			textMesh.transform.parent = this.items[i].transform;
			textMesh.transform.localPosition = Vector3.forward * 25f;
			textMesh.characterSize = this.itemNames[i].size;
			if (textMesh.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				textMesh.GetComponent<Renderer>().material.SetColor("_TintColor", Color.black);
			}
			this.backDrops[i] = textMesh;
		}
	}

	public void DestroyAllItems()
	{
		if (this.backDrops != null)
		{
			for (int i = 0; i < this.backDrops.Length; i++)
			{
				UnityEngine.Object.Destroy(this.backDrops[i].gameObject);
			}
		}
		if (this.items != null)
		{
			for (int j = 0; j < this.items.Length; j++)
			{
				UnityEngine.Object.Destroy(this.items[j].gameObject);
			}
		}
		this.itemEnabled = null;
		this.items = null;
		this.backDrops = null;
	}

	public void SnapHiglightToTarget()
	{
		this.menuHighlight.SetTargetPos(this.menuHighlight.TargetPos, true);
	}

	protected virtual void Update()
	{
		this.highlightIndex = Mathf.Clamp(this.highlightIndex, 0, this.items.Length - 1);
		if (this.menuActive)
		{
			this.CheckInput();
			this.RunInput();
			this.menuHighlight.SetTargetSize(this.items[this.highlightIndex].GetComponent<Renderer>().bounds.size + ((!this.itemNames[this.highlightIndex].isBetaAccess) ? Vector3.zero : (Vector3.one * 10f)));
			for (int i = 0; i < this.items.Length; i++)
			{
				if (i == this.highlightIndex)
				{
					if (this.selectedFontMaterialOverride != null)
					{
						this.items[i].GetComponent<Renderer>().material = this.selectedFontMaterialOverride;
					}
					this.items[i].transform.localScale = this.textPrefab.transform.localScale;
				}
				else
				{
					if (this.deselectedFontMaterialOverride != null)
					{
						this.items[i].GetComponent<Renderer>().material = this.deselectedFontMaterialOverride;
					}
					this.items[i].transform.localScale = this.textPrefab.transform.localScale * this.deselectedTextScale;
				}
			}
			if (this.moveHighlight)
			{
				this.menuHighlight.SetTargetPos(this.items[this.highlightIndex].transform.position, true);
			}
			else
			{
				for (int j = 0; j < this.items.Length; j++)
				{
					TextMesh textMesh = this.items[j];
					int num = Mathf.Abs(j - this.highlightIndex);
					if (this.fadeItems)
					{
						Color color = textMesh.color;
						if (j != this.highlightIndex && num > this.fadeDistance)
						{
							float to = Mathf.Clamp(0.5f - (float)(num - this.fadeDistance) * 0.2f, 0.01f, 0.5f);
							color.a = Mathf.Lerp(color.a, to, Time.deltaTime * 3f);
						}
						else
						{
							color.a = Mathf.Lerp(color.a, 1f, Time.deltaTime * 7f);
						}
						textMesh.color = color;
					}
					Vector3 vector = this.menuHighlight.transform.position;
					if (num == 1)
					{
						vector = this.menuHighlight.transform.position + Vector3.up * Mathf.Sign((float)(this.highlightIndex - j)) * this.verticalSpacing;
					}
					else if (Mathf.Abs(num) != 0)
					{
						vector = this.menuHighlight.transform.position + Vector3.up * Mathf.Sign((float)(this.highlightIndex - j)) * this.verticalSpacing + Vector3.up * Mathf.Sign((float)(this.highlightIndex - j)) * (float)(num - 1) * this.verticalSpacingCompressed;
					}
					if (vector != Vector3.zero)
					{
						textMesh.transform.position = Vector3.Lerp(textMesh.transform.position, vector, Time.deltaTime * 10f);
					}
				}
				Vector3 vector2 = new Vector3(0.9f + Mathf.Sin(Time.time * 5f) * 0.1f, 0.8f + Mathf.Sin(Time.time * 5.5f) * 0.2f, 1f);
				this.menuHighlight.GetComponent<SpriteSM>().SetSize(512f * vector2.x, 64f * vector2.y);
			}
		}
	}

	public void SnapItemsToFinalPosition()
	{
		for (int i = 0; i < this.items.Length; i++)
		{
			TextMesh textMesh = this.items[i];
			int num = Mathf.Abs(i - this.highlightIndex);
			Color color = textMesh.GetComponent<Renderer>().material.GetColor("_TintColor");
			if (i != this.highlightIndex)
			{
				float a = 0.5f;
				color.a = a;
			}
			else
			{
				color.a = 1f;
			}
			textMesh.GetComponent<Renderer>().material.SetColor("_TintColor", color);
			Vector3 vector = this.menuHighlight.transform.position;
			if (num == 1)
			{
				vector = this.menuHighlight.transform.position + Vector3.up * Mathf.Sign((float)(this.highlightIndex - i)) * this.verticalSpacing;
			}
			else if (Mathf.Abs(num) != 0)
			{
				vector = this.menuHighlight.transform.position + Vector3.up * Mathf.Sign((float)(this.highlightIndex - i)) * this.verticalSpacing + Vector3.up * Mathf.Sign((float)(this.highlightIndex - i)) * (float)(num - 1) * this.verticalSpacingCompressed;
			}
			if (vector != Vector3.zero)
			{
				textMesh.transform.position = vector;
			}
		}
	}

	protected void Bounce()
	{
		this.bouncingHilight = true;
		this.bouncingHilightCounter = 0f;
	}

	protected virtual void CheckInput()
	{
		this.decline = false; this.up = (this.down = (this.left = (this.right = (this.jump = (this.accept = (this.decline ))))));
		if (Time.time - this.lastInputTime < 0.15f)
		{
			return;
		}
		if (!this.IsAnyItemActive)
		{
			return;
		}
		if (!InputReader.GetMenuInputStandardKeys(ref this.up, ref this.down, ref this.left, ref this.right, ref this.accept, ref this.decline))
		{
			if (this.controlledByControllerID == -1)
			{
				InputReader.GetMenuInputCombined(ref this.up, ref this.down, ref this.left, ref this.right, ref this.accept, ref this.decline);
			}
			else
			{
				InputReader.GetMenuInput(this.controlledByControllerID, ref this.up, ref this.down, ref this.left, ref this.right, ref this.accept, ref this.decline);
			}
		}
	}

	protected virtual void RunInput()
	{
		if (this.up)
		{
			do
			{
				this.highlightIndex = (this.highlightIndex - 1 + this.items.Length) % this.items.Length;
			}
			while (!this.itemEnabled[this.highlightIndex]);
			Sound instance = Sound.GetInstance();
			instance.PlaySoundEffect(this.drumSounds.attackSounds[0], 0.25f);
			this.lastInputTime = Time.time;
		}
		if (this.down)
		{
			do
			{
				this.highlightIndex = (this.highlightIndex + 1) % this.items.Length;
			}
			while (!this.itemEnabled[this.highlightIndex]);
			Sound instance2 = Sound.GetInstance();
			instance2.PlaySoundEffect(this.drumSounds.attackSounds[0], 0.25f);
			this.lastInputTime = Time.time;
		}
		if (this.accept && this.itemNames[this.highlightIndex].invokeMethod != string.Empty)
		{
			base.SendMessage(this.itemNames[this.highlightIndex].invokeMethod, 0f);
			this.lastInputTime = Time.time;
			Sound.GetInstance().PlaySoundEffect(this.drumSounds.attackSounds[1], 0.25f);
		}
	}

	public void SetMenuItemActive(string name, bool active)
	{
		for (int i = 0; i < this.itemNames.Length; i++)
		{
			if (name.ToUpper().Equals(this.itemNames[i].name.ToUpper()))
			{
				this.SetMenuItemActive(i, active);
				return;
			}
		}
		UnityEngine.Debug.LogError("Menu Item with name " + name + " not found, could not enable/disable");
	}

	public void SetMenuItemActive(int index, bool active)
	{
		this.itemEnabled[index] = active;
		if (active)
		{
			Color color = this.items[index].GetComponent<Renderer>().material.color;
			color.a = 1f;
			this.items[index].GetComponent<Renderer>().material.color = color;
			if (this.items[index].GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				color = this.items[index].GetComponent<Renderer>().material.GetColor("_TintColor");
				color.a = 1f;
				this.items[index].GetComponent<Renderer>().material.SetColor("_TintColor", color);
			}
		}
		else
		{
			Color color2 = this.items[index].GetComponent<Renderer>().material.color;
			color2.a = 0.5f;
			this.items[index].GetComponent<Renderer>().material.color = color2;
			if (this.items[index].GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				color2 = this.items[index].GetComponent<Renderer>().material.GetColor("_TintColor");
				color2.a = 0f;
				this.items[index].GetComponent<Renderer>().material.SetColor("_TintColor", color2);
			}
			this.items[index].GetComponent<Renderer>().enabled = false;
		}
	}

	public bool startActive = true;

	public MenuBarItem[] itemNames;

	public TextMesh textPrefab;

	public TextMesh textBackdropPrefab;

	public MenuHighlightTween menuHighlight;

	public float verticalSpacing;

	public float verticalSpacingCompressed;

	public SoundHolder drumSounds;

	public bool moveHighlight = true;

	public bool overrideIndivdualItemCharacterSizes;

	public float characterSizes;

	public float lineSpacing;

	public float deselectedTextScale = 1f;

	protected TextMesh[] items;

	protected TextMesh[] backDrops;

	protected int highlightIndex;

	protected float bouncingHilightCounter;

	protected bool bouncingHilight;

	protected bool[] itemEnabled;

	protected Vector3 targetPos;

	protected int drumSoundNum;

	public SpriteSM paidBetaTextPrefab;

	public SpriteSM paidAlphaTextPrefab;

	public SpriteSM experiencedTextPrefab;

	public bool fadeItems = true;

	public int fadeDistance;

	public Material selectedFontMaterialOverride;

	public Material deselectedFontMaterialOverride;

	private bool menuActive = true;

	public GameObject menuHolder;

	public int controlledByControllerID = -1;

	protected bool up;

	protected bool down;

	protected bool left;

	protected bool right;

	protected bool jump;

	protected bool accept;

	protected bool decline;

	public float lastInputTime;
}
