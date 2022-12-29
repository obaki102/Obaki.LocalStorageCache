using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obaki.LocalStorageCache.Test.TestHelper
{
    public class DummyObject
    {
        public int Id { get; set; }
        public string  Name { get; set; } = string.Empty;
        public DummyObject()
        {
        }
        public DummyObject(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
