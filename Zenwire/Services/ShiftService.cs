﻿using System.Linq;
using System.Collections.Generic;
using Zenwire.Domain;
using Zenwire.Repositories;

namespace Zenwire.Services
{
    public class ShiftService : IShiftService
    {
        private readonly IRepository<Shift> _shiftRepository;

        public ShiftService(IRepository<Shift> shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        public Shift Get(int id)
        {
            return _shiftRepository.Find(id);
        }

        public List<Shift> Get()
        {
            return _shiftRepository.Get.ToList();
        }

        public void Add(Shift shift)
        {
            
            _shiftRepository.Add(shift);
        }

        public void Update(Shift shift)
        {
            _shiftRepository.Update(shift);
        }

        public void Remove(int id)
        {
            Shift shift = Get(id);
            _shiftRepository.Remove(shift);
        }
    }
}