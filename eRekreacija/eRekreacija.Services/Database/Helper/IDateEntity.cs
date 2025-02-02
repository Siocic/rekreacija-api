using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRekreacija.Services.Database.Helper
{
    public interface IDateEntity
    {
        DateTime? created_date { get; set; }
        DateTime? updated_date { get; set; }
    }
}
