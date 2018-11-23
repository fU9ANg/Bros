// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class CreditsTextScroller : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
		Map.MapData = null;
		MissionScreenController.SetVariables(string.Empty, WeatherType.Night, RainType.Raining);
		WeatherController.SwitchWeather(RainType.Raining, true);
		WeatherController.SwitchWeather(RainType.Raining, true);
		this.textMeshes = new List<TextMesh>();
		this.AddNextLine();
	}

	private void Update()
	{
		for (int i = this.textMeshes.Count - 1; i >= 0; i--)
		{
			this.textMeshes[i].transform.Translate(Vector3.up * this.speed * Time.deltaTime);
			if (this.textMeshes[i].transform.position.y > base.transform.position.y + this.camera.orthographicSize + this.textPrefab.characterSize + 32f)
			{
				UnityEngine.Object.Destroy(this.textMeshes[i].gameObject);
				this.textMeshes.RemoveAt(i);
				if (this.textMeshes.Count == 0)
				{
					this.missionScreenController.leaveCounter = 0.01f;
				}
			}
		}
		if (this.textMeshes.Count > 0 && this.textMeshes[this.textMeshes.Count - 1].transform.position.y > base.transform.position.y - this.camera.orthographicSize + 32f)
		{
			this.AddNextLine();
		}
		if (InputReader.GetControllerHoldingFire() > -1)
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, 10f, Time.deltaTime);
		}
		else
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.deltaTime);
		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.LoadLevel("MainMenuExpendabros");
		}
	}

	private void AddNextLine()
	{
		if (this.currentTextIndex >= this.text.Length)
		{
			return;
		}
		TextMesh textMesh = UnityEngine.Object.Instantiate(this.textPrefab, new Vector3(base.transform.position.x, base.transform.position.y - this.camera.orthographicSize - this.text[this.currentTextIndex].size, base.transform.position.z + 5f), Quaternion.identity) as TextMesh;
		textMesh.transform.parent = base.transform;
		textMesh.text = this.text[this.currentTextIndex].name;
		textMesh.characterSize = this.text[this.currentTextIndex].size * this.textScale;
		this.textMeshes.Add(textMesh);
		this.currentTextIndex++;
		textMesh.GetComponent<Renderer>().material = this.overrideTextMaterial;
	}

	public TextMesh textPrefab;

	public Material overrideTextMaterial;

	public MenuBarItem[] text;

	public float speed;

	public new Camera camera;

	private List<TextMesh> textMeshes;

	private int currentTextIndex;

	public MissionScreenController missionScreenController;

	public float textScale = 0.5f;
}
