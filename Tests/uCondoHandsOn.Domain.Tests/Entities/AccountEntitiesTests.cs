

namespace uCondoHandsOn.Domain.Tests.Entities
{
    public class AccountEntitiesTests
    {
        
        [Fact]
        public void Account_ByCode_Correctly()
        {
            var entitie = new List<Account>
            {
                new Account(Code ="6"),
                new Account(Code ="1"),
                new Account(Code ="4"),
                new Account(Code ="5"),
                new Account(Code ="2"),
                new Account(Code ="3"),
            };

            var orderEntities = entitie.OrderBy(x => x).ToList();

            Assert.Equal("1", orderEntitie[0].Code);
            Assert.Equal("2", orderEntitie[1].Code);
            Assert.Equal("3", orderEntitie[2].Code);
            Assert.Equal("4", orderEntitie[3].Code);
            Assert.Equal("5", orderEntitie[4].Code);
            Assert.Equal("6", orderEntitie[5].Code);
        }

        [Fact]
        public void SortAccountsByCode_ShouldReturnInAscendingOrder()
        {
            var entitie = new List<Account>
            {
                new Account
                {
                    Code = "1",
                    Children = new List<Account>
                    {
                        new Account{ Code = "1.1"},
                        new Account{ Code = "1.16"},
                        new Account{ Code = "1.2"},
                        new Account{ Code = "1.3"},
                        new Account{ Code = "1.8"},
                        new Account{ Code = "1.4"},      
                    }

                }
            };

            var orderEntity = entitie.OrderByDescending(x => x).ToList();

            Assert.Equal("6", orderEntity[0].Code);
            Assert.Equal("5", orderEntity[1].Code);
            Assert.Equal("4", orderEntity[2].Code);
            Assert.Equal("3", orderEntity[3].Code);
            Assert.Equal("2", orderEntity[4].Code);
            Assert.Equal("1", orderEntity[5].Code);
        }
}