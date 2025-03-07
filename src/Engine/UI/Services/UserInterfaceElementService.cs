﻿using Engine.Controls.Services.Contracts;
using Engine.DiskModels.UI.Contracts;
using Engine.DiskModels.UI.Elements;
using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Elements;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.UI.Services
{
	/// <summary>
	/// Represents a user interface element service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface element service.
	/// </remarks>
	/// <param name="gameServices">The game service.</param>
	public class UserInterfaceElementService(GameServiceContainer gameServices) : IUserInterfaceElementService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the user interface elements.
		/// </summary>
		private List<IAmAUiElement> UserInterfaceElements { get; set; } = [];

		/// <summary>
		/// Gets the element dimensions.
		/// </summary>
		/// <param name="uiScreenZone">The user interface screen zone.</param>
		/// <param name="elementModel">The user interface element model.</param>
		/// <returns>The element dimensions.</returns>
		public Vector2? GetElementDimensions(UiScreenZone uiScreenZone, IAmAUiElementModel elementModel)
		{
			if (true == elementModel.FixedSized.HasValue)
			{ 
				return elementModel.FixedSized.Value;
			}

			if (null == uiScreenZone?.Area)
			{
				return null;
			}

			var uiElementSizeType = Enum.IsDefined(typeof(UiElementSizeTypes), elementModel.SizeType)
									? (UiElementSizeTypes)elementModel.SizeType
									: UiElementSizeTypes.None;

			switch (uiElementSizeType)
			{
				default:
				case UiElementSizeTypes.None:
				case UiElementSizeTypes.Fill:
					return null;
				case UiElementSizeTypes.ExtraSmall:
					return new Vector2(uiScreenZone.Area.Width / 5, uiScreenZone.Area.Height / 6);
				case UiElementSizeTypes.Small:
					return new Vector2(uiScreenZone.Area.Width / 4, uiScreenZone.Area.Height / 5);
				case UiElementSizeTypes.Medium:
					return new Vector2(uiScreenZone.Area.Width / 3, uiScreenZone.Area.Height / 4);
				case UiElementSizeTypes.Large:
					return new Vector2(uiScreenZone.Area.Width / 2, uiScreenZone.Area.Height / 3);
				case UiElementSizeTypes.ExtraLarge:
					return new Vector2(uiScreenZone.Area.Width / 1, uiScreenZone.Area.Height / 2);
				case UiElementSizeTypes.Full:
					return new Vector2(uiScreenZone.Area.Width, uiScreenZone.Area.Height);
			}
		}

		/// <summary>
		/// Processes the user interface element being pressed.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void ProcessUiElementPress(IAmAUiElement element, Vector2 elementLocation)
		{
			switch (element)
			{ 
				case UiButton button:
					var controlService = this._gameServices.GetService<IControlService>();
					var mouseLocation = controlService.ControlState.MouseState.Position;
					var clickableLocation = new Vector2(elementLocation.X + ((element.Area.X - button.ClickableArea.X) / 2), elementLocation.Y + ((element.Area.Y - button.ClickableArea.Y) / 2));

					if (clickableLocation.X <= mouseLocation.X &&
						clickableLocation.X + button.ClickableArea.X >= mouseLocation.X &&
						clickableLocation.Y <= mouseLocation.Y &&
						clickableLocation.Y + button.ClickableArea.Y >= mouseLocation.Y)
					{
						button.RaiseClickEvent();
					}

					break;
			}
		}

		/// <summary>
		/// Process the user interface button being clicked.
		/// </summary>
		/// <param name="button">The button.</param>
		public void ProcessUiButtonClick(UiButton button)
		{
			var uiService = this._gameServices.GetService<IUserInterfaceService>();
			uiService.ToggleUserInterfaceGroupVisibility(button.VisibilityGroup == 1 ? 2 : 1);

			if (null != button.ClickAnimation)
			{
				var animationService = this._gameServices.GetService<IAnimationService>();
				animationService.TriggerAnimation(button.ClickAnimation, true);
			}
		}

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="fillWidth">The fill width of the user interface element.</param>
		/// <param name="fillHeight">The fill height of the user interface element model.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface element.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel, UiScreenZone uiZone, float fillWidth, float fillHeight, int visibilityGroup)
		{
			var elementSize = this.GetElementDimensions(uiZone, uiElementModel);
			var width = true == elementSize.HasValue
						? elementSize.Value.X
						: fillWidth;
			var height = true == elementSize.HasValue
						 ? elementSize.Value.Y
						 : fillHeight;

			var imageService = this._gameServices.GetService<IImageService>();
			var image = imageService.GetImage(uiElementModel.BackgroundTextureName, (int)width, (int)height);
			var area = new Vector2(width, height);
			IAmAUiElement uiElement = uiElementModel switch
			{
				UiTextModel textModel => this.GetUiText(textModel, area),
				UiButtonModel buttonModel => this.GetUiButton(buttonModel, area),
				_ => null,
			};

			if (null != uiElement)
			{
				uiElement.VisibilityGroup = visibilityGroup;
				uiElement.Image = image;
				uiElement.PressEvent += this.ProcessUiElementPress;
				this.UserInterfaceElements.Add(uiElement);
			}

			return uiElement;
		}

		/// <summary>
		/// Gets the user interface text.
		/// </summary>
		/// <param name="textModel">The text model.</param>
		/// <param name="area">The area.</param>
		/// <returns>The user interface text.</returns>
		private UiText GetUiText(UiTextModel textModel, Vector2 area)
		{
			return new UiText
			{
				UiElementName = textModel.UiElementName,
				Text = textModel.Text,
				LeftPadding = textModel.LeftPadding,
				RightPadding = textModel.RightPadding,
				ElementType = UiElementTypes.Button,
				Area = area,
			};
		}

		/// <summary>
		/// Gets the user interface button.
		/// </summary>
		/// <param name="buttonModel">The user interface button model.</param>
		/// <param name="area">The area.</param>
		/// <returns>The user interface button.</returns>
		private UiButton GetUiButton(UiButtonModel buttonModel, Vector2 area)
		{
			var animationService = this._gameServices.GetService<IAnimationService>();
			var clickableArea = new Vector2(area.X * buttonModel.ClickableAreaScaler.X, area.Y * buttonModel.ClickableAreaScaler.Y);
			var button =  new UiButton
			{
				UiElementName = buttonModel.UiElementName,
				Text = buttonModel.Text,
				LeftPadding = buttonModel.LeftPadding,
				RightPadding = buttonModel.RightPadding,
				ElementType = UiElementTypes.Button,
				Area = area,
				ClickableArea = clickableArea
			};

			button.ClickEvent += this.ProcessUiButtonClick;

			if (null != buttonModel.ClickableAreaAnimation)
			{
				var clickAnimation = animationService.GetAnimation(buttonModel.ClickableAreaAnimation, (int)clickableArea.X, (int)clickableArea.Y);

				if (clickAnimation is TriggeredAnimation triggeredAnimation)
				{
					button.ClickAnimation = triggeredAnimation;
				}
			}

			return button;
		}
	}
}
