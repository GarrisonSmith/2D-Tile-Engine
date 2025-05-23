﻿using Engine.DiskModels.UI.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Elements;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.UI.Services.Contracts
{
	/// <summary>
	/// Represents a user interface element service.
	/// </summary>
	public interface IUserInterfaceElementService
	{
		/// <summary>
		/// Gets or sets the button click event processors.
		/// </summary>
		public Dictionary<string, Action<UiButton, Vector2>> ButtonClickEventProcessors { get; set; } 

		/// <summary>
		/// Gets the element dimensions.
		/// </summary>
		/// <param name="uiScreenZone">The user interface screen zone.</param>
		/// <param name="elementModel">The user interface element model.</param>
		/// <returns>The element dimensions.</returns>
		public Vector2? GetElementDimensions(UiScreenZone uiScreenZone, IAmAUiElementModel elementModel);

		/// <summary>
		/// Updates the element height.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="height">The height.</param>
		public void UpdateElementHeight(IAmAUiElement element, float height);

		/// <summary>
		/// Processes the user interface element being pressed.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void ProcessUiElementPress(IAmAUiElement element, Vector2 elementLocation);

		/// <summary>
		/// Process the user interface button being clicked.
		/// </summary>
		/// <param name="button">The button.</param>
		/// <param name="elementLocation">The element location.</param>
		public void ProcessUiButtonClick(UiButton button, Vector2 elementLocation);

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="fillWidth">The fill width of the user interface element.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface element.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel, UiScreenZone uiZone, float fillWidth, int visibilityGroup);
	}
}
