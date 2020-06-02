using AutoMapper;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eventus.BusinessLogic.Interfaces;
using Eventus.DAL.Interfaces;
using Eventus.DAL.Models;

namespace Eventus.BusinessLogic.Services
{
    public class MasterService : IMasterService
    {
        private readonly IRepository<MasterDto> _masterRepository;

        private readonly IMapper _mapper;

        public MasterService(IRepository<MasterDto> masterRepository, IMapper mapper)
        {
            _masterRepository = masterRepository;
            _mapper = mapper;
        }

        public async Task Add(Master master)
        {
            var newMaster = _mapper.Map<MasterDto>(master);
            await _masterRepository.Create(newMaster);
        }

        public async Task Delete(int id)
        {
            var master = await _masterRepository.FindById(id);
            await _masterRepository.Remove(master);
        }

        public async Task Update(Master master)
        {
            var newMaster = _mapper.Map<MasterDto>(master);
            await _masterRepository.Update(newMaster);
        }

        public async Task<Master> FindById(int id)
        {
            return _mapper.Map<Master>(await _masterRepository.FindById(id));
        }

        public async Task<IEnumerable<Master>> GetAll()
        {
            return _mapper.Map<IEnumerable<Master>>(await _masterRepository.Get());
        }

        public async Task<Master> FindByDriverLicenseNumber(string licenseNumber)
        {
            var masters = await _masterRepository.Get();
            return _mapper.Map<Master>(masters.FirstOrDefault(e => e.MasterLicenseNumber.Equals(licenseNumber)));
        }

        public async Task GiveCar(int masterId, int eventId)
        {
            var master = await _masterRepository.FindById(masterId);
            master.EventId = eventId;
            await _masterRepository.Update(master);
        }

        public async Task<bool> UniquenessCheck(Master master)
        {
            var masters = await _masterRepository.Get();
            try
            {
                var resultOfFind = masters.Single(e => e.MasterLicenseNumber.Equals(master.MasterLicenseNumber) || e.DateOfIssueOfDriversLicense.Equals(master.DateOfIssueOfDriversLicense) || e.CallSign == master.CallSign);
                return false;
            }
            catch (ArgumentNullException)
            {
                return true;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }
    }
}