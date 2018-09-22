using Company.Application.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Company.Application.Data.Context
{
    /// <summary>
    /// Bu sınıf bizim DbContext sınıfımız yani database ile ilgili tanımlamaların ve bağlantıların bulunduğu sınıf 
    /// Normal şartlarda DbContext base sınıfından kalıtım almamız gerekiyor ancak projede Identity alt yapısını kullanacağımız için
    /// IdentityDbContext kullanıyoruz, User ve Role sınıflarımızı bu generic base class a tip olarak gönderiyoruz.
    /// Aslında IdentityDbContext i inceleyecek olursanız onunda DbContext base classından kalıtım aldığını görebilirsiniz.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>, ApplicationUserRole, ApplicationUserLogin, IdentityRoleClaim<Guid>, ApplicationUserToken>
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
            //Loglama yapılması için gerekli komut
            //this.EnsureAutoHistory();
        }
        #endregion

        #region ModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Loglamayı aktif hale getiriyoruz
            modelBuilder.EnableAutoHistory(int.MaxValue);
        }
        #endregion

        #region OnConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
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
