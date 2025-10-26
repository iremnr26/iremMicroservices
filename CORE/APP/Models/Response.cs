namespace CORE.APP.Models;

//Bu sınıf tüm API yanıtlarının ortak özelliklerini tanımlar.
// Her CRUD (Create, Read, Update, Delete) işlemi sonucunda:
// Id → veritabanındaki kaydın ID’si,
// Guid → benzersiz kimliği (string UUID)
// olarak döndürülür.
//Diğer response sınıfları bundan **miras alır** (örneğin `UserResponse`, `GroupResponse`).
public abstract class Response
{
    /// Gets or sets the integer unique identifier of the response.
    /// Typically used to correlate responses with database entities.
    /// Defined as virtual to allow overriding in derived classes.
    public virtual int Id { get; set; }
    
    /// Gets or sets the string unique identifier of the response.
    /// Defined as virtual to allow overriding in derived classes.
    /// Dış sistemlerle haberleşmede veya güvenli URL’lerde kullanılabilir.
    /// `virtual` → alt sınıflar isterse değiştirilebilir.
    public virtual string Guid { get; set; }
    
    /// Constructor with parameter to set the Id from a sub (child) class
    /// constructor using Constructor Chaining.
    /// <param name="id">The integer unique identifier parameter.</param>
    protected Response(int id)
    {
        Id = id;
    }
    /*Burada base(id) → Response(int id) constructor’ına değer gönderiyor.Böylece Id otomatik atanıyor.
     public class GroupResponse : Response
       {
           public string Title { get; set; }
       
           public GroupResponse(int id, string title) : base(id)
           {
               Title = title;
           }
       }*/
    
    /// Default constructor (constructor without any parameters)
    /// that will set the Id to the integer default value (0).
    /// protected olduğu için sadece miras alan sınıflar erişebilir.
    protected Response(){}
    
}