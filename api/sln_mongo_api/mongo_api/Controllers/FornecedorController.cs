using MediatR;
using Microsoft.AspNetCore.Mvc;
using mongo_api.Models.Fornecedores;
using mongo_api.Models.Produto;

namespace mongo_api.Controllers
{

    [ApiController]
    [Route("api/fornecedor")]
    public class FornecedorController : Controller
    {
        IMediator _mediator;

        public FornecedorController(IMediator mediator)
        {
            _mediator = mediator;   
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
