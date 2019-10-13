using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transubro.CMS.Model;

namespace Transubro.CMS.API
{
    public class TestSalespersonService : ISalespersonService
    {
        private List<SalesPerson> mockSales = new List<SalesPerson>()
        {
            new SalesPerson()
            {
                Id = "TS-1",
                Name = "Bobby"
            },
            new SalesPerson()
            {
                Id = "TS-2",
                Name = "David Knight"
            },
            new SalesPerson()
            {
                Id = "TS-3",
                Name = "Joel"
            },
            new SalesPerson()
            {
                Id = "TS-4",
                Name = "Jonathan"
            },
            new SalesPerson()
            {
                Id = "TS-5",
                Name = "Rudd Lowry"
            },
             
            new SalesPerson()
            {
                Id = "TS-6",
                Name = "Transerv"
            },
            new SalesPerson()
            {
                Id = "TS-7",
                Name = "Zack"
            }
        };

        public List<SalesPerson> GetAllSalesPersons()
        {
            return mockSales;
        }
    }
}
