using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WindowsGame1;
using WindowsGame1.Character_Components;
using WindowsGame1.Entity_Components;

namespace Scripts
{
	public partial class Script
	{
		const float SCRIPT_FINISH = float.NegativeInfinity;
		const float SCRIPT_WAIT = float.PositiveInfinity;

		public static float scriptWaitForInput(UI _ui)
		{
			//Call function in ui to reset input choice / state to default
			return float.PositiveInfinity;
		}

		public static float finishScript()
		{
			//Close dialogue if this is the main script!
			return SCRIPT_FINISH;
		}

		public static int getPlayerGold(Player player)
		{
			return 4; //player.gold
		}

		public static void callScript(string scriptName, object[] _params, Game1 game)
		{
			game.script.callScript(scriptName, _params);
		}

		//parses a string into a list of strings, each item represents one screen of text, and is automatically
		//split over several lines. WOW.
		public static List<string> parseString(string text, UI ui)
		{
			int linesPerFrame = ui.dialogueMaxLines - 1;
			string line = "";
			List<string> lines = new List<string>();
			string[] wordArray = text.Split(' ');

			// Go through each word in string
			foreach (string word in wordArray)
			{
				if (ui.cinematicFont.MeasureString(line + word).X * ui.cinematicTextScale > ui.dialogueMaxTextWidth)
				{
					//line contains the minimum amount of text before a new line is needed
					lines.Add(line);
					line = "";
				}
				line += word + " ";
			}
			lines.Add(line);

			line = "";
			List<string> output = new List<string>();
			int frameItr = 0;
			//Split lines into groups of max_lines
			while (lines.Count > linesPerFrame)
			{
				for(int i  = 0; i < linesPerFrame; i++)
				{
					line += lines[i] + "\n";
				}
				lines.RemoveRange(0, linesPerFrame);
				output.Insert(0, line);
				line = "";
			}
			//Split last group
			foreach (string nextLine in lines)
			{
				line += nextLine + "\n";
			}
			output.Insert(0, line);
			output.Reverse();
			return output;
		}

		public static int getSelectedOption(UI _ui)
		{
			return _ui.dialogueSelectedOption;
		}

		public static void someFunction(Character bob, Character target, ObjManager obj)
		{
			obj.addProjectile(2, bob, target, (float)(0));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 1 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 2 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 3 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 4 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 5 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 6 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 7 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 8 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 9 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 10 / 6));
			obj.addProjectile(2, bob, target, (float)(Math.PI * 11 / 6));
		}

		public static void onCollide_p_sword(Projectile proj, Collidable_Sprite target, ObjManager obj)
		{
			float xDifference = target.circleOrigin.X - proj.parent.circleOrigin.X;
			float yDifference = target.circleOrigin.Y - proj.parent.circleOrigin.Y;
			obj.addProjectile(2, (Character)target, proj.parent, (float)Math.Atan2(-xDifference, -yDifference));
		}

		public static IEnumerable<float> dialogueScript(Game1 game)
		{
			List<string> exampleText = parseString("Hi, how are you?", game.UI);
			game.UI.startCinematicMode();
			game.UI.setSpeaker((Character)game.ObjectManager.Entities[999]);
			while (exampleText.Count > 0)
			{
				game.UI.setDialogue(exampleText[0]);
				exampleText.RemoveAt(0);
				yield return scriptWaitForInput(game.UI);
			}
			List<string> options = new List<string>();
			options.Add(parseString("I'm fine.", game.UI)[0]);
			options.Add(parseString("I've been better.", game.UI)[0]);
			options.Add(parseString("I'm fucking pissed.", game.UI)[0]);
			options.Add(parseString("Another option. ", game.UI)[0]);
			options.Add(parseString("Yet another option. ", game.UI)[0]);
			options.Add(parseString("Wut. ", game.UI)[0]);
			options.Add(parseString("Hi. ", game.UI)[0]);
			game.UI.setDialogue(options);
			yield return scriptWaitForInput(game.UI);
			switch (getSelectedOption(game.UI))
			{
				case 0:
					game.UI.setDialogue("That's nice. ");
					yield return scriptWaitForInput(game.UI);
					break;
				case 1:
					game.UI.setDialogue("Why? What's wrong? ");
					yield return scriptWaitForInput(game.UI);
					break;
				case 2:
					game.UI.setDialogue("I'm sorry to hear that. ");
					yield return scriptWaitForInput(game.UI);
					break;
			}
			game.UI.endCinematicMode();
		}
	}
}