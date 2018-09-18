using Company.Application.Common.Data;
using System;
using System.Linq;

namespace Company.Application.Common.Repository
{
    /// <summary>
    /// Bu interface bizim için oluşturacağımız GenericRepository'nin neleri içermesi gerektiğini belirtiyor.
    /// GenericRepository sınıfından doğrudan instance almak yerine Dependency Injection kullanarak Bu interface üzerinden instance alacağız
    /// İsimlendirme yaparken I ile başlama bunun bir interface olduğunu bana söylüyor bu şekilde kullanım zorunlu değil ancak yaygın kullanım bu şekilde.
    /// İstersek bu interface in adına isimlendirme kurallarına uymak kaydı ile istediğimizi yazabiliriz.
    /// İsimden sonra gelen <T> ibaresi bu interface in generic olduğunu gösterir. Bu interface i kullanacaksak where ile sınırlandırılmış olan T parametresini yani 
    /// EntityBase den kalıtım almış bir sınıfı buraya vermemiz gerekiyor.
    /// </summary>
    /// <typeparam name="T">where diyerek T nin EntityBase den türemiş bir sınıf olması gerektiği kısıtını koda ekliyoruz</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Geriye içinde linq ile arama yapılabilecek bir queryable döndürür
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Bu method kendisine verilecek Guid valueType'ına sahip Id değişkeni ile sorgulama yapacak ve geriye interface e verilen tipte dönüş yapacak
        /// </summary>
        /// <param name="Id">Aranan kaydın Guid tipinde Id bilgisi</param>
        /// <returns></returns>
        T Find(Guid Id);

        /// <summary>
        /// Kendisine gönderilen tipteki sınıfı veritabanına eklemek için kullanılacak
        /// </summary>
        /// <param name="entity">Hangi sınıf eklenecek ise onun bir örneği verilmeli</param>
        void Add(T entity);

        /// <summary>
        /// Kendisine gönderilen tipteki sınıfı veritabanında güncellemek için kullanılacak
        /// </summary>
        /// <param name="entityToUpdate">Güncellenmesi istenen sınıfın bir örneği gönderilmeli</param>
        void Update(T entityToUpdate);

        /// <summary>
        /// Kendisine gelen Guid ValueType'ına sahip Id bilgisi ile veritabanında silme işlemi yapacak
        /// </summary>
        /// <param name="Id">Silinmesi istenen kaydın Guid tipinde Id bilgisi</param>
        void Delete(Guid Id);

        /// <summary>
        /// Kendisine gelen sınıf ile veritabanında silme işlemi yapacak
        /// </summary>
        /// <param name="entityToDelete">Silinmesi istenen kayda ait sınıfın örneği</param>
        void Delete(T entityToDelete);
    }
}
