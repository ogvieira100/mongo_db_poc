using MediatR;
using Microsoft.AspNetCore.Mvc;
using mongo_api.Models.Cliente;
using mongo_api.Models.Pedidos;

namespace mongo_api.Controllers
{

    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : Controller
    {

        private readonly IMediator _mediator;

        public PedidoController(IMediator mediator)
        {
            _mediator = mediator;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pedidoInserirCommand"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> Post([FromBody] PedidoInserirCommand pedidoInserirCommand)
        {
            var resp = await _mediator.Send(pedidoInserirCommand);
            return Ok(resp);
        }

    }
}
