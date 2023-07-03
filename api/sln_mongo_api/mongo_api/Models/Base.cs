namespace mongo_api.Models
{

    public  class BaseMongo
    {

        public string Id { get; set; }

        public string RelationalId { get; set; }

        public string TableName { get; set; }

    }
    public abstract class Base
    {

        public Guid Id { get; set; }


        protected Base()
        {
            Id = Guid.NewGuid();    
        }
    }
}
