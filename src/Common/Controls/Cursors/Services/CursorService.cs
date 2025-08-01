﻿using Common.Controls.Constants;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Engine.Core.Textures.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Common.Controls.Cursors.Services
{
	/// <summary>
	/// Represents a cursors service.
	/// </summary>
	/// <remarks>
	/// Initializes the cursor service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class CursorService(GameServiceContainer gameServices) : ICursorService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the active cursor.
		/// </summary>
		public Cursor ActiveCursor { get; private set; }

		/// <summary>
		/// Gets the cursors.
		/// </summary>
		public Dictionary<string, Cursor> Cursors { get; private set; } = [];

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
			var textureService = this._gameServices.GetService<ITextureService>();
			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			if (false == textureService.TryGetTexture("mouse", out var cursorTexture))
			{
				cursorTexture = textureService.DebugTexture;
			}

			var position = new Position
			{
				Coordinates = default
			};

			var cursor = new Cursor
			{
				CursorName = CommonCursorNames.PrimaryCursorName,
				TextureName = cursorTexture.Name,
				Offset = default,
				CursorUpdater = this.BasicCursorUpdater,
				TextureBox = new Rectangle(0, 0, 18, 28),
				Texture = cursorTexture,
				Position = position,
				DrawLayer = 1
			};

			this.Cursors.Add(cursor.CursorName, cursor);
			this.SetActiveCursor(cursor);
		}

		/// <summary>
		/// Sets the active cursor.
		/// </summary>
		/// <param name="cursor"></param>
		public void SetActiveCursor(Cursor cursor)
		{
			if (this.ActiveCursor == cursor)
			{
				return;
			}

			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			this.DisableAllCursors();
			this.ActiveCursor = cursor;
			runTimeDrawService.AddOverlaidDrawable(cursor);
			runTimeUpdateService.AddUpdateable(cursor);
		}

		/// <summary>
		/// Disables all cursors.
		/// </summary>
		public void DisableAllCursors()
		{
			if (true != this.Cursors?.Any())
			{
				return;
			}

			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			foreach (var cursor in this.Cursors.Values)
			{
				runTimeDrawService.RemoveOverlaidDrawable(cursor);
				runTimeUpdateService.RemoveUpdateable(cursor);
			}
		}

		/// <summary>
		/// Process the cursor and control state.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public void ProcessCursorControlState(Cursor cursor, ControlState controlState, ControlState priorControlState)
		{
			var uiService = this._gameServices.GetService<IUserInterfaceService>();
			var uiObject = uiService.GetUiObjectAtScreenLocation(cursor.Position.Coordinates);

			if (null == uiObject)
			{
				return;
			}

			switch (uiObject)
			{
				case UiElementWithLocation uiElementWithLocation:
					if (controlState.MouseState.LeftButton == ButtonState.Pressed &&
						priorControlState.MouseState.LeftButton != ButtonState.Pressed)
					{
						uiElementWithLocation.Element.RaisePressEvent(uiElementWithLocation.Location);

						return;
					}
					else
					{
						uiElementWithLocation.Element.RaiseHoverEvent(uiElementWithLocation.Location);

						return;
					}
				case UiRow uiRow:

					break;
				case UiZone uiZone:
					uiZone.RaiseHoverEvent(uiZone.Position.Coordinates);

					return;
			}
		}

		/// <summary>
		/// Updates the cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="gameTime">The game time.</param>
		public void BasicCursorUpdater(Cursor cursor, GameTime gameTime)
		{
			var controlService = this._gameServices.GetService<IControlService>();
			var uiService = this._gameServices.GetService<IUserInterfaceService>();

			if (null == controlService.ControlState)
			{
				return;
			}

			cursor.Position.Coordinates = controlService.ControlState.MouseState.Position.ToVector2();
			this.ProcessCursorControlState(cursor, controlService.ControlState, controlService.PriorControlState);
		}
	}
}
