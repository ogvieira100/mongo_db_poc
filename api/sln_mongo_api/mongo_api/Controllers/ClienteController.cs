using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using mongo_api.Models.Cliente;
using ZstdSharp.Unsafe;

namespace mongo_api.Controllers
{
    [ApiController]
    [Route("api/clientes")]

    public class ClienteController : Controller
    {


        private readonly IMediator _mediator;
        readonly IClienteQuery _clienteQuery;   

        public ClienteController(IMediator mediator, IClienteQuery clienteQuery)
        {
            _mediator = mediator;
            _clienteQuery = clienteQuery;       
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
            => Ok(await _mediator.Send(new ClienteDeletarCommand { Id =  id }));


        /// <summary>
        /// Get List By mongo DB
        /// </summary>
        /// <param name="clientePagedRequest"></param>
        /// <returns></returns>
        [HttpGet]

        public async Task<IActionResult> Get([FromQuery] ClientePagedRequest clientePagedRequest )
        {
            return Ok( await _clienteQuery.PagedCliente(clientePagedRequest));
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteSalvarRequest"></param>
        /// <returns></returns>

        [HttpPost]
        
        public async Task<IActionResult> Post([FromBody] ClienteInserirCommand clienteSalvarRequest)
        { 
            var resp = await _mediator.Send(clienteSalvarRequest);
            return Ok(resp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteAtualizarCommand"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ClienteAtualizarCommand  clienteAtualizarCommand)
        {
            var resp = await _mediator.Send(clienteAtualizarCommand);
            return Ok(resp);
        }
    }
}
