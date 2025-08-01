﻿using Common.DiskModels.Common.Tiling;
using Common.DiskModels.UI;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.DiskModels
{
	/// <summary>
	/// Represents a model processor exporter.
	/// </summary>
	public static class ModelProcessorExporter
	{
		/// <summary>
		/// Gets the model processing mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The model processing mappings.</returns>
		public static (Type typeIn, Delegate)[] GetModelProcessingMappings(GameServiceContainer gameServices)
		{
			var tileService = gameServices.GetService<ITileService>();
			var uiService = gameServices.GetService<IUserInterfaceService>();

			return
			[
				(typeof(AnimatedTileModel), tileService.GetTile),
				(typeof(TileMapLayerModel), tileService.GetTileMapLayer),
				(typeof(TileMapModel), tileService.GetTileMap),
				(typeof(TileModel), tileService.GetTile),
				(typeof(UiGroupModel), uiService.GetUiGroup)
			];
		}
	}
}
