using PetSiteAPI.Models.EFModels;

namespace PetSiteAPI.Models.Dtos.Terry
{
    public class OrderEntity
    {
        
        public int OrderId { get; set; }
        public string? CustomerAccount { get; set; }
        private List<OrderItemEntity> _Items;
        public List<OrderItemEntity> Items 
        {
            get => _Items;
            set => _Items = (value != null || value.Count == 0) ? value : throw new Exception("Items不能是空的");

        }
        public decimal Total { get; set; }

        public string  Receiver { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public DateTime CreateDate { get; set; }
        public string ShipmentMethod { get; set; }
        public string ShipmentStatus { get; set; }
        public string MemberName { get; set; }
        public string Email { get; set; }
        public string MemberMobile { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }


    }
    public static partial class OrderExts
    {
        public static OrderEntity ToOrderEntity(this ProductOrder source)
        {
            if (source == null) return null;
            List<OrderItemEntity> items = source.ProductOrderItems.Select(x => x.ToEntity()).ToList();
            return new OrderEntity
            {
                OrderId = source.ProductOrderId,
                CustomerAccount = source.Member.Account,
                Items = items,
                Total = source.Total,
                Receiver = source.Receiver,
                Address = source.Address,
                Mobile = source.Mobile,
                CreateDate = source.CreateDate,
                ShipmentMethod = source.Shipment.ShipmentMethod,
                ShipmentStatus = source.ShipmentStatus,
                MemberName = source.Member.Name,
                Email = source.Member.Email,
                MemberMobile = source.Member.Mobile,
                PaymentMethod = source.Payment.PaymentMethod,
                PaymentStatus = source.PaymentStatus,
            };
            
        }
    }
}
