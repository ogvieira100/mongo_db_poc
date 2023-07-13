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
        readonly IPedidoQuery _pedidoQuery;
        public PedidoController(IMediator mediator, IPedidoQuery pedidoQuery)
        {
            _mediator = mediator;
            _pedidoQuery = pedidoQuery; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pedidoAtualizarCommand"></param>
        /// <returns></returns>
        [HttpPut]

        public async Task<IActionResult> Put([FromBody] PedidoAtualizarCommand pedidoAtualizarCommand)
        {
            var resp = await _mediator.Send(pedidoAtualizarCommand);
            return Ok(resp);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pedidoPagedRequest"></param>
        /// <returns></returns>

        [HttpGet]

        public async Task<IActionResult> Get([FromQuery] PedidoPagedRequest pedidoPagedRequest)
        {
            var resp = await _pedidoQuery.PagedPedidos(pedidoPagedRequest); 
            return Ok(resp);
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
