using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;

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

        private List<Event> _notificacoes;

        [NotMapped]
        public IReadOnlyCollection<Event> Notificacoes => _notificacoes?.AsReadOnly();

        public void AdicionarEvento(Event evento)
        {
            _notificacoes = _notificacoes ?? new List<Event>();
            _notificacoes.Add(evento);
        }

        public void RemoverEvento(Event eventItem)
        {
            _notificacoes?.Remove(eventItem);
        }

        public void LimparEventos()
        {
            _notificacoes?.Clear();
        }
        public Guid Id { get; set; }


        protected Base()
        {
            Id = Guid.NewGuid();    
        }
    }
}
