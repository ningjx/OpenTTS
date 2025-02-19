using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenTTS.Models
{
    public class StyleResource : Dictionary<SpeechStyleEnum, string>
    {
        public static StyleResource LoadStyleResource()
        {
            return JsonConvert.DeserializeObject<StyleResource>(Resources.AllSpeachStyles);
        }

        public static StyleResource Create(IEnumerable<SpeechStyleEnum> styles)
        {
            var styleresource = new StyleResource();
            foreach (var style in styles)
            {
                if (!styleresource.ContainsKey(style))
                {
                    ResourcePool.StyleResource.TryGetValue(style, out string value);
                    styleresource.Add(style, value);
                }
            }
            return styleresource;
        }
    }
}
