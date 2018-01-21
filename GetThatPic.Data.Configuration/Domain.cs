using System.Collections;
using System.Text.RegularExpressions;

namespace GetThatPic.Data.Configuration
{
    public class Domain
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public Regex Path { get; set; }
    }
}
