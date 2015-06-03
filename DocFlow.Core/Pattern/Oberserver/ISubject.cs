using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocFlow.Core.Pattern.Oberserver
{
    public interface ISubject
    {        
        void addObserver(IObserver observer);        

        void removeObserver(IObserver observer);

        void notifyObservers();        
    }
}
