﻿using Engine.Drawing.Models;
using Engine.Signals.Models;
using Engine.Signals.Models.Contracts;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Enums;
using Microsoft.Xna.Framework;

namespace Engine.UI.Models.Elements
{
	/// <summary>
	/// Represents a user interface button.
	/// </summary>
	public class UiButton : IAmAUiElement, IHaveASignal
	{
		/// <summary>
		/// Gets or sets the user interface element name.
		/// </summary>
		public string UiElementName { get; set; }

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
		/// Gets or sets the area.
		/// </summary>
		public Rectangle Area { get; set; }

		/// <summary>
		/// Gets or sets the sprite.
		/// </summary>
		public Sprite Sprite { get; set; }

		/// <summary>
		/// Gets or set the signal.
		/// </summary>
		public Signal Signal { get; set; }

		/// <summary>
		/// Disposes of the user interface button.
		/// </summary>
		public void Dispose()
		{ 
			this.Sprite.Dispose();
		}
	}
}
