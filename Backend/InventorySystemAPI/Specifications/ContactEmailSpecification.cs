using InventorySystemAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventorySystemAPI.Specifications
{
    public class ContactEmailSpecification : BaseSpecification<Contact>
    {
        public ContactEmailSpecification(string email) : base(c => c.Email == email )
        {
        }

    }
}
