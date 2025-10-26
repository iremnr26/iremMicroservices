namespace CORE.APP.Models.Paging
{
    // Sayfalama (pagination) işlemleri için temel arayüz
    public interface IPageRequest
    {
        // Kaçıncı sayfa (1'den başlar)
        public int PageNumber { get; set; }

        // Her sayfada kaç kayıt gösterilecek
        public int CountPerPage { get; set; }

        // Toplam kayıt sayısı (bilgi amaçlı)
        public int TotalCountForPaging { get; set; }
    }
}