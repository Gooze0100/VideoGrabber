using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoGrabberMVVMGUI.DI
{
    public class BusinessService : IBusinessService
    {
        private readonly IRepository _repository;

        public BusinessService(IRepository repository)
        {
            _repository = repository;
        }
    }
}
