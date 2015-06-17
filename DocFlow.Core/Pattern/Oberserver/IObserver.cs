using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFlow.Core.Pattern.Oberserver
{
    public interface IObserver
    {
        void Update(ISubject subject, object args);
    }
}
