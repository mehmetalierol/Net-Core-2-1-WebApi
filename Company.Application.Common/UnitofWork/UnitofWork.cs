using Company.Application.Common.Data;
using Company.Application.Common.Repository;
using Microsoft.EntityFrameworkCore;
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
        private bool _disposed;

        #endregion

        #region Constructor

        /// <summary>
        /// Yapıcı method dependency injection ile DbContext nesnesi türetiyor
        /// </summary>
        /// <param name="context"></param>
        public UnitofWork(DbContext context)
        {
            _context = context;
        }

        #endregion

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
        /// İşlemlerin veritabanına kaydedilmesi için bu method tetikleniyor.
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //Context boş ise hata fırlatıyoruz
                    if (_context == null)
                    {
                        throw new ArgumentException("Context is null");
                    }
                    //Save changes metodundan dönen int result ı yakalayarak geri dönüyoruz.
                    int result =  _context.SaveChanges();

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

        #endregion

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

        #endregion
    }
}

