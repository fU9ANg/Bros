// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SpriteBase : MonoBehaviour
{
	protected virtual void Awake()
	{
		this.meshFilter = (MeshFilter)base.gameObject.GetComponent(typeof(MeshFilter));
		this.meshRenderer = (MeshRenderer)base.gameObject.GetComponent(typeof(MeshRenderer));
		this.meshFilter.sharedMesh = null;
		if (this.meshRenderer.sharedMaterial != null)
		{
			this.texture = this.meshRenderer.sharedMaterial.GetTexture("_MainTex");
		}
		else
		{
			UnityEngine.Debug.LogWarning("Sprite on GameObject \"" + base.name + "\" has not been assigned a material.");
		}
		if (!SpriteAnimationPump.pumpIsRunning && Application.isPlaying)
		{
			SpriteAnimationPump.Instance.StartAnimationPump();
		}
		if (this.createNormals)
		{
			this.normals = new Vector3[4];
			for (int i = 0; i < 4; i++)
			{
				this.normals[i] = Vector3.back;
			}
		}
	}

	protected virtual void Start()
	{
		this.prevUVRect = this.uvRect;
		if (this.texture != null)
		{
			this.pixelsPerUV.x = (float)this.texture.width;
			this.pixelsPerUV.y = (float)this.texture.height;
		}
		this.SetCamera(Camera.main);
	}

	protected virtual void Init()
	{
		if (this.mesh == null)
		{
			this.meshFilter.sharedMesh = new Mesh();
			this.mesh = this.meshFilter.sharedMesh;
		}
		this.mesh.Clear();
		this.mesh.vertices = this.vertices;
		if (this.createNormals)
		{
			this.mesh.normals = this.normals;
		}
		this.mesh.uv = this.uvs;
		this.mesh.colors = this.colors;
		this.mesh.triangles = this.faces;
		this.SetWindingOrder(this.winding);
		this.CalcUVs();
		this.SetBleedCompensation(this.bleedCompensation);
		if (this.pixelPerfect)
		{
			if (this.texture == null && this.meshRenderer.sharedMaterial != null)
			{
				this.texture = this.meshRenderer.sharedMaterial.GetTexture("_MainTex");
			}
			if (this.texture != null)
			{
				this.pixelsPerUV.x = (float)this.texture.width;
				this.pixelsPerUV.y = (float)this.texture.height;
			}
			this.SetCamera(Camera.main);
		}
		else
		{
			this.SetSize(this.width, this.height);
		}
		this.SetColor(this.color);
	}

	public virtual void Clear()
	{
		this.billboarded = false;
		this.SetColor(Color.white);
		this.offset = Vector3.zero;
		this.animCompleteDelegate = null;
	}

	public void OnDisable()
	{
		if (this.animating)
		{
			this.RemoveFromAnimatedList();
			this.animating = true;
		}
	}

	public void OnEnable()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.animating)
		{
			this.animating = false;
			this.AddToAnimatedList();
		}
	}

	public virtual void Copy(SpriteBase s)
	{
		base.GetComponent<Renderer>().sharedMaterial = s.GetComponent<Renderer>().sharedMaterial;
		this.texture = base.GetComponent<Renderer>().sharedMaterial.mainTexture;
		if (this.texture != null)
		{
			this.pixelsPerUV.x = (float)this.texture.width;
			this.pixelsPerUV.y = (float)this.texture.height;
		}
		this.plane = s.plane;
		this.winding = s.winding;
		this.offset = s.offset;
		this.anchor = s.anchor;
		this.autoResize = s.autoResize;
		this.pixelPerfect = s.pixelPerfect;
		this.SetColor(s.color);
	}

	public virtual void CalcUVs()
	{
	}

	public virtual void RecalcTexture()
	{
		this.texture = base.GetComponent<Renderer>().sharedMaterial.mainTexture;
		if (this.texture != null)
		{
			this.pixelsPerUV.x = (float)this.texture.width;
			this.pixelsPerUV.y = (float)this.texture.height;
		}
	}

	public void CalcEdges()
	{
		switch (this.anchor)
		{
		case SpriteBase.ANCHOR_METHOD.UPPER_LEFT:
			this.topLeft.x = 0f;
			this.topLeft.y = 0f;
			this.bottomRight.x = this.width;
			this.bottomRight.y = -this.height;
			break;
		case SpriteBase.ANCHOR_METHOD.UPPER_CENTER:
			this.topLeft.x = this.width * -0.5f;
			this.topLeft.y = 0f;
			this.bottomRight.x = this.width * 0.5f;
			this.bottomRight.y = -this.height;
			break;
		case SpriteBase.ANCHOR_METHOD.UPPER_RIGHT:
			this.topLeft.x = -this.width;
			this.topLeft.y = 0f;
			this.bottomRight.x = 0f;
			this.bottomRight.y = -this.height;
			break;
		case SpriteBase.ANCHOR_METHOD.MIDDLE_LEFT:
			this.topLeft.x = 0f;
			this.topLeft.y = this.height * 0.5f;
			this.bottomRight.x = this.width;
			this.bottomRight.y = this.height * -0.5f;
			break;
		case SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER:
			this.topLeft.x = this.width * -0.5f;
			this.topLeft.y = this.height * 0.5f;
			this.bottomRight.x = this.width * 0.5f;
			this.bottomRight.y = this.height * -0.5f;
			break;
		case SpriteBase.ANCHOR_METHOD.MIDDLE_RIGHT:
			this.topLeft.x = -this.width;
			this.topLeft.y = this.height * 0.5f;
			this.bottomRight.x = 0f;
			this.bottomRight.y = this.height * -0.5f;
			break;
		case SpriteBase.ANCHOR_METHOD.BOTTOM_LEFT:
			this.topLeft.x = 0f;
			this.topLeft.y = this.height;
			this.bottomRight.x = this.width;
			this.bottomRight.y = 0f;
			break;
		case SpriteBase.ANCHOR_METHOD.BOTTOM_CENTER:
			this.topLeft.x = this.width * -0.5f;
			this.topLeft.y = this.height;
			this.bottomRight.x = this.width * 0.5f;
			this.bottomRight.y = 0f;
			break;
		case SpriteBase.ANCHOR_METHOD.BOTTOM_RIGHT:
			this.topLeft.x = -this.width;
			this.topLeft.y = this.height;
			this.bottomRight.x = 0f;
			this.bottomRight.y = 0f;
			break;
		}
	}

	public void CalcSize()
	{
		if (this.pixelPerfect)
		{
			this.worldUnitsPerScreenPixel = this.curCamera.orthographicSize * 2f / SpriteBase.screenSize.y;
			this.width = this.worldUnitsPerScreenPixel * this.uvRect.width * this.pixelsPerUV.x;
			this.height = this.worldUnitsPerScreenPixel * this.uvRect.height * this.pixelsPerUV.y;
		}
		else if (this.autoResize && this.prevUVRect.width != 0f && this.prevUVRect.height != 0f)
		{
			this.tempUV.x = this.uvRect.width / this.prevUVRect.width;
			this.tempUV.y = this.uvRect.height / this.prevUVRect.height;
			this.width *= this.tempUV.x;
			this.height *= this.tempUV.y;
		}
		this.prevUVRect = this.uvRect;
		this.SetSize(this.width, this.height);
	}

	public void SetSize(float width, float height)
	{
		switch (this.plane)
		{
		case SpriteBase.SPRITE_PLANE.XY:
			this.SetSizeXY(width, height);
			break;
		case SpriteBase.SPRITE_PLANE.XZ:
			this.SetSizeXZ(width, height);
			break;
		case SpriteBase.SPRITE_PLANE.YZ:
			this.SetSizeYZ(width, height);
			break;
		}
		if (this.resizedDelegate != null)
		{
			this.resizedDelegate(width, height, this);
		}
	}

	protected void SetSizeXY(float w, float h)
	{
		if (this.mesh == null)
		{
			UnityEngine.Debug.LogError("Awake has not been called yet");
		}
		this.width = w;
		this.height = h;
		this.CalcEdges();
		this.vertices[0].x = this.offset.x + this.topLeft.x;
		this.vertices[0].y = this.offset.y + this.topLeft.y;
		this.vertices[0].z = this.offset.z;
		this.vertices[1].x = this.offset.x + this.topLeft.x;
		this.vertices[1].y = this.offset.y + this.bottomRight.y;
		this.vertices[1].z = this.offset.z;
		this.vertices[2].x = this.offset.x + this.bottomRight.x;
		this.vertices[2].y = this.offset.y + this.bottomRight.y;
		this.vertices[2].z = this.offset.z;
		this.vertices[3].x = this.offset.x + this.bottomRight.x;
		this.vertices[3].y = this.offset.y + this.topLeft.y;
		this.vertices[3].z = this.offset.z;
		this.mesh.vertices = this.vertices;
		this.mesh.RecalculateBounds();
	}

	protected void SetSizeXZ(float w, float h)
	{
		this.width = w;
		this.height = h;
		this.CalcEdges();
		this.vertices[0].x = this.offset.x + this.topLeft.x;
		this.vertices[0].y = this.offset.y;
		this.vertices[0].z = this.offset.z + this.topLeft.y;
		this.vertices[1].x = this.offset.x + this.topLeft.x;
		this.vertices[1].y = this.offset.y;
		this.vertices[1].z = this.offset.z + this.bottomRight.y;
		this.vertices[2].x = this.offset.x + this.bottomRight.x;
		this.vertices[2].y = this.offset.y;
		this.vertices[2].z = this.offset.z + this.bottomRight.y;
		this.vertices[3].x = this.offset.x + this.bottomRight.x;
		this.vertices[3].y = this.offset.y;
		this.vertices[3].z = this.offset.z + this.topLeft.y;
		this.mesh.vertices = this.vertices;
		this.mesh.RecalculateBounds();
	}

	protected void SetSizeYZ(float w, float h)
	{
		this.width = w;
		this.height = h;
		this.CalcEdges();
		this.vertices[0].x = this.offset.x;
		this.vertices[0].y = this.offset.y + this.topLeft.y;
		this.vertices[0].z = this.offset.z + this.topLeft.x;
		this.vertices[1].x = this.offset.x;
		this.vertices[1].y = this.offset.y + this.bottomRight.y;
		this.vertices[1].z = this.offset.z + this.topLeft.x;
		this.vertices[2].x = this.offset.x;
		this.vertices[2].y = this.offset.y + this.bottomRight.y;
		this.vertices[2].z = this.offset.z + this.bottomRight.x;
		this.vertices[3].x = this.offset.x;
		this.vertices[3].y = this.offset.y + this.topLeft.y;
		this.vertices[3].z = this.offset.z + this.bottomRight.x;
		this.mesh.vertices = this.vertices;
		this.mesh.RecalculateBounds();
	}

	public void UpdateUVs()
	{
		if (this.winding == SpriteBase.WINDING_ORDER.CW)
		{
			this.uvs[0].x = this.uvRect.x;
			this.uvs[0].y = this.uvRect.yMax;
			this.uvs[1].x = this.uvRect.x;
			this.uvs[1].y = this.uvRect.y;
			this.uvs[2].x = this.uvRect.xMax;
			this.uvs[2].y = this.uvRect.y;
			this.uvs[3].x = this.uvRect.xMax;
			this.uvs[3].y = this.uvRect.yMax;
		}
		else
		{
			this.uvs[3].x = this.uvRect.x;
			this.uvs[3].y = this.uvRect.yMax;
			this.uvs[2].x = this.uvRect.x;
			this.uvs[2].y = this.uvRect.y;
			this.uvs[1].x = this.uvRect.xMax;
			this.uvs[1].y = this.uvRect.y;
			this.uvs[0].x = this.uvRect.xMax;
			this.uvs[0].y = this.uvRect.yMax;
		}
		this.mesh.uv = this.uvs;
	}

	public void TransformBillboarded(Transform t)
	{
	}

	public void SetColor(Color c)
	{
		this.color = c;
		this.colors[0] = this.color;
		this.colors[1] = this.color;
		this.colors[2] = this.color;
		this.colors[3] = this.color;
		this.mesh.colors = this.colors;
	}

	public void SetCamera(Camera c)
	{
		if (c == null)
		{
			return;
		}
		SpriteBase.screenSize.x = c.pixelWidth;
		SpriteBase.screenSize.y = c.pixelHeight;
		this.curCamera = c;
		this.CalcSize();
	}

	public void SetAnimCompleteDelegate(SpriteBase.AnimCompleteDelegate del)
	{
		this.animCompleteDelegate = del;
	}

	public void SetSpriteResizedDelegate(SpriteBase.SpriteResizedDelegate del)
	{
		this.resizedDelegate = del;
	}

	public virtual bool StepAnim(float time)
	{
		return false;
	}

	public void PauseAnim()
	{
		this.RemoveFromAnimatedList();
	}

	public virtual void StopAnim()
	{
	}

	public void RevertToStatic()
	{
		if (this.animating)
		{
			this.StopAnim();
		}
		this.CalcUVs();
		this.SetBleedCompensation();
		if (this.autoResize || this.pixelPerfect)
		{
			this.CalcSize();
		}
	}

	protected void AddToAnimatedList()
	{
		if (!SpriteAnimationPump.pumpIsRunning)
		{
			SpriteAnimationPump.Instance.StartAnimationPump();
		}
		if (this.animating)
		{
			return;
		}
		this.animating = true;
		SpriteAnimationPump.Add(this);
	}

	protected void RemoveFromAnimatedList()
	{
		SpriteAnimationPump.Remove(this);
		this.animating = false;
	}

	public bool IsAnimating()
	{
		return this.animating;
	}

	public void SetBleedCompensation()
	{
		this.SetBleedCompensation(this.bleedCompensation);
	}

	public void SetBleedCompensation(float x, float y)
	{
		this.SetBleedCompensation(new Vector2(x, y));
	}

	public void SetBleedCompensation(Vector2 xy)
	{
		this.bleedCompensation = xy;
		this.bleedCompensationUV = this.PixelSpaceToUVSpace(this.bleedCompensation);
		this.uvRect.x = this.uvRect.x + this.bleedCompensationUV.x;
		this.uvRect.y = this.uvRect.y + this.bleedCompensationUV.y;
		this.uvRect.xMax = this.uvRect.xMax - this.bleedCompensationUV.x * 2f;
		this.uvRect.yMax = this.uvRect.yMax - this.bleedCompensationUV.y * 2f;
		this.UpdateUVs();
	}

	public void SetPlane(SpriteBase.SPRITE_PLANE p)
	{
		this.plane = p;
		this.SetSize(this.width, this.height);
	}

	public void SetWindingOrder(SpriteBase.WINDING_ORDER order)
	{
		this.winding = order;
		if (this.winding == SpriteBase.WINDING_ORDER.CCW)
		{
			this.faces[0] = 0;
			this.faces[1] = 1;
			this.faces[2] = 3;
			this.faces[3] = 3;
			this.faces[4] = 1;
			this.faces[5] = 2;
		}
		else
		{
			this.faces[0] = 0;
			this.faces[1] = 3;
			this.faces[2] = 1;
			this.faces[3] = 3;
			this.faces[4] = 2;
			this.faces[5] = 1;
		}
		if (this.mesh != null)
		{
			this.mesh.triangles = this.faces;
		}
	}

	public void SetUVs(Rect uv)
	{
		this.uvRect = uv;
		this.SetBleedCompensation();
		if (this.autoResize || this.pixelPerfect)
		{
			this.CalcSize();
		}
	}

	public void SetUVsFromPixelCoords(Rect pxCoords)
	{
		this.tempUV = this.PixelCoordToUVCoord((int)pxCoords.x, (int)pxCoords.y);
		this.uvRect.x = this.tempUV.x;
		this.uvRect.y = this.tempUV.y;
		this.tempUV = this.PixelCoordToUVCoord((int)pxCoords.xMax, (int)pxCoords.yMax);
		this.uvRect.xMax = this.tempUV.x;
		this.uvRect.yMax = this.tempUV.y;
		this.SetBleedCompensation();
		if (this.autoResize || this.pixelPerfect)
		{
			this.CalcSize();
		}
	}

	public void SetAnchor(SpriteBase.ANCHOR_METHOD a)
	{
		this.anchor = a;
		this.SetSize(this.width, this.height);
	}

	public void SetOffset(Vector3 o)
	{
		this.offset = o;
		this.SetSize(this.width, this.height);
	}

	public Vector2 PixelSpaceToUVSpace(Vector2 xy)
	{
		if (this.texture == null)
		{
			return Vector2.zero;
		}
		return new Vector2(xy.x / (float)this.texture.width, xy.y / (float)this.texture.height);
	}

	public Vector2 PixelSpaceToUVSpace(int x, int y)
	{
		return this.PixelSpaceToUVSpace(new Vector2((float)x, (float)y));
	}

	public Vector2 PixelCoordToUVCoord(Vector2 xy)
	{
		Vector2 result = this.PixelSpaceToUVSpace(xy);
		result.y = 1f - result.y;
		return result;
	}

	public Vector2 PixelCoordToUVCoord(int x, int y)
	{
		return this.PixelCoordToUVCoord(new Vector2((float)x, (float)y));
	}

	public SpriteBase.SPRITE_PLANE plane;

	public SpriteBase.WINDING_ORDER winding = SpriteBase.WINDING_ORDER.CW;

	public float width;

	public float height;

	public Vector2 bleedCompensation;

	public SpriteBase.ANCHOR_METHOD anchor = SpriteBase.ANCHOR_METHOD.MIDDLE_CENTER;

	public bool pixelPerfect;

	public bool autoResize;

	protected Vector2 bleedCompensationUV;

	protected Rect uvRect;

	protected Vector2 topLeft;

	protected Vector2 bottomRight;

	[HideInInspector]
	public bool billboarded;

	public Vector3 offset = default(Vector3);

	public Color color = Color.white;

	protected MeshFilter meshFilter;

	protected MeshRenderer meshRenderer;

	protected Mesh mesh;

	protected Texture texture;

	protected Vector3[] vertices = new Vector3[4];

	protected Color[] colors = new Color[4];

	protected Vector2[] uvs = new Vector2[4];

	protected Vector3[] normals;

	protected int[] faces = new int[6];

	public bool createNormals;

	protected static Vector2 screenSize;

	protected Camera curCamera;

	protected Rect prevUVRect;

	protected Vector2 pixelsPerUV;

	protected float worldUnitsPerScreenPixel;

	public bool playAnimOnStart;

	public int defaultAnim;

	protected SpriteBase.AnimCompleteDelegate animCompleteDelegate;

	protected SpriteBase.SpriteResizedDelegate resizedDelegate;

	protected float timeSinceLastFrame;

	protected float timeBetweenAnimFrames;

	protected int framesToAdvance;

	protected bool animating;

	protected int i;

	protected Vector2 tempUV;

	public enum SPRITE_PLANE
	{
		XY,
		XZ,
		YZ
	}

	public enum ANCHOR_METHOD
	{
		UPPER_LEFT,
		UPPER_CENTER,
		UPPER_RIGHT,
		MIDDLE_LEFT,
		MIDDLE_CENTER,
		MIDDLE_RIGHT,
		BOTTOM_LEFT,
		BOTTOM_CENTER,
		BOTTOM_RIGHT
	}

	public enum WINDING_ORDER
	{
		CCW,
		CW
	}

	public delegate void AnimCompleteDelegate();

	public delegate void SpriteResizedDelegate(float newWidth, float newHeight, SpriteBase sprite);
}
