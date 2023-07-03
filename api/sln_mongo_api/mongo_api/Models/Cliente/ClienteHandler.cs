using MediatR;
using mongo_api.Data.Repository;

namespace mongo_api.Models.Cliente
{
    public class ClienteHandler
                : IRequestHandler<ClienteInserirCommand, ClienteResponse>,
                  IRequestHandler<ClienteAtualizarCommand, ClienteResponse>,
                  IRequestHandler<ClienteDeletarCommand,ClienteResponse> 

    {

        readonly IUnitOfWork _unitOfWork;
        readonly IClienteQuery _clienteQuery;
        readonly IBaseRepository<Clientes> _clienteRepository;
        readonly IBaseRepository<Endereco> _enderecoRepository;
        readonly IBaseConsultRepositoryMongo<ClientesMongo> _baseConsultRepositoryClienteMongo;
        readonly IBaseConsultRepositoryMongo<EnderecoMongo> _baseConsultRepositoryEnderecoMongo;


        public ClienteHandler(IUnitOfWork unitOfWork,
                             IClienteQuery clienteQuery,
                             IBaseRepository<Endereco> enderecoRepository,
                             IBaseConsultRepositoryMongo<EnderecoMongo> baseConsultRepositoryEnderecoMongo,
                             IBaseConsultRepositoryMongo<ClientesMongo> baseConsultRepositoryClienteMongo,
                             IBaseRepository<Clientes> clienteRepository)
        {
            _unitOfWork = unitOfWork;
            _clienteQuery = clienteQuery;
            _baseConsultRepositoryClienteMongo = baseConsultRepositoryClienteMongo;
            _clienteRepository = clienteRepository;
            _baseConsultRepositoryEnderecoMongo = baseConsultRepositoryEnderecoMongo;
            _enderecoRepository = enderecoRepository;
        }
        public async Task<ClienteResponse> Handle(ClienteInserirCommand request,
                                                 CancellationToken cancellationToken)
        {
            var resp = new ClienteResponse();

            var novoCliente = new Clientes();
            novoCliente.CPF = request.CPF;
            novoCliente.Nome = request.Nome;


            novoCliente.Enderecos.AddRange(request.Enderecos.Select(x => new Endereco { Cliente = novoCliente, Estado = x.Estado, Logradouro = x.Logradouro }));

            await _clienteRepository.AddAsync(novoCliente);
            await _unitOfWork.CommitAsync();
            return resp;
        }

        public async Task<ClienteResponse> Handle(ClienteAtualizarCommand request, 
            CancellationToken cancellationToken)
        {
            /*buscando informações do mongo para preparar o Objeto para atualizar*/
            var resp = new ClienteResponse();
            var cliMongo = await _clienteQuery.GetCliMongoByRelationId(request.Id.ToString());
            /*pegando os ids existentes no banco*/
            var idsEnderecos = cliMongo.Enderecos.Select(x => x.RelationalId);

            /*criando um objeto de cliente e equalizando com dados da base não relacional*/
            var cliUpdate = new Clientes();
            cliUpdate.Id = new Guid(cliMongo.RelationalId);
            cliUpdate.Nome = cliMongo.Nome;
            cliUpdate.CPF = cliMongo.CPF;


            //_enderecoRepository
            /*equalizando endereços*/
            foreach (var x in cliMongo.Enderecos)
            {
                var end = new Endereco
                {
                    Id = new Guid(x.RelationalId),
                    Estado = x.Estado,
                    Logradouro = x.Logradouro,
                };
                _enderecoRepository.Update(end);
                cliUpdate.Enderecos.Add(end);
            }


            /*Objeto equalizado atualizando request com objeto equalizado*/
            cliUpdate.Nome = request.Nome;
            cliUpdate.CPF = request.CPF;
            cliUpdate.Enderecos.ForEach(x =>
            {
                var endAtu = request.Enderecos.FirstOrDefault(z => z.Id == x.Id);
                if (endAtu != null)
                {
                    x.Estado = endAtu.Estado;
                    x.Logradouro = endAtu.Logradouro;

                }
            });

            /*adicionando endereços novos para incluir no cliente atualizado*/

            foreach (var x in request.Enderecos.Where(x => !idsEnderecos.Contains(x.Id.ToString())))
            {
               var endNew =  new Endereco
                {
                    Estado = x.Estado,
                    Logradouro = x.Logradouro,
                };
                await _clienteRepository.AddAsync(endNew);  
                cliUpdate.Enderecos.Add(endNew);
            }

            /*relacionando cliente para atualizar no mongo*/
            cliUpdate.Enderecos.ForEach(x =>
            {
                x.Cliente = x.Cliente;
            });

            /*trackeando objeto cliente para ser salvo*/
            _clienteRepository.Update(cliUpdate);

            /*atualizando as informações*/
            await _unitOfWork.CommitAsync();

            return resp;
        }

        public async Task<ClienteResponse> Handle(ClienteDeletarCommand request,
                                                  CancellationToken cancellationToken)
        {

            var resp = new ClienteResponse();
           // var cliMongo = await _clienteQuery.GetCliMongoByRelationId(request.Id.ToString());
            var cliUpdate = new Clientes();
            cliUpdate.Id = request.Id;
            _clienteRepository.Remove(cliUpdate);
            /*atualizando as informações*/
            await _unitOfWork.CommitAsync();

            return resp;
            
        }
    }
}
