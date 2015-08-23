using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiaiKhatNgocMai.Infrastructure.Utils;

namespace GiaiKhatNgocMaiUnitTest
{
    [TestClass]
    public class ModelObjectHelperTest
    {

        [TestMethod]
        public void NormalCopyModel()
        {
            var source = new SourceObject() { Id = 1, Name = "Fruit juice", PostedDate = DateTime.Now, Price = 18000 };
            var dest = new DestObject();
            ModelObjectHelper.CopyObject(source, dest);
            Assert.AreEqual(source.Id, dest.Id);
            Assert.AreEqual(source.PostedDate, dest.PostedDate);
            Assert.AreEqual(source.Name, dest.Name);
            Assert.AreEqual(source.Price, dest.Price);
        }
        [TestMethod]
        public void Different_Properties_In_Destination_Should_Have_Default_Value()
        {
            var source = new SourceObject() { Id = 1, Name = "Fruit juice", PostedDate = DateTime.Now, Price = 18000 };
            var dest = new PartialyDestObject();
            ModelObjectHelper.CopyObject(source, dest);
            Assert.AreEqual(source.Id, dest.Id);
            Assert.AreEqual(source.PostedDate, dest.PostedDate);
            Assert.AreEqual(source.Name, dest.Name);
            Assert.IsNull(dest.dest);
        }
        [TestMethod]
        public void Same_Name_Different_Type_And_Vice_Versa_Shouldnot_Be_Copied()
        {
            var source = new SourceObject() { Id = 1, Name = "Fruit juice", PostedDate = DateTime.Now, Price = 18000 };
            var dest = new FakeDestObject();
            ModelObjectHelper.CopyObject(source, dest);
            Assert.AreNotEqual(source.Id, dest.Id);
            Assert.IsInstanceOfType(dest.Id, typeof(decimal));
            Assert.AreEqual(source.PostedDate, dest.PostedDate);
            Assert.AreNotEqual(source.Name, dest.MyName);
        }

        [TestMethod]
        public void Public_Field_With_Private_Set_Shoud_Be_Copied()
        {
            var source = new SourceObject() { Id = 1, Name = "Fruit juice", PostedDate = DateTime.Now, Price = 18000 };
            var name = "this will be override";
            var dest = new HasPrivateDestObject(name);
            //before copy
            Assert.AreNotEqual(source.Name, dest.Name);
            Assert.AreEqual(name, dest.Name);
            ModelObjectHelper.CopyObject(source, dest);
            //after copy
            Assert.AreEqual(source.Id, dest.Id);
            Assert.AreEqual(source.PostedDate, dest.PostedDate);
            Assert.AreEqual(source.Name, dest.Name);
        }
    }

    public class SourceObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PostedDate { get; set; }
        public decimal Price { get; set; }

        public void GetTotal(int quantity)
        {
            this.Price = this.Price * quantity;
        }
    }

    public class DestObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PostedDate { get; set; }
        public decimal Price { get; set; }
    }

    public class PartialyDestObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime PostedDate { get; set; }
        public DestObject dest { get; set; }
    }

    public class FakeDestObject
    {
        public decimal Id { get; set; }
        public string MyName { get; set; }
        public DateTime PostedDate { get; set; }
    }

    public class HasPrivateDestObject
    {
        public HasPrivateDestObject(string name)
        {
            this.Name = name;
        }
        public int Id { get; set; }

        public string Name {get; private set;}

        //public string NameSetter {
        //    get { return this.Name; }
        //    set { this.Name = value;  }

        //}

        public string GetName() { return this.Name; }
        public DateTime PostedDate { get; set; }
        public decimal Price { get; set; }

        public void GetTotal(int quantity)
        {
            this.Price = this.Price * quantity;
        }
    }

}
