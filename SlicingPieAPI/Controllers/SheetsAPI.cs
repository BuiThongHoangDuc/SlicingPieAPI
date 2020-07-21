using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.EntityFrameworkCore;
using SlicingPieAPI.DTOs;
using SlicingPieAPI.Enums;
using SlicingPieAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SlicingPieAPI.Controllers
{
    public class SheetsAPI
    {
        static readonly String[] Scopes = { SheetsService.Scope.Spreadsheets };

        static readonly String ApplicationName = "SlicingPie";

        static readonly String SpreadSheetID = "1zp82P1kPLHTUzMo6LCF5YO9768CdSGPIOC9mbTIVKXM";

        static SheetsService service;

        private readonly SWDSlicingPieContext _context;


        public SheetsAPI(SWDSlicingPieContext context)
        {
            _context = context;
            GoogleCredential credential;
            using (var stream = new FileStream("SlicingPieProject-9b006e535ee5.json", FileMode.OpenOrCreate, FileAccess.Read)) {
                credential = GoogleCredential.FromStream(stream)
                                    .CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        public void CreateEntry(String Sheet,String shID,String shName, double shSlice) {
            var range = $"{Sheet}!A:C";
            var valueRange = new ValueRange();

            var objectList = new List<object>() { shID, shName, shSlice };
            valueRange.Values = new List<IList<object>> { objectList };

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadSheetID, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendRespone = appendRequest.Execute();
        }

        public async Task<IEnumerable<SheetDto>> UpdateEntry(String Sheet, String companyId) {
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            var ListMainUserInfo = await _context.StakeHolders
                                                    .Where(stInfo => stInfo.CompanyId == companyId && stInfo.Shstatus == Status.ACTIVE)
                                                    .OrderBy(sh => sh.DateTimeAdd)
                                                    .Select(stInfo => new SheetDto
                                                    {
                                                        SHID = stInfo.AccountId,
                                                        SHName = stInfo.ShnameForCompany,
                                                        SliceAssets = stInfo.Account.SliceAssets
                                                                                                .Where(asset => asset.CompanyId == companyId && asset.AssetStatus == Status.ACTIVE)
                                                                                                .Select(asset => asset.AssetSlice).Sum() ?? 0
                                                    })
                                                    .ToListAsync();

            for (int i = 2; i < ListMainUserInfo.Count + 2; i++)
            {
                var range = $"{Sheet}!A{i}:C{i}";
                var valueRange = new ValueRange();

                var objectList = new List<object>() { ListMainUserInfo[i - 2].SHID, ListMainUserInfo[i - 2].SHName, ListMainUserInfo[i-2].SliceAssets };
                valueRange.Values = new List<IList<object>> { objectList };

                var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadSheetID, range);
                updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
                var appendRespone = updateRequest.Execute();
            }

            return ListMainUserInfo;
        }

        public async Task<bool> DeleteEntry(String Sheet, String companyId,String accountID)
        {

            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            var ListMainUserInfo = await _context.StakeHolders
                                                    .Where(stInfo => stInfo.CompanyId == companyId)
                                                    .OrderBy(sh => sh.DateTimeAdd)
                                                    .Select(stInfo => new SheetDto
                                                    {
                                                        SHID = stInfo.AccountId,
                                                        SHName = stInfo.ShnameForCompany,
                                                        SliceAssets = stInfo.Account.SliceAssets
                                                                                                .Where(asset => asset.CompanyId == companyId && asset.AssetStatus == Status.ACTIVE)
                                                                                                .Select(asset => asset.AssetSlice).Sum() ?? 0
                                                    })
                                                    .ToListAsync();

            for (int i = 2; i < ListMainUserInfo.Count + 2; i++)
            {    
                if(String.Compare(accountID, ListMainUserInfo[i - 2].SHID, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    int row = i;
                    var range = $"{Sheet}!A{i}:C";
                    var requestBody = new ClearValuesRequest();

                    var deleteRequest = service.Spreadsheets.Values.Clear(requestBody, SpreadSheetID, range);
                    var deleteReponse = deleteRequest.Execute();
                    return true;
                }
            }
            return false;

            
        }


    }
}
