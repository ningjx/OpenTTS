using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OpenTTS.Models
{
    public class SpeechResource : List<Voice>
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
        public GenderEnum Gender { get; set; }
        [JsonProperty("语言")]
        public LanguageEnum Language { get; set; }
        [JsonProperty("语气")]
        public SpeechStyleEnum Style { get; set; }
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
        public RoleEnum Role { get; set; }

        [JsonProperty("支持的语气")]
        public List<SpeechStyleEnum> Styles { get; set; } = new List<SpeechStyleEnum>();
        [JsonProperty("支持的角色")]
        public List<RoleEnum> Roles { get; set; } = new List<RoleEnum>();

        [JsonIgnore]
        public bool SupportSSML => Styles?.Any() == true;
        [JsonIgnore]
        public bool SupportRole => Roles?.Any() == true;

        private float _styleDegree = 1;
    }
    public class SpeachLang
    {
        [JsonProperty("语言")]
        public LanguageEnum Language { get; set; }
        [JsonProperty("描述")]
        public string Description { get; set; }
    }

    /// <summary>
    /// 从文件中反序列化
    /// </summary>
    //public class SpeachStylesStorage : List<SpeechStyle> { }
    //public class RolesStorage : List<SpeechRole> { }
    //public class LanguagesStorage : List<SpeachLang> { }

    /// <summary>
    /// 语言类型
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LanguageEnum
    {
        [Description("zh-CN")]
        普通话,
        [Description("zh-HK")]
        广东,
        [Description("zh-TW")]
        台湾,
        [Description("zh-CN-GUANGXI")]
        广西,
        [Description("zh-CN-henan")]
        河南,
        [Description("zh-CN-liaoning")]
        东北,
        [Description("zh-CN-shaanxi")]
        陕西,
        [Description("zh-CN-shandong")]
        山东,
        [Description("zh-CN-sichuan")]
        四川,
        [Description("zh-CN-anhui")]
        安徽,
        [Description("zh-CN-hunan")]
        湖南,
        [Description("zh-CN-gansu")]
        甘肃,
        [Description("nan-CN")]
        nan_CN,
        [Description("yue-CN")]
        yue_CN,
        [Description("wuu-CN")]
        wuu_CN
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GenderEnum
    {
        男,
        女,
        沃尔玛购物袋,
        武装直升机
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoleEnum
    {
        Unspecified,
        Girl,
        Boy,
        YoungAdultFemale,
        YoungAdultMale,
        OlderAdultFemale,
        OlderAdultMale,
        SeniorFemale,
        SeniorMale
    }
}
