using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace curs
{
    public interface ISaveList<T>
    {
        void Save(T data, string path);
        T Load(string path);
    }
}
