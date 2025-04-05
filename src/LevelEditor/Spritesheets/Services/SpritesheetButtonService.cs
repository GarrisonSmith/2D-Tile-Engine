﻿using Engine.Core.Textures.Contracts;
using Engine.DiskModels.UI.Elements;
using Engine.UI.Models.Elements;
using LevelEditor.Spritesheets.Models.Constants;
using LevelEditor.Spritesheets.Services.Contracts;
using Microsoft.Xna.Framework;

namespace LevelEditor.Spritesheets.Services
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	/// <remarks>
	/// Initializes the spritesheet button service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class SpritesheetButtonService(GameServiceContainer gameServices) : ISpritesheetButtonService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// The spritesheet button click event processor.
		/// </summary>
		/// <param name="button">The button.</param>
		public void SpritesheetButtonClickEventProcessor(UiButton button)
		{

		}

		/// <summary>
		/// Gets the user interface buttons for the spritesheet.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="spriteDimensions">The sprite dimensions.</param>
		/// <returns></returns>
		public UiButtonModel[][] GetUiButtonsForSpritesheet(string spritesheetName, Point spriteDimensions)
		{
			if (true == string.IsNullOrEmpty(spritesheetName))
			{
				return null;
			}

			var textureService = this._gameServices.GetService<ITextureService>();

			if (false == textureService.TryGetTexture(spritesheetName, out var texture))
			{
				return null;
			}	

			var horizontalSize = texture.Width / spriteDimensions.X;
			var verticalSize = texture.Height / spriteDimensions.Y;

			var buttons = new UiButtonModel[horizontalSize][];

			for (var i = 0; i < horizontalSize; i++)
			{
				buttons[i] = new UiButtonModel[verticalSize];

				for (var j = 0; j < verticalSize; j++)
				{
					var textureName = textureService.GetTextureName(spritesheetName, new Rectangle(i * spriteDimensions.X, j * spriteDimensions.Y, spriteDimensions.X, spriteDimensions.Y));
					
					if (false == textureService.TryGetTexture(textureName, out var buttonTexture))
					{
						continue;
					}

					buttons[i][j] = new UiButtonModel
					{
						UiElementName = textureName,
						LeftPadding = 2,
						RightPadding = 2,
						FixedSized = new Vector2(spriteDimensions.X, spriteDimensions.Y),
						BackgroundTextureName = textureName,
						Text = string.Empty,
						ClickableAreaScaler = new Vector2(1, 1),
						ButtonClickEventName = ButtonClickEventNameConstants.Spritesheet
					};
				}
			}

			return buttons;
		}
	}
}
