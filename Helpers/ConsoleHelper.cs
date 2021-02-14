﻿using System;
using System.Drawing;
using System.Runtime.Versioning;
using SRAM.Comparison.Services;

namespace SRAM.Comparison.Helpers
{
	public class ConsoleHelper
	{
		private const int InitialConsoleWidth = 130;
		private const int InitialConsoleHeight = 400;
		private const int ConsoleBufferHeight = 1000;

		private static readonly IConsolePrinter ConsolePrinter = ComparisonServices.ConsolePrinter;
		private static readonly bool IsWindows = OperatingSystem.IsWindows();

		[SupportedOSPlatform("windows")]
		public static void EnsureMinConsoleWidth(int minWidth)
		{
			if (!IsWindows) return;

			try
			{
				var width = Math.Min(minWidth, Console.LargestWindowWidth);

				if (Console.WindowWidth >= width && Console.BufferWidth >= width)
					return;

				Console.BufferWidth = Console.WindowWidth = width;
			}
			catch
			{
				// Ignore
			}
		}

		public static void Initialize(IOptions options)
		{
			if (options.UILanguage is not null)
				CultureHelper.TrySetCulture(options.UILanguage, ConsolePrinter);

			ConsolePrinter.ColorizeOutput = options.ColorizeOutput;

			SetInitialConsoleSize();
		}

		[SupportedOSPlatform("windows")]
		public static void SetInitialConsoleSize()
		{
			if (!IsWindows) return;

			try
			{
				if (Console.BufferWidth < InitialConsoleWidth || Console.BufferHeight < ConsoleBufferHeight)
					Console.SetBufferSize(InitialConsoleWidth, ConsoleBufferHeight);

				var width = Math.Min(InitialConsoleWidth, Console.LargestWindowWidth);
				var height = Math.Min(InitialConsoleHeight, Console.LargestWindowHeight);

				if (Console.WindowWidth >= width && Console.WindowHeight >= height) return;

				Console.SetWindowSize(width, height);
			}
			catch
			{
				// Ignore
			}
		}

		[SupportedOSPlatform("windows")]
		public static void RedefineConsoleColors(Color color = default, Color bgColor = default)
		{
			if (!IsWindows) return;

			try
			{
				if (color == default && bgColor == default) return;

				if (color == default) color = Color.FromName(nameof(ConsoleColor.Gray));
				if (bgColor == default) bgColor = Color.FromName(nameof(ConsoleColor.Black));

				PaletteHelper.SetScreenColors(color, bgColor);

				Console.Clear();
			}
			catch
			{
				// Ignore
			}
		}
	}
}
