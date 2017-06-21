using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using DataLoader;
using NLua;

namespace WindowsGame1.Utility
{
	public class ScriptEngine
	{
		public Dictionary<string, int> scriptVariables; //Saves current state of scripts

		public Script mainScript;	//This is the dialogue script which uses input
		public List<Script> runningScripts = new List<Script>();
		public Dictionary<string, MethodInfo> functions = new Dictionary<string,MethodInfo>();
		public Game1 game;
		float isScriptFinished = float.NegativeInfinity;
		
		//Have data to store all quests / dialogue

		//One Enumerator for current dialogue / cinematic script?

		public ScriptEngine(Game1 game)
		{
			//Load all files from script directory
		}

		public void callScript(string scriptName, object[] _param)
		{
			//Check to see if it returns anything
			if (functions.ContainsKey(scriptName))
			{
				MethodInfo method = functions[scriptName];
				if (method.ReturnType == typeof(IEnumerable<float>))
				{
					//Script returns values - add to running scripts
					startScript(scriptName, _param);
				}
				else
				{
					//Script can just be run instantly and return
					functions[scriptName].Invoke(null, _param);
				}
			}
		}

		public void runScripts(float gameTime)
		{
			foreach (Script _script in runningScripts)
			{
				_script.Update(gameTime);
			}

			if (mainScript != null)
			{
				//Update main script
				if (!float.IsPositiveInfinity(mainScript.timer))
				{
					//Script is not waiting on input
					mainScript.Update(gameTime);

					if (mainScript.isComplete)
					{
						//Remove script
						mainScript = null;
					}
				}
			}

			//Removes completed scripts
			runningScripts.RemoveAll(s => s.isComplete);
		}

		public void onGetDialogueInput()
		{
			//Recieved input is accessible from UI
			if (mainScript != null)
			{
				mainScript.results.MoveNext();
				mainScript.timer = mainScript.results.Current;
				if (mainScript.isFinished())
					mainScript = null;
			}
		}

		//Function to handle input choice, which directly calls script

		public void setMainScript(string scriptName, Game game)
		{
			if (!functions.ContainsKey(scriptName)) return;

			//ReturnObj returnVal = new ReturnObj();
			//var returnVal = Enumerable.Empty<float>();
			//functions[scriptName].Invoke(returnVal, new object[] { game });
			//IEnumerator<float> result = returnVal.GetEnumerator();
			IEnumerator<float> result =
				((IEnumerable<float>)functions[scriptName].Invoke(null, new object[] { game })).GetEnumerator();
			result.MoveNext();
			//Returning negative infinity indicates the script ahs finished
			if (!float.IsNegativeInfinity(result.Current) && result.Current > 0)
			{
				//Script is sleeping
				mainScript = new Script(result);
			}	//Else script is complete!
		}

		public void startScript(string scriptName, Game game)
		{
			if (!functions.ContainsKey(scriptName)) return;

			IEnumerator<float> result = 
				((IEnumerable<float>)functions[scriptName].Invoke(null, new object[] { game })).GetEnumerator();
			result.MoveNext();
			//Returning negative infinity indicates the script ahs finished
			//If 0 or less, it has also finished.
			if (!float.IsNegativeInfinity(result.Current) && result.Current > 0)
			{		
				//Script is sleeping
				runningScripts.Add(new Script(result));
			}
		}

		public void startScript(string scriptName, Object[] paramaters)
		{
			if (!functions.ContainsKey(scriptName)) return;

			IEnumerator<float> result =
				((IEnumerable<float>)functions[scriptName].Invoke(null, paramaters)).GetEnumerator();
			result.MoveNext();
			//Returning negative infinity indicates the script ahs finished
			//If 0 or less, it has also finished.
			if (!float.IsNegativeInfinity(result.Current) && result.Current > 0)
			{
				//Script is sleeping
				runningScripts.Add(new Script(result));
			}
		}

		public void continueDialogueScript()
		{
			//Might need to only continue if time = +inf
			//Flag current main script to continue
			mainScript.timer = 0.0f;

			return;
		}
	}

	public class Script
	{
		public bool isComplete = false;
		
		public float timer;
		public IEnumerator<float> results;

		public Script(IEnumerator<float> method)
		{
			timer = method.Current;
			results = method;
		}

		//If waiting for input, gameTime = 0
		public void Update(float gameTime)
		{
			timer -= gameTime;
			if (timer <= 0)
			{
				results.MoveNext();
				timer = results.Current;

				if (float.IsNegativeInfinity(timer))
					isComplete = true;
			}
		}

		public bool isFinished()
		{ return float.IsNegativeInfinity(timer); }

	}
}
