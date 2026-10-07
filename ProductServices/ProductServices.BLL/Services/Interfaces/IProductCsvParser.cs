using Microsoft.AspNetCore.Http;
using ProductServices.BLL.Services.Import;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductServices.BLL.Services.Interfaces
{
    public interface IProductCsvParser
    {
        Task<List<ProductImportDto>> ParseAsync(Stream stream);
    }
}
