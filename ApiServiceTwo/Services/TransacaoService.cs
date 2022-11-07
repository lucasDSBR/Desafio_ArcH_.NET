using ApiTransacao.Domain;
using ApiTransacao.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
namespace ApiTransacao.Services
{
    public class TransacaoService : ITransacaoService
    {
        //private IRepository<ReservationEntity> _repository;
        //public TransacaoService(/*IRepository<ReservationEntity> repository*/)
        //{
        //    _repository = repository;
        //}
        public async Task<TransacaoDomain> Post(TransacaoDomain transacao)
        {
            var client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false");
            var database = client.GetDatabase("admin");
            var collection = database.GetCollection<BsonDocument>("transacoes");
            var doc = new BsonDocument
            {
                {"IdTransacao", transacao.IdTransacao},
                {"DataTransacao", transacao.DataTransacao},
                {"Valor", transacao.Valor},
                {"IdContaDestino", transacao.IdContaDestino },
                {"IdContaOrigem", transacao.IdContaOrigem },
                {"Status", transacao.Status }
            };

            collection.InsertOne(doc);
            RecalcSaldo();
            return new TransacaoDomain() { }; //await _repository.InsertAsync(reservation);
        }
        public void RecalcSaldo()
        {
            var client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&directConnection=true&ssl=false");
            var database = client.GetDatabase("admin");
            var collectionTransacoes = database.GetCollection<BsonDocument>("transacoes");
            var collectionSaldos = database.GetCollection<BsonDocument>("saldo");

        }
    }
}
    