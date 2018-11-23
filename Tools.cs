// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public static class Tools
{
	private static string GetHex(int num)
	{
		return string.Empty + "0123456789ABCDEF"[num];
	}

	private static int HexToInt(char hexChar)
	{
		switch (hexChar)
		{
		case '0':
			return 0;
		case '1':
			return 1;
		case '2':
			return 2;
		case '3':
			return 3;
		case '4':
			return 4;
		case '5':
			return 5;
		case '6':
			return 6;
		case '7':
			return 7;
		case '8':
			return 8;
		case '9':
			return 9;
		case 'A':
			return 10;
		case 'B':
			return 11;
		case 'C':
			return 12;
		case 'D':
			return 13;
		case 'E':
			return 14;
		case 'F':
			return 15;
		}
		return -1;
	}

	public static string RGBToHex(Color color)
	{
		float num = color.r * 255f;
		float num2 = color.g * 255f;
		float num3 = color.b * 255f;
		string hex = Tools.GetHex(Mathf.FloorToInt(num / 16f));
		string hex2 = Tools.GetHex(Mathf.RoundToInt(num) % 16);
		string hex3 = Tools.GetHex(Mathf.FloorToInt(num2 / 16f));
		string hex4 = Tools.GetHex(Mathf.RoundToInt(num2) % 16);
		string hex5 = Tools.GetHex(Mathf.FloorToInt(num3 / 16f));
		string hex6 = Tools.GetHex(Mathf.RoundToInt(num3) % 16);
		return string.Concat(new string[]
		{
			"#",
			hex,
			hex2,
			hex3,
			hex4,
			hex5,
			hex6
		});
	}

	public static Color HexToRGB(string color)
	{
		float r = ((float)Tools.HexToInt(color[1]) + (float)Tools.HexToInt(color[0]) * 16f) / 255f;
		float g = ((float)Tools.HexToInt(color[3]) + (float)Tools.HexToInt(color[2]) * 16f) / 255f;
		float b = ((float)Tools.HexToInt(color[5]) + (float)Tools.HexToInt(color[4]) * 16f) / 255f;
		return new Color
		{
			r = r,
			g = g,
			b = b,
			a = 1f
		};
	}

	private static int HexStringToInt(string hexString)
	{
		int num = 0;
		int num2 = 1;
		hexString = hexString.ToUpper();
		char[] array = hexString.ToCharArray(0, hexString.Length);
		for (int i = hexString.Length - 1; i >= 0; i--)
		{
			int num3;
			if (array[i] >= '0' && array[i] <= '9')
			{
				num3 = (int)(array[i] - '0');
			}
			else
			{
				if (array[i] < 'A' || array[i] > 'F')
				{
					return -1;
				}
				num3 = (int)(array[i] - 'A' + '\n');
			}
			num += num3 * num2;
			num2 *= 16;
		}
		return num;
	}

	public static void FancyLabel(Rect rect, string text, Font normalFont, Font boldFont, Font italicFont, TextAlignment alignment)
	{
		int num = 0;
		bool flag = false;
		bool flag2 = false;
		Color contentColor = GUI.contentColor;
		Color textColor = new Color(contentColor.r, contentColor.g, contentColor.b, contentColor.a);
		Font font = GUI.skin.font;
		Font font2 = null;
		GUIStyle guistyle = new GUIStyle();
		if (normalFont != null)
		{
			guistyle.font = normalFont;
		}
		else
		{
			guistyle.font = font;
		}
		guistyle.padding.bottom = -5;
		GUILayout.BeginArea(rect);
		GUILayout.BeginVertical(new GUILayoutOption[]
		{
			GUILayout.ExpandHeight(true),
			GUILayout.Width(rect.height),
			GUILayout.MinWidth(rect.height)
		});
		GUILayout.BeginHorizontal(new GUILayoutOption[]
		{
			GUILayout.ExpandWidth(true),
			GUILayout.Width(rect.width),
			GUILayout.MinWidth(rect.width)
		});
		if (alignment == TextAlignment.Right || alignment == TextAlignment.Center)
		{
			GUILayout.FlexibleSpace();
		}
		while (!flag)
		{
			int num2 = 0;
			int num3 = text.IndexOf("#", num);
			int num4 = text.IndexOf("\n", num);
			int num5;
			if (num3 != -1 && (num4 == -1 || num3 < num4))
			{
				num5 = num3;
			}
			else
			{
				num5 = num4;
			}
			if (num5 == -1)
			{
				num5 = text.Length - 1;
				flag = true;
			}
			guistyle.normal.textColor = textColor;
			if (font2 != null)
			{
				guistyle.font = font2;
				font2 = null;
			}
			if (!flag)
			{
				if (text.Substring(num5, 1) == "#")
				{
					if (text.Length - num5 >= 2 && text.Substring(num5 + 1, 1) == "#")
					{
						num2 = 2;
					}
					else if (text.Length - num5 >= 2 && text.Substring(num5 + 1, 1) == "!")
					{
						textColor = new Color(contentColor.r, contentColor.g, contentColor.b, contentColor.a);
						num5--;
						num2 = 3;
					}
					else if (text.Length - num5 >= 2 && text.Substring(num5 + 1, 1) == "n")
					{
						if (normalFont != null)
						{
							font2 = normalFont;
						}
						else
						{
							font2 = font;
						}
						num5--;
						num2 = 3;
					}
					else if (text.Length - num5 >= 2 && text.Substring(num5 + 1, 1) == "x")
					{
						if (boldFont != null)
						{
							font2 = boldFont;
						}
						else
						{
							font2 = font;
						}
						num5--;
						num2 = 3;
					}
					else if (text.Length - num5 >= 2 && text.Substring(num5 + 1, 1) == "i")
					{
						if (italicFont != null)
						{
							font2 = italicFont;
						}
						else
						{
							font2 = font;
						}
						num5--;
						num2 = 3;
					}
					else
					{
						if (text.Length - num5 < 10)
						{
							UnityEngine.Debug.Log("Invalid # escape sequence");
							return;
						}
						string hexString = text.Substring(num5 + 1, 2);
						string hexString2 = text.Substring(num5 + 3, 2);
						string hexString3 = text.Substring(num5 + 5, 2);
						string hexString4 = text.Substring(num5 + 7, 2);
						float num6 = (float)Tools.HexStringToInt(hexString) / 255f;
						float num7 = (float)Tools.HexStringToInt(hexString2) / 255f;
						float num8 = (float)Tools.HexStringToInt(hexString3) / 255f;
						float num9 = (float)Tools.HexStringToInt(hexString4) / 255f;
						if (num6 < 0f || num7 < 0f || num8 < 0f || num9 < 0f)
						{
							UnityEngine.Debug.Log("Invalid color sequence");
							return;
						}
						textColor = new Color(num6, num7, num8, num9);
						num2 = 10;
						num5--;
					}
				}
				else
				{
					if (text.Length - num5 < 1 || !(text.Substring(num5, 1) == "\n"))
					{
						UnityEngine.Debug.Log("Invalid escape sequence");
						return;
					}
					flag2 = true;
					num5--;
					num2 = 2;
				}
			}
			string text2 = text.Substring(num, num5 - num + 1);
			GUILayout.Label(text2, guistyle, new GUILayoutOption[0]);
			int num10 = text2.Length - text2.TrimEnd(new char[]
			{
				' '
			}).Length;
			GUILayout.Space((float)num10 * 5f);
			if (flag2)
			{
				if (alignment == TextAlignment.Left || alignment == TextAlignment.Center)
				{
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true),
					GUILayout.Width(rect.width),
					GUILayout.MinWidth(rect.width)
				});
				if (alignment == TextAlignment.Right || alignment == TextAlignment.Center)
				{
					GUILayout.FlexibleSpace();
				}
				flag2 = false;
			}
			num = num5 + num2;
		}
		if (alignment == TextAlignment.Left || alignment == TextAlignment.Center)
		{
			GUILayout.FlexibleSpace();
		}
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
