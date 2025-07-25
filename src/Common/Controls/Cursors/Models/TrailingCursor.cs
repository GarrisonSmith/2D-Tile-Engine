﻿using Engine.Graphics.Models;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.Cursors.Models
{
    /// <summary>
    /// Represents a trailing cursor.
    /// </summary>
    public class TrailingCursor : IAmSubDrawable, IDisposable
    {
        /// <summary>
        /// Gets or sets a value describing if the trailing cursor is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the trailing cursor name.
        /// </summary>
        public string TrailingCursorName { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Gets the graphic.
        /// </summary>
        public Image Graphic { get; init; }

        /// <summary>
        /// Gets or sets the cursor updater.
        /// </summary>
        public Action<Cursor, TrailingCursor, GameTime> CursorUpdater { get; set; }

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

            drawingService.Draw(gameTime, Graphic, position, offset);
        }

        /// <summary>
        /// Disposes of the draw data texture.
        /// </summary>
        public void Dispose()
        {
            Graphic?.Dispose();
        }
    }
}
