using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel;

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
            if (styles == null) return null;

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

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SpeechStyleEnum
    {
        [Description("advertisement-upbeat")]
        广告_积极,

        [Description("affectionate")]
        亲切,

        [Description("angry")]
        生气,

        [Description("assistant")]
        助手,

        [Description("calm")]
        平静,

        [Description("chat")]
        闲聊,

        [Description("cheerful")]
        欢快,

        [Description("customerservice")]
        客服,

        [Description("depressed")]
        抑郁,

        [Description("disgruntled")]
        不满,

        [Description("documentary-narration")]
        纪录片旁白,

        [Description("embarrassed")]
        尴尬,

        [Description("empathetic")]
        共情,

        [Description("envious")]
        嫉妒,

        [Description("excited")]
        兴奋,

        [Description("fearful")]
        害怕,

        [Description("friendly")]
        友好,

        [Description("gentle")]
        温柔,

        [Description("hopeful")]
        希望,

        [Description("lyrical")]
        抒情,

        [Description("narration-professional")]
        专业旁白,

        [Description("narration-relaxed")]
        轻松旁白,

        [Description("newscast")]
        新闻播报,

        [Description("newscast-casual")]
        新闻播报_随意,

        [Description("newscast-formal")]
        新闻播报_正式,

        [Description("poetry-reading")]
        诗歌朗诵,

        [Description("sad")]
        悲伤,

        [Description("serious")]
        严肃,

        [Description("shouting")]
        喊叫,

        [Description("sports-commentary")]
        体育解说,

        [Description("sports-commentary-excited")]
        体育解说_激昂,

        [Description("whispering")]
        耳语,

        [Description("terrified")]
        惊恐,

        [Description("unfriendly")]
        不友好
    }
}
