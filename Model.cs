using System.Collections.Generic;
using ConsoleApplication1.Entity;

namespace performORM.Model
{
    public class pages
    {
        public int ID { get; set; }
        public string name { get; set; }
    }
    public class categories
    {
        public int ID { get; set; }
        public string name { get; set; }
    }
    public class product
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public int price { get; set; }
    }
    public class MyTable : Entity<MyTable>
    {
        public List<pages> pages { get; set; }
        public List<categories> categories { get; set; }
        public List<product> products { get; set; }
    }
}
