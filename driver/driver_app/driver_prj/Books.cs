using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace driver_prj
{
    public class Books
    {

        [BsonElement("_id")]
        public Guid Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("writer")]
        public string? Writer { get; set; }


    }
}
