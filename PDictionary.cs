// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;

public class PDictionary : Dictionary<string, object>
{
	public PDictionary()
	{
	}

	public PDictionary(IDictionary data)
	{
		if (data != null)
		{
			foreach (object obj in data.Keys)
			{
				string key = (string)obj;
				this[key] = data[key];
			}
		}
	}

	protected long GetLong(string s)
	{
		long result = 0L;
		if (this.ContainsKey(s))
		{
			long.TryParse(this[s].ToString(), out result);
		}
		return result;
	}

	protected float GetFloat(string s)
	{
		float result = 0f;
		if (this.ContainsKey(s))
		{
			float.TryParse(this[s].ToString(), out result);
		}
		return result;
	}

	protected bool GetBool(string s)
	{
		bool result = false;
		if (this.ContainsKey(s))
		{
			bool.TryParse(this[s].ToString(), out result);
		}
		return result;
	}

	protected int GetInt(string s)
	{
		int result = 0;
		if (this.ContainsKey(s))
		{
			int.TryParse(this[s].ToString(), out result);
		}
		return result;
	}

	protected string GetString(string s)
	{
		return (!this.ContainsKey(s)) ? string.Empty : this[s].ToString();
	}

	protected PDictionary GetDictionary(string s)
	{
		PDictionary result = new PDictionary();
		if (this.ContainsKey(s) && this[s] is IDictionary)
		{
			result = (PDictionary)this[s];
		}
		return result;
	}

	protected List<T> GetList<T>(string s)
	{
		List<T> list = null;
		if (this.ContainsKey(s) && this[s] is IList)
		{
			list = new List<T>();
			foreach (object obj in ((IList)this[s]))
			{
				if (obj is T)
				{
					list.Add((T)((object)obj));
				}
			}
		}
		return list;
	}

	protected void SetProperty(string key, object value)
	{
		if (this.ContainsKey(key))
		{
			this[key] = value;
		}
		else
		{
			this.Add(key, value);
		}
	}

	public override string ToString()
	{
		return (!this.ContainsKey("name")) ? ("No name field for type: " + base.GetType().ToString()) : this["name"].ToString();
	}
}
