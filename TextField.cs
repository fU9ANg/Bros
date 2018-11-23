// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class TextField : MonoBehaviour
{
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			this.text = value.ToUpper();
			if (this.textMesh == null)
			{
				this.textMesh = base.GetComponent<TextMesh>();
			}
			this.textMesh.text = value.ToUpper();
		}
	}

	private void Awake()
	{
		this.textMesh = base.GetComponent<TextMesh>();
		this.text = this.textMesh.text.ToUpper();
	}

	private void Update()
	{
		if (this.focused)
		{
			this.textMesh.color = this.focusedColor;
			this.caretFlashTimer -= Time.deltaTime;
			if (this.caretFlashTimer < 0f)
			{
				this.showCaret = !this.showCaret;
				this.caretFlashTimer += this.caretFlashInterval;
			}
			if (this.showCaret)
			{
				this.textMesh.text = this.text + "_";
			}
			else
			{
				this.textMesh.text = this.text;
			}
			if (this.keyboardInput)
			{
				if (Input.GetKeyDown(KeyCode.Backspace) && this.text.Length > 0)
				{
					this.text = this.text.Substring(0, this.text.Length - 1);
				}
				if (this.text.Length < this.MaxCharacterLimit)
				{
					KeyCode[] array = (KeyCode[])Enum.GetValues(typeof(KeyCode));
					for (int i = 0; i < array.Length; i++)
					{
						if (Input.GetKeyDown(array[i]))
						{
							string text = array[i].ToString();
							text = text.Replace("Alpha", string.Empty);
							if (array[i] == KeyCode.Space)
							{
								text = " ";
							}
							if (array[i] == KeyCode.Underscore)
							{
								text = "_";
							}
							if (array[i] == KeyCode.Minus)
							{
								text = "-";
							}
							if (array[i] == KeyCode.Equals)
							{
								text = "=";
							}
							if (text.Length == 1)
							{
								string text2 = string.Empty + text[0];
								if (this.AcceptedCharacters.Contains(text2))
								{
									this.text += text2;
								}
							}
						}
					}
				}
			}
			else
			{
				bool flag6;
				bool flag5;
				bool flag4;
				bool flag3;
				bool flag2;
				flag6 = false; bool flag = flag2 = (flag3 = (flag4 = (flag5 = (flag6 ))));
				InputReader.GetMenuInputCombined(ref flag2, ref flag, ref flag3, ref flag4, ref flag5, ref flag6);
				if (Input.anyKeyDown)
				{
					if (Time.time - this.ReadTime <= 0.18f)
					{
						return;
					}
					this.ReadTime = Time.time;
				}
				else
				{
					this.ReadTime = Time.time - 1f;
				}
				if (!flag4 && !flag3)
				{
					if (flag)
					{
						this.PrevLetter();
					}
					if (flag2)
					{
						this.NextLetter();
					}
				}
				if (!flag && !flag2)
				{
					if (flag4)
					{
						this.AddLetter();
					}
					if (flag3)
					{
						this.DeleteLetter();
					}
				}
			}
			return;
		}
		this.textMesh.color = this.unfocusedColor;
		this.textMesh.text = this.text;
	}

	public void NextLetter()
	{
		if (!this.focused)
		{
			return;
		}
		if (this.text.Length == 0)
		{
			return;
		}
		List<int> list = this.Encode(this.text);
		List<int> list3;
		List<int> list2 = list3 = list;
		int num;
		int index = num = list.Count - 1;
		num = list3[num];
		list2[index] = num + 1;
		if (list[list.Count - 1] >= this.AcceptedCharacters.Length)
		{
			List<int> list5;
			List<int> list4 = list5 = list;
			int index2 = num = list.Count - 1;
			num = list5[num];
			list4[index2] = num - this.AcceptedCharacters.Length;
		}
		this.text = this.Decode(list);
	}

	public void PrevLetter()
	{
		if (!this.focused)
		{
			return;
		}
		if (this.text.Length == 0)
		{
			return;
		}
		List<int> list = this.Encode(this.text);
		List<int> list3;
		List<int> list2 = list3 = list;
		int num;
		int index = num = list.Count - 1;
		num = list3[num];
		list2[index] = num - 1;
		if (list[list.Count - 1] < 0)
		{
			List<int> list5;
			List<int> list4 = list5 = list;
			int index2 = num = list.Count - 1;
			num = list5[num];
			list4[index2] = num + this.AcceptedCharacters.Length;
		}
		this.text = this.Decode(list);
	}

	public void AddLetter()
	{
		if (!this.focused)
		{
			return;
		}
		if (this.text.Length >= this.MaxCharacterLimit)
		{
			return;
		}
		this.text += 'A';
	}

	public void DeleteLetter()
	{
		if (!this.focused)
		{
			return;
		}
		if (this.text.Length == 0)
		{
			return;
		}
		this.text = this.text.Substring(0, this.text.Length - 1);
	}

	private List<int> Encode(string text)
	{
		List<int> list = new List<int>();
		foreach (char c in text)
		{
			list.Add(this.GetCode(c));
		}
		return list;
	}

	private string Decode(List<int> encodedText)
	{
		string text = string.Empty;
		foreach (int code in encodedText)
		{
			text += this.GetChar(code);
		}
		return text;
	}

	private int GetCode(char c)
	{
		for (int i = 0; i < this.AcceptedCharacters.Length; i++)
		{
			if (this.AcceptedCharacters[i] == c)
			{
				return i;
			}
		}
		return 0;
	}

	private char GetChar(int code)
	{
		if (code < this.AcceptedCharacters.Length)
		{
			return this.AcceptedCharacters[code];
		}
		return ' ';
	}

	public void Focus()
	{
		this.focused = true;
	}

	public void LooseFocus()
	{
		this.focused = false;
	}

	private Color focusedColor = Color.red;

	private Color unfocusedColor = Color.white;

	private bool keyboardInput = true;

	public int MaxCharacterLimit = 12;

	public int MinCharacterLimit = 6;

	private TextMesh textMesh;

	private string text;

	private string AcceptedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890 .-?!_-";

	private float ReadTime;

	public bool focused;

	private bool showCaret;

	private float caretFlashTimer;

	private float caretFlashInterval = 0.4f;
}
