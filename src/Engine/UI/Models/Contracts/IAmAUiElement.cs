﻿using Engine.Drawables.Models;
using Engine.UI.Models.Enums;
using Microsoft.Xna.Framework;
using System;

namespace Engine.UI.Models.Contracts
{
	/// <summary>
	/// Represents a user interface element.
	/// </summary>
	public interface IAmAUiElement : IDisposable
    {
        /// <summary>
        /// Gets or sets the user interface element name.
        /// </summary>
        public string UiElementName { get; set; }

		/// <summary>
		/// Gets or sets the visibility group.
		/// </summary>
		public int VisibilityGroup { get; set; }

		/// <summary>
		/// Gets or sets the left padding.
		/// </summary>
		public float LeftPadding { get; set; }

		/// <summary>
		/// Gets or sets the right padding.
		/// </summary>
		public float RightPadding { get; set; }

		/// <summary>
		/// Gets or sets the user interface element type.
		/// </summary>
		public UiElementTypes ElementType { get; set; }

		/// <summary>
		/// Gets or sets the user interface size type.
		/// </summary>
		public UiElementSizeTypes SizeType { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public Vector2 Area { get; set; }

		/// <summary>
		/// Gets ors sets the image.
		/// </summary>
		public Image Image { get; set; }

		/// <summary>
		/// Gets or set the press event.
		/// </summary>
		public event Action<IAmAUiElement, Vector2> PressEvent;

		/// <summary>
		/// Raises the press event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaisePressEvent(Vector2 elementLocation);
	}
}
