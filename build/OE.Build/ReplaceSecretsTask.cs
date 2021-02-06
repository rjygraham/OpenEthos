using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace OE.Build
{
	public class ReplaceSecretsTask: Task
	{
		[Required]
		public ITaskItem[] Files { get; set; }

		[Required]
		public string TargetFileName { get; set; }

		[Required]
		public string Placeholders { get; set; }

		public override bool Execute()
		{
			if (Files.Length > 0 && !string.IsNullOrEmpty(TargetFileName) && !string.IsNullOrEmpty(Placeholders))
			{
				for (int i = 0; i < Files.Length; i++)
				{
					ITaskItem item = Files[i];
					string path = item.GetMetadata("FullPath");
					if (TargetFileName.Equals(Path.GetFileName(path), StringComparison.OrdinalIgnoreCase))
					{
						string generatedPath = path.Replace(".cs", ".g.cs");
						string content = File.ReadAllText(path);

						string[] placeholders = Placeholders.Split(',');
						foreach (string placeholder in placeholders)
						{
							content = Regex.Replace(content, $"\\${placeholder}\\$", Environment.GetEnvironmentVariable(placeholder));
						}
						File.WriteAllText(generatedPath, content);
						Files[i] = new TaskItem(Path.GetFileName(generatedPath));

						return true;
					}
				}
			}

			return false;
		}
	}
}
