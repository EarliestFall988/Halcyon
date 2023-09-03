using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweening_Lib
{
    /// <summary>
    /// the tag given to a gameobject
    /// </summary>
    public struct Tag
    {
        /// <summary>
        /// the name of the tag
        /// </summary>
        public string name { get; set; }

        public Tag(string name)
        {
            this.name = name;
        }

        public static bool operator ==(Tag a, Tag b)
        {
            return a.name == b.name;
        }

        public static bool operator !=(Tag a, Tag b)
        {
            return a.name != b.name;
        }

        public override bool Equals(object obj)
        {
            return obj is Tag tag &&
                   name == tag.name;
        }

        public override int GetHashCode()
        {
            return 363513814 + EqualityComparer<string>.Default.GetHashCode(name);
        }

        public override string ToString()
        {
            return "tag: " + name;
        }
    }
}
