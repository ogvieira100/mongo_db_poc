using MediatR;
using Microsoft.AspNetCore.Mvc;
using mongo_api.Models.Cliente;
using mongo_api.Models.Produto;

namespace mongo_api.Controllers
{
    [ApiController]
    [Route("api/produto")]
    public class ProdutoController : Controller
    {
        IMediator _mediator;
        IProdutoQuery _produtoQuery;

        public ProdutoController(IProdutoQuery produtoQuery, IMediator mediator)
        {
            _mediator = mediator;
            _produtoQuery = produtoQuery;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProdutoPagedRequest produtoPagedRequest )
            => Ok(await _produtoQuery.PagedProdutos(produtoPagedRequest));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="produtoInserirCommand"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> Post([FromBody] ProdutoInserirCommand produtoInserirCommand )
        {
            var resp = await _mediator.Send(produtoInserirCommand);
            return Ok(resp);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="produtoAtualizarCommand"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProdutoAtualizarCommand produtoAtualizarCommand)
        {
            var resp = await _mediator.Send(produtoAtualizarCommand);
            return Ok(resp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/id")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var resp = await _mediator.Send(new ProdutoDeletarCommand { Id = id });
            return Ok(resp);
        }

    }
}
