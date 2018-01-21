using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GetThatPic.Data.Configuration;

namespace GetThatPic.Parsing
{
    public class Link
    {
        public Link()
        {
            InitializeConfig();
        }

        public IList<Domain> Domains { get; } = new List<Domain>();

        public void InitializeConfig(bool clearFirst)
        {
            if (clearFirst)
            {
                Domains.Clear();
            }

            Domains.Add(new Domain
            {
                Name = "dilbert.com",
                Url = "http://dilbert.com",
                Path = new Regex("^/strip/((?:[0-9]+-?)+)$")
            });
        }

        public Domain IdentifyDomain(Uri link)
        {
            return Domains.FirstOrDefault(domain => domain.Url == link.Host);
        }
    }
}
