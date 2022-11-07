using ApiTransacao.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ApiTransacao.Interfaces
{
    public interface ITransacaoService
    {
        Task<TransacaoDomain> Post(TransacaoDomain transacao);
    }
}