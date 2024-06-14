using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

using System;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace SpeechGenerator.Models
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

        private float _styleDegree = 1;
        private SpeechRole _role;
        private SpeechStyle _style;
    }

    public class SpeechStyle
    {
        public SpeechStyle(SpeechStyleEnum style, string description)
        {
            Style = style; Description = description;
        }
        [JsonProperty("语气")]
        public SpeechStyleEnum Style { get; set; }
        [JsonProperty("描述")]
        public string Description { get; set; }
    }

    public class SpeechRole
    {
        [JsonProperty("角色")]
        public RoleEnum Role { get; set; }
        [JsonProperty("描述")]
        public string Description { get; set; }
    }
    public class AllSpeachStyles : List<SpeechStyle>
    {

    }

    /// <summary>
    /// 语言类型
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LanguageEnum
    {
        zh_CN,
        zh_HK,
        zh_TW,
        yue_CN,
        wuu_CN,
            zh_cn_GUANGXI, zh_cn_henan, zh_cn_liaoning, zh_cn_shaanxi, zh_cn_shandong, zh_cn_sichuan
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
    public enum SpeechStyleEnum
    {
        advertisement_upbeat,
        affectionate,
        angry,
        assistant, calm, chat, cheerful, customerservice, depressed, disgruntled, documentary_narration,
        embarrassed, empathetic, envious, excited, fearful, friendly, gentle, hopeful, lyrical, narration_professional
            , narration_relaxed, newscast, newscast_casual, newscast_formal, poetry_reading, sad, serious, shouting,
        sports_commentary, sports_commentary_excited, whispering, terrified, unfriendly
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RoleEnum
    {
        Girl, Boy, YoungAdultFemale, YoungAdultMale, OlderAdultFemale, OlderAdultMale, SeniorFemale, SeniorMale
    }
}
