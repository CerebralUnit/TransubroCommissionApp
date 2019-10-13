using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transubro.CMS.Model;

namespace Transubro.CMS.API
{
    public interface IClientService
    {
        List<Client> GetAllClients();
    }
}
