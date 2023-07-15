using MediatR;
using Microsoft.AspNetCore.Mvc;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Pedidos;
using mongo_api.Models.Produto;

namespace mongo_api.Controllers
{

    [ApiController]
    [Route("api/fornecedor")]
    public class FornecedorController : Controller
    {
        IMediator _mediator;
        IFornecedorQuery _fornecedorQuery;

        public FornecedorController(IMediator mediator, IFornecedorQuery fornecedorQuery)
        {
            _mediator = mediator;
            _fornecedorQuery = fornecedorQuery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pedidoPagedRequest"></param>
        /// <returns></returns>
        [HttpGet]

        public async Task<IActionResult> Get([FromQuery] FornecedorPagedRequest pedidoPagedRequest)
        {
            var resp = await _fornecedorQuery.PagedFornecedores(pedidoPagedRequest);
            return Ok(resp);
        }


        /// <summary>
        /// /s
        /// </summary>
        /// <param name="fornecedorAtualizarCommand"></param>
        /// <returns></returns>

        [HttpPut]

        public async Task<IActionResult> Put([FromBody] FornecedorAtualizarCommand fornecedorAtualizarCommand)
        {
            var resp = await _mediator.Send(fornecedorAtualizarCommand);
            return Ok(resp);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="_fornecedorInserirCommand"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FornecedorInserirCommand _fornecedorInserirCommand )
        {
            var resp = await _mediator.Send(_fornecedorInserirCommand);
            return Ok(resp);
        }
    }
}
