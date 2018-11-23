// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class MapData
{
	public MapData()
	{
		this.DoodadList = new List<DoodadInfo>();
		this.TriggerList = new List<TriggerInfo>();
	}

	public MapData(int w, int h)
	{
		this.ResetMap(w, h);
	}

	[XmlAttribute("Width")]
	public int Width { get; set; }

	[XmlAttribute("Height")]
	public int Height { get; set; }

	public List<TriggerInfo> TriggerList { get; set; }

	public List<DoodadInfo> DoodadList { get; set; }

	public static bool IsForegroundType(TerrainType terrainType)
	{
		if (terrainType == TerrainType.BackgroundBunker || terrainType == TerrainType.BackgroundEarth || terrainType == TerrainType.BackgroundStone || terrainType == TerrainType.BackgroundFactory || terrainType == TerrainType.BackgroundBathroom || terrainType == TerrainType.BackgroundShaft || terrainType == TerrainType.BackgroundStoneDoodads || terrainType == TerrainType.WoodBackground)
		{
			return false;
		}
		if (terrainType == TerrainType.Empty || terrainType == TerrainType.Earth || terrainType == TerrainType.Stone || terrainType == TerrainType.Bunker || terrainType == TerrainType.Bridge || terrainType == TerrainType.Bridge2 || terrainType == TerrainType.Ladder || terrainType == TerrainType.Wood || terrainType == TerrainType.Tyre || terrainType == TerrainType.BigBlock || terrainType == TerrainType.FallingBlock || terrainType == TerrainType.Steel || terrainType == TerrainType.Barrel || terrainType == TerrainType.BuriedRocket || terrainType == TerrainType.PropaneBarrel || terrainType == TerrainType.Roof || terrainType == TerrainType.CaveRock || terrainType == TerrainType.WatchTower || terrainType == TerrainType.AlienEgg || terrainType == TerrainType.Sand || terrainType == TerrainType.Statue || terrainType == TerrainType.HutScaffolding || terrainType == TerrainType.ThatchRoof || terrainType == TerrainType.FactoryRoof || terrainType == TerrainType.Pipe || terrainType == TerrainType.AlienEarth || terrainType == TerrainType.AssMouth)
		{
			return true;
		}
		UnityEngine.Debug.LogError("Could not determine whether terraintype " + terrainType + " is foreground.");
		return true;
	}

	[XmlArray("Foreground")]
	public TerrainType[][] ForegroundBlocksJagged
	{
		get
		{
			TerrainType[][] array = new TerrainType[this.Width][];
			for (int i = 0; i < this.Width; i++)
			{
				array[i] = new TerrainType[this.Height];
				for (int j = 0; j < this.Height; j++)
				{
					array[i][j] = this.foregroundBlocks[i, j];
				}
			}
			return array;
		}
		set
		{
			this.foregroundBlocks = new TerrainType[value.Length, value[0].Length];
			for (int i = 0; i < value.Length; i++)
			{
				for (int j = 0; j < value[0].Length; j++)
				{
					this.foregroundBlocks[i, j] = value[i][j];
				}
			}
		}
	}

	[XmlArray("Background")]
	public TerrainType[][] BackgroundBlocksJagged
	{
		get
		{
			TerrainType[][] array = new TerrainType[this.Width][];
			for (int i = 0; i < this.Width; i++)
			{
				array[i] = new TerrainType[this.Height];
				for (int j = 0; j < this.Height; j++)
				{
					array[i][j] = this.backgroundBlocks[i, j];
				}
			}
			return array;
		}
		set
		{
			this.backgroundBlocks = new TerrainType[value.Length, value[0].Length];
			for (int i = 0; i < value.Length; i++)
			{
				for (int j = 0; j < value[0].Length; j++)
				{
					this.backgroundBlocks[i, j] = value[i][j];
				}
			}
		}
	}

	public void ResetMap(int w, int h)
	{
		UnityEngine.Debug.Log("ResetMap " + w);
		this.Width = w;
		this.Height = h;
		this.backgroundBlocks = new TerrainType[w, h];
		this.foregroundBlocks = new TerrainType[w, h];
		if (this.DoodadList == null)
		{
			this.DoodadList = new List<DoodadInfo>();
		}
		if (this.TriggerList == null)
		{
			this.TriggerList = new List<TriggerInfo>();
		}
		this.DoodadList.Clear();
	}

	public GroundType GetBackgroundGroundType(int c, int r)
	{
		return this.GetGroundType(this.backgroundBlocks, c, r);
	}

	public GroundType GetForegroundGroundType(int c, int r)
	{
		return this.GetGroundType(this.foregroundBlocks, c, r);
	}

	public List<DoodadInfo> GetDoodadsAt(int collumn, int row)
	{
		List<DoodadInfo> list = new List<DoodadInfo>();
		foreach (DoodadInfo doodadInfo in this.DoodadList)
		{
			if (doodadInfo.position.c == collumn && doodadInfo.position.r == row)
			{
				list.Add(doodadInfo);
			}
		}
		return list;
	}

	public GroundType GetGroundType(TerrainType[,] blocks, int c, int r)
	{
		if (r < 0)
		{
			return GroundType.Earth;
		}
		if (r >= blocks.GetLength(1))
		{
			return GroundType.Empty;
		}
		if (c < 0)
		{
			return GroundType.Earth;
		}
		if (c >= blocks.GetLength(0))
		{
			return GroundType.Earth;
		}
		TerrainType terrainType = blocks[c, r];
		if (terrainType == TerrainType.Empty)
		{
			return GroundType.Empty;
		}
		if (terrainType == TerrainType.Earth)
		{
			GroundType groundType = this.GetGroundType(blocks, c, r + 1);
			if (groundType == GroundType.Earth)
			{
				return GroundType.Earth;
			}
			if (groundType == GroundType.EarthMiddle)
			{
				return GroundType.Earth;
			}
			if (groundType == GroundType.EarthTop || groundType == GroundType.Brick || groundType == GroundType.BrickTop || groundType == GroundType.Wall || groundType == GroundType.WallTop)
			{
				return GroundType.EarthMiddle;
			}
			return GroundType.EarthTop;
		}
		else if (terrainType == TerrainType.Stone)
		{
			GroundType groundType2 = this.GetGroundType(blocks, c, r + 1);
			if (groundType2 == GroundType.Brick)
			{
				return GroundType.Brick;
			}
			if (groundType2 == GroundType.BrickMiddle)
			{
				return GroundType.Brick;
			}
			if (groundType2 == GroundType.BrickTop)
			{
				return GroundType.BrickMiddle;
			}
			return GroundType.BrickTop;
		}
		else
		{
			if (terrainType == TerrainType.CaveRock)
			{
				return GroundType.CaveRock;
			}
			if (terrainType == TerrainType.BackgroundEarth)
			{
				return GroundType.EarthBehind;
			}
			if (terrainType == TerrainType.BackgroundFactory)
			{
				return GroundType.FactoryBehind;
			}
			if (terrainType == TerrainType.BackgroundStone)
			{
				return GroundType.BrickBehind;
			}
			if (terrainType == TerrainType.BackgroundFactory)
			{
				return GroundType.FactoryBehind;
			}
			if (terrainType == TerrainType.BackgroundStoneDoodads)
			{
				return GroundType.BrickBehindDoodads;
			}
			if (terrainType == TerrainType.Wood)
			{
				return GroundType.WoodenBlock;
			}
			if (terrainType == TerrainType.Tyre)
			{
				return GroundType.TyreBlock;
			}
			if (terrainType == TerrainType.Bunker)
			{
				return GroundType.Bunker;
			}
			if (terrainType == TerrainType.Sand)
			{
				return GroundType.Sand;
			}
			if (terrainType == TerrainType.Pipe)
			{
				return GroundType.Pipe;
			}
			if (terrainType == TerrainType.AssMouth)
			{
				return GroundType.AssMouth;
			}
			if (terrainType == TerrainType.HutScaffolding)
			{
				return GroundType.HutScaffolding;
			}
			if (terrainType == TerrainType.ThatchRoof)
			{
				return GroundType.ThatchRoof;
			}
			if (terrainType == TerrainType.FactoryRoof)
			{
				return GroundType.FactoryRoof;
			}
			if (terrainType == TerrainType.Statue)
			{
				return GroundType.Statue;
			}
			if (terrainType == TerrainType.WoodBackground)
			{
				return GroundType.WoodBackground;
			}
			if (terrainType == TerrainType.BackgroundBunker)
			{
				return GroundType.BunkerBehind;
			}
			if (terrainType == TerrainType.BackgroundBathroom)
			{
				return GroundType.BathroomBehind;
			}
			if (terrainType == TerrainType.BackgroundShaft)
			{
				return GroundType.ShaftBehind;
			}
			if (terrainType == TerrainType.Steel)
			{
			}
			if (terrainType == TerrainType.WatchTower)
			{
				return GroundType.WatchTower;
			}
			if (terrainType == TerrainType.Beehive)
			{
				return GroundType.Beehive;
			}
			if (terrainType == TerrainType.AlienEarth)
			{
				return GroundType.AlienEarth;
			}
			GroundType result;
			try
			{
				result = (GroundType)((int)Enum.Parse(typeof(GroundType), terrainType.ToString()));
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogError("Could not find a corresponding GroundType for TerrainType " + terrainType.ToString());
				result = GroundType.Empty;
			}
			return result;
		}
	}

	[XmlIgnore]
	public TerrainType[,] foregroundBlocks;

	[XmlIgnore]
	public TerrainType[,] backgroundBlocks;

	[XmlIgnore]
	public List<TriggerInfo> entityTriggers = new List<TriggerInfo>();

	public bool suppressAnnouncer;

	public float cameraSpeed = 32f;

	public HeroSpawnMode heroSpawnMode;

	public CameraFollowMode cameraFollowMode;

	public float regualrMookSpawnProbability = 0.45f;

	public float bigMookSpawnProbability = 0.15f;

	public float suicideMookSpawnProbability = 0.2f;

	public float riotShieldMookSpawnProbability = 0.2f;

	public float oilBarrelSpawnProbability = 0.5f;

	public float propaneTankSpawnProbability = 0.4f;

	public float spikeTrapSpawnProbability = 0.4f;

	public float mineFieldSpawnProbability = 0.4f;

	public float coconutProbability;

	public bool spawnAmmoCrates = true;

	public string themeScene = string.Empty;

	public string levelDescription = string.Empty;

	public LevelTheme theme;

	public HeroType forcedBro = HeroType.Random;

	public List<HeroType> forcedBros;

	public MusicType musicType;

	public WeatherType weatherType = WeatherType.NoChange;
}
