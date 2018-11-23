// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class BrofortEdit : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.firstFrame)
		{
			this.firstFrame = false;
			this.Setup();
		}
		Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		int num = (int)((vector.x / 16f - (float)this.startCollumnPos) / 4f);
		float num2 = (vector.y / 16f - (float)this.groundRow) / 4f;
		int num3 = (num2 < 0f) ? ((int)num2 - 1) : ((int)num2);
		this.roomPlacementSprite.transform.position = new Vector3((float)(this.startCollumnPos * 16 + num * 4 * 16), this.groundHeight + (float)(num3 * 4 * 16), -20f);
		BrofortRoomType roomType = BrofortRoomType.None;
		if (this.rooms.Count == 0)
		{
			this.text.text = "Living Room";
			roomType = BrofortRoomType.LivingRoom;
		}
		else if (this.rooms.Count == 1)
		{
			this.text.text = "Communications Room";
			roomType = BrofortRoomType.Communications;
		}
		else if (this.rooms.Count > 1)
		{
			this.text.text = "Armoury";
			roomType = BrofortRoomType.Armoury;
		}
		if (!this.RoomExists(num, num3) && !this.RoomExists(num + 1, num3))
		{
			this.roomPlacementSprite.SetColor(Color.white);
			if (Input.GetMouseButtonDown(0))
			{
				this.PlaceRoom(num, num3, 2, roomType);
			}
		}
		else
		{
			this.roomPlacementSprite.SetColor(Color.red * 0.6f);
		}
	}

	protected void Setup()
	{
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		if (Physics.Raycast(new Vector3(8f, 800f, 0f), Vector3.down, out this.raycastHit, 1000f, this.groundLayer))
		{
			this.groundHeight = this.raycastHit.point.y;
			this.groundRow = Map.GetRow(this.groundHeight - 8f);
			UnityEngine.Debug.Log(this.groundHeight);
		}
		else
		{
			UnityEngine.Debug.LogError("No Ground!");
		}
	}

	protected void PlaceRoom(int roomCollumn, int roomRow, int roomWidth, BrofortRoomType roomType)
	{
		int num = this.startCollumnPos + roomCollumn * 4;
		int num2 = this.groundRow + roomRow * 4;
		BrofortRoom item = new BrofortRoom(roomCollumn, roomRow, num, num2, roomType);
		for (int i = num; i < num + ((roomWidth != 2) ? 5 : 9); i++)
		{
			int num3 = num2;
			if (!Map.IsBlockLadder(i, num3))
			{
				LevelEditorGUI.PlaceGround(i, num3, TerrainType.Stone);
			}
			else
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					" Avoid placing over ladder ",
					i,
					"  ",
					num3
				}));
			}
		}
		for (int j = num; j < num + ((roomWidth != 2) ? 5 : 9); j++)
		{
			int r = num2 + 4;
			LevelEditorGUI.PlaceGround(j, r, TerrainType.Stone);
		}
		for (int k = num2; k < num2 + 4; k++)
		{
			int c = num;
			LevelEditorGUI.PlaceGround(c, k, TerrainType.Stone);
		}
		for (int l = num2; l < num2 + 4; l++)
		{
			int c2 = num + ((roomWidth != 2) ? 4 : 8);
			LevelEditorGUI.PlaceGround(c2, l, TerrainType.Stone);
		}
		for (int m = num + 1; m < num + ((roomWidth != 2) ? 4 : 8); m++)
		{
			for (int n = num2 + 1; n <= num2 + 3; n++)
			{
				LevelEditorGUI.PlaceGround(m, n, TerrainType.BackgroundStone);
			}
		}
		bool flag = roomRow == 0 || this.RoomExists(roomCollumn - 1, roomRow);
		UnityEngine.Debug.Log("Found left " + flag);
		if (flag)
		{
			int c3 = num;
			for (int num4 = num2 + 1; num4 < num2 + 3; num4++)
			{
				LevelEditorGUI.PlaceGround(c3, num4, TerrainType.BackgroundStone);
			}
			this.CreateDoor(new GridPos(c3, num2 + 1));
		}
		bool flag2 = roomRow == 0 || this.RoomExists(roomCollumn + roomWidth, roomRow);
		UnityEngine.Debug.Log("Found Right " + flag2);
		if (flag2)
		{
			int c4 = num + ((roomWidth != 2) ? 4 : 8);
			for (int num5 = num2 + 1; num5 < num2 + 3; num5++)
			{
				LevelEditorGUI.PlaceGround(c4, num5, TerrainType.BackgroundStone);
			}
			this.CreateDoor(new GridPos(c4, num2 + 1));
		}
		if (roomRow < 0 || roomRow > 0)
		{
			this.CreateLadder(ref item);
		}
		for (int num6 = num - 1; num6 < num + 10; num6++)
		{
			for (int num7 = num2 - 1; num7 < num2 + 5; num7++)
			{
				this.RefreshSingleBlock(num6, num7);
			}
		}
		this.PlaceDoodads(ref item);
		this.rooms.Add(item);
	}

	protected void PlaceDoodads(ref BrofortRoom room)
	{
		if (room.type == BrofortRoomType.None)
		{
			UnityEngine.Debug.LogError("No Room Type");
			return;
		}
		if (room.doodads.Count > 0)
		{
			for (int i = 0; i < room.doodads.Count; i++)
			{
				UnityEngine.Object.Destroy(room.doodads[i].gameObject);
			}
			room.doodads.Clear();
		}
		AutoPlaceDoodad[] doodadPrefabs = this.GetDoodadPrefabs(room.type);
		bool[] array = new bool[]
		{
			true,
			default(bool),
			default(bool),
			default(bool),
			default(bool),
			default(bool),
			default(bool),
			default(bool),
			true
		};
		List<AutoPlaceDoodad> list = new List<AutoPlaceDoodad>();
		AutoPlaceDoodad autoPlaceDoodad = this.GetLargeDoodad(doodadPrefabs);
		int targetCollumn;
		if (room.hasLadder)
		{
			if (room.ladderPos > 0)
			{
				targetCollumn = UnityEngine.Random.Range(1, 3);
				array[6] = true;
			}
			else
			{
				targetCollumn = UnityEngine.Random.Range(3, 5);
				array[2] = true;
			}
		}
		else
		{
			targetCollumn = UnityEngine.Random.Range(2, 4);
		}
		this.AddDoodad(ref room, ref array, ref list, autoPlaceDoodad, targetCollumn);
		for (int j = 1; j < 8; j++)
		{
			int unoccupiedSpaceSize = this.GetUnoccupiedSpaceSize(j, ref array);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Try place doodad at ",
				j,
				" is occupied ",
				array[j],
				" unoccupied space found ",
				unoccupiedSpaceSize
			}));
			if (unoccupiedSpaceSize > 0)
			{
				autoPlaceDoodad = this.GetDoodad(ref list, doodadPrefabs, unoccupiedSpaceSize);
				if (autoPlaceDoodad != null)
				{
					this.AddDoodad(ref room, ref array, ref list, autoPlaceDoodad, j);
				}
				else
				{
					UnityEngine.Debug.LogError("No Suitable Doodad");
				}
			}
		}
	}

	protected int GetUnoccupiedSpaceSize(int startCollumn, ref bool[] collumnsOccupied)
	{
		if (collumnsOccupied.Length <= startCollumn)
		{
			UnityEngine.Debug.LogError("Collumns to short");
			return 0;
		}
		if (collumnsOccupied[startCollumn])
		{
			return 0;
		}
		int num = 1;
		for (int i = startCollumn + 1; i < collumnsOccupied.Length; i++)
		{
			if (collumnsOccupied[i])
			{
				break;
			}
			num++;
		}
		return num;
	}

	protected void AddDoodad(ref BrofortRoom room, ref bool[] roomCollumnsOccupied, ref List<AutoPlaceDoodad> currentDoodads, AutoPlaceDoodad currentDoodad, int targetCollumn)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Add doodad ",
			currentDoodad.name,
			" at target collumn ",
			targetCollumn
		}));
		currentDoodad.collumnTarget = targetCollumn;
		currentDoodads.Add(currentDoodad);
		for (int i = targetCollumn; i < targetCollumn + currentDoodad.collumnsOccupied; i++)
		{
			roomCollumnsOccupied[i] = true;
		}
		float blocksX = Map.GetBlocksX(room.worldCollumn + currentDoodad.collumnTarget);
		float blocksY = Map.GetBlocksY(room.worldRow + 1);
		Doodad doodad = UnityEngine.Object.Instantiate(currentDoodad, new Vector3(blocksX, blocksY, 0f), Quaternion.identity) as Doodad;
		room.AddDoodad(doodad);
	}

	protected AutoPlaceDoodad GetDoodad(ref List<AutoPlaceDoodad> currentDoodads, AutoPlaceDoodad[] doodadPrefabs, int maxSize)
	{
		List<AutoPlaceDoodad> list = new List<AutoPlaceDoodad>();
		for (int i = 0; i < doodadPrefabs.Length; i++)
		{
			if (doodadPrefabs[i].collumnsOccupied <= maxSize)
			{
				list.Add(doodadPrefabs[i]);
			}
		}
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Get Doodad ",
			maxSize,
			" candidates ",
			list.Count
		}));
		int count = list.Count;
		int num = 0;
		while (num < count && list.Count > 1)
		{
			AutoPlaceDoodad autoPlaceDoodad = list[UnityEngine.Random.Range(0, list.Count)];
			int num2 = 0;
			while (num2 < currentDoodads.Count && list.Count > 1)
			{
				if (currentDoodads[num2].name == autoPlaceDoodad.name)
				{
					list.Remove(autoPlaceDoodad);
				}
				num2++;
			}
			num++;
		}
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Get Doodad (after removing) ",
			maxSize,
			" candidates ",
			list.Count
		}));
		if (list.Count > 0)
		{
			UnityEngine.Debug.Log("return doodad " + list[UnityEngine.Random.Range(0, list.Count)].name);
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		if (doodadPrefabs.Length > 0)
		{
			UnityEngine.Debug.LogError("Crappy Doodads");
			return doodadPrefabs[0];
		}
		UnityEngine.Debug.LogError("No Doodads");
		return null;
	}

	protected AutoPlaceDoodad GetLargeDoodad(AutoPlaceDoodad[] doodadPrefabs)
	{
		List<AutoPlaceDoodad> list = new List<AutoPlaceDoodad>();
		int num = 1;
		for (int i = 0; i < doodadPrefabs.Length; i++)
		{
			if (doodadPrefabs[i].collumnsOccupied > num)
			{
				num = doodadPrefabs[i].collumnsOccupied;
			}
		}
		for (int j = 0; j < doodadPrefabs.Length; j++)
		{
			if (doodadPrefabs[j].collumnsOccupied >= num)
			{
				list.Add(doodadPrefabs[j]);
			}
		}
		if (list.Count > 0)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		if (doodadPrefabs.Length > 0)
		{
			UnityEngine.Debug.LogError("Crappy Doodads");
			return doodadPrefabs[0];
		}
		UnityEngine.Debug.LogError("No Doodads");
		return null;
	}

	protected AutoPlaceDoodad[] GetDoodadPrefabs(BrofortRoomType roomType)
	{
		switch (roomType)
		{
		case BrofortRoomType.LivingRoom:
			return this.livingRoomDoodads;
		case BrofortRoomType.Communications:
			return this.communicationsRoomDoodads;
		case BrofortRoomType.Armoury:
			return this.armouryRoomDoodads;
		default:
			UnityEngine.Debug.LogError("Bad Room Type");
			return null;
		}
	}

	protected void PlaceLadder(ref BrofortRoom newRoom, int ladderSide)
	{
		int num = this.startCollumnPos + newRoom.roomCollumn * 4;
		int num2 = this.groundRow + newRoom.roomRow * 4;
		int num3 = num + ladderSide * 2 + 4;
		LevelEditorGUI.PlaceGround(num3, num2 + 4, TerrainType.BackgroundStone);
		for (int i = num2 + 1; i < num2 + 5; i++)
		{
			LevelEditorGUI.PlaceGround(num3, i, TerrainType.Ladder);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Place Ladder ",
				num3,
				"  ",
				i
			}));
		}
		newRoom.hasLadder = true;
		newRoom.ladderPos = ladderSide;
	}

	protected void CreateLadder(ref BrofortRoom newRoom)
	{
		if (newRoom.roomRow < 0 && !this.RoomHasLadder(newRoom.roomCollumn - 1, newRoom.roomRow) && !this.RoomHasLadder(newRoom.roomCollumn + newRoom.size, newRoom.roomRow))
		{
			this.PlaceLadder(ref newRoom, (newRoom.roomCollumn >= 5) ? -1 : 1);
		}
		if (newRoom.roomRow > 0 && !this.RoomCanReachUpWithLadder(newRoom.roomCollumn, newRoom.roomRow) && (!this.RoomExists(newRoom.roomCollumn + newRoom.size, newRoom.roomRow) || !this.RoomCanReachUpWithLadder(newRoom.roomCollumn + newRoom.size, newRoom.roomRow)) && (!this.RoomExists(newRoom.roomCollumn - 1, newRoom.roomRow) || !this.RoomCanReachUpWithLadder(newRoom.roomCollumn - newRoom.size, newRoom.roomRow)))
		{
			bool flag = false;
			BrofortRoom room = this.GetRoom(newRoom.roomCollumn, newRoom.roomRow - 1);
			if (room != null && !room.hasLadder)
			{
				if (room.roomCollumn < newRoom.roomCollumn)
				{
					this.PlaceLadder(ref room, 1);
					this.PlaceDoodads(ref room);
					flag = true;
				}
				else
				{
					this.PlaceLadder(ref room, (newRoom.roomCollumn >= 5) ? -1 : 1);
					this.PlaceDoodads(ref room);
					flag = true;
				}
			}
			if (!flag && newRoom.size > 1)
			{
				room = this.GetRoom(newRoom.roomCollumn + 1, newRoom.roomRow - 1);
				if (room != null && !room.hasLadder)
				{
					this.PlaceLadder(ref room, -1);
					this.PlaceDoodads(ref room);
					flag = true;
				}
			}
			if (!flag)
			{
				room = this.GetRoom(newRoom.roomCollumn - 1, newRoom.roomRow - 1);
				if (room != null && !room.hasLadder)
				{
					this.PlaceLadder(ref room, 1);
					this.PlaceDoodads(ref room);
					flag = true;
				}
			}
			if (!flag)
			{
				UnityEngine.Debug.LogError("No Ladder Placed!");
			}
		}
	}

	protected void CreateDoor(GridPos gridPos)
	{
		if (!this.DoorExists(gridPos))
		{
			GameObject gameObject = Map.Instance.PlaceDoodad(new DoodadInfo(gridPos, DoodadType.Door, 0));
			Doodad component = gameObject.GetComponent<Doodad>();
			if (component != null)
			{
				this.doors.Add(component);
				UnityEngine.Debug.Log("add Door");
			}
			else
			{
				UnityEngine.Debug.LogError("fail add Door");
			}
		}
	}

	protected bool DoorExists(GridPos gridPos)
	{
		foreach (Doodad doodad in this.doors)
		{
			if (doodad.collumn == gridPos.c && doodad.row == gridPos.r)
			{
				return true;
			}
		}
		return false;
	}

	protected bool RoomCanReachUpWithLadder(int roomCollumn, int roomRow)
	{
		foreach (BrofortRoom brofortRoom in this.rooms)
		{
			if (brofortRoom.roomRow == roomRow - 1 && brofortRoom.hasLadder)
			{
				if (brofortRoom.roomCollumn == roomCollumn)
				{
					return true;
				}
				if (brofortRoom.roomCollumn == roomCollumn - 1 && brofortRoom.ladderPos >= 0)
				{
					return true;
				}
				if (brofortRoom.roomCollumn == roomCollumn + 1 && brofortRoom.ladderPos <= 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	protected bool RoomHasLadder(int roomCollumn, int roomRow)
	{
		foreach (BrofortRoom brofortRoom in this.rooms)
		{
			if (brofortRoom.roomRow == roomRow && (brofortRoom.roomCollumn == roomCollumn || brofortRoom.roomCollumn + (brofortRoom.size - 1) == roomCollumn) && brofortRoom.hasLadder)
			{
				return true;
			}
		}
		return false;
	}

	protected BrofortRoom GetRoom(int roomCollumn, int roomRow)
	{
		foreach (BrofortRoom brofortRoom in this.rooms)
		{
			if (brofortRoom.roomRow == roomRow && (brofortRoom.roomCollumn == roomCollumn || brofortRoom.roomCollumn + (brofortRoom.size - 1) == roomCollumn))
			{
				return brofortRoom;
			}
		}
		return null;
	}

	protected bool RoomExists(int roomCollumn, int roomRow)
	{
		foreach (BrofortRoom brofortRoom in this.rooms)
		{
			if (brofortRoom.roomRow == roomRow && (brofortRoom.roomCollumn == roomCollumn || brofortRoom.roomCollumn + (brofortRoom.size - 1) == roomCollumn))
			{
				return true;
			}
		}
		return false;
	}

	private void PlaceGround(int c, int r, TerrainType terrainType)
	{
		if (terrainType == TerrainType.Empty)
		{
			if (Map.IsBlockSolid(c, r))
			{
			}
			Map.ClearForegroundBlock(c, r);
			Map.ClearBackgroundBlock(c, r);
			Map.MapData.foregroundBlocks[c, r] = terrainType;
			Map.MapData.backgroundBlocks[c, r] = terrainType;
		}
		else if (terrainType == TerrainType.PropaneBarrel && r < Map.MapData.foregroundBlocks.GetUpperBound(1))
		{
			Map.MapData.foregroundBlocks[c, r] = terrainType;
			Map.MapData.foregroundBlocks[c, r + 1] = TerrainType.Empty;
			Map.ClearForegroundBlock(c, r);
			Map.ClearForegroundBlock(c, r + 1);
			Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.foregroundBlocks, c, r), c, r, ref Map.blocks);
		}
		else if (MapData.IsForegroundType(terrainType))
		{
			if (Map.DoesForegroundBlockHaveBackground(terrainType))
			{
				Map.MapData.backgroundBlocks[c, r] = TerrainType.Empty;
				Map.ClearBackgroundBlock(c, r);
			}
			Map.MapData.foregroundBlocks[c, r] = terrainType;
			Map.ClearForegroundBlock(c, r);
			Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.foregroundBlocks, c, r), c, r, ref Map.blocks);
		}
		else
		{
			Map.MapData.backgroundBlocks[c, r] = terrainType;
			if (Map.MapData.foregroundBlocks[c, r] != TerrainType.Ladder && Map.MapData.foregroundBlocks[c, r] != TerrainType.Barrel && Map.MapData.foregroundBlocks[c, r] != TerrainType.PropaneBarrel && Map.MapData.foregroundBlocks[c, r] != TerrainType.Wood && Map.MapData.foregroundBlocks[c, r] != TerrainType.Tyre && Map.MapData.foregroundBlocks[c, r] != TerrainType.Bridge2 && Map.MapData.foregroundBlocks[c, r] != TerrainType.Bridge && Map.MapData.foregroundBlocks[c, r] != TerrainType.BuriedRocket)
			{
				Map.MapData.foregroundBlocks[c, r] = TerrainType.Empty;
				Map.ClearForegroundBlock(c, r);
			}
			Map.ClearBackgroundBlock(c, r);
			Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.backgroundBlocks, c, r), c, r, ref Map.backGroundBlocks);
		}
	}

	private void RefreshSingleBlock(int c, int r)
	{
		if (c >= 0 && c < Map.Width && r >= 0 && r < Map.Height)
		{
			if (Map.MapData.foregroundBlocks[c, r] != TerrainType.Empty)
			{
				Map.ClearForegroundBlock(c, r);
				Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.foregroundBlocks, c, r), c, r, ref Map.blocks);
			}
			if (Map.MapData.backgroundBlocks[c, r] != TerrainType.Empty)
			{
				Map.ClearBackgroundBlock(c, r);
				Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.backgroundBlocks, c, r), c, r, ref Map.backGroundBlocks);
			}
		}
	}

	private void RefreshBlock(int c, int r)
	{
		for (int i = c - 1; i <= c + 1; i++)
		{
			for (int j = r - 1; j <= r + 1; j++)
			{
				if (i >= 0 && i < Map.Width && j >= 0 && j < Map.Height)
				{
					if (Map.MapData.foregroundBlocks[i, j] != TerrainType.Empty)
					{
						Map.ClearForegroundBlock(i, j);
						Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.foregroundBlocks, i, j), i, j, ref Map.blocks);
					}
					if (Map.MapData.backgroundBlocks[i, j] != TerrainType.Empty)
					{
						Map.ClearBackgroundBlock(i, j);
						Map.Instance.PlaceGround(Map.MapData.GetGroundType(Map.MapData.backgroundBlocks, i, j), i, j, ref Map.backGroundBlocks);
					}
				}
			}
		}
	}

	private LayerMask groundLayer;

	private RaycastHit raycastHit;

	private float groundHeight = 16f;

	public int startCollumnPos = 10;

	protected int groundRow = 1;

	public TextMesh text;

	private List<BrofortRoom> rooms = new List<BrofortRoom>();

	private List<Doodad> doors = new List<Doodad>();

	public AutoPlaceDoodad[] livingRoomDoodads;

	public AutoPlaceDoodad[] communicationsRoomDoodads;

	public AutoPlaceDoodad[] armouryRoomDoodads;

	public SpriteSM roomPlacementSprite;

	private bool firstFrame = true;
}
