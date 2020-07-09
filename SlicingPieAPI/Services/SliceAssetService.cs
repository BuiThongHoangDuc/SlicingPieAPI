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

        public SliceAssetService(ISliceAssetRepository sliceRepository, IStakeHolderRepository shRepository, ITypeAssetRepo typeRepository)
        {
            _sliceRepository = sliceRepository;
            _shRepository = shRepository;
            _typeRepository = typeRepository;
        }

        public async Task<bool> addSliceSV(String companyID, String userID, SliceAssetDetailDto asset)
        {
            string lastid = await _sliceRepository.getLastIDAsset(companyID);
            string[] splitString = lastid.Split(' ');

            SalaryGapDto salary = _shRepository.GetSalaryGap(userID, companyID).Result;
            double? SalaryGap = salary.ShmarketSalary - salary.Shsalary;


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

                int multiplierInTime = Convert.ToInt32(_typeRepository.GetMultiplierById(Convert.ToInt32(asset.TypeAssetId)).Result);

                asset.MultiplierInTime = multiplierInTime;
                switch (asset.TypeAssetId)
                {
                    case 1:
                        {
                            double ManMonth = asset.Quantity / 160;
                            double Slice = Convert.ToDouble(SalaryGap) / ManMonth * multiplierInTime;
                            asset.AssetSlice = Slice;
                            break;
                        }
                    case 2:
                        {

                            break;
                        }
                };
                //double ManMonth = 

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
