using System.Globalization;
using CORE.APP.Models;

namespace CORE.APP.Services;

/// <summary>
/// Tüm servislerde ortak kullanılan temel sınıf.
/// - Varsayılan kültürü ayarlar (en-US)
/// - Success / Error gibi ortak response metodlarını içerir.
/// </summary>
public abstract class ServiceBase
{
    /// <summary>
    /// Thread seviyesinde kültür bilgisini tutan alan.
    /// Tarih, sayı formatlama, lokalizasyon vb. işlemleri doğrudan etkiler.
    /// </summary>
    private CultureInfo _cultureInfo;

    /// <summary>
    /// Kültür bilgisi set edildiğinde hem CurrentCulture hem CurrentUICulture güncellenir.
    /// Bu sayede servis içinde yapılan tüm işlemler aynı kültüre göre davranır.
    /// </summary>
    protected CultureInfo CultureInfo
    {
        get { return _cultureInfo; }
        set
        {
            _cultureInfo = value;

            // Thread üzerindeki kültür ayarları global olarak değiştirilir.
            Thread.CurrentThread.CurrentCulture = _cultureInfo;
            Thread.CurrentThread.CurrentUICulture = _cultureInfo;
        }
    }

    /// <summary>
    /// Constructor: Varsayılan kültürü en-US olarak ayarlar.
    /// Bu, veritabanı ve API işlemlerinde tarih/sayı format hatalarını engeller.
    /// </summary>
    protected ServiceBase()
    {
        CultureInfo = new CultureInfo("en-US");
    }
    
    /// <summary>
    /// Başarılı bir işlem için standart command response döndürür.
    /// </summary>
    protected CommandResponse Success(string message, int id) =>
        new CommandResponse(true, message, id);

    /// <summary>
    /// Hatalı bir işlem için standart command response döndürür.
    /// </summary>
    protected CommandResponse Error(string message) =>
        new CommandResponse(false, message);
}