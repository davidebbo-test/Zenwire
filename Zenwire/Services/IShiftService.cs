
using System.Collections.Generic;
using Zenwire.Domain;
namespace Zenwire.Services
{
    public interface IShiftService
    {
        Shift Get(int id);
        List<Shift> Get();
        void Add(Shift shift);
        void Update(Shift shift);
        void Remove(int id);
    }
}