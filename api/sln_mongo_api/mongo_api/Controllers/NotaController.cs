using MediatR;
using Microsoft.AspNetCore.Mvc;
using mongo_api.Models.Cliente;
using mongo_api.Models.Notas;
using mongo_api.Models.Pedidos;

namespace mongo_api.Controllers
{
    [ApiController]
    [Route("api/nota")]
    public class NotaController : Controller
    {

        IMediator _mediator;
        readonly INotaQuery _notaQuery;
        public NotaController(IMediator mediator, INotaQuery notaQuery)
        {
                _mediator = mediator;   
                _notaQuery = notaQuery;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pedidoPagedRequest"></param>
        /// <returns></returns>

        [HttpGet]

        public async Task<IActionResult> Get([FromQuery]  NotaPagedRequest pedidoPagedRequest)
        {
            var resp = await _notaQuery.PagedNotas(pedidoPagedRequest);
            return Ok(resp);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
            => Ok(await _mediator.Send(new ClienteDeletarCommand { Id = id }));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pedidoInserirCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NotaInserirCommand pedidoInserirCommand)
        {
            var resp = await _mediator.Send(pedidoInserirCommand);
            return Ok(resp);
        }

    }
}
