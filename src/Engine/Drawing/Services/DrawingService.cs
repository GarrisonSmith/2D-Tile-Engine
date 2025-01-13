﻿using Engine.Drawing.Models.Contracts;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Models;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Engine.Drawing.Services
{
	/// <summary>
	/// Represents a drawing service.
	/// </summary>
	public class DrawingService : IDrawingService
    {
		private readonly GameServiceContainer _gameServices;
        
		/// <summary>
		/// Gets the sprite batch.
		/// </summary>
		private SpriteBatch SpriteBatch { get; }

		/// <summary>
		/// Initializes a new instance of the drawing service.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
        public DrawingService(GameServiceContainer gameServices)
		{
			this._gameServices = gameServices;
			var graphicDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			this.SpriteBatch = new SpriteBatch(graphicDeviceService.GraphicsDevice);
		}

		/// <summary>
		/// Begins the draw.
		/// </summary>
		public void BeginDraw()
		{ 
			this.SpriteBatch.Begin();
		}

		/// <summary>
		/// Ends the draw.
		/// </summary>
		public void EndDraw()
		{ 
			this.SpriteBatch.End();
		}

		/// <summary>
		/// Draws the drawable. 
		/// </summary>
		public void Draw(Texture2D texture, Vector2 coordinates, Rectangle sourceRectangle, Color color)
		{
			this.SpriteBatch.Draw(texture, coordinates, sourceRectangle, color);
		}

		/// <summary>
		/// Draws the drawable. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawable">The drawable.</param>
		public void Draw(GameTime gameTime, IAmDrawable drawable)
		{
			this.SpriteBatch.Draw(drawable.Image.Texture, drawable.Position.Coordinates, drawable.Image.TextureBox, Color.White);
		}

		/// <summary>
		/// Draws the user interface zone element.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="uiZoneElement">The user interface zone element.</param>
		public void Draw(GameTime gameTime, UiZoneElement uiZoneElement)
		{
			if (null != uiZoneElement.Image)
			{
				this.SpriteBatch.Draw(uiZoneElement.Image.Texture, uiZoneElement.Position.Coordinates, uiZoneElement.Image.TextureBox, Color.White);
			}

			if (true != uiZoneElement.ElementRows?.Any())
			{
				return;
			}

			var height = uiZoneElement.ElementRows.Sum(e => e.Height + e.BottomPadding + e.TopPadding);
			var rowVerticalOffset = uiZoneElement.JustificationType switch
			{
				UiZoneElementJustificationTypes.None => 0,
				UiZoneElementJustificationTypes.Center => height / 2,
				UiZoneElementJustificationTypes.Top => 0,
				UiZoneElementJustificationTypes.Bottom => height,
				_ => 0,
			};

			foreach (var elementRow in uiZoneElement.ElementRows)
			{
				switch (uiZoneElement.JustificationType)
				{
					case UiZoneElementJustificationTypes.Bottom:
						rowVerticalOffset -= elementRow.BottomPadding;
						this.Draw(gameTime, elementRow, uiZoneElement.Position, rowVerticalOffset);
						rowVerticalOffset -= (elementRow.TopPadding + elementRow.Height);
						break;
					case UiZoneElementJustificationTypes.Center:
					case UiZoneElementJustificationTypes.None:
					case UiZoneElementJustificationTypes.Top:
					default:
						rowVerticalOffset += elementRow.TopPadding;
						this.Draw(gameTime, elementRow, uiZoneElement.Position, rowVerticalOffset);
						rowVerticalOffset += (elementRow.BottomPadding + elementRow.Height);
						break;
				}
			}
		}

		/// <summary>
		/// Draws the user interface row.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="uiRow">The user interface row.</param>
		/// <param name="position">The position.</param>
		/// <param name="heightOffset">The height offset.</param>
		private void Draw(GameTime gameTime, UiRow uiRow, Position position, float heightOffset)
		{
			if (null != uiRow.Image)
			{
				this.SpriteBatch.Draw(uiRow.Image.Texture, position.Coordinates + new Vector2(0, heightOffset), uiRow.Image.TextureBox, Color.White);
			}

			if (true != uiRow?.SubElements.Any())
			{
				return;
			}

			var width = uiRow.SubElements.Sum(e => e.Area.X + e.LeftPadding + e.RightPadding);
			var elementHorizontalOffset = uiRow.JustificationType switch
			{
				UiRowJustificationTypes.None => 0,
				UiRowJustificationTypes.Center => width / 2,
				UiRowJustificationTypes.Left => 0,
				UiRowJustificationTypes.Right => width,
				_ => 0,
			};

			foreach (var element in uiRow.SubElements)
			{
				switch (uiRow.JustificationType)
				{
					case UiRowJustificationTypes.Right:
						elementHorizontalOffset -= element.RightPadding;
						this.Draw(gameTime, element, position, new Vector2(elementHorizontalOffset, heightOffset));
						elementHorizontalOffset -= (element.LeftPadding + element.Area.X);
						break;
					case UiRowJustificationTypes.Center:
					case UiRowJustificationTypes.None:
					case UiRowJustificationTypes.Left:
					default:
						elementHorizontalOffset += element.LeftPadding;
						this.Draw(gameTime, element, position, new Vector2(elementHorizontalOffset, heightOffset));
						elementHorizontalOffset += (element.RightPadding + element.Area.X);
						break;
				}
			}
		}

		/// <summary>
		/// Draws the user interface element.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="element">The element.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		private void Draw(GameTime gameTime, IAmAUiElement element, Position position, Vector2 offset)
		{
			this.Draw(element.Image.Texture, position.Coordinates + offset, element.Image.TextureBox, Color.White);
		}
	}
}
