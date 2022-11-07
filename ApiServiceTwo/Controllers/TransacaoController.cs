using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiTransacao;
using ApiTransacao.Domain;
using ApiTransacao.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace ApiTransacao.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransacaoController : ControllerBase
    {
        private readonly ILogger<TransacaoController> _logger;
        private ITransacaoService _transacaoService;
        private IMemoryCache _memoryCache;
        private IDistributedCache _cache;
        public TransacaoController(
            ILogger<TransacaoController> logger, 
            ITransacaoService transacaoService,
            IMemoryCache memoryCache,
            IDistributedCache cache
        )
        {
            _logger = logger;
            _transacaoService = transacaoService;
            _memoryCache = memoryCache;
            _cache = cache;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get()
        {
            return Ok("Api Trasacao métodos aceitos: POST");
        }
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Post([FromBody] TransacaoDomain transacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); //400 bad request - solicitação inválida
            }
            try
            {
                _logger.LogInformation("Página visitada em {date}, Transacao {transacaoID}", DateTime.UtcNow.ToLongTimeString(), transacao.IdTransacao);

                //Criando cache
                var chaveDoCache = transacao.IdTransacao;
                string dataDoCache = transacao.DataTransacao.ToString();
                if (!_memoryCache.TryGetValue(chaveDoCache, out dataDoCache))
                {
                    var opcoesDoCache = new MemoryCacheEntryOptions()
                    {
                        //Expiração para 30 segundos
                        AbsoluteExpiration = DateTime.Now.AddSeconds(30)
                    };
                    _memoryCache.Set(chaveDoCache, dataDoCache, opcoesDoCache);
                    _transacaoService.Post(transacao);
                }

                _transacaoService.Post(transacao);
                return Ok();

            }
            catch (ArgumentException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); //500 Internal Error -- Erro interno do servidor.
            }
        }
    }
}
