using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpCommunityToolkitMVVM.Models
{
    public class ProductChangedMessage
    {
        private readonly string _message;

        public ProductChangedMessage(string message)
        {
            _message = message;
        }
        public string ChagneMessage => _message;
    }
}
