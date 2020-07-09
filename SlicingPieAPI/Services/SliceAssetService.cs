using SlicingPieAPI.DTOs;
using SlicingPieAPI.Models;
using SlicingPieAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SlicingPieAPI.Services
{
    public class SliceAssetService : ISliceAssetService
    {
        private readonly ISliceAssetRepository _sliceRepository;
        private readonly IStakeHolderRepository _shRepository;
        private readonly ITypeAssetRepo _typeRepository;
        private readonly ICompanyRepository _cpRepository;

        public SliceAssetService(ISliceAssetRepository sliceRepository, IStakeHolderRepository shRepository, ITypeAssetRepo typeRepository, ICompanyRepository cpRepository)
        {
            _sliceRepository = sliceRepository;
            _shRepository = shRepository;
            _typeRepository = typeRepository;
            _cpRepository = cpRepository;
        }

        public async Task<bool> addSliceSV(String companyID, String userID, SliceAssetDetailDto asset)
        {
            string lastid = await _sliceRepository.getLastIDAsset(companyID);
            string[] splitString = lastid.Split(' ');

            SalaryGapDto salary = await _shRepository.GetSalaryGap(userID, companyID);
            double? SalaryGap = salary.ShmarketSalary - salary.Shsalary;

            double cashPerSlice = await _cpRepository.GetMoneyPerSlice(companyID);

            if (lastid == null || splitString.Length == 1)
            {
                asset.AssetId = companyID + " Asset1";
            }
            else
            {
                Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
                Match result = re.Match(splitString[1]);

                string alphaPart = result.Groups[1].Value;
                string numberPart = result.Groups[2].Value;

                int numberProject = Int32.Parse(numberPart) + 1;

                asset.AssetId = splitString[0] + " " + alphaPart + numberProject;

                String multiplier = await _typeRepository.GetMultiplierById(Convert.ToInt32(asset.TypeAssetId));
                int multiplierInTime;
                if (multiplier == "NC") multiplierInTime = await _cpRepository.GetNonCashMP(companyID);
                else multiplierInTime = await _cpRepository.GetCashMP(companyID);

                asset.MultiplierInTime = multiplierInTime;
                switch (asset.TypeAssetId)
                {
                    case 1:
                        {
                            double ManMonth = asset.Quantity / 160;
                            double Slice = ManMonth * Convert.ToDouble(SalaryGap) / cashPerSlice * multiplierInTime;
                            asset.AssetSlice = Slice;
                            break;
                        }
                    case 2:
                        {
                            double Slice = asset.Quantity / cashPerSlice * multiplierInTime;
                            asset.AssetSlice = Slice;
                            break;
                        }
                    case 3:
                        {
                            double Slice = asset.Quantity / 500000 * multiplierInTime;
                            asset.AssetSlice = Slice;
                            break;
                        }
                    case 4:
                        {
                            double Slice = asset.Quantity / 500000;
                            asset.AssetSlice = Slice;
                            break;
                        }
                };

            }

            try
            {
                await _sliceRepository.addSlice(asset);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task<IEnumerable<TypeAssetCompany>> getType(string companyid, int typeid)
        {
            return _sliceRepository.getType(companyid, typeid);
        }
    }
    public interface ISliceAssetService
    {
        Task<bool> addSliceSV(String companyID, String userID, SliceAssetDetailDto asset);
        Task<IEnumerable<TypeAssetCompany>> getType(String companyid, int typeid);
    }
}
