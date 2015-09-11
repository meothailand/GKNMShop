using GiaiKhatNgocMai.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GiaiKhatNgocMai.Models;
namespace GiaiKhatNgocMai.Infrastructure.Implementation
{
    public class GKNMDBContext : IGiaiKhatNgocMaiDBContext 
    {
        public static GKNMDBContext Instance { get; private set; }

        static GKNMDBContext()
        {
            Instance = new GKNMDBContext();
        }
        public User GetUser(string email, string password)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                var user = context.Users.Include("Orders").SingleOrDefault(u => u.Email == email && u.Password == password);
                return user;
            }
        }

        public IEnumerable<User> GetUsers(Func<User, bool> predicate)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var users = context.Users.Where(predicate).ToArray();
                return users;
            }
        }

        public int CreateUser(User user)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Users.Add(user);
                context.SaveChanges();
                return user.Id;
            }
        }

        public void UpdateUser(User user)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var dbUser = context.Users.SingleOrDefault(u => u.Id == user.Id);
                if (dbUser == null) throw new NullReferenceException("Can not found user " + user.UserName);
                context.Entry(user).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var dbUser = context.Users.SingleOrDefault(u => u.Id == id);
                context.Users.Remove(dbUser);
                context.SaveChanges();
            }
        }

        public Customer GetCustomer(string email, string password)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var dbcus = context.Customers.SingleOrDefault(i => i.CustomerEmail == email && i.CustomerPassword == password);
                return dbcus;
            }
        }

        public Customer GetCustomer(Func<Customer, bool> predicate)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var dbcus = context.Customers.Include("Orders").Where(predicate).FirstOrDefault();
                return dbcus;
            }
        }
        public IEnumerable<Customer> GetCustomers(Func<Customer,bool> predicate)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var dbcus = context.Customers.Where(predicate).ToArray();
                return dbcus;
            }
        }

        public int CreateCustomer(Customer customer)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return customer.Id;
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var dbcustomer = context.Customers.SingleOrDefault(i => i.Id == customer.Id);
                if (dbcustomer == null) throw new NullReferenceException();
                dbcustomer.RecieveInfo = customer.RecieveInfo;
                dbcustomer.CustomerName = customer.CustomerName;
                dbcustomer.CustomerPassword = customer.CustomerPassword;
                dbcustomer.CustomerPoint = customer.CustomerPoint;
                dbcustomer.CustomerPhone = customer.CustomerPhone;
                dbcustomer.ShipAddress = customer.ShipAddress;
                dbcustomer.ShipDistrict = customer.ShipDistrict;
                context.Entry(dbcustomer).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCustomer(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Customers.Remove(context.Customers.SingleOrDefault(c => c.Id == id));
                context.SaveChanges();
            }
        }

        public Category GetCategory(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var dbcate = context.Categories.Include("Category1").Include("Items").SingleOrDefault(i => i.Id == id);
                return dbcate;
            }
        }

        public IEnumerable<Category> GetCategories(Func<Category, bool> predicate, bool passbyItem = true)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var dbcate = passbyItem ? context.Categories.Include("Category1").Where(predicate).ToArray() :
                                          context.Categories.Include("Category1").Include("Items").Where(predicate).ToArray();
                return dbcate;
            }
        }

        public int CreateCategory(Category category)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return category.Id;
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Categories.Add(category);
                context.Entry(category).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteCategory(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Categories.Remove(context.Categories.SingleOrDefault(c => c.Id == id));
                context.SaveChanges();
            }
        }

        public Item GetItem(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var item = context.Items.Include("Category").Include("ItemPhotoes").SingleOrDefault(i => i.Id == id);
                return item;
            }
        }

        public IEnumerable<Item> GetItems(Func<Item, bool> predicate)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var items = context.Items.Include("Category").Where(predicate).ToArray();
                return items;
            }
        }

        public int CreateItem(Item item)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Items.Add(item);
                context.SaveChanges();
                return item.Id;
            }
        }

        public void BatchCreateItems(IEnumerable<Item> items)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Items.AddRange(items);
                context.SaveChanges();
            }
        }

        public void UpdateItem(Item item)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.Items.Add(item);
                context.SaveChanges();
            }
        }

        public void BatchUpdateItem(IEnumerable<Item> items)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                items.Select(i => context.Entry(i).State = System.Data.Entity.EntityState.Modified);
                context.Items.AddRange(items);
                context.SaveChanges();
            }
        }

        public void DeleteItem(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Items.Remove(context.Items.SingleOrDefault(i => i.Id == id));
                context.ItemPhotoes.RemoveRange(context.ItemPhotoes.Where(i => i.ItemId == id));
                context.SaveChanges();
            }
        }

        public void UploadPhotoes(IEnumerable<ItemPhoto> photoes)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.ItemPhotoes.AddRange(photoes);
                context.SaveChanges();
            }
        }
        public void DeletePhotoes(IEnumerable<int> ids)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                foreach (var i in ids)
                {
                    if (context.ItemPhotoes.SingleOrDefault(p => p.Id == i) != null)
                    {
                        context.ItemPhotoes.Remove(context.ItemPhotoes.SingleOrDefault(p => p.Id == i));
                    }
                }
            }
        }

        public Order GetOrder(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var order = context.Orders.Include("OrderDetails").Include("User").Include("Customer").SingleOrDefault(i => i.Id == id);
                return order;
            }
        }

        public string CreateOrder(Order order)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                if (order.OrderDetails.Count <= 0) throw new Exception();
                context.Orders.Add(order);
                context.SaveChanges();
                context.Entry(order).Reload();
                return order.OrderNumber;
            }
        }

        public IEnumerable<Order> GetOrders(Func<Order, bool> predicate)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var orders = context.Orders.Where(predicate).ToArray();
                return orders;
            }
        }

        public void UpdateOrder(Order order)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {                
                var dbItemList = context.OrderDetails.Where(i => i.OrderId == order.Id).OrderBy(i => i.Id);
                var newItemList = order.OrderDetails.OrderBy(i => i.Id).ToList();
                foreach (var dt in dbItemList)
                {
                    var existedItem = newItemList.SingleOrDefault(i => i.Id == dt.Id);
                    if (existedItem == null)
                    {
                        context.Entry(dt).State = System.Data.Entity.EntityState.Deleted;
                        continue;
                    }                        

                    context.Entry(dt).State = System.Data.Entity.EntityState.Modified;
                    newItemList.Remove(existedItem);
                }

                newItemList.Select(i => context.Entry(i).State = System.Data.Entity.EntityState.Added);

                context.Entry(order).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteOrder(int orderId)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var order = context.Orders.SingleOrDefault(i => i.Id == orderId);
                context.Orders.Remove(order);
                context.SaveChanges();
            }
        }

        public IEnumerable<Unit> GetUnits()
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var units = context.Units.ToArray();
                return units;
            }
        }

        public void BatchUpdateUnit(IEnumerable<Unit> units)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var newUnits = units.OrderBy(i => i.Id).ToList();
                var dbUnits = context.Units.OrderBy(i => i.Id);
                foreach (var u in dbUnits)
                {
                    var existedUnit = newUnits.SingleOrDefault(i => i.Id == u.Id);
                    if (existedUnit == null)
                    {
                        context.Entry(u).State = System.Data.Entity.EntityState.Deleted;
                        continue;
                    }

                    context.Entry(u).State = System.Data.Entity.EntityState.Modified;
                    newUnits.Remove(existedUnit);
                }
                newUnits.Select(i => context.Entry(i).State = System.Data.Entity.EntityState.Added);
                context.SaveChanges();
            }
        }

        public IEnumerable<ShipmentFee> GetShipmentFees()
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var feeList = context.ShipmentFees.ToArray();
                return feeList;
            }
        }

        public void BatchUpdateShipmentFee(IEnumerable<ShipmentFee> fee)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var newFeeList = fee.OrderBy(i => i.Id).ToList();
                var dbFeeList = context.ShipmentFees.OrderBy(i => i.Id);
                foreach (var item in dbFeeList)
                {
                    var existedFee = newFeeList.SingleOrDefault(i => i.Id == item.Id);
                    if (existedFee == null)
                    {
                        context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        continue;
                    }

                    context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    newFeeList.Remove(existedFee);
                }
                newFeeList.Select(i => context.Entry(i).State = System.Data.Entity.EntityState.Added);
                context.SaveChanges();
            }
        }

        public News GetNews(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var news = context.News.Include("User").SingleOrDefault(i => i.Id == id);
                return news;
            }
        }

        public IEnumerable<News> GetConditionalNews(Func<News, bool> predicate)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var newsList = context.News.Where(predicate).ToArray();
                return newsList;
            }
        }

        public int CreateNews(News news)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.News.Add(news);
                context.SaveChanges();
                return news.Id;
            }
        }

        public void UpdateNews(News news)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Entry(news).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteNews(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var news = context.News.SingleOrDefault(i => i.Id == id);
                context.News.Remove(news);
                context.SaveChanges();
            }
        }

        public SiteNew GetPolicyNews(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var policy = context.SiteNews.SingleOrDefault(i => i.Id == id);
                return policy;
            }
        }

        public IEnumerable<SiteNew> GetAllPolicyNews()
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var policies = context.SiteNews.ToArray();
                return policies;
            }
        }

        public void UpdatePolicyNews(SiteNew news)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                news.DateUpdated = DateTime.Now;
                context.Entry(news).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }


        public Banner GetBanner(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var banner = context.Banners.SingleOrDefault(i => i.Id == id);
                return banner;
            }
        }

        public IEnumerable<Banner> GetBanners(Func<Banner, bool> predicate)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var banners = context.Banners.Where(predicate).ToArray();
                return banners;
            }
        }


        public int CreateBanner(Banner banner)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                context.Banners.Add(banner);
                context.SaveChanges();
                return banner.Id;
            }
        }

        public void DeleteBanner(int id)
        {
            using (var context = new GiaikhatNgocMaiEntities())
            {
                var banner = context.Banners.SingleOrDefault(i => i.Id == id);
                if (banner != null)
                {
                    context.Banners.Remove(banner);
                    context.SaveChanges();
                }                
            }
        }
    }
}