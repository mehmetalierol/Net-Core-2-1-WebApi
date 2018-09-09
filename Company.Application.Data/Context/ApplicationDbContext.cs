using Company.Application.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Company.Application.Data.Context
{
    /// <summary>
    /// Bu sınıf bizim DbContext sınıfımız yani database ile ilgili tanımlamaların ve bağlantıların bulunduğu sınıf 
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Constructor
        /// <summary>
        /// Constructor yani yapıcı method bizim için bu sınıf türetildiğinde devreye ilk girecek olan kısımdır.
        /// Constructor DbContextOptions isminde bir sınıf türetilmesini sağlıyor ve bu sınıfı kalıtım aldığı IdentityDbContext sınıfına parametre olarak gönderiyor.
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        #endregion

        #region DbSets
        /// <summary>
        /// Projemiz içerisinde kullanacağımız tüm entity sınıflarımızın bu kısımda DbSet ile tanımlıyoruz
        /// Bu sayede migration yaparken bu sınıflar veritabanında birer tablo olarak oluşturulacak. ve yapacağımız CRUD işlemler veritabanına yansıyacak.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<AppResource> AppRecource { get; set; }
        #endregion
    }
}
