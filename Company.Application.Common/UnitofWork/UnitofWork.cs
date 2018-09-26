using Company.Application.Common.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Company.Application.Common.UnitofWork
{
    /// <summary>
    /// Repository ile CRUD işlemleri tek bir noktada topladık ve veritabanına kaydedilmek üzere kuyruğa aldık
    /// UNitofWork ile de bu kuyruğu tek noktadan kaydedeceğiz.
    /// </summary>
    public class UnitofWork : IUnitofWork
    {
        #region Variables

        /// <summary>
        /// Sınıf içerisinde kullanacağımız değişkenler
        /// veritabanı işlemleri için DbContext sınıfı
        /// _disposed isimli bir bool değişken bu değişken ile context in dispose olup olmadığını kontrol edeceğiz.
        /// </summary>
        private readonly DbContext _context;

        private IDbContextTransaction _transation;
        private bool _disposed;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Yapıcı method dependency injection ile DbContext nesnesi türetiyor
        /// </summary>
        /// <param name="context"></param>
        public UnitofWork(DbContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region BusinessSection

        /// <summary>
        /// Gerekli olduğu durumlda istenen entity için reposiyory oluşturarak geri dönüyor
        /// </summary>
        /// <typeparam name="T">Repository si istenen entity</typeparam>
        /// <returns></returns>
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }

        /// <summary>
        /// Yeni bir transaction yaratmak için kullanacağımız metod
        /// Bu metodu sıralı işler yapacaksak işlemlere başlamadan önce çağırmamız gerekir
        /// işlemler bittiğinde ise save changes diyerek transation'ı commit etmiş oluruz.
        /// Ayrıca bir transacitoncommit metodu oluşturmaya gerek duymadım zaten save changes içerisinde önceden oluşturulmuş bir transaction var mı diye kontrol ediyor ve var ise o transaction üzerinden devam ediyoruz.
        /// </summary>
        /// <returns></returns>
        public bool BeginNewTransaction()
        {
            try
            {
                _transation = _context.Database.BeginTransaction();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Herhangi bir nedenden dolayı başlattığımız transation'ı geri alacaksak rollback metodunu çağıracağız.
        /// </summary>
        /// <returns></returns>
        public bool RollBackTransaction()
        {
            try
            {
                _transation.Rollback();
                _transation = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// İşlemlerin veritabanına kaydedilmesi için bu method tetikleniyor.
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            var transaction = _transation != null ? _transation : _context.Database.BeginTransaction();
            using (transaction)
            {
                try
                {
                    //Context boş ise hata fırlatıyoruz
                    if (_context == null)
                    {
                        throw new ArgumentException("Context is null");
                    }
                    //Save changes metodundan dönen int result ı yakalayarak geri dönüyoruz.
                    int result = _context.SaveChanges();

                    //Sorun yok ise kuyruktaki tüm işlemleri commit ederek bitiriyoruz.
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    //Hata ile karşılaşılır ise işlemler geri alınıyor
                    transaction.Rollback();
                    throw new Exception("Error on save changes ", ex);
                }
            }
        }

        #endregion BusinessSection

        #region DisposingSection

        /// <summary>
        /// Context ile işimiz bittiğinde dispose edilmesini sağlıyoruz
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion DisposingSection
    }
}