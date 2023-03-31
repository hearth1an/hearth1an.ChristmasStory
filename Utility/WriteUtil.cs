using OWML.Common;

namespace ChristmasStory.Utility
{
	public static class WriteUtil
	{
		public static void WriteDebug(string line)
		{
#if DEBUG
			WriteLine("Debug: " + line, MessageType.Info);
#endif
		}

		public static void WriteLine(string line) => WriteLine(line, MessageType.Info);
		public static void WriteError(string line) => WriteLine(line, MessageType.Error);
		public static void WriteLine(string line, MessageType type) => ChristmasStory.Instance.ModHelper.Console.WriteLine($"{type}: " + line, type);
	}
}
