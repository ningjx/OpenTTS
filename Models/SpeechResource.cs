using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SpeechGenerator.Models
{
    public class SpeechResource: List<Voice>
    {
        public static SpeechResource LoadSpeechResources()
        {
            return JsonConvert.DeserializeObject<SpeechResource>(Resources.SpeechResources);
        }
    }

    public class Voice
    {
        [JsonProperty("名字")]
        public string Name { get; set; }
        [JsonProperty("编码")]
        public string Code { get; set; }
        [JsonProperty("性别")]
        public bool Female { get; set; }
        [JsonProperty("语言")]
        public Lan Language { get; set; }
        [JsonProperty("语气")]
        public SpeechStyle Style
        {
            get
            {
                return _style ?? Styles?.FirstOrDefault();
            }
            set
            {
                _style = value;
            }
        }
        [JsonProperty("语气强度")]
        public float StyleDegree
        {
            get { return _styleDegree; }
            set
            {
                if (value < 0.01) _styleDegree = 0.01F;
                else if (value > 2) _styleDegree = 2;
                else _styleDegree = value;
            }
        }
        [JsonProperty("角色")]
        public SpeechRole Role
        {
            get
            {
                return _role ?? Roles?.FirstOrDefault();
            }
            set
            {
                _role = value;
            }
        }
        [JsonProperty("支持的语气")]
        public List<SpeechStyle> Styles { get; set; } = new List<SpeechStyle>();
        [JsonProperty("支持的角色")]
        public List<SpeechRole> Roles { get; set; } = new List<SpeechRole>();

        [JsonIgnore]
        public bool SupportSSML => Styles?.Any() == true;
        [JsonIgnore]
        public bool SupportRole => Roles?.Any() == true;

        /// <summary>
        /// 语言类型
        /// </summary>
        public enum Lan
        {
            CN,
            HK,
            TW
        }

        private float _styleDegree = 1;
        private SpeechRole _role;
        private SpeechStyle _style;
    }

    public class SpeechStyle
    {
        public SpeechStyle(string style, string description)
        {
            Style = style; Description = description;
        }
        [JsonProperty("语气")]
        public string Style;
        [JsonProperty("描述")]
        public string Description;
    }

    public class SpeechRole
    {
        [JsonProperty("角色")]
        public string Role;
        [JsonProperty("描述")]
        public string Description;
    }
}
