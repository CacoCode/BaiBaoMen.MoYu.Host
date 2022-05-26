// ======================================================================
// 
//           Copyright (C) 2019-2020 湖南心莱信息科技有限公司
//           All rights reserved
// 
//           filename : FileController.cs
//           description :
// 
//           created by 雪雁 at  2019-06-14 11:22
//           开发文档: docs.xin-lai.com
//           公众号教程：magiccodes
//           QQ群：85318032（编程交流）
//           Blog：http://www.cnblogs.com/codelove/
//           Home：http://xin-lai.com
// 
// ======================================================================

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Authorization;
using Abp.UI;
using Abp.Web.Models;
using Magicodes.Admin.Application.Core.Dto;
using Magicodes.Admin.Core.Storage;
using Magicodes.Admin.Unity.Storage.Default;
using Magicodes.Admin.Unity.Storage.Local;
using Magicodes.Admin.Web.Host.Files;
using Magicodes.Storage.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Magicodes.Admin.Web.Host.Controllers
{
    [Produces("application/json")]
    [Route("api/services/client/[controller]")]
    [AbpAllowAnonymous]
    public class FileController : AdminControllerBase
    {
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILocalStorageManager _localStorageManager;

        public FileController(
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            ILocalStorageManager localStorageManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _localStorageManager = localStorageManager;
        }

        [DisableAuditing]
        [HttpGet("DownloadTempFile")]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null) return NotFound(L("RequestedFileDoesNotExists"));

            return File(fileBytes, file.FileType, file.FileName);
        }

        [DisableAuditing]
        [HttpPost("DownloadBinaryFile")]
        public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        {
            var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
            if (fileObject == null) return StatusCode((int) HttpStatusCode.NotFound);

            return File(fileObject.Bytes, contentType, fileName);
        }

        [HttpPost("UploadImage")]
        //[ProducesResponseType(typeof(FileUploadOutputDto), StatusCodes.Status200OK)]
        [DisableAuditing]
        public async Task<ActionResult> UploadImage()
        {
            var files = Request.Form.Files;
            if (files == null || files.Count == 0)
            {
                throw new UserFriendlyException("文件不能为空");
            }
            try
            {
                var output = new FileUploadOutputDto();
                foreach (var item in files)
                {
                    if (item == null)
                    {
                        throw new UserFriendlyException("上传报错");
                    }
                    //5M
                    if (item.Length > 5242880)
                    {
                        throw new UserFriendlyException("文件大小不能超过5M");
                    }

                    if (!item.ContentType.StartsWith("image/"))
                    {
                        throw new UserFriendlyException("只允许上传图片");
                    }

                    try
                    {
                        using (var stream = item.OpenReadStream())
                        {
                            var tempFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(item.FileName);
                            await _localStorageManager.StorageProvider.SaveBlobStream("Images", tempFileName, stream);
                            var blobInfo = await _localStorageManager.StorageProvider.GetBlobFileInfo("Images", tempFileName);
                            output.Name = blobInfo.Name;
                            output.Path = blobInfo.Url;
                        }
                    }
                    catch (StorageException ex)
                    {
                        throw new UserFriendlyException(message: ex.ProviderMessage);
                    }
                }
                return Json(new AjaxResponse(output));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

    }
}