using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GetThatPic.Data.Configuration
{
    class  DomElement
    {
        enum TargetType
        {
            Attribute,
            Html
        }
        string Selector { get; set; }
        string AttributeName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        TargetType Type { get; set; }

        Regex Pattern { get; set; } = new Regex("^(.*)$");

        string Replace { get; set; } = "$1";
    }
}
