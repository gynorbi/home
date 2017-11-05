using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonThings
{
    public interface IProvider
    {
        Task ReadAllFiles();
        void Clear();
        int Count { get; }
    }
}
