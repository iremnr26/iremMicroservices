namespace CORE.APP.Models;
//Abstract base class for all requests.
//Bu sınıf, tüm istek modelleri (Request DTO) için ortak bir temel sağlar.
//Entity veritabanı nesneleri (tablolar) içindi,
//Request ise API’ye gelen istekler (DTO’lar) içindir.
public abstract class Request
{
    /// Gets or sets the ID of the request.
    /// Defined as virtual to allow overriding in derived classes.
    public virtual int Id { get; set; }
    
    //virtual → “Bu özelliği alt sınıflar isterse yeniden tanımlayabilir” demek.
    //Çünkü derived class’lar (örneğin UserRequest, GroupRequest) isterlerse bu özelliği ezebilir (override).
}