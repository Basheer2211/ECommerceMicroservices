using ProductServices.BLL.Services.Import;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductServices.BLL.Services.Interfaces
{
    public interface IProductImportValidator
    {
        string? Validate(List<ProductImportDto> items);
    }
}
