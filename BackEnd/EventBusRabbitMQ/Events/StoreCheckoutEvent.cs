using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Events
{
    public class StoreCheckoutEvent
    {
        public int StoreId { get; set; }
        public int RoleId { get; set; }
        public int? RoleIdNotModule { get; set; }

    }
    public class StoreprodcuteCheckoutEvent
    {
        public EventBusRabbitMQ.Events.StoreType StoreType { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }
        public int? ZookaarPercentFromSale { get; set; }
        public int? MarkerPercentFromSaleNotagent { get; set; }
        public int? AgentPercentFromSale { get; set; }
        public int? MarkerPercentFromSale { get; set; }
        public string MarketingCodeStore { get; set; }

    }
    public enum StoreType
    {
        Product = 0,
        Service = 1
    }
    public class StoreExpireDateCheckoutEvent
    {
        public int StoreId { get; set; }
        public DateTime StoreExpireDate { get; set; }

    }

    public class CustomerProdcutDeleteCheckoutEvent
    {
        public List<int> DefineProdcute { get; set; }
    }
    public class MigrationProductCheckoutEvent
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string[] ProductIds { get; set; }
    }  
    public class MigrationBarndCheckoutEvent
    {
        public int BrandId { get; set; }
        public string BrandTitle { get; set; }
        public string[] BrandIds { get; set; }
    } 
    public class UpdatePriceCheckoutEvent
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public decimal Number { get; set; }

        public TypePriceAdd TypePrice { get; set; }
    }
      public class DefinedProductStoreCheckoutEvent
    {
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsDeleteByStore { get; set; }
        public int DefinedProductId { get; set; }
        public int CustomerId { get; set; }
        public int? CustomerPirceId { get; set; }
        public int StoreId { get; set; }
        public Guid StoreGuid { get; set; }
        public int? ColorId { get; set; }
        public int Qty { get; set; }
        public string StoreTitle { get; set; }
        /// <summary>
        /// قیمت نقدی نهایی
        /// </summary>
        public long PriceMain { get; set; }

    }

    public enum TypePriceAdd
    {
        Up,
        Down
    }

    public class DeleteListCheckoutEvent
    {
        public int StoreId { get; set; }
        public TypeDelete TypeDelete { get; set; }

        public int? ProductId { get; set; }
        public int? BarndId { get; set; }
    }

    public class CustomerProdcutCountCheckoutEvent
    {
        public int CustomerProductPriceId { get; set; }
        public int Qty { get; set; }
    }
    public enum TypeDelete
    {
        Product,
        Barnd
    }
}
