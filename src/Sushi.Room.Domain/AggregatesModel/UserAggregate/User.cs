using Sushi.Room.Domain.AggregatesModel.ProductAggregate;
using Sushi.Room.Domain.SeedWork;
using System.Collections.Generic;

namespace Sushi.Room.Domain.AggregatesModel.UserAggregate
{
    public class User : Entity, IAggregateRoot
    {
        public User()
        {

        }

        private User(string userName, string password, string firstName, string lastName)
        {
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public bool IsActive { get; private set; }

        public virtual ICollection<ProductPriceChangeHistory> ProductPriceChangeHistories { get; private set; }
        public virtual ICollection<Product> Products { get; private set; }

        public void UpdateMetaData(string userName, string password, string firstName, string lastName)
        {
            UserName = userName;
            Password ??= password;
            FirstName = firstName;
            LastName = lastName;
        }

        public static User CreateNew(string userName, string password, string firstName, string lastName, bool isActive)
        {
            var user = new User(userName, password, firstName, lastName);

            if (isActive)
            {
                user.MarkAsActive();
            }
            else
            {
                user.MarkAsNotActive();
            }

            return user;
        }

        public void MarkAsActive()
        {
            IsActive = true;
        }

        public void MarkAsNotActive()
        {
            IsActive = false;
        }
    }
}
