﻿using Engine.Controls.Services;
using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Controls.Models
{
	/// <summary>
	/// Represents a cursor.
	/// </summary>
	public class Cursor : Image, IHaveAnImage, IAmSubUpdateable
	{
		/// <summary>
		/// A value describing if the cursor is active or not.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets the cursor name.
		/// </summary>
		public string CursorName { get; set; }

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		public int UpdateOrder { get; set; }

		/// <summary>
		/// Gets or sets the offset.
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get => this; }

		/// <summary>
		/// Gets or sets the hover cursor.
		/// </summary>
		public HoverCursor HoverCursor { get; set; }

		/// <summary>
		/// Gets or sets the cursor updater.
		/// </summary>
		public Action<Cursor, GameTime> CursorUpdater { get; set; }

		/// <summary>
		/// Gets or sets the trailing cursors.
		/// </summary>
		public IList<TrailingCursor> TrailingCursors { get; set; }

		/// <summary>
		/// Initializes a new cursor.
		/// </summary>
		public Cursor()
		{
			ControlManager.ControlStateUpdated += this.Update;
		}

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			if (false == this.IsActive)
			{
				return;
			}

			if (true == this.HoverCursor?.IsActive)
			{
				this.HoverCursor.Draw(gameTime, gameServices, this.Position);
				this.HoverCursor.IsActive = false;

				return;
			}

			drawingService.Draw(gameTime, this, this.Offset);

			if (true != this.TrailingCursors?.Any())
			{
				return;
			}

			foreach (var trailingCursor in this.TrailingCursors)
			{
				trailingCursor.Draw(gameTime, gameServices, this.Position, trailingCursor.Offset);
			}
		}

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Update(GameTime gameTime, GameServiceContainer gameServices)
		{
			if (false == this.IsActive)
			{
				return;
			}

			this.CursorUpdater?.Invoke(this, gameTime);

			if (true != this.TrailingCursors?.Any())
			{
				return;
			}

			foreach (var trailingCursor in this.TrailingCursors)
			{
				trailingCursor.CursorUpdater.Invoke(this, trailingCursor, gameTime);
			}
		}

		/// <summary>
		/// Disposes of the draw data texture.
		/// </summary>
		new public void Dispose()
		{
			ControlManager.ControlStateUpdated -= this.Update;
			this.Texture?.Dispose();

			if (true != this.TrailingCursors?.Any())
			{
				return;
			}

			foreach (var trailingCursor in this.TrailingCursors)
			{
				trailingCursor.Dispose();
			}
		}
	}
}
