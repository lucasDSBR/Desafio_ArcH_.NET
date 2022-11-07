using System;

namespace ApiTransacao.Domain
{
    public class TransacaoDomain
    {
        public int IdTransacao { get; set; }
        public DateTime? DataTransacao { get; set; } = DateTime.Now;
        public double Valor { get; set; }
        public int IdContaDestino { get; set; }
        public int IdContaOrigem { get; set; }
        public int Status { get; set; }
    }
}

