using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Pdf.Api.External.Dto
{
    [JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumMemberConverter))]
    public enum Language
    {
        [EnumMember(Value = "af")]
        Afrikaans, 
        [EnumMember(Value = "sq")]
        Albanian,
        [EnumMember(Value = "am")]
        Amharic,
        [EnumMember(Value = "ar")]
        Arabic,
        [EnumMember(Value = "hy")]
        Armenian,
        [EnumMember(Value = "az")]
        Azerbaijani,
        [EnumMember(Value = "bn")]
        Bengali,
        [EnumMember(Value = "bs")]
        Bosnian,
        [EnumMember(Value = "bg")]
        Bulgarian,
        [EnumMember(Value = "ca")]
        Catalan,
        [EnumMember(Value = "zh")]
        ChineseSimplified,
        [EnumMember(Value = "zh-TW")]
        ChineseTraditional,
        [EnumMember(Value = "hr")]
        Croatian,
        [EnumMember(Value = "cs")]
        Czech,
        [EnumMember(Value = "da")]
        Danish,
        [EnumMember(Value = "fa-AF")]
        Dari,
        [EnumMember(Value = "nl")]
        Dutch,
        [EnumMember(Value = "en")]
        English,
        [EnumMember(Value = "et")]
        Estonian,
        [EnumMember(Value = "fa")]
        FarsiPersian,
        [EnumMember(Value = "tl")]
        FilipinoTagalog,
        [EnumMember(Value = "fi")]
        Finnish,
        [EnumMember(Value = "fr")]
        French,
        [EnumMember(Value = "fr-CA")]
        FrenchCanada,
        [EnumMember(Value = "ka")]
        Georgian,
        [EnumMember(Value = "de")]
        German,
        [EnumMember(Value = "el")]
        Greek,
        [EnumMember(Value = "gu")]
        Gujarati,
        [EnumMember(Value = "ht")]
        HaitianCreole,
        [EnumMember(Value = "ha")]
        Hausa,
        [EnumMember(Value = "he")]
        Hebrew,
        [EnumMember(Value = "hi")]
        Hindi,
        [EnumMember(Value = "hu")]
        Hungarian,
        [EnumMember(Value = "is")]
        Icelandic,
        [EnumMember(Value = "id")]
        Indonesian,
        [EnumMember(Value = "it")]
        Italian,
        [EnumMember(Value = "ja")]
        Japanese,
        [EnumMember(Value = "kn")]
        Kannada,
        [EnumMember(Value = "kk")]
        Kazakh,
        [EnumMember(Value = "ko")]
        Korean,
        [EnumMember(Value = "lv")]
        Latvian,
        [EnumMember(Value = "lt")]
        Lithuanian,
        [EnumMember(Value = "mk")]
        Macedonian,
        [EnumMember(Value = "ms")]
        Malay,
        [EnumMember(Value = "ml")]
        Malayalam,
        [EnumMember(Value = "mt")]
        Maltese,
        [EnumMember(Value = "mn")]
        Mongolian,
        [EnumMember(Value = "no")]
        Norwegian,
        [EnumMember(Value = "fa")]
        Persian,
        [EnumMember(Value = "ps")]
        Pashto,
        [EnumMember(Value = "pl")]
        Polish,
        [EnumMember(Value = "pt")]
        Portuguese,
        [EnumMember(Value = "ro")]
        Romanian,
        [EnumMember(Value = "ru")]
        Russian,
        [EnumMember(Value = "sr")]
        Serbian,
        [EnumMember(Value = "si")]
        Sinhala,
        [EnumMember(Value = "sk")]
        Slovak,
        [EnumMember(Value = "sl")]
        Slovenian,
        [EnumMember(Value = "so")]
        Somali,
        [EnumMember(Value = "es")]
        Spanish,
        [EnumMember(Value = "es-MX")]
        SpanishMexico,
        [EnumMember(Value = "sw")]
        Swahili,
        [EnumMember(Value = "sv")]
        Swedish,
        [EnumMember(Value = "tl")]
        Tagalog,
        [EnumMember(Value = "ta")]
        Tamil,
        [EnumMember(Value = "te")]
        Telugu,
        [EnumMember(Value = "th")]
        Tha,
        [EnumMember(Value = "tr")]
        Turkish,
        [EnumMember(Value = "uk")]
        Ukrainian,
        [EnumMember(Value = "ur")]
        Urdu,
        [EnumMember(Value = "uz")]
        Uzbek,
        [EnumMember(Value = "vi")]
        Vietnamese,
        [EnumMember(Value = "cy")]
        Welsh,
    }
}