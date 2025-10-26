namespace CORE.APP.Domain;
//Ortak özelliklerin tanımlandığı temel sınıf
// Miras alınır (User : Entity)
//Onun doğrudan bir nesnesini oluşturamazsın (new Entity() diyemezsin).
// Ama diğer tüm varlık (entity) sınıfları bundan miras alır (inherit).
public abstract class  Entity
{
 //Bu property, veritabanındaki birincil anahtarı (primary key) temsil eder.
 // EF Core bunu otomatik olarak algılar ve tablo oluştururken Id sütununu PRIMARY KEY yapar.
    public int Id { get; set; }
    
//Bu, her kaydın benzersiz bir kimliği olarak kullanılan evrensel benzersiz tanımlayıcı (UUID).
//oluştururken user.Guid = Guid.NewGuid().ToString();
    public string Guid { get; set; }
}