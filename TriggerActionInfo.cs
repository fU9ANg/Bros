// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Xml.Serialization;

[XmlInclude(typeof(CharacterActionInfo))]
[XmlInclude(typeof(BombardmentActionInfo))]
[XmlInclude(typeof(BurnActionInfo))]
[XmlInclude(typeof(CameraActionInfo))]
[XmlInclude(typeof(SpawnMooksActionInfo))]
[XmlInclude(typeof(CollapseActionInfo))]
[XmlInclude(typeof(SpawnResourceActionInfo))]
[XmlInclude(typeof(ExplosionActionInfo))]
[XmlInclude(typeof(SpawnBlockActionInfo))]
[XmlInclude(typeof(VariableActionInfo))]
[XmlInclude(typeof(LevelEventActionInfo))]
[XmlInclude(typeof(ExecuteFunctionActionInfo))]
[XmlInclude(typeof(WeatherActionInfo))]
[Serializable]
public abstract class TriggerActionInfo
{
	public abstract void ShowGUI(LevelEditorGUI gui);

	public string name;

	public float timeOffset;

	public TriggerActionType type;
}
