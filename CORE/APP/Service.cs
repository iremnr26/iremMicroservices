using CORE.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace CORE.APP.Services
{
    /// <summary>
    /// Tüm entity’ler için tekrar eden CRUD (Create/Read/Update/Delete) işlemlerini
    /// tek bir yerde toplayan GENERIC alt yapı (Repository/Service).
    /// ServiceBase’ten aldığı Success/Error gibi yardımcı dönüş metodlarını da kullanır.
    /// </summary>
    /// <typeparam name="TEntity">
    /// Üzerinde çalışılacak entity tipi. CORE.APP.Domain.Entity’den türemeli ve parametresiz ctor’a sahip olmalı.
    /// </typeparam>
    
    /// Burada sadece miras alabilirsin
    public abstract class Service<TEntity> : ServiceBase, IDisposable where TEntity : Entity, new()
    {
        /// <summary>
        /// EF Core üzerinden veritabanı işlemlerini yapacağımız DbContext örneği.
        /// </summary>
        private readonly DbContext _db;

        /// <summary>
        /// DI (Dependency Injection) ile dışarıdan bir DbContext alır.
        /// Protected: Bu sınıf soyut olduğu için doğrudan örneklenmez, sadece kalıtım alınır.
        /// </summary>
        protected Service(DbContext db)
        {
            _db = db;
        }


        /*
         * Senkron vs Asenkron:
         * - Senkron metotlar: İşlem bitmeden bir sonraki adıma geçmez (bloklar).
         * - Asenkron metotlar: I/O (DB) gibi uzun süren işleri beklerken thread'i bloklamaz (async/await).
         *   API handler’ları genelde asenkron tercih edilir.
         */


        // ========== SENKRON REPOSITORY İŞLEMLERİ ==========

        /// <summary>
        /// TEntity set’ini IQueryable olarak döndürür.
        /// isNoTracking=true ise EF değişiklik takibi yapmaz → salt-okuma senaryolarında performans artar.
        /// isNoTracking=false ise update senaryoları için takip açık olur.
        /// </summary>
        protected virtual IQueryable<TEntity> Query(bool isNoTracking = true)
        {
            return isNoTracking ? _db.Set<TEntity>().AsNoTracking() : _db.Set<TEntity>();
        }

        /// <summary>
        /// Senkron değişiklikleri veritabanına yazar. SaveChanges() çağrısı.
        /// Küçük bir sugar: tek satırlık ifade-bodied method kullanımı.
        /// </summary>
        protected virtual int Save() => _db.SaveChanges();

        /// <summary>
        /// Yeni bir entity ekler. İstersen hemen kaydeder (save=true).
        /// Burada Guid alanını otomatik dolduruyoruz (yeni kayıtlar için benzersiz kimlik).
        /// </summary>
        protected void Create(TEntity entity, bool save = true)
        {
            entity.Guid = Guid.NewGuid().ToString();   // ← Entity tablosundaki Guid alanına yeni değer
            _db.Set<TEntity>().Add(entity);            // EF state: Added
            if (save)
                Save();                                // Anında DB’ye yaz
        }

        /// <summary>
        /// Var olan bir entity’yi günceller. İstersen hemen kaydeder (save=true).
        /// </summary>
        protected void Update(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Update(entity);         // EF state: Modified
            if (save)
                Save();
        }

        /// <summary>
        /// Var olan bir entity’yi siler. İstersen hemen kaydeder (save=true).
        /// </summary>
        protected void Delete(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Remove(entity);         // EF state: Deleted
            if (save)
                Save();
        }
        
        // ========== ASENKRON REPOSITORY İŞLEMLERİ ==========

        /// <summary>
        /// Asenkron SaveChanges. Handler’larda (MediatR) genelde asenkron versiyon kullanılır.
        /// </summary>
        protected virtual async Task<int> Save(CancellationToken cancellationToken)
            => await _db.SaveChangesAsync(cancellationToken);

        /// <summary>
        /// Asenkron Create. Kaydı ekler ve save=true ise SaveChangesAsync çağırır.
        /// </summary>
        protected async Task Create(TEntity entity, CancellationToken cancellationToken, bool save = true)
        {   

            entity.Guid = Guid.NewGuid().ToString();   // Yeni kayıt için benzersiz Guid
            _db.Set<TEntity>().Add(entity);
            if (save)
                await Save(cancellationToken);
        }

        /// <summary>
        /// Asenkron Update.
        /// </summary>
        protected async Task Update(TEntity entity, CancellationToken cancellationToken, bool save = true)
        {
            _db.Set<TEntity>().Update(entity);
            if (save)
                await Save(cancellationToken);
        }

        /// <summary>
        /// Asenkron Delete.
        /// </summary>
        protected async Task Delete(TEntity entity, CancellationToken cancellationToken, bool save = true)
        {
            _db.Set<TEntity>().Remove(entity);
            if (save)
                await Save(cancellationToken);
        }
        
        // ========== İLİŞKİLİ VERİ İŞLEMLERİ (JOIN/READ-ONLY) ==========

        /// <summary>
        /// İlişkili başka bir entity tipi (TRelationalEntity) için AsNoTracking bir IQueryable döndürür.
        /// Complex sorgularda (join/left join) LINQ ile birleştirip kullanabilirsin.
        /// </summary>
        public IQueryable<TRelationalEntity> Query<TRelationalEntity>()
            where TRelationalEntity : Entity, new()
        {
            return _db.Set<TRelationalEntity>().AsNoTracking();
        }

        /// <summary>
        /// Verilen ilişkili entity listesini topluca siler.
        /// Not: SaveChanges çağrısı içermez; çağıran taraf Save() / SaveAsync yapmalı.
        /// </summary>
        protected void Delete<TRelationalEntity>(List<TRelationalEntity> relationalEntities)
            where TRelationalEntity : Entity, new()
        {
            _db.Set<TRelationalEntity>().RemoveRange(relationalEntities);
        }

        /// <summary>
        /// Kaynakları serbest bırakır. DbContext’i dispose eder.
        /// DI ile gelen context scope bittiğinde zaten dispose olur; burada garanti altına alıyoruz.
        /// </summary>
        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this); // Finalizer çağrılmasın (performans/pratiklik)
        }
    }
}
