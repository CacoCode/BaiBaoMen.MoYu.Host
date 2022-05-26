using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Timing;
using Abp.UI;
using Magicodes.Admin.Application.Core;
using Magicodes.Admin.Application.Core.Dto;
using Magicodes.Admin.Core.Custom.Authorization;
using Magicodes.Admin.Application.Custom.WxUsers.Dto;
using Magicodes.Admin.Core.Custom.WxUsers;
using Microsoft.EntityFrameworkCore;

namespace Magicodes.Admin.Application.Custom.WxUsers
{
    public class WxUserAppService : CustomCrudeServiceBase<WxUser, WxUserListDto, long, GetAllWxUserInput, CreateWxUserInputDto, UpdateWxUserInput, ExportWxUserDto>, IWxUserAppService
    {

        public WxUserAppService(IRepository<WxUser, long> repository) : base(repository)
        {
            this.GetAllPermissionName = AppCustomPermissions.Customs_WxUser;
            this.GetPermissionName = AppCustomPermissions.Customs_WxUser;
            this.CreatePermissionName = AppCustomPermissions.Customs_WxUser_Create;
            this.UpdatePermissionName = AppCustomPermissions.Customs_WxUser_Edit;
            this.DeletePermissionName = AppCustomPermissions.Customs_WxUser_Delete;
            this.ExportPermissionName = AppCustomPermissions.Customs_WxUser_Export;
        }

        protected override IQueryable<WxUser> CreateFilteredQuery(GetAllWxUserInput input)
        {
            var query = Repository.GetAllIncluding(a => a.MoYuRecords);
            query = query.WhereIf(!input.FilterText.IsNullOrWhiteSpace(), p => p.NickName.Contains(input.FilterText));
            return query;
        }

        public override async Task<PagedResultDto<WxUserListDto>> GetAll(GetAllWxUserInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            var models = entities.Select(MapToEntityDto).AsQueryable();
            models = ApplySorting(models, input);
            models = ApplyPaging(models, input);

            return new PagedResultDto<WxUserListDto>(
                totalCount,
                models.ToList()
            );
        }

        public override async Task<FileDto> GetExport(GetAllWxUserInput input)
        {
            CheckPermission(ExportPermissionName);
            List<ExportWxUserDto> exportData = null;
            var query = CreateFilteredQuery(input);
            var results = await query
                .OrderBy(input.Sorting)
                .ToListAsync();

            exportData = results.Select(MapToExportDto).OrderByDescending(a=>a.Integral).ToList();

            if (exportData.Count == 0)
            {
                throw new UserFriendlyException(L("NoDataToExport"));
            }

            var fileDto = new FileDto("打卡排行榜.xlsx",
                MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            var byteArray = await ExcelExporter.ExportAsByteArray(exportData);
            TempFileCacheManager.SetFile(fileDto.FileToken, byteArray);
            return fileDto;
        }

        private IQueryable<WxUserListDto> ApplySorting(IQueryable<WxUserListDto> query, GetAllWxUserInput input)
        {
            return query.OrderByDescending(a => a.Integral); 
        }

        private IQueryable<WxUserListDto> ApplyPaging(IQueryable<WxUserListDto> query, GetAllWxUserInput input)
        {
            var pagedInput = input as IPagedResultRequest;
            if (pagedInput != null)
            {
                return query.PageBy(pagedInput);
            }

            //Try to limit query result if available
            var limitedInput = input as ILimitedResultRequest;
            if (limitedInput != null)
            {
                return query.Take(limitedInput.MaxResultCount);
            }

            //No paging
            return query;
        }
    }
}
