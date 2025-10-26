namespace CORE.APP.Models.Ordering
{
    // Sıralama işlemleri için kullanılacak arayüz
    public interface IOrderRequest
    {
        // Hangi sütuna (özelliğe) göre sıralanacağını belirtir
        public string OrderEntityPropertyName { get; set; }

        // Sıralama yönü: true → azalan (DESC), false → artan (ASC)
        public bool IsOrderDescending { get; set; }
    }
}