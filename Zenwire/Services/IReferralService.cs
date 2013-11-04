using System.Collections.Generic;
using Zenwire.Domain;

namespace Zenwire.Services
{
    public interface IReferralService
    {
        List<Referral> Get();
        Referral Get(int id);
        void Add(Referral referral);
        void Update(Referral referral);
        void Remove(int id);
    }
}