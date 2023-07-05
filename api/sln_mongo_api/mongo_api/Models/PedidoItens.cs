﻿namespace mongo_api.Models
{

    public class PedidoItensMongo:BaseMongo
    {


        public int Qtd { get; set; }
        public string ProdutoId { get; set; }
        public  ProdutoMongo Produto { get; set; }
        public decimal Price { get; set; }
        public string PedidoId { get; set; }
        public  PedidoMongo Pedido { get; set; }
    }
    public class PedidoItens:Base
    {

        public int Qtd { get; set; }
        public virtual Guid ProdutoId { get; set; }
        public virtual  Produto Produto { get; set; }
        public  decimal Price { get; set; }
        public virtual Guid PedidoId { get; set;}
        public virtual Pedido Pedido { get; set; }

    }
}
