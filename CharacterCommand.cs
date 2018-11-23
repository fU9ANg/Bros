// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class CharacterCommand
{
	public override string ToString()
	{
		switch (this.type)
		{
		case CharacterCommandType.Move:
			return this.type.ToString() + ((!(this.target == null)) ? (" to " + this.target.ToString()) : string.Empty);
		case CharacterCommandType.AICommand:
			return string.Concat(new object[]
			{
				"AI: ",
				this.aiCommandType,
				" ",
				(!(this.target == null)) ? (" to " + this.target.ToString()) : string.Empty,
				", ",
				this.maxTime
			});
		}
		return this.type.ToString();
	}

	public CharacterCommandType type;

	public GridPoint target;

	public int variant;

	public float maxTime = 5f;

	public EnemyActionType aiCommandType;
}
