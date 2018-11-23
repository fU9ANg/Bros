// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WorldMapWavyGrass : MonoBehaviour
{
	private void Start()
	{
		this.currentLowerLeftPixel = this.lowerLeftPixel;
		this.currentPixelDimensions = this.pixelDimensions;
		this.currentWidth = this.width;
		this.currentHeight = this.height;
		this.CalculateUVs();
		if (this.waveEffectTransform == null)
		{
			Helicopter2D helicopter2D = UnityEngine.Object.FindObjectOfType(typeof(Helicopter2D)) as Helicopter2D;
			if (helicopter2D != null)
			{
				this.waveEffectTransform = helicopter2D.transform;
			}
		}
		this.meshFilter = base.GetComponent<MeshFilter>();
		if (this.mesh == null)
		{
			this.meshFilter.sharedMesh = new Mesh();
			this.mesh = this.meshFilter.sharedMesh;
		}
		this.BuildMesh(this.width, this.height);
		this.faces[0] = 0;
		this.faces[1] = 3;
		this.faces[2] = 1;
		this.faces[3] = 3;
		this.faces[4] = 2;
		this.faces[5] = 1;
		if (this.mesh != null)
		{
			this.mesh.triangles = this.faces;
		}
		this.SetColor(new Color(1f, 1f, 1f, 1f));
		this.UpdateUVs();
		this.mesh.RecalculateBounds();
	}

	protected void CalculateUVs()
	{
		Texture mainTexture = base.GetComponent<Renderer>().sharedMaterial.mainTexture;
		this.uvMax = new Vector3((this.lowerLeftPixel.x + this.pixelDimensions.x) / (float)mainTexture.width, 1f - this.lowerLeftPixel.y / (float)mainTexture.height + this.pixelDimensions.y / (float)mainTexture.height);
		this.uvMin = new Vector3(this.lowerLeftPixel.x / (float)mainTexture.width, 1f - this.lowerLeftPixel.y / (float)mainTexture.height);
	}

	public void SetColor(Color c)
	{
		this.colors[0] = c;
		this.colors[1] = c;
		this.colors[2] = c;
		this.colors[3] = c;
		this.mesh.colors = this.colors;
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			if (this.emitFire && this.canEmitFire)
			{
				this.fireCounter += Time.deltaTime;
				if (this.fireCounter > 0.1f)
				{
					this.fireCounter -= 0.1f;
					WorldMapEffectsController.EmitFire(base.transform.position + base.transform.TransformDirection(Vector3.up) * this.height * 0.5f + base.transform.TransformDirection(Vector3.right) * this.windBlowM * this.topXOffset * 0.5f + base.transform.TransformDirection(Vector3.forward) * 0.02f, base.transform.TransformDirection(Vector3.up) * 0.2f + UnityEngine.Random.onUnitSphere * 0.02f + base.transform.TransformDirection(Vector3.right) * this.topXOffset * 0.1f, Color.white, 3f);
				}
			}
			this.windCounter += Time.deltaTime;
			if (this.windCounter > 6f + base.transform.position.x - base.transform.position.z * 0.5f)
			{
				this.windCounter -= 6f;
				this.windBlowTime = 0.4f + UnityEngine.Random.value * 1f;
				this.windBlowAmount = 1.5f + 3f * UnityEngine.Random.value;
			}
			if (this.windBlowTime > 0f)
			{
				this.windBlowTime -= Time.deltaTime;
				this.windXI += this.windBlowAmount * Time.deltaTime;
			}
			this.windXI -= this.topXOffset * Time.deltaTime * 45f;
			this.windXI *= 1f - Time.deltaTime * 2f;
			this.waveCouner += Time.deltaTime;
			if (this.waveCouner >= 0.2f)
			{
				this.waveCouner -= 0.5f * UnityEngine.Random.value + 0.1f;
				Vector3 vector = new Vector3(this.waveEffectTransform.position.x - base.transform.position.x, 0f, this.waveEffectTransform.position.z - base.transform.position.z);
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude < 1.5f)
				{
					float num = 1f - sqrMagnitude / 1.5f;
					this.windXI = Mathf.Sign(vector.x) * -2f * num;
				}
			}
			this.topXOffset += this.windXI * Time.deltaTime;
			this.BuildMesh(this.width, this.height);
		}
		else if (!this.useWholeTexture && (this.currentLowerLeftPixel != this.lowerLeftPixel || this.currentPixelDimensions != this.pixelDimensions || this.currentWidth != this.width || this.currentHeight != this.height))
		{
			this.currentLowerLeftPixel = this.lowerLeftPixel;
			this.currentPixelDimensions = this.pixelDimensions;
			this.currentWidth = this.width;
			this.currentHeight = this.height;
			this.CalculateUVs();
			this.BuildMesh(this.width, this.height);
			this.UpdateUVs();
		}
	}

	public void CalcEdges()
	{
		this.topLeft.x = this.width * -0.5f;
		this.topLeft.y = this.height;
		this.bottomRight.x = this.width * 0.5f;
		this.bottomRight.y = 0f;
	}

	public void UpdateUVs()
	{
		this.uvs[0].x = ((!this.useWholeTexture) ? this.uvMin.x : 0f);
		this.uvs[0].y = ((!this.useWholeTexture) ? this.uvMax.y : 1f);
		this.uvs[1].x = ((!this.useWholeTexture) ? this.uvMin.x : 0f);
		this.uvs[1].y = ((!this.useWholeTexture) ? this.uvMin.y : 0f);
		this.uvs[2].x = ((!this.useWholeTexture) ? this.uvMax.x : 1f);
		this.uvs[2].y = ((!this.useWholeTexture) ? this.uvMin.y : 0f);
		this.uvs[3].x = ((!this.useWholeTexture) ? this.uvMax.x : 1f);
		this.uvs[3].y = ((!this.useWholeTexture) ? this.uvMax.y : 1f);
		this.mesh.uv = this.uvs;
	}

	protected void BuildMesh(float w, float h)
	{
		if (this.mesh == null)
		{
			UnityEngine.Debug.LogError("Awake has not been called yet");
		}
		this.width = w;
		this.height = h;
		this.CalcEdges();
		this.vertices[0].x = this.offset.x + this.topLeft.x + this.topXOffset * this.windBlowM;
		this.vertices[0].y = this.offset.y + this.topLeft.y;
		this.vertices[0].z = this.offset.z;
		this.vertices[1].x = this.offset.x + this.topLeft.x;
		this.vertices[1].y = this.offset.y + this.bottomRight.y;
		this.vertices[1].z = this.offset.z;
		this.vertices[2].x = this.offset.x + this.bottomRight.x;
		this.vertices[2].y = this.offset.y + this.bottomRight.y;
		this.vertices[2].z = this.offset.z;
		this.vertices[3].x = this.offset.x + this.bottomRight.x + this.topXOffset * this.windBlowM;
		this.vertices[3].y = this.offset.y + this.topLeft.y;
		this.vertices[3].z = this.offset.z;
		this.mesh.vertices = this.vertices;
	}

	public Vector3 offset = default(Vector3);

	protected Mesh mesh;

	protected Vector2 topLeft;

	protected Vector2 bottomRight;

	public Transform waveEffectTransform;

	protected Vector3[] vertices = new Vector3[4];

	protected Color[] colors = new Color[4];

	protected Vector2[] uvs = new Vector2[4];

	protected int[] faces = new int[6];

	protected MeshFilter meshFilter;

	public float windBlowM = 1f;

	public float width = 0.2f;

	public float height = 0.2f;

	protected float topXOffset;

	protected float windCounter;

	protected float waveCouner;

	protected float windXI;

	protected float windBlowTime;

	protected float windBlowAmount;

	public bool emitFire;

	public bool canEmitFire;

	protected float fireCounter;

	public bool useWholeTexture;

	public Vector2 lowerLeftPixel;

	public Vector2 pixelDimensions;

	protected Vector2 currentLowerLeftPixel;

	protected Vector2 currentPixelDimensions;

	protected float currentWidth;

	protected float currentHeight;

	protected Vector2 uvMax;

	protected Vector2 uvMin;
}
