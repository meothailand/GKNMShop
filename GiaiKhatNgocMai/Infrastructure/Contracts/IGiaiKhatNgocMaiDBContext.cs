using GiaiKhatNgocMai.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaiKhatNgocMai.Infrastructure.Interfaces
{
    public interface IGiaiKhatNgocMaiDBContext
    {        
        //User & Customer account
        User GetUser(string userEmail, string password);
        IEnumerable<User> GetUsers(Func<User, bool> predicate);
        int CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        Customer GetCustomer(string email, string password);
        Customer GetCustomer(Func<Customer, bool> predicate);
        IEnumerable<Customer> GetCustomers(Func<Customer, bool> predicate);
        int CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        //About Product
        Category GetCategory(int id);
        IEnumerable<Category> GetCategories(Func<Category, bool> predicate, bool passbyItem= true);
        int CreateCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
        Item GetItem(int id);
        IEnumerable<Item> GetItems(Func<Item, bool> predicate);
        int CreateItem(Item item);
        void BatchCreateItems(IEnumerable<Item> items);
        void UpdateItem(Item item);
        void BatchUpdateItem(IEnumerable<Item> items);
        void DeleteItem(int id);
        void UploadPhotoes(IEnumerable<ItemPhoto> photoes);
        void DeletePhotoes(IEnumerable<int> ids);
        //About Order
        Order GetOrder(int id);
        IEnumerable<Order> GetOrders(Func<Order, bool> predicate);
        string CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int order);
        //About News and System Settings
        IEnumerable<Unit> GetUnits();
        IEnumerable<ShipmentFee> GetShipmentFees();
        void BatchUpdateShipmentFee(IEnumerable<ShipmentFee> fee);
        News GetNews(int id);
        IEnumerable<News> GetConditionalNews(Func<News, bool> predicate);
        int CreateNews(News news);
        void UpdateNews(News news);
        void DeleteNews(int id);
        SiteNew GetPolicyNews(int id);
        IEnumerable<SiteNew> GetAllPolicyNews();
        void UpdatePolicyNews(SiteNew news);
        int CreateBanner(Banner banner);
        Banner GetBanner(int id);
        IEnumerable<Banner> GetBanners(Func<Banner, bool> predicate);
        void DeleteBanner(int id);
    }
}
