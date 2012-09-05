//  
//  TestIdentity.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2012 CSF Software Limited
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using CSF.Entities;
using NUnit.Framework;

namespace Test.CSF.Entities
{
  [TestFixture]
  public class TestIdentity
  {
    #region tests
    
    [Test]
    public void TestEquals()
    {
      string stringTest = "foo bar";
      uint numericTest = 3; 
      Identity<uint> three = new Identity<uint>(typeof(Person), 3);
      Identity<uint> four = new Identity<uint>(typeof(Person), 4);
      Identity<uint> threeAgain = new Identity<uint>(typeof(Person), 3);
      Identity<uint> threeProduct = new Identity<uint>(typeof(Product), 3);
      
      Assert.IsFalse(three.Equals(stringTest), "Identity does not equal a string");
      Assert.IsFalse(three.Equals(numericTest), "Identity does not equal a uint");
      Assert.IsFalse(three.Equals(four), "Non-matching identities not equal");
      
      Assert.IsTrue(three.Equals(three), "Copies of the same object are equal");
      Assert.IsTrue(three.Equals(threeAgain), "Identical instances are equal");
      
      Assert.IsFalse(three.Equals(threeProduct), "Non-matching types not equal");
    }
    
    [Test]
    public void TestToString()
    {
      Identity<uint> three = new Identity<uint>(typeof(Person), 3);
      Assert.AreEqual(String.Format("[{0}: {1}]", typeof(Person).FullName, 3),
                      three.ToString(),
                      "Correct string representation");
    }
    
    [Test]
    public void TestOperatorEquality()
    {
      Identity<uint> three = new Identity<uint>(typeof(Person), 3);
      Identity<uint> four = new Identity<uint>(typeof(Person), 4);
      Identity<uint> threeAgain = new Identity<uint>(typeof(Person), 3);
      Identity<uint> threeProduct = new Identity<uint>(typeof(Product), 3);
      
      Assert.IsFalse(three == four, "Non-matching identities not equal");
#pragma warning disable 1718
      // Disabling CS1718 - the point of this test is to compare the object to itself
      Assert.IsTrue(three == three, "Copies of the same object are equal");
#pragma warning restore 1718
      Assert.IsTrue(three == threeAgain, "Identical instances are equal");
      Assert.IsFalse(three == threeProduct, "Non-matching types not equal");
    }
    
    [Test]
    public void TestOperatorInequality()
    {
      Identity<uint> three = new Identity<uint>(typeof(Person), 3);
      Identity<uint> four = new Identity<uint>(typeof(Person), 4);
      Identity<uint> threeAgain = new Identity<uint>(typeof(Person), 3);
      Identity<uint> threeProduct = new Identity<uint>(typeof(Product), 3);
      
      Assert.IsTrue(three != four, "Non-matching identities not equal");
#pragma warning disable 1718
      // Disabling CS1718 - the point of this test is to compare the object to itself
      Assert.IsFalse(three != three, "Copies of the same object are equal");
#pragma warning restore 1718
      Assert.IsFalse(three != threeAgain, "Identical instances are equal");
      Assert.IsTrue(three != threeProduct, "Non-matching types not equal");
    }
    
    [Test]
    public void TestCreate()
    {
      IIdentity identity = Identity.Create<Person>(5);
      Assert.AreEqual(typeof(Person), identity.Type, "Correct type");
      Assert.AreEqual(5, identity.Value, "Correct value");
    }
    
    [Test]
    public void TestCreateSubclass()
    {
      IIdentity identity = Identity.Create<Employee>(5);
      Assert.AreEqual(typeof(Employee), identity.Type, "Correct type");
      Assert.AreEqual(5, identity.Value, "Correct value");
    }
    
    #endregion
    
    #region mocks
    
    public class Person : Entity<uint> {}
    
    public class Employee : Person {}
    
    public class Product : Entity<uint> {}
    
    #endregion
  }
}
