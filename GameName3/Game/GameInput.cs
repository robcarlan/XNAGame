using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoGame;
using MonoGame.Framework;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameName3.Game {
	public class GameInput {
		Game1 game;

		KeyboardState prevKeyState;
		KeyboardState keyboardState;
		MouseState prevMouseState;
		MouseState mouseState;
		Point mousePos;
		Point mouseMovement;

		public GameInput(Game1 _game) {
			game = _game;
		}

		/// <summary>
		/// The main update frame for the input class
		/// </summary>
		/// <param name="game"></param>
		public void Update() {
			updateInputStates();

			//Blah blah switch base on game state etc.
		}

		void updateInputStates() {
			prevKeyState = keyboardState;
			prevMouseState = mouseState;
			keyboardState = Keyboard.GetState();
			mouseState = Mouse.GetState();
			mouseMovement.X = mouseState.X - mousePos.X;
			mouseMovement.Y = mouseState.Y - mousePos.Y;
			mousePos.X = mouseState.X;
			mousePos.Y = mouseState.Y;
		}

		//Input Functions
		public Point getMousePosition() {
			return mousePos;
		}

		public Point getMouseMovement() {
			return mouseMovement;
		}

		public bool isKeyDown(Keys key) {
			return keyboardState.IsKeyDown(key);
		}

		public bool isKeyUp(Keys key) {
			return keyboardState.IsKeyUp(key);
		}

		public bool wasJustPressed(Keys key) {
			return (keyboardState.IsKeyDown(key) && prevKeyState.IsKeyUp(key));
		}

		public bool wasJustReleased(Keys key) {
			return (keyboardState.IsKeyUp(key) && prevKeyState.IsKeyDown(key));
		}

	}
}
