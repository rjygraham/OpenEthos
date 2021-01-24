using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace OE.Models.ActivityStreams
{
	public class ActivityStreamsSerializationBinder : ISerializationBinder
	{
		private readonly Assembly activityStreamAssembly;

		public ActivityStreamsSerializationBinder()
		{
			activityStreamAssembly = typeof(ActivityStreamsSerializationBinder).Assembly;
		}

		public void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			_ = serializedType ?? throw new ArgumentNullException(nameof(serializedType));

			typeName = serializedType.Name;
			assemblyName = "";
		}

		public Type BindToType(string assemblyName, string typeName)
		{
			return activityStreamAssembly.GetType($"OE.Models.ActivityStreams.{typeName}");
		}
	}
}
