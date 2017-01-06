namespace CsDoc.Model
{
    using System.Collections.Generic;

    public class ListDocInfo
    {
        public string Type { get; set; }

        public ListItemDocInfo Header { get; private set; } = new ListItemDocInfo();

        public List<ListItemDocInfo> Items = new List<ListItemDocInfo>();
    }

    public class ListItemDocInfo
    {
        public string Term { get; set; }
        public string Description { get; set; }
    }    
}