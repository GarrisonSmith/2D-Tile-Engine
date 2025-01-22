﻿using Common.DiscModels.Common.Tiling;
using System.Runtime.Serialization;

namespace Common.DiscModels.Engine.Scenes
{
    [DataContract(Name = "scene")]
	public class SceneModel
	{
		[DataMember(Name = "sceneName", Order = 1)]
		public string SceneName { get; set; }

		[DataMember(Name = "tileMap", Order = 2)]
		public TileMapModel TileMap { get; set; }
	}
}
