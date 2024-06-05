using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzurePartitionDuplicator
{
    public class KeyRow
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
