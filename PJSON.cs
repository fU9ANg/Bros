// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

internal class PJSON
{
	public static object JsonDecode(string json)
	{
		PJSON.instance.lastDecode = json;
		if (json != null)
		{
			char[] json2 = json.ToCharArray();
			int num = 0;
			bool flag = true;
			object result = PJSON.instance.ParseValue(json2, ref num, ref flag);
			if (flag)
			{
				PJSON.instance.lastErrorIndex = -1;
			}
			else
			{
				PJSON.instance.lastErrorIndex = num;
			}
			return result;
		}
		return null;
	}

	public static string JsonEncode(object json)
	{
		StringBuilder stringBuilder = new StringBuilder(5000);
		bool flag = PJSON.instance.SerializeValue(json, stringBuilder);
		return (!flag) ? null : stringBuilder.ToString();
	}

	public static bool LastDecodeSuccessful()
	{
		return PJSON.instance.lastErrorIndex == -1;
	}

	public static int GetLastErrorIndex()
	{
		return PJSON.instance.lastErrorIndex;
	}

	public static string GetLastErrorSnippet()
	{
		if (PJSON.instance.lastErrorIndex == -1)
		{
			return string.Empty;
		}
		int num = PJSON.instance.lastErrorIndex - 5;
		int num2 = PJSON.instance.lastErrorIndex + 15;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 >= PJSON.instance.lastDecode.Length)
		{
			num2 = PJSON.instance.lastDecode.Length - 1;
		}
		return PJSON.instance.lastDecode.Substring(num, num2 - num + 1);
	}

	protected Dictionary<string, object> ParseObject(char[] json, ref int index)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		this.NextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = this.LookAhead(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				this.NextToken(json, ref index);
			}
			else
			{
				if (num == 2)
				{
					this.NextToken(json, ref index);
					return dictionary;
				}
				string text = this.ParseString(json, ref index);
				if (text == null)
				{
					return null;
				}
				num = this.NextToken(json, ref index);
				if (num != 5)
				{
					return null;
				}
				bool flag2 = true;
				object value = this.ParseValue(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				dictionary[text] = value;
			}
		}
		return dictionary;
	}

	protected List<object> ParseArray(char[] json, ref int index)
	{
		List<object> list = new List<object>();
		this.NextToken(json, ref index);
		bool flag = false;
		while (!flag)
		{
			int num = this.LookAhead(json, index);
			if (num == 0)
			{
				return null;
			}
			if (num == 6)
			{
				this.NextToken(json, ref index);
			}
			else
			{
				if (num == 4)
				{
					this.NextToken(json, ref index);
					break;
				}
				bool flag2 = true;
				object item = this.ParseValue(json, ref index, ref flag2);
				if (!flag2)
				{
					return null;
				}
				list.Add(item);
			}
		}
		return list;
	}

	protected object ParseValue(char[] json, ref int index, ref bool success)
	{
		switch (this.LookAhead(json, index))
		{
		case 1:
			return this.ParseObject(json, ref index);
		case 3:
			return this.ParseArray(json, ref index);
		case 7:
			return this.ParseString(json, ref index);
		case 8:
			return this.ParseNumber(json, ref index);
		case 9:
			this.NextToken(json, ref index);
			return bool.Parse("TRUE");
		case 10:
			this.NextToken(json, ref index);
			return bool.Parse("FALSE");
		case 11:
			this.NextToken(json, ref index);
			return null;
		}
		success = false;
		return null;
	}

	protected string ParseString(char[] json, ref int index)
	{
		StringBuilder stringBuilder = new StringBuilder(string.Empty);
		this.EatWhitespace(json, ref index);
		char c = json[index++];
		bool flag = false;
		while (!flag)
		{
			if (index == json.Length)
			{
				break;
			}
			c = json[index++];
			if (c == '"')
			{
				flag = true;
				break;
			}
			if (c == '\\')
			{
				if (index == json.Length)
				{
					break;
				}
				c = json[index++];
				if (c == '"')
				{
					stringBuilder.Append('"');
				}
				else if (c == '\\')
				{
					stringBuilder.Append('\\');
				}
				else if (c == '/')
				{
					stringBuilder.Append('/');
				}
				else if (c == 'b')
				{
					stringBuilder.Append('\b');
				}
				else if (c == 'f')
				{
					stringBuilder.Append('\f');
				}
				else if (c == 'n')
				{
					stringBuilder.Append('\n');
				}
				else if (c == 'r')
				{
					stringBuilder.Append('\r');
				}
				else if (c == 't')
				{
					stringBuilder.Append('\t');
				}
				else if (c == 'u')
				{
					int num = json.Length - index;
					if (num < 4)
					{
						break;
					}
					char[] array = new char[4];
					Array.Copy(json, index, array, 0, 4);
					uint utf = uint.Parse(new string(array), NumberStyles.HexNumber);
					stringBuilder.Append(char.ConvertFromUtf32((int)utf));
					index += 4;
				}
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		if (!flag)
		{
			return null;
		}
		return stringBuilder.ToString();
	}

	protected double ParseNumber(char[] json, ref int index)
	{
		this.EatWhitespace(json, ref index);
		int lastIndexOfNumber = this.GetLastIndexOfNumber(json, index);
		int num = lastIndexOfNumber - index + 1;
		char[] array = new char[num];
		Array.Copy(json, index, array, 0, num);
		index = lastIndexOfNumber + 1;
		return double.Parse(new string(array), CultureInfo.InvariantCulture);
	}

	protected int GetLastIndexOfNumber(char[] json, int index)
	{
		int i;
		for (i = index; i < json.Length; i++)
		{
			if ("0123456789+-.eE".IndexOf(json[i]) == -1)
			{
				break;
			}
		}
		return i - 1;
	}

	protected void EatWhitespace(char[] json, ref int index)
	{
		while (index < json.Length)
		{
			if (" \t\n\r".IndexOf(json[index]) == -1)
			{
				break;
			}
			index++;
		}
	}

	protected int LookAhead(char[] json, int index)
	{
		int num = index;
		return this.NextToken(json, ref num);
	}

	protected int NextToken(char[] json, ref int index)
	{
		this.EatWhitespace(json, ref index);
		if (index == json.Length)
		{
			return 0;
		}
		char c = json[index];
		index++;
		char c2 = c;
		switch (c2)
		{
		case '"':
			return 7;
		default:
			switch (c2)
			{
			case '[':
				return 3;
			default:
			{
				switch (c2)
				{
				case '{':
					return 1;
				case '}':
					return 2;
				}
				index--;
				int num = json.Length - index;
				if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
				{
					index += 5;
					return 10;
				}
				if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
				{
					index += 4;
					return 9;
				}
				if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
				{
					index += 4;
					return 11;
				}
				return 0;
			}
			case ']':
				return 4;
			}
			break;
		case ',':
			return 6;
		case '-':
		case '0':
		case '1':
		case '2':
		case '3':
		case '4':
		case '5':
		case '6':
		case '7':
		case '8':
		case '9':
			return 8;
		case ':':
			return 5;
		}
	}

	protected bool SerializeObjectOrArray(object objectOrArray, StringBuilder builder)
	{
		if (objectOrArray is Dictionary<string, object>)
		{
			return this.SerializeObject((Dictionary<string, object>)objectOrArray, builder);
		}
		return objectOrArray is List<object> && this.SerializeArray((List<object>)objectOrArray, builder);
	}

	protected bool SerializeObject(Dictionary<string, object> anObject, StringBuilder builder)
	{
		builder.Append("{");
		IDictionaryEnumerator dictionaryEnumerator = anObject.GetEnumerator();
		bool flag = true;
		while (dictionaryEnumerator.MoveNext())
		{
			string aString = dictionaryEnumerator.Key.ToString();
			object value = dictionaryEnumerator.Value;
			if (!flag)
			{
				builder.Append(", ");
			}
			this.SerializeString(aString, builder);
			builder.Append(":");
			if (!this.SerializeValue(value, builder))
			{
				return false;
			}
			flag = false;
		}
		builder.Append("}");
		return true;
	}

	protected bool SerializeArray(IList anArray, StringBuilder builder)
	{
		builder.Append("[");
		bool flag = true;
		for (int i = 0; i < anArray.Count; i++)
		{
			object value = anArray[i];
			if (!flag)
			{
				builder.Append(", ");
			}
			if (!this.SerializeValue(value, builder))
			{
				return false;
			}
			flag = false;
		}
		builder.Append("]");
		return true;
	}

	protected bool SerializeValue(object value, StringBuilder builder)
	{
		if (value is string)
		{
			this.SerializeString((string)value, builder);
		}
		else if (value is Dictionary<string, object>)
		{
			this.SerializeObject((Dictionary<string, object>)value, builder);
		}
		else if (value is IList)
		{
			this.SerializeArray((IList)value, builder);
		}
		else if (this.IsNumeric(value))
		{
			this.SerializeNumber(Convert.ToDouble(value), builder);
		}
		else if (value is bool && (bool)value)
		{
			builder.Append("true");
		}
		else if (value is bool && !(bool)value)
		{
			builder.Append("false");
		}
		else
		{
			if (value != null)
			{
				return false;
			}
			builder.Append("null");
		}
		return true;
	}

	protected void SerializeString(string aString, StringBuilder builder)
	{
		builder.Append("\"");
		foreach (char c in aString.ToCharArray())
		{
			if (c == '"')
			{
				builder.Append("\\\"");
			}
			else if (c == '\\')
			{
				builder.Append("\\\\");
			}
			else if (c == '\b')
			{
				builder.Append("\\b");
			}
			else if (c == '\f')
			{
				builder.Append("\\f");
			}
			else if (c == '\n')
			{
				builder.Append("\\n");
			}
			else if (c == '\r')
			{
				builder.Append("\\r");
			}
			else if (c == '\t')
			{
				builder.Append("\\t");
			}
			else
			{
				int num = Convert.ToInt32(c);
				if (num >= 32 && num <= 126)
				{
					builder.Append(c);
				}
				else
				{
					builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
				}
			}
		}
		builder.Append("\"");
	}

	protected void SerializeNumber(double number, StringBuilder builder)
	{
		builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
	}

	protected bool IsNumeric(object o)
	{
		try
		{
			double.Parse(o.ToString());
		}
		catch (Exception)
		{
			return false;
		}
		return true;
	}

	public const int TOKEN_NONE = 0;

	public const int TOKEN_CURLY_OPEN = 1;

	public const int TOKEN_CURLY_CLOSE = 2;

	public const int TOKEN_SQUARED_OPEN = 3;

	public const int TOKEN_SQUARED_CLOSE = 4;

	public const int TOKEN_COLON = 5;

	public const int TOKEN_COMMA = 6;

	public const int TOKEN_STRING = 7;

	public const int TOKEN_NUMBER = 8;

	public const int TOKEN_TRUE = 9;

	public const int TOKEN_FALSE = 10;

	public const int TOKEN_NULL = 11;

	private const int BUILDER_CAPACITY = 5000;

	private static PJSON instance = new PJSON();

	protected int lastErrorIndex = -1;

	protected string lastDecode = string.Empty;
}
