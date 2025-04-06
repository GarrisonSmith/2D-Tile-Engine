﻿using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.Models
{
	/// <summary>
	/// Represents a trailing cursor.
	/// </summary>
	public class TrailingCursor : IAmSubDrawable
	{
		/// <summary>
		/// Gets or sets the offset.
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets the image.
		/// </summary>
		public Image Image { get; init; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			drawingService.Draw(gameTime, this, position, offset);
		}
	}
}
