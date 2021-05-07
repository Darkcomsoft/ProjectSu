﻿using Projectsln.darkcomsoft.src.debug;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Projectsln.darkcomsoft.src.consolecli.systemconsole
{
	public class WindowsConsole : ClassBase
	{
		public static WindowsConsole instance { get; private set; }
		public static bool isOpen { get; private set; }

		private TextWriter oldOutput;
		public event System.Action<string> OnInputText;
		public string inputString;

		public WindowsConsole() 
		{ 
			instance = this;

			ShowDisposeDebugMsg = false;

			InitializeConsole(); 
			isOpen = true; 
		}

		public void Tick()
		{
			ConsoleTick();
		}

		protected override void OnDispose()
		{
			isOpen = false;
			ShutdownConsole();
			instance = null;
			base.OnDispose();
		}

		#region ConsoleWindow
		public void InitializeConsole()
		{
			//
			// Attach to any existing consoles we have
			// failing that, create a new one.
			//
			if (!AttachConsole(0x0ffffffff))
			{
				AllocConsole();
			}

			oldOutput = Console.Out;

			try
			{
				IntPtr stdHandle = GetStdHandle(STD_OUTPUT_HANDLE);
				Microsoft.Win32.SafeHandles.SafeFileHandle safeFileHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle(stdHandle, true);
				FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
				System.Text.Encoding encoding = System.Text.Encoding.ASCII;
				StreamWriter standardOutput = new StreamWriter(fileStream, encoding);
				standardOutput.AutoFlush = true;
				Console.SetOut(standardOutput);
			}
			catch (System.Exception e)
			{
				Debug.LogError("Couldn't redirect output: " + e.Message);
			}
		}

		public void ShutdownConsole()
		{
			Console.SetOut(oldOutput);
			FreeConsole();
		}

		public void SetTitleConsole(string strName)
		{
			SetConsoleTitle(strName);
		}

		private const int STD_OUTPUT_HANDLE = -11;

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool AttachConsole(uint dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool AllocConsole();

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool FreeConsole();

		[DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll")]
		static extern bool SetConsoleTitle(string lpConsoleTitle);
		#endregion

		#region ConsoleInput
		public void WriteLine(string msg)
		{
			System.Console.WriteLine(msg);
		}

		public void ConsoleClearLine()
		{
			Console.CursorLeft = 0;
			Console.Write(new String(' ', Console.BufferWidth));
			Console.CursorTop--;
			Console.CursorLeft = 0;
		}

		public void ConsoleRedrawInputLine()
		{
			if (inputString.Length == 0) return;

			if (Console.CursorLeft > 0)
				ConsoleClearLine();

			System.Console.ForegroundColor = ConsoleColor.Green;
			System.Console.Write(inputString);
			System.Console.ForegroundColor = ConsoleColor.White;
		}

		internal void ConsoleOnBackspace()
		{
			//if ( inputString.Length <= 0 ) return;

			if (inputString.Length <= 1)
			{
				ConsoleClearLine();

				var strtext = inputString;
				inputString = "";

				if (OnInputText != null)
				{
					OnInputText(strtext);
				}
			}
			else
			{

				inputString = inputString.Substring(0, inputString.Length - 1);
				ConsoleRedrawInputLine();
			}
		}

		internal void ConsoleOnEscape()
		{
			ConsoleClearLine();
			inputString = "";
		}

		internal void OnEnter()
		{
			ConsoleClearLine();

			System.Console.ForegroundColor = ConsoleColor.Green;
			string[] textarray = inputString.Split(" "[0]);
			//Commands.ReadInputCommand(textarray);
			WriteLine(inputString);
			System.Console.ForegroundColor = ConsoleColor.White;

			var strtext = inputString;
			inputString = "";

			if (OnInputText != null)
			{
				OnInputText(strtext);
			}
		}

		public void ConsoleTick()
		{
			if (!Console.KeyAvailable) return;
			var key = Console.ReadKey();

			if (key.Key == ConsoleKey.Enter)
			{
				OnEnter();
				return;
			}

			if (key.Key == ConsoleKey.Backspace)
			{
				ConsoleOnBackspace();
				return;
			}

			if (key.Key == ConsoleKey.Escape)
			{
				ConsoleOnEscape();
				return;
			}

			if (key.KeyChar != '\u0000')
			{
				inputString += key.KeyChar;
				ConsoleRedrawInputLine();
				return;
			}
		}
		#endregion
	}
}