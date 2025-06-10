using System.Collections.Generic;
using System.Linq;

namespace MyAspNetCoreApp.Models
{
    public class MockProductService
    {
        public List<Product> GetFeaturedProducts()
        {
            return GetAllProducts().Where(p => p.IsFeatured).ToList();
        }

        public List<Product> GetNewProducts()
        {
            return GetAllProducts().Where(p => p.IsNew).ToList();
        }

        private List<Product> GetAllProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "ĐẠI HỌC CÔNG NGHỆ TP.HCM", ImageUrl = "/image/HUTECH2.jpg", Price = 25000000, IsFeatured = true, IsNew = false, Description = "Trường đại học công nghệ hàng đầu", CreatedDate = DateTime.Now },
                new Product { Id = 2, Name = "ĐẠI HỌC XÃ HỘI VÀ NHÂN VĂN", ImageUrl = "/image/nhanvan.png", Price = 20000000, IsFeatured = true, IsNew = true, Description = "Trường đại học chuyên về khoa học xã hội", CreatedDate = DateTime.Now },
                new Product { Id = 3, Name = "ĐẠI HỌC BÁCH KHOA TP.HCM", ImageUrl = "/image/bachkhoa.jpg", Price = 30000000, IsFeatured = false, IsNew = true, Description = "Trường đại học kỹ thuật hàng đầu", CreatedDate = DateTime.Now },
            };
        }
    }
}
